using FluentValidation;
using GestionActivos.Application.ReubicacionApplication.Commands;

namespace GestionActivos.Application.ReubicacionApplication.Validators
{
    /// <summary>
    /// Validador para el comando de transferencia de activo por auditor.
    /// </summary>
    public class AuditorTransferCommandValidator : AbstractValidator<AuditorTransferCommand>
    {
        public AuditorTransferCommandValidator()
        {
            RuleFor(x => x.IdAuditor)
                .NotEmpty()
                .WithMessage("El ID del auditor es obligatorio.");

            RuleFor(x => x.IdActivo)
                .NotEmpty()
                .WithMessage("El ID del activo es obligatorio.");

            RuleFor(x => x.IdUsuarioDestino)
                .NotEmpty()
                .WithMessage("El ID del usuario destino es obligatorio.");

            RuleFor(x => x.Motivo)
                .NotEmpty()
                .WithMessage("El motivo de la transferencia es obligatorio.")
                .MaximumLength(500)
                .WithMessage("El motivo no puede exceder 500 caracteres.");
        }
    }
}
