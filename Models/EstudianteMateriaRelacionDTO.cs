namespace ApiEstudiantes.Models
{
    public class EstudianteMateriaRelacionDTO
    {
        public int Id { get; set; }
        public int IdEstudiante { get; set; }
        public string? EstudianteNombre { get; set; }
        public string? EstudianteIdentificacion { get; set; }
        public string? EstudianteCorreo { get; set; }
        public int IdMateria { get; set; }
        public string? MateriaNombre { get; set; }
        public int? MateriaCreditos { get; set; }
        public string? ProfesorNombre { get; set; }
        public DateTime? FechaRegistro { get; set; }
    }
}
