using ApiEstudiantes.Models;
using ApiEstudiantes.Resources;

namespace ApiEstudiantes.Interfaces
{
    public interface IServiceEstudiante
    {
        Task<IResponseService> Get(int id);
        Task<IResponseService> GetByIdentificacion(string identificacion);
        Task<IResponseService> GetByCorreo(string correo);
        Task<IResponseService> GetListAll();
        Task<IResponseService> Add(EstudianteModel model);
        Task<IResponseService> Edit(int id, EstudianteModel model);
        Task<IResponseService> Delete(int id);
        Task<IResponseService> ToggleEstado(int id);
    }
}
