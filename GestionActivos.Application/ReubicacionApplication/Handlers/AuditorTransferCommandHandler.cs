using GestionActivos.Application.ReubicacionApplication.Commands;
using GestionActivos.Domain.Entities;
using GestionActivos.Domain.Exceptions;
using GestionActivos.Domain.Interfaces.UnitsOfWork;
using MediatR;

namespace GestionActivos.Application.ReubicacionApplication.Handlers
{
    /// <summary>
    /// Handler para procesar la transferencia directa de un activo por parte de un auditor.
    /// Ejecuta todas las operaciones en una transacción atómica.
    /// </summary>
    public class AuditorTransferCommandHandler : IRequestHandler<AuditorTransferCommand, bool>
    {
        private readonly IActivosUnitOfWork _uow;

        public AuditorTransferCommandHandler(IActivosUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<bool> Handle(
            AuditorTransferCommand request,
            CancellationToken cancellationToken)
        {
            await _uow.BeginTransactionAsync();

            try
            {
                // 1. Validar que el auditor existe
                var auditor = await _uow.Usuarios.GetByIdAsync(request.IdAuditor);
                if (auditor == null)
                {
                    throw new NotFoundException($"No se encontró el auditor con ID {request.IdAuditor}.");
                }

                // 2. Obtener el activo
                var activo = await _uow.Activos.GetByIdAsync(request.IdActivo);
                if (activo == null)
                {
                    throw new NotFoundException($"No se encontró el activo con ID {request.IdActivo}.");
                }

                // Validar que el activo esté activo
                if (activo.Estatus != "Activo")
                {
                    throw new BusinessException($"El activo '{activo.Etiqueta}' no está activo. Estado actual: {activo.Estatus}");
                }

                // 3. Validar que el usuario destino existe
                var usuarioDestino = await _uow.Usuarios.GetByIdAsync(request.IdUsuarioDestino);
                if (usuarioDestino == null)
                {
                    throw new NotFoundException($"No se encontró el usuario destino con ID {request.IdUsuarioDestino}.");
                }

                // Validar que no sea el mismo usuario
                if (activo.ResponsableId == request.IdUsuarioDestino)
                {
                    throw new BusinessException($"El activo ya está asignado al usuario destino.");
                }

                // Guardar el ID del usuario anterior para la reubicación y obtener su información
                int idUsuarioAnterior = activo.ResponsableId;
                var usuarioAnterior = await _uow.Usuarios.GetByIdAsync(idUsuarioAnterior);

                // Construir información descriptiva del activo para los mensajes
                string descripcionActivo = !string.IsNullOrWhiteSpace(activo.Descripcion) 
                    ? activo.Descripcion 
                    : "Sin descripción";
                
                string infoActivo = $"'{activo.Etiqueta}' - {descripcionActivo}";
                if (!string.IsNullOrWhiteSpace(activo.Marca) || !string.IsNullOrWhiteSpace(activo.Modelo))
                {
                    infoActivo += $" ({activo.Marca} {activo.Modelo})".Trim();
                }

                // 4. Crear registro de reubicación
                var reubicacion = new Reubicacion
                {
                    IdActivo = activo.IdActivo,
                    IdUsuarioAnterior = idUsuarioAnterior,
                    IdUsuarioNuevo = request.IdUsuarioDestino,
                    Fecha = DateTime.Now,
                    Motivo = $"[AUDITOR] {request.Motivo}"
                };
                await _uow.Reubicaciones.AddAsync(reubicacion);

                // 5. Actualizar responsable del activo
                activo.ResponsableId = request.IdUsuarioDestino;
                await _uow.Activos.UpdateAsync(activo);

                // 6. Crear notificación para el usuario anterior (revocación)
                var notificacionAnterior = new Notificacion
                {
                    IdUsuarioDestino = idUsuarioAnterior,
                    Origen = "Auditoría",
                    Tipo = "Revocación de Activo",
                    Titulo = "Activo reasignado por auditoría",
                    Mensaje = $"El activo {infoActivo} ha sido reasignado por un auditor. Motivo: {request.Motivo}",
                    Fecha = DateTime.Now,
                    Leida = false
                };
                await _uow.Notificaciones.AddAsync(notificacionAnterior);

                // 7. Crear notificación para el usuario nuevo (asignación)
                var notificacionNuevo = new Notificacion
                {
                    IdUsuarioDestino = request.IdUsuarioDestino,
                    Origen = "Auditoría",
                    Tipo = "Asignación de Activo",
                    Titulo = "Nuevo activo asignado",
                    Mensaje = $"Se te ha asignado el activo {infoActivo}. Motivo: {request.Motivo}",
                    Fecha = DateTime.Now,
                    Leida = false
                };
                await _uow.Notificaciones.AddAsync(notificacionNuevo);

                // 8. Guardar cambios y confirmar transacción
                await _uow.SaveChangesAsync();
                await _uow.CommitAsync();

                return true;
            }
            catch
            {
                await _uow.RollbackAsync();
                throw;
            }
        }
    }
}
