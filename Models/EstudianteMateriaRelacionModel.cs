using System.ComponentModel.DataAnnotations;

namespace ApiEstudiantes.Models
{
    public class EstudianteMateriaRelacionModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El ID del estudiante es obligatorio.")]
        [Range(1, int.MaxValue, ErrorMessage = "Debe proporcionar un ID de estudiante válido.")]
        public int IdEstudiante { get; set; }

        [Required(ErrorMessage = "El ID de la materia es obligatorio.")]
        [Range(1, int.MaxValue, ErrorMessage = "Debe proporcionar un ID de materia válido.")]
        public int IdMateria { get; set; }

        public DateTime? FechaRegistro { get; set; }

        public EstudianteMateriaRelacionDTO ConvertToDTO(
            string? estudianteNombre = null,
            string? estudianteIdentificacion = null,
            string? estudianteCorreo = null,
            string? materiaNombre = null,
            int? materiaCreditos = null,
            string? profesorNombre = null)
        {
            return new EstudianteMateriaRelacionDTO
            {
                Id = this.Id,
                IdEstudiante = this.IdEstudiante,
                EstudianteNombre = estudianteNombre,
                EstudianteIdentificacion = estudianteIdentificacion,
                EstudianteCorreo = estudianteCorreo,
                IdMateria = this.IdMateria,
                MateriaNombre = materiaNombre,
                MateriaCreditos = materiaCreditos,
                ProfesorNombre = profesorNombre,
                FechaRegistro = this.FechaRegistro
            };
        }
    }
}
