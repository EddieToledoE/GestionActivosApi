namespace GestionActivos.Domain.Entities
{
    public class Permiso
    {
        public int IdPermiso { get; set; }
        public string Nombre { get; set; } = string.Empty;

        public ICollection<RolPermiso> Roles { get; set; } = new List<RolPermiso>();
    }
}
