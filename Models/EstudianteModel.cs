using System.ComponentModel.DataAnnotations;

namespace ApiEstudiantes.Models
{
    public class EstudianteModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El tipo de documento es obligatorio.")]
        [StringLength(20, ErrorMessage = "El tipo de documento no puede superar los 20 caracteres.")]
        public string TipoDocumento { get; set; } = string.Empty;

        [Required(ErrorMessage = "La identificación es obligatoria.")]
        [StringLength(50, ErrorMessage = "La identificación no puede superar los 50 caracteres.")]
        public string Identificacion { get; set; } = string.Empty;

        [Required(ErrorMessage = "Los nombres son obligatorios.")]
        [StringLength(100, ErrorMessage = "Los nombres no pueden superar los 100 caracteres.")]
        public string Nombres { get; set; } = string.Empty;

        [Required(ErrorMessage = "Los apellidos son obligatorios.")]
        [StringLength(100, ErrorMessage = "Los apellidos no pueden superar los 100 caracteres.")]
        public string Apellidos { get; set; } = string.Empty;

        [Required(ErrorMessage = "El correo electrónico es obligatorio.")]
        [EmailAddress(ErrorMessage = "El formato del correo electrónico no es válido.")]
        [StringLength(150, ErrorMessage = "El correo no puede superar los 150 caracteres.")]
        public string CorreoElectronico { get; set; } = string.Empty;

        [Required(ErrorMessage = "La contraseña es obligatoria.")]
        [StringLength(255, ErrorMessage = "La contraseña no puede superar los 255 caracteres.")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "La fecha de nacimiento es obligatoria.")]
        public DateTime FechaNacimiento { get; set; }

        [Required(ErrorMessage = "La ciudad de residencia es obligatoria.")]
        [StringLength(100, ErrorMessage = "La ciudad de residencia no puede superar los 100 caracteres.")]
        public string CiudadResidencia { get; set; } = string.Empty;

        public bool Estado { get; set; } = true;

        public DateTime? FechaRegistro { get; set; }

        public DateTime? FechaModifica { get; set; }

        public EstudianteDTO ConvertToDTO()
        {
            return new EstudianteDTO
            {
                Id = this.Id,
                TipoDocumento = this.TipoDocumento,
                Identificacion = this.Identificacion,
                Nombres = this.Nombres,
                Apellidos = this.Apellidos,
                NombreCompleto = $"{this.Nombres} {this.Apellidos}".Trim(),
                CorreoElectronico = this.CorreoElectronico,
                FechaNacimiento = this.FechaNacimiento,
                CiudadResidencia = this.CiudadResidencia,
                Estado = this.Estado,
                FechaRegistro = this.FechaRegistro,
                FechaModifica = this.FechaModifica
            };
        }
    }
}
