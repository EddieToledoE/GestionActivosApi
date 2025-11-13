using GestionActivos.Domain.Entities;
using GestionActivos.Domain.Exceptions;
using GestionActivos.Domain.Interfaces.UnitsOfWork;
using MediatR;

namespace GestionActivos.Application.SolicitudApplication.Commands
{
    /// <summary>
    /// Comando para aceptar una solicitud de transferencia de activo.
    /// </summary>
    public record AceptarSolicitudCommand(int IdSolicitud, int IdUsuarioAprobador) : IRequest<bool>;

    public class AceptarSolicitudHandler : IRequestHandler<AceptarSolicitudCommand, bool>
    {
        private readonly IActivosUnitOfWork _uow;

        public AceptarSolicitudHandler(IActivosUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<bool> Handle(
            AceptarSolicitudCommand request,
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
                    throw new BusinessException(
                        $"La solicitud ya ha sido procesada. Estado actual: {solicitud.Estado}"
                    );
                }

                // Validar que el usuario que aprueba sea el receptor
                if (solicitud.IdReceptor != request.IdUsuarioAprobador)
                {
                    throw new BusinessException("Solo el receptor de la solicitud puede aceptarla.");
                }

                // 2. Actualizar estado de la solicitud
                solicitud.Estado = "Aceptada";
                await _uow.Solicitudes.UpdateAsync(solicitud);

                // 3. Obtener el activo
                var activo = await _uow.Activos.GetByIdAsync(solicitud.IdActivo);
                if (activo == null)
                {
                    throw new NotFoundException($"No se encontró el activo con ID {solicitud.IdActivo}.");
                }

                // 4. Crear reubicación
                var reubicacion = new Reubicacion
                {
                    IdActivo = activo.IdActivo,
                    IdUsuarioAnterior = activo.ResponsableId,
                    IdUsuarioNuevo = solicitud.IdReceptor,
                    Fecha = DateTime.Now,
                    Motivo = $"Transferencia por solicitud #{solicitud.IdSolicitud}",
                };
                await _uow.Reubicaciones.AddAsync(reubicacion);

                // 5. Actualizar responsable del activo
                activo.ResponsableId = solicitud.IdReceptor;
                await _uow.Activos.UpdateAsync(activo);

                // 6. Guardar cambios y confirmar transacción
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
