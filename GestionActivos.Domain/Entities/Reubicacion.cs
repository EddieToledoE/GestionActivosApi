namespace GestionActivos.Domain.Entities
{
    public class Reubicacion
    {
        public int IdReubicacion { get; set; }
        public Guid IdActivo { get; set; }
        public Guid IdUsuarioAnterior { get; set; }
        public Guid IdUsuarioNuevo { get; set; }
        public DateTime Fecha { get; set; } = DateTime.Now;
        public string? Motivo { get; set; }

        public Activo Activo { get; set; } = null!;
        public Usuario UsuarioAnterior { get; set; } = null!;
        public Usuario UsuarioNuevo { get; set; } = null!;
    }
}
