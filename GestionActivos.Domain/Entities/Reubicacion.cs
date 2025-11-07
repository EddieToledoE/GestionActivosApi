namespace GestionActivos.Domain.Entities
{
    public class Reubicacion
    {
        public int IdReubicacion { get; set; }
        public int IdActivo { get; set; }
        public int IdUsuarioAnterior { get; set; }
        public int IdUsuarioNuevo { get; set; }
        public DateTime Fecha { get; set; } = DateTime.Now;
        public string? Motivo { get; set; }

        public Activo Activo { get; set; } = null!;
        public Usuario UsuarioAnterior { get; set; } = null!;
        public Usuario UsuarioNuevo { get; set; } = null!;
    }
}
