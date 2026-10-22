using ApiEstudiantes.Models;
using ApiEstudiantes.Resources;

namespace ApiEstudiantes.Interfaces
{
    public interface IServiceEstudianteMateriaRelacion
    {
        Task<IResponseService> Get(int id);
        Task<IResponseService> GetListAll();
        Task<IResponseService> GetByEstudiante(int idEstudiante);
        Task<IResponseService> GetByMateria(int idMateria);
        Task<IResponseService> Add(EstudianteMateriaRelacionModel model);
        Task<IResponseService> Delete(int id);
    }
}
