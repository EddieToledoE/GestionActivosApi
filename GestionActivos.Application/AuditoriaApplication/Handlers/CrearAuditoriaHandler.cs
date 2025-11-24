using GestionActivos.Application.AuditoriaApplication.Commands;
using GestionActivos.Domain.Entities;
using GestionActivos.Domain.Exceptions;
using GestionActivos.Domain.Interfaces.UnitsOfWork;
using MediatR;

namespace GestionActivos.Application.AuditoriaApplication.Handlers
{
    public class CrearAuditoriaHandler : IRequestHandler<CrearAuditoriaCommand, Guid>
    {
        private readonly IAuditoriaUnitOfWork _uow;

        public CrearAuditoriaHandler(IAuditoriaUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<Guid> Handle(
            CrearAuditoriaCommand request,
            CancellationToken cancellationToken)
        {
            await _uow.BeginTransactionAsync();

            try
            {
                var dto = request.Auditoria;

                var auditor = await _uow.Usuarios.GetByIdAsync(dto.IdAuditor);
                if (auditor == null)
                {
                    throw new NotFoundException($"No se encontró el auditor con ID {dto.IdAuditor}.");
                }

                if (!auditor.Activo)
                {
                    throw new BusinessException($"El auditor '{auditor.Nombres} {auditor.ApellidoPaterno}' está inactivo.");
                }

                var usuarioAuditado = await _uow.Usuarios.GetByIdAsync(dto.IdUsuarioAuditado);
                if (usuarioAuditado == null)
                {
                    throw new NotFoundException($"No se encontró el usuario auditado con ID {dto.IdUsuarioAuditado}.");
                }

                if (!usuarioAuditado.Activo)
                {
                    throw new BusinessException($"El usuario auditado '{usuarioAuditado.Nombres} {usuarioAuditado.ApellidoPaterno}' está inactivo.");
                }

                if (!usuarioAuditado.IdCentroCosto.HasValue)
                {
                    throw new BusinessException($"El usuario auditado '{usuarioAuditado.Nombres} {usuarioAuditado.ApellidoPaterno}' no tiene un centro de costo asignado.");
                }

                int idCentroCostoHistorico = usuarioAuditado.IdCentroCosto.Value;

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

                string tipoAuditoria = dto.IdAuditor == dto.IdUsuarioAuditado ? "Auto" : "Externa";

                var auditoria = new Auditoria
                {
                    IdAuditoria = Guid.NewGuid(),
                    Tipo = tipoAuditoria,
                    IdAuditor = dto.IdAuditor,
                    IdUsuarioAuditado = dto.IdUsuarioAuditado,
                    IdCentroCosto = idCentroCostoHistorico,
                    Fecha = DateTime.Now,
                    Observaciones = dto.Observaciones,
                    Detalles = dto.Detalles.Select(d => new DetalleAuditoria
                    {
                        IdActivo = d.IdActivo,
                        Estado = d.Estado,
                        Comentarios = d.Comentarios
                    }).ToList()
                };

                await _uow.Auditorias.AddAsync(auditoria);
                await _uow.SaveChangesAsync();
                await _uow.CommitAsync();

                return auditoria.IdAuditoria;
            }
            catch
            {
                await _uow.RollbackAsync();
                throw;
            }
        }
    }
}
