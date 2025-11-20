using GestionActivos.Domain.Exceptions;
using GestionActivos.Domain.Interfaces.UnitsOfWork;
using MediatR;

namespace GestionActivos.Application.SolicitudApplication.Commands
{
    /// <summary>
    /// Comando para rechazar una solicitud.
    /// </summary>
    public record RechazarSolicitudCommand(Guid IdSolicitud, Guid IdUsuarioAprobador, string? MotivoRechazo) : IRequest<bool>;

    public class RechazarSolicitudHandler : IRequestHandler<RechazarSolicitudCommand, bool>
    {
        private readonly ISolicitudUnitOfWork _uow;

        public RechazarSolicitudHandler(ISolicitudUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<bool> Handle(
            RechazarSolicitudCommand request,
            CancellationToken cancellationToken
        )
        {
            await _uow.BeginTransactionAsync();

            try
            {
                // 1. Obtener la solicitud
                var solicitud = await _uow.Solicitudes.GetByIdAsync(request.IdSolicitud);
                if (solicitud == null)
                {
                    throw new NotFoundException($"No se encontró la solicitud con ID {request.IdSolicitud}.");
                }

                if (solicitud.Estado != "Pendiente")
                {
                    throw new BusinessException($"La solicitud ya ha sido procesada. Estado actual: {solicitud.Estado}");
                }

                // Validar que el usuario que rechaza sea el receptor
                if (solicitud.IdReceptor != request.IdUsuarioAprobador)
                {
                    throw new BusinessException("Solo el receptor de la solicitud puede rechazarla.");
                }

                // 2. Actualizar estado de la solicitud
                solicitud.Estado = "Rechazada";
                
                // Agregar motivo de rechazo al mensaje si se proporciona
                if (!string.IsNullOrWhiteSpace(request.MotivoRechazo))
                {
                    solicitud.Mensaje = solicitud.Mensaje + $"\n[RECHAZADA] Motivo: {request.MotivoRechazo}";
                }
                
                await _uow.Solicitudes.UpdateAsync(solicitud);

                // 3. Guardar cambios y confirmar transacción
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
