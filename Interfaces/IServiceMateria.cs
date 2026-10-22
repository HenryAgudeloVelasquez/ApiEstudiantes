using ApiEstudiantes.Models;
using ApiEstudiantes.Resources;

namespace ApiEstudiantes.Interfaces
{
    public interface IServiceMateria
    {
        Task<IResponseService> Get(int id);
        Task<IResponseService> GetListAll();
        Task<IResponseService> GetByProfesor(int idProfesor);
        Task<IResponseService> Add(MateriaModel model);
        Task<IResponseService> Edit(int id, MateriaModel model);
        Task<IResponseService> Delete(int id);
        Task<IResponseService> ToggleEstado(int id);
    }
}
