using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionActivos.Application.UsuarioApplication.DTOs
{
    public class UpdateUsuarioDto
    {
        public int Id { get; set; }
        public string? Nombre { get; set; } = string.Empty;
        public string? ApellidoPaterno { get; set; } = string.Empty;
        public string? ApellidoMaterno { get; set; } = string.Empty;
        public string? Correo { get; set; } = string.Empty;
        public string? Contrasena { get; set; } = string.Empty;
    }
}
