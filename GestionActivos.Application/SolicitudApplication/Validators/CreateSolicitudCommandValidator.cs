using FluentValidation;
using GestionActivos.Application.SolicitudApplication.DTOs;

namespace GestionActivos.Application.SolicitudApplication.Validators
{
    public class CreateSolicitudCommandValidator : AbstractValidator<CreateSolicitudDto>
    {
        public CreateSolicitudCommandValidator()
        {
            RuleFor(x => x.IdEmisor)
                .NotEmpty()
                .WithMessage("El ID del emisor es obligatorio.");

            RuleFor(x => x.IdReceptor)
                .NotEmpty()
                .WithMessage("El ID del receptor es obligatorio.");

            RuleFor(x => x.IdReceptor)
                .NotEqual(x => x.IdEmisor)
                .WithMessage("El receptor no puede ser el mismo que el emisor.");

            RuleFor(x => x.IdActivo)
                .NotEmpty()
                .WithMessage("El ID del activo es obligatorio.");

            RuleFor(x => x.Tipo)
                .NotEmpty()
                .WithMessage("El tipo de solicitud es obligatorio.")
                .MaximumLength(50)
                .WithMessage("El tipo no puede exceder 50 caracteres.")
                .Must(tipo => new[] { "Transferencia", "Baja", "Diagnóstico", "Auditoría" }.Contains(tipo))
                .WithMessage("El tipo debe ser: Transferencia, Baja, Diagnóstico o Auditoría.");

            RuleFor(x => x.Mensaje)
                .MaximumLength(300)
                .WithMessage("El mensaje no puede exceder 300 caracteres.");
        }
    }
}
