using GestionActivos.Domain.Exceptions;
using GestionActivos.Domain.Interfaces;
using MediatR;

namespace GestionActivos.Application.ActivoApplication.Commands
{
    public record DeleteActivoCommand(int Id) : IRequest;

    public class DeleteActivoHandler : IRequestHandler<DeleteActivoCommand>
    {
        private readonly IActivoRepository _activoRepository;

    public DeleteActivoHandler(IActivoRepository activoRepository)
        {
            _activoRepository = activoRepository;
        }

   public async Task Handle(DeleteActivoCommand request, CancellationToken cancellationToken)
   {
        var activo = await _activoRepository.GetByIdAsync(request.Id);

  if (activo == null)
            {
  throw new NotFoundException($"No se encontró el activo con ID {request.Id}.");
      }

        await _activoRepository.DeleteAsync(request.Id);
        }
    }
}
