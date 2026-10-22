using System.Collections;

namespace ApiEstudiantes.Resources
{
    public class ResponseService : IResponseService
    {
        public bool Estado { get; set; } = false;
        public object? Resultado { get; set; }
        public string? Error { get; set; }
        public int? TotalItems { get; set; }

        public void EstablecerRespuesta(bool estado, object? resultado, int? totalItems = null)
        {
            Estado = estado;
            Resultado = resultado;
            TotalItems = totalItems;
            Error = estado ? null : Error;
        }

        public void EstablecerRespuestaLista(bool estado, object? resultado, int? totalItems = null)
        {
            Estado = estado;
            Resultado = resultado;
            if (resultado is ICollection collection)
            {
                TotalItems = totalItems ?? collection.Count;
            }
            else
            {
                TotalItems = totalItems;
            }
            Error = estado ? null : Error;
        }
    }
}
