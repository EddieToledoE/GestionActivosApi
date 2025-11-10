using FluentValidation;
using GestionActivos.Application.ActivoApplication.DTOs;

namespace GestionActivos.Application.ActivoApplication.Validators
{
    public class CreateActivoCommandValidator : AbstractValidator<CreateActivoDto>
    {
     public CreateActivoCommandValidator()
        {
            RuleFor(x => x.ResponsableId)
 .GreaterThan(0)
         .WithMessage("El responsable es obligatorio.");

            RuleFor(x => x.IdCategoria)
              .GreaterThan(0)
       .WithMessage("La categoría es obligatoria.");

        RuleFor(x => x.Etiqueta)
          .NotEmpty()
          .WithMessage("La etiqueta es obligatoria.")
     .MaximumLength(50)
                .WithMessage("La etiqueta no puede exceder 50 caracteres.");

       RuleFor(x => x.NumeroSerie)
       .MaximumLength(100)
          .WithMessage("El número de serie no puede exceder 100 caracteres.");

 RuleFor(x => x.Marca)
           .MaximumLength(100)
          .WithMessage("La marca no puede exceder 100 caracteres.");

      RuleFor(x => x.Modelo)
       .MaximumLength(100)
         .WithMessage("El modelo no puede exceder 100 caracteres.");

            RuleFor(x => x.Descripcion)
      .MaximumLength(200)
     .WithMessage("La descripción no puede exceder 200 caracteres.");

        // Si no es donación, entonces Factura, ValorAdquisicion y FechaAdquisicion son obligatorios
When(x => !x.Donacion, () =>
{
          RuleFor(x => x.Factura)
        .NotEmpty()
           .WithMessage("La factura es obligatoria cuando no es donación.")
       .MaximumLength(100)
        .WithMessage("La factura no puede exceder 100 caracteres.");

         RuleFor(x => x.ValorAdquisicion)
   .NotNull()
       .WithMessage("El valor de adquisición es obligatorio cuando no es donación.")
            .GreaterThan(0)
   .WithMessage("El valor de adquisición debe ser mayor a 0.");

          RuleFor(x => x.FechaAdquisicion)
   .NotNull()
         .WithMessage("La fecha de adquisición es obligatoria cuando no es donación.")
             .LessThanOrEqualTo(DateTime.Now)
         .WithMessage("La fecha de adquisición no puede ser futura.");
 });

            // Validación de imagen (opcional, pero si existe validar formato)
            When(x => x.Imagen != null, () =>
  {
       RuleFor(x => x.Imagen!.Length)
     .LessThanOrEqualTo(5 * 1024 * 1024)
        .WithMessage("La imagen no puede exceder 5MB.");

    RuleFor(x => x.Imagen!.ContentType)
        .Must(contentType => contentType.StartsWith("image/"))
          .WithMessage("El archivo debe ser una imagen válida.");
    });
        }
    }
}
