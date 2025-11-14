using AutoMapper;
using GestionActivos.Application.SolicitudApplication.DTOs;
using GestionActivos.Domain.Interfaces;
using MediatR;

namespace GestionActivos.Application.SolicitudApplication.Queries
{
    public record GetSolicitudesPendientesByReceptorQuery(Guid ReceptorId) : IRequest<IEnumerable<SolicitudDto>>;

    public class GetSolicitudesPendientesByReceptorHandler : IRequestHandler<GetSolicitudesPendientesByReceptorQuery, IEnumerable<SolicitudDto>>
    {
        private readonly ISolicitudRepository _solicitudRepository;
        private readonly IMapper _mapper;

        public GetSolicitudesPendientesByReceptorHandler(
            ISolicitudRepository solicitudRepository,
            IMapper mapper)
        {
            _solicitudRepository = solicitudRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<SolicitudDto>> Handle(
            GetSolicitudesPendientesByReceptorQuery request,
            CancellationToken cancellationToken)
        {
            var solicitudes = await _solicitudRepository.GetByReceptorIdAsync(request.ReceptorId);
            
            // Filtrar solo las pendientes
            var solicitudesPendientes = solicitudes.Where(s => s.Estado == "Pendiente");
            
            return _mapper.Map<IEnumerable<SolicitudDto>>(solicitudesPendientes);
        }
    }
}
