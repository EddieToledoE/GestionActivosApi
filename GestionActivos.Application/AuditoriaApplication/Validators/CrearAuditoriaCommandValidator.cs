using FluentValidation;
using GestionActivos.Application.AuditoriaApplication.DTOs;

namespace GestionActivos.Application.AuditoriaApplication.Validators
{
    public class CrearAuditoriaCommandValidator : AbstractValidator<CrearAuditoriaDto>
    {
        public CrearAuditoriaCommandValidator()
        {
            RuleFor(x => x.IdAuditor)
                .NotEmpty()
                .WithMessage("El ID del auditor es obligatorio.");

            RuleFor(x => x.IdUsuarioAuditado)
                .NotEmpty()
                .WithMessage("El ID del usuario auditado es obligatorio.");

            RuleFor(x => x.IdCentroCosto)
                .GreaterThan(0)
                .WithMessage("El ID del centro de costo debe ser mayor a 0.");

            RuleFor(x => x.Detalles)
                .NotEmpty()
                .WithMessage("Debe incluir al menos un detalle de auditoría.");

            RuleForEach(x => x.Detalles).ChildRules(detalle =>
            {
                detalle.RuleFor(d => d.IdActivo)
                    .NotEmpty()
                    .WithMessage("El ID del activo es obligatorio en cada detalle.");

                detalle.RuleFor(d => d.Estado)
                    .NotEmpty()
                    .WithMessage("El estado del activo es obligatorio.")
                    .MaximumLength(50)
                    .WithMessage("El estado no puede exceder 50 caracteres.");

                detalle.RuleFor(d => d.Comentarios)
                    .MaximumLength(200)
                    .WithMessage("Los comentarios no pueden exceder 200 caracteres.");
            });
        }
    }
}
