using System.ComponentModel.DataAnnotations;

namespace ApiEstudiantes.Models
{
    public class MateriaModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre de la materia es obligatorio.")]
        [StringLength(100, ErrorMessage = "El nombre no puede superar los 100 caracteres.")]
        public string Nombre { get; set; } = string.Empty;

        [Range(1, 10, ErrorMessage = "Los créditos deben estar entre 1 y 10.")]
        public int Creditos { get; set; } = 3;

        [Required(ErrorMessage = "El ID del profesor es obligatorio.")]
        [Range(1, int.MaxValue, ErrorMessage = "Debe proporcionar un ID de profesor válido.")]
        public int IdProfesor { get; set; }

        public bool Estado { get; set; } = true;

        public DateTime? FechaRegistro { get; set; }

        public MateriaDTO ConvertToDTO(string? profesorNombre = null)
        {
            return new MateriaDTO
            {
                Id = this.Id,
                Nombre = this.Nombre,
                Creditos = this.Creditos,
                IdProfesor = this.IdProfesor,
                ProfesorNombre = profesorNombre,
                Estado = this.Estado,
                FechaRegistro = this.FechaRegistro
            };
        }
    }
}
