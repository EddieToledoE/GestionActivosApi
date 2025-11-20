using AutoMapper;
using GestionActivos.Application.SolicitudApplication.DTOs;
using GestionActivos.Domain.Entities;
using GestionActivos.Domain.Exceptions;
using GestionActivos.Domain.Interfaces.UnitsOfWork;
using MediatR;
using System;

namespace GestionActivos.Application.SolicitudApplication.Commands
{
    public record CreateSolicitudCommand(CreateSolicitudDto Solicitud) : IRequest<Guid>;

    public class CreateSolicitudHandler : IRequestHandler<CreateSolicitudCommand, Guid>
    {
        private readonly ISolicitudUnitOfWork _uow;
        private readonly IMapper _mapper;

        public CreateSolicitudHandler(
            ISolicitudUnitOfWork uow,
            IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<Guid> Handle(
            CreateSolicitudCommand request,
            CancellationToken cancellationToken)
        {
            if (request.Solicitud == null)
            {
                throw new ArgumentNullException(
                    nameof(request.Solicitud),
                    "La solicitud no puede ser nula.");
            }

            // 1. Validar que el emisor existe y está activo
            var emisor = await _uow.Usuarios.GetByIdAsync(request.Solicitud.IdEmisor);
            if (emisor == null)
            {
                throw new NotFoundException(
                    $"No se encontró el usuario emisor con ID {request.Solicitud.IdEmisor}.");
            }

            if (!emisor.Activo)
            {
                throw new BusinessException(
                    $"El usuario emisor '{emisor.Nombres} {emisor.ApellidoPaterno}' está inactivo y no puede crear solicitudes.");
            }

            // 2. Validar que el receptor existe y está activo
            var receptor = await _uow.Usuarios.GetByIdAsync(request.Solicitud.IdReceptor);
            if (receptor == null)
            {
                throw new NotFoundException(
                    $"No se encontró el usuario receptor con ID {request.Solicitud.IdReceptor}.");
            }

            if (!receptor.Activo)
            {
                throw new BusinessException(
                    $"El usuario receptor '{receptor.Nombres} {receptor.ApellidoPaterno}' está inactivo y no puede recibir solicitudes.");
            }

            // 3. Validar que el activo existe y está activo
            var activo = await _uow.Activos.GetByIdAsync(request.Solicitud.IdActivo);
            if (activo == null)
            {
                throw new NotFoundException(
                    $"No se encontró el activo con ID {request.Solicitud.IdActivo}.");
            }

            if (activo.Estatus != "Activo")
            {
                throw new BusinessException(
                    $"El activo '{activo.Etiqueta}' no está activo (Estado: {activo.Estatus}). Solo se pueden crear solicitudes para activos activos.");
            }

            // 4. Validar que no exista una solicitud pendiente para este activo
            var existeSolicitudPendiente = await _uow.Solicitudes.ExisteSolicitudPendienteParaActivoAsync(request.Solicitud.IdActivo);
            if (existeSolicitudPendiente)
            {
                throw new BusinessException(
                    $"Ya existe una solicitud pendiente para el activo '{activo.Etiqueta}'. No se pueden crear solicitudes duplicadas hasta que la anterior sea procesada.");
            }

            // 5. Validar que el emisor sea el responsable actual del activo (opcional, depende de tu regla de negocio)
            if (activo.ResponsableId != request.Solicitud.IdEmisor)
            {
                throw new BusinessException(
                    $"Solo el responsable actual del activo '{activo.Etiqueta}' puede crear una solicitud de transferencia. Responsable actual: ID {activo.ResponsableId}");
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
