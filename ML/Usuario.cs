using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ML
{
    public class Usuario
    {
        [Key]
        public int IdUsuario { get; set; }

        [Required(ErrorMessage = "Este campo no puede estar vacio")]
        [MaxLength(50)]
        [RegularExpression(@"^[a-zA-Z0-9]*$", ErrorMessage = "No se permiten espacios")]
        public string? UserName { get; set; }
        
        [Required(ErrorMessage = "Este campo no puede estar vacio")]
        [MaxLength(50)]
        [RegularExpression(@"^[a-zA-Z\s]*$", ErrorMessage = "No se permiten numeros")]
        
        public string? Nombre { get; set; }
        
        [Required(ErrorMessage = "Este campo no puede estar vacio")]
        [MaxLength(50)]
        [RegularExpression(@"^[a-zA-Z\s]*$", ErrorMessage = "No se permiten numeros")]
        public string? ApellidoPaterno { get; set; }
        
        [Required(ErrorMessage = "Este campo no puede estar vacio")]
        [MaxLength(50)]
        [RegularExpression(@"^[a-zA-Z\s]*$", ErrorMessage = "No se permiten numeros")]
        public string? ApellidoMaterno { get; set; }
        
        [Required(ErrorMessage = "Este campo no puede estar vacio")]
        [EmailAddress(ErrorMessage = "El correo electrónico no es valido")]
        [MaxLength(100)]
        public string? Email { get; set; }
        
        [Required(ErrorMessage = "Este campo no puede estar vacio")]
        [MaxLength(50)]
        [RegularExpression(@"^[a-zA-Z0-9._%-]*$", ErrorMessage = "Ingresa una contraseña minimo con una Minus, una Mayus, un número y un caracter especial")]
        public string? Password { get; set; }
        
        [Required(ErrorMessage = "Este campo no puede estar vacio")]
        public string? Sexo { get; set; }
        
        [Required(ErrorMessage = "Selecciona una opción")]
        [MaxLength(15)]
        [RegularExpression(@"^[0-9]*$", ErrorMessage = "No se permiten letras")]
        public string? Telefono { get; set; }
        
        [Required(ErrorMessage = "Este campo no puede estar vacio")]
        [MaxLength(15)]
        [RegularExpression(@"^[0-9]*$", ErrorMessage = "No se permiten letras")]
        public string? Celular { get; set; }
        
        [Required(ErrorMessage = "Este campo no puede estar vacio")]
        [RegularExpression(@"^(0[1-9]|[12][0-9]|3[01])[/](0[1-9]|1[0-2])[/](19|20)\d{2}$", ErrorMessage = "Ingresa una Fecha Valida dd/mm/yyyy")]
        public string? FechaNacimiento { get; set; }
        
        [Required(ErrorMessage = "Este campo no puede estar vacio")]
        [RegularExpression(@"^([A-Z][AEIOUX][A-Z]{2}\d{2}(?:0[1-9]|1[0-2])(?:0[1-9]|[12]\d|3[01])[HM](?:AS|B[CS]|C[CLMSH]|D[FG]|G[TR]|HG|JC|M[CNS]|N[ETL]|OC|PL|Q[TR]|S[PLR]|T[CSL]|VZ|YN|ZS)[B-DF-HJ-NP-TV-Z]{3}[A-Z\d])(\d)$", ErrorMessage = "Ingresa una CURP valida")]
        public string? Curp { get; set; }
        public List<object>? Usuarios { get; set; }
        public List<object>? Errores { get; set; }
        public List<object>? Correctos { get; set; }
        
        [Required(ErrorMessage = "Selecciona una opción")]
        public ML.Rol? Rol { get; set; }


        //public ML.ImagenUsuario ImagenUsuario { get; set; }
        //public ML.Direccion? Direccion { get; set; }
    }
}
