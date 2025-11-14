using FluentValidation;
using GestionActivos.Application.ActivoApplication.DTOs;

namespace GestionActivos.Application.ActivoApplication.Validators
{
    public class CreateActivoCommandValidator : AbstractValidator<CreateActivoDto>
    {
        public CreateActivoCommandValidator()
        {
            RuleFor(x => x.ResponsableId)
                .NotEmpty()
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

            RuleFor(x => x.CuentaContable)
         .MaximumLength(100)
             .WithMessage("La cuenta contable no puede exceder 100 caracteres.");

            // Si no es donación, entonces al menos una factura, CuentaContable, ValorAdquisicion y FechaAdquisicion son obligatorios
    When(x => !x.Donacion, () =>
     {
      RuleFor(x => x)
         .Must(x => x.FacturaPDF != null || x.FacturaXML != null)
  .WithMessage("Debe proporcionar al menos una factura (PDF o XML) cuando no es donación.");

     RuleFor(x => x.CuentaContable)
       .NotEmpty()
    .WithMessage("La cuenta contable es obligatoria cuando no es donación.");

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

        // Si NO porta etiqueta, CuentaContableEtiqueta NO puede ser true
            When(x => !x.PortaEtiqueta, () =>
 {
     RuleFor(x => x.CuentaContableEtiqueta)
    .Must(value => value == false)
            .WithMessage("CuentaContableEtiqueta debe ser false cuando no porta etiqueta.");
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

// Validación de FacturaPDF (opcional, pero si existe validar formato)
            When(x => x.FacturaPDF != null, () =>
            {
       RuleFor(x => x.FacturaPDF!.Length)
     .LessThanOrEqualTo(10 * 1024 * 1024)
         .WithMessage("La factura PDF no puede exceder 10MB.");

     RuleFor(x => x.FacturaPDF!.ContentType)
        .Must(contentType => contentType == "application/pdf")
        .WithMessage("El archivo debe ser un PDF válido.");
            });

      // Validación de FacturaXML (opcional, pero si existe validar formato)
            When(x => x.FacturaXML != null, () =>
      {
  RuleFor(x => x.FacturaXML!.Length)
      .LessThanOrEqualTo(5 * 1024 * 1024)
              .WithMessage("La factura XML no puede exceder 5MB.");

 RuleFor(x => x.FacturaXML!.ContentType)
   .Must(contentType => contentType == "application/xml" || contentType == "text/xml")
    .WithMessage("El archivo debe ser un XML válido.");
            });
        }
    }
}
