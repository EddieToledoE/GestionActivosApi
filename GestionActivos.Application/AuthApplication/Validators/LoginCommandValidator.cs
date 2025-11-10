using System.Text.RegularExpressions;
using FluentValidation;
using GestionActivos.Application.AuthApplication.Commands;

namespace GestionActivos.Application.AuthApplication.Validators
{
    public class LoginCommandValidator : AbstractValidator<LoginCommand>
    {
        private static readonly Regex EmailRegex = new(
            @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$",
            RegexOptions.Compiled | RegexOptions.IgnoreCase
        );

        public LoginCommandValidator()
        {
            RuleFor(x => x.Correo)
                .NotEmpty()
                .WithMessage("El correo electrónico es obligatorio.")
                .Must(correo => !string.IsNullOrWhiteSpace(correo) && IsValidEmail(correo))
                .WithMessage("El correo electrónico no es válido. Debe tener un formato válido (ejemplo: usuario@dominio.com).");

            RuleFor(x => x.Contrasena)
                .NotEmpty()
                .WithMessage("La contraseña es obligatoria.");
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

