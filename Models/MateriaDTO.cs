namespace ApiEstudiantes.Models
{
    public class MateriaDTO
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public int Creditos { get; set; }
        public int IdProfesor { get; set; }
        public string? ProfesorNombre { get; set; }
        public bool Estado { get; set; }
        public DateTime? FechaRegistro { get; set; }
    }
}
