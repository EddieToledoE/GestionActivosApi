namespace GestionActivos.Domain.Entities
{
    public class Usuario
    {
        public int IdUsuario { get; set; }
        public string Nombres { get; set; } = string.Empty;
        public string ApellidoPaterno { get; set; } = string.Empty;
        public string? ApellidoMaterno { get; set; }
        public string? ClaveFortia { get; set; }
        public string? Correo { get; set; }
        public string Contrasena { get; set; } = string.Empty;
        public bool Activo { get; set; } = true;

        // Relaciones
        public ICollection<UsuarioRol> Roles { get; set; } = new List<UsuarioRol>();
        public ICollection<Activo> ActivosResponsables { get; set; } = new List<Activo>();
        public ICollection<Reubicacion> ReubicacionesAnteriores { get; set; } =
            new List<Reubicacion>();
        public ICollection<Reubicacion> ReubicacionesNuevas { get; set; } = new List<Reubicacion>();
        public ICollection<Diagnostico> Diagnosticos { get; set; } = new List<Diagnostico>();
        public ICollection<Solicitud> SolicitudesEmitidas { get; set; } = new List<Solicitud>();
        public ICollection<Solicitud> SolicitudesRecibidas { get; set; } = new List<Solicitud>();
        public int? IdCentroCosto { get; set; }
        public CentroCosto? CentroCosto { get; set; }
    }
}
