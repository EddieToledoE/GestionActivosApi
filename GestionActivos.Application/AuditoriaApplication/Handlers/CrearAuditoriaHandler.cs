using GestionActivos.Application.AuditoriaApplication.Commands;
using GestionActivos.Domain.Entities;
using GestionActivos.Domain.Exceptions;
using GestionActivos.Domain.Interfaces.UnitsOfWork;
using MediatR;

namespace GestionActivos.Application.AuditoriaApplication.Handlers
{
    /// <summary>
    /// Handler para registrar una nueva auditoría completa usando Unit of Work.
    /// 
    /// Flujo:
    /// 1. Inicia una transacción explícita
    /// 2. Valida existencia y estado del auditor
    /// 3. Valida existencia y estado del usuario auditado
    /// 4. Valida que todos los activos en los detalles existan
    /// 5. Determina automáticamente el tipo de auditoría:
    ///    - Auto: si IdAuditor == IdUsuarioAuditado
    ///    - Externa: si son personas diferentes
    /// 6. Crea la auditoría con sus detalles en memoria
    /// 7. Persiste en una transacción atómica
    /// 8. Commit si todo es exitoso, Rollback automático si hay error
    /// 9. Retorna el ID de la auditoría creada
    /// </summary>
    public class CrearAuditoriaHandler : IRequestHandler<CrearAuditoriaCommand, Guid>
    {
        private readonly IActivosUnitOfWork _uow;

        public CrearAuditoriaHandler(IActivosUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<Guid> Handle(
            CrearAuditoriaCommand request,
            CancellationToken cancellationToken)
        {
            // Iniciar transacción explícita
            await _uow.BeginTransactionAsync();

            try
            {
                var dto = request.Auditoria;

                // 1. Validar que el auditor existe y está activo
                var auditor = await _uow.Usuarios.GetByIdAsync(dto.IdAuditor);
                if (auditor == null)
                {
                    throw new NotFoundException($"No se encontró el auditor con ID {dto.IdAuditor}.");
                }

                if (!auditor.Activo)
                {
                    throw new BusinessException($"El auditor '{auditor.Nombres} {auditor.ApellidoPaterno}' está inactivo.");
                }

                // 2. Validar que el usuario auditado existe y está activo
                var usuarioAuditado = await _uow.Usuarios.GetByIdAsync(dto.IdUsuarioAuditado);
                if (usuarioAuditado == null)
                {
                    throw new NotFoundException($"No se encontró el usuario auditado con ID {dto.IdUsuarioAuditado}.");
                }

                if (!usuarioAuditado.Activo)
                {
                    throw new BusinessException($"El usuario auditado '{usuarioAuditado.Nombres} {usuarioAuditado.ApellidoPaterno}' está inactivo.");
                }

                // 3. Validar que los activos en los detalles existen
                foreach (var detalle in dto.Detalles)
                {
                    var activo = await _uow.Activos.GetByIdAsync(detalle.IdActivo);
                    if (activo == null)
                    {
                        throw new NotFoundException($"No se encontró el activo con ID {detalle.IdActivo}.");
                    }

                    if (activo.Estatus != "Activo")
                    {
                        throw new BusinessException($"El activo con etiqueta '{activo.Etiqueta}' no está activo (Estado: {activo.Estatus}).");
                    }
                }

                // 4. Determinar el tipo de auditoría automáticamente
                string tipoAuditoria = dto.IdAuditor == dto.IdUsuarioAuditado ? "Auto" : "Externa";

                // 5. Crear la entidad Auditoria con sus detalles
                var auditoria = new Auditoria
                {
                    IdAuditoria = Guid.NewGuid(),
                    Tipo = tipoAuditoria, // ? Calculado automáticamente
                    IdAuditor = dto.IdAuditor,
                    IdUsuarioAuditado = dto.IdUsuarioAuditado,
                    IdCentroCosto = dto.IdCentroCosto,
                    Fecha = DateTime.Now,
                    Observaciones = dto.Observaciones,
                    Detalles = dto.Detalles.Select(d => new DetalleAuditoria
                    {
                        IdActivo = d.IdActivo,
                        Estado = d.Estado,
                        Comentarios = d.Comentarios
                    }).ToList()
                };

                // 6. Agregar al contexto (sin guardar todavía)
                await _uow.Auditorias.AddAsync(auditoria);

                // 7. Guardar cambios y confirmar transacción
                await _uow.SaveChangesAsync();
                await _uow.CommitAsync();

                // 8. Retornar el ID de la auditoría creada
                return auditoria.IdAuditoria;
            }
            catch
            {
                // Revertir transacción en caso de error
                await _uow.RollbackAsync();
                throw;
            }
        }
    }
}
