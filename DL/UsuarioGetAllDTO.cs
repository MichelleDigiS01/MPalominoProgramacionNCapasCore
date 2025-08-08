using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DL
{
    public class UsuarioGetAllDTO
    {
        public int IdUsuario { get; set; }

        public string UserName { get; set; } = null!;

        public string UsuarioNombre { get; set; } = null!;

        public string ApellidoPaterno { get; set; } = null!;

        public string? ApellidoMaterno { get; set; }

        public string Email { get; set; } = null!;

        public string Password { get; set; } = null!;

        public string Sexo { get; set; } = null!;

        public string Telefono { get; set; } = null!;

        public string? Celular { get; set; }

        public string? FechaNacimiento { get; set; }

        public string? Curp { get; set; }

        public int IdRol { get; set; }

        public string? RolNombre { get; set; }
    }
}
