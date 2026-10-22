using ApiEstudiantes.Models;
using ApiEstudiantes.Resources;

namespace ApiEstudiantes.Interfaces
{
    public interface IServiceProfesor
    {
        Task<IResponseService> Get(int id);
        Task<IResponseService> GetByIdentificacion(string identificacion);
        Task<IResponseService> GetListAll();
        Task<IResponseService> Add(ProfesorModel model);
        Task<IResponseService> Edit(int id, ProfesorModel model);
        Task<IResponseService> Delete(int id);
        Task<IResponseService> ToggleEstado(int id);
    }
}
