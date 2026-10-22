namespace ApiEstudiantes.Models
{
    public class EstudianteDTO
    {
        public int Id { get; set; }
        public string TipoDocumento { get; set; } = string.Empty;
        public string Identificacion { get; set; } = string.Empty;
        public string Nombres { get; set; } = string.Empty;
        public string Apellidos { get; set; } = string.Empty;
        public string NombreCompleto { get; set; } = string.Empty;
        public string CorreoElectronico { get; set; } = string.Empty;
        public DateTime FechaNacimiento { get; set; }
        public string CiudadResidencia { get; set; } = string.Empty;
        public bool Estado { get; set; }
        public DateTime? FechaRegistro { get; set; }
        public DateTime? FechaModifica { get; set; }
    }
}
