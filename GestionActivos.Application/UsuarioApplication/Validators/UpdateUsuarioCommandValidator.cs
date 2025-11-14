using System.Text.RegularExpressions;
using FluentValidation;
using GestionActivos.Application.UsuarioApplication.Commands;

namespace GestionActivos.Application.UsuarioApplication.Validators
{
    public class UpdateUsuarioCommandValidator : AbstractValidator<UpdateUsuarioCommand>
    {
        private static readonly Regex EmailRegex = new(
            @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$",
            RegexOptions.Compiled | RegexOptions.IgnoreCase
        );

        public UpdateUsuarioCommandValidator()
        {
            RuleFor(x => x.Usuario)
                .NotNull()
                .WithMessage("El usuario no puede ser nulo.");

            When(x => x.Usuario != null, () =>
            {
                RuleFor(x => x.Usuario.Id)
                    .NotEmpty()
                    .WithMessage("El ID del usuario es obligatorio.");

                RuleFor(x => x.Usuario.Nombres)
                    .MaximumLength(100)
                    .WithMessage("El nombre no puede exceder 100 caracteres.")
                    .When(x => !string.IsNullOrEmpty(x.Usuario.Nombres));

                RuleFor(x => x.Usuario.ApellidoPaterno)
                    .MaximumLength(100)
                    .WithMessage("El apellido paterno no puede exceder 100 caracteres.")
                    .When(x => !string.IsNullOrEmpty(x.Usuario.ApellidoPaterno));

                RuleFor(x => x.Usuario.ApellidoMaterno)
                    .MaximumLength(100)
                    .WithMessage("El apellido materno no puede exceder 100 caracteres.")
                    .When(x => !string.IsNullOrEmpty(x.Usuario.ApellidoMaterno));

                RuleFor(x => x.Usuario.Correo)
                    .Must((command, correo) => string.IsNullOrEmpty(correo) || IsValidEmail(correo))
                    .WithMessage("El correo electrónico no es válido. Debe tener un formato válido (ejemplo: usuario@dominio.com).")
                    .MaximumLength(100)
                    .WithMessage("El correo no puede exceder 100 caracteres.")
                    .When(x => !string.IsNullOrEmpty(x.Usuario.Correo));

                RuleFor(x => x.Usuario.Contrasena)
                    .MinimumLength(6)
                    .WithMessage("La contraseña debe tener al menos 6 caracteres.")
                    .MaximumLength(200)
                    .WithMessage("La contraseña no puede exceder 200 caracteres.")
                    .When(x => !string.IsNullOrEmpty(x.Usuario.Contrasena));
            });
        }

        private static bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            // Validación básica: debe tener @ y al menos un punto después del @
            if (!email.Contains('@') || email.IndexOf('@') == 0 || email.IndexOf('@') == email.Length - 1)
                return false;

            var parts = email.Split('@');
            if (parts.Length != 2)
                return false;

            var localPart = parts[0];
            var domainPart = parts[1];

            // La parte local no puede estar vacía
            if (string.IsNullOrWhiteSpace(localPart))
                return false;

            // El dominio debe tener al menos un punto
            if (!domainPart.Contains('.') || domainPart.IndexOf('.') == 0 || domainPart.IndexOf('.') == domainPart.Length - 1)
                return false;

            // Validación con expresión regular
            return EmailRegex.IsMatch(email);
        }
    }
}

