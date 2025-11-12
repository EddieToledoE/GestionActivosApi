using AutoMapper;
using GestionActivos.Application.SolicitudApplication.DTOs;
using GestionActivos.Domain.Interfaces;
using MediatR;

namespace GestionActivos.Application.SolicitudApplication.Queries
{
    public record GetSolicitudesPendientesByEmisorQuery(int EmisorId) : IRequest<IEnumerable<SolicitudDto>>;

    public class GetSolicitudesPendientesByEmisorHandler : IRequestHandler<GetSolicitudesPendientesByEmisorQuery, IEnumerable<SolicitudDto>>
    {
        private readonly ISolicitudRepository _solicitudRepository;
        private readonly IMapper _mapper;

        public GetSolicitudesPendientesByEmisorHandler(
            ISolicitudRepository solicitudRepository,
            IMapper mapper)
        {
            _solicitudRepository = solicitudRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<SolicitudDto>> Handle(
            GetSolicitudesPendientesByEmisorQuery request,
            CancellationToken cancellationToken)
        {
            var solicitudes = await _solicitudRepository.GetByEmisorIdAsync(request.EmisorId);
            
            // Filtrar solo las pendientes
            var solicitudesPendientes = solicitudes.Where(s => s.Estado == "Pendiente");
            
            return _mapper.Map<IEnumerable<SolicitudDto>>(solicitudesPendientes);
        }
    }
}
