using AutoMapper;
using GestionActivos.Application.SolicitudApplication.DTOs;
using GestionActivos.Domain.Entities;
using GestionActivos.Domain.Exceptions;
using GestionActivos.Domain.Interfaces.UnitsOfWork;
using MediatR;

namespace GestionActivos.Application.SolicitudApplication.Commands
{
    public record CreateSolicitudCommand(CreateSolicitudDto Solicitud) : IRequest<int>;

    public class CreateSolicitudHandler : IRequestHandler<CreateSolicitudCommand, int>
    {
        private readonly IActivosUnitOfWork _uow;
        private readonly IMapper _mapper;

        public CreateSolicitudHandler(
            IActivosUnitOfWork uow,
            IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<int> Handle(
            CreateSolicitudCommand request,
            CancellationToken cancellationToken)
        {
            if (request.Solicitud == null)
            {
                throw new ArgumentNullException(
                    nameof(request.Solicitud),
                    "La solicitud no puede ser nula.");
            }

            // Validar que el emisor existe
            var emisor = await _uow.Usuarios.GetByIdAsync(request.Solicitud.IdEmisor);
            if (emisor == null)
            {
                throw new NotFoundException(
                    $"No se encontró el usuario emisor con ID {request.Solicitud.IdEmisor}.");
            }

            // Validar que el receptor existe
            var receptor = await _uow.Usuarios.GetByIdAsync(request.Solicitud.IdReceptor);
            if (receptor == null)
            {
                throw new NotFoundException(
                    $"No se encontró el usuario receptor con ID {request.Solicitud.IdReceptor}.");
            }

            // Validar que el activo existe
            var activo = await _uow.Activos.GetByIdAsync(request.Solicitud.IdActivo);
            if (activo == null)
            {
                throw new NotFoundException(
                    $"No se encontró el activo con ID {request.Solicitud.IdActivo}.");
            }

            // Mapear el DTO a la entidad
            var solicitud = _mapper.Map<Solicitud>(request.Solicitud);

            // Guardar la solicitud
            await _uow.Solicitudes.AddAsync(solicitud);
            await _uow.SaveChangesAsync();

            return solicitud.IdSolicitud;
        }
    }
}
