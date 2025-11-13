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
                .GreaterThan(0)
                .WithMessage("El ID del auditor es obligatorio y debe ser mayor a 0.");

            RuleFor(x => x.IdActivo)
                .GreaterThan(0)
                .WithMessage("El ID del activo es obligatorio y debe ser mayor a 0.");

            RuleFor(x => x.IdUsuarioDestino)
                .GreaterThan(0)
                .WithMessage("El ID del usuario destino es obligatorio y debe ser mayor a 0.");

            RuleFor(x => x.Motivo)
                .NotEmpty()
                .WithMessage("El motivo de la transferencia es obligatorio.")
                .MaximumLength(500)
                .WithMessage("El motivo no puede exceder 500 caracteres.");
        }
    }
}
