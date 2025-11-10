namespace GestionActivos.Domain.Exceptions
{
    public class ValidationException : Exception
    {
        public IDictionary<string, string[]> Errors { get; }

        public ValidationException() : base("Una o más validaciones fallaron.")
        {
            Errors = new Dictionary<string, string[]>();
        }

        public ValidationException(IDictionary<string, string[]> errors) : this()
        {
            Errors = errors;
        }

        public ValidationException(string property, string message) : this()
        {
            Errors.Add(property, new[] { message });
        }
    }
}

