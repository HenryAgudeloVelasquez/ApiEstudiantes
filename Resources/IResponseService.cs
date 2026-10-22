namespace ApiEstudiantes.Resources
{
    public interface IResponseService
    {
        bool Estado { get; set; }
        object? Resultado { get; set; }
        string? Error { get; set; }
        int? TotalItems { get; set; }

        void EstablecerRespuesta(bool estado, object? resultado, int? totalItems = null);
        void EstablecerRespuestaLista(bool estado, object? resultado, int? totalItems = null);
    }
}
