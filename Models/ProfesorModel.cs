using System.ComponentModel.DataAnnotations;

namespace ApiEstudiantes.Models
{
    public class ProfesorModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "La identificación es obligatoria.")]
        [StringLength(50, ErrorMessage = "La identificación no puede superar los 50 caracteres.")]
        public string Identificacion { get; set; } = string.Empty;

        [Required(ErrorMessage = "Los nombres son obligatorios.")]
        [StringLength(100, ErrorMessage = "Los nombres no pueden superar los 100 caracteres.")]
        public string Nombres { get; set; } = string.Empty;

        [Required(ErrorMessage = "Los apellidos son obligatorios.")]
        [StringLength(100, ErrorMessage = "Los apellidos no pueden superar los 100 caracteres.")]
        public string Apellidos { get; set; } = string.Empty;

        public bool Estado { get; set; } = true;

        public DateTime? FechaRegistro { get; set; }

        public DateTime? FechaModifica { get; set; }

        public ProfesorDTO ConvertToDTO()
        {
            return new ProfesorDTO
            {
                Id = this.Id,
                Identificacion = this.Identificacion,
                Nombres = this.Nombres,
                Apellidos = this.Apellidos,
                NombreCompleto = $"{this.Nombres} {this.Apellidos}".Trim(),
                Estado = this.Estado,
                FechaRegistro = this.FechaRegistro,
                FechaModifica = this.FechaModifica
            };
        }
    }
}
