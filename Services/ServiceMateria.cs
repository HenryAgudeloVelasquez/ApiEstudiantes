using System.Data;
using Microsoft.Data.SqlClient;
using Dapper;
using ApiEstudiantes.Data;
using ApiEstudiantes.Interfaces;
using ApiEstudiantes.Models;
using ApiEstudiantes.Resources;

namespace ApiEstudiantes.Services
{
    public class ServiceMateria(DataConnectionContext context, IResponseService responseService) : IServiceMateria
    {
        private readonly DataConnectionContext _context = context;
        private readonly IResponseService _responseService = responseService;

        #region SERVICIOS GET

        public async Task<IResponseService> Get(int id)
        {
            try
            {
                using var connection = _context.CreateConnection();
                const string query = @"
                    SELECT 
                        m.id AS Id,
                        m.nombre AS Nombre,
                        m.creditos AS Creditos,
                        m.idProfesor AS IdProfesor,
                        CONCAT(p.nombres, ' ', p.apellidos) AS ProfesorNombre,
                        m.estado AS Estado,
                        m.fecharegistro AS FechaRegistro
                    FROM materia m
                    INNER JOIN profesores p ON m.idProfesor = p.id
                    WHERE m.id = @Id;";

                var param = new DynamicParameters();
                param.Add("@Id", id, DbType.Int32, ParameterDirection.Input);

                var resultado = await connection.QueryFirstOrDefaultAsync<MateriaDTO>(query, param);

                if (resultado != null)
                {
                    _responseService.EstablecerRespuesta(true, resultado);
                }
                else
                {
                    _responseService.EstablecerRespuesta(false, null);
                    _responseService.Error = Messages.RecordNotFound;
                }
            }
            catch (SqlException ex)
            {
                _responseService.Estado = false;
                _responseService.Error = $"Error de base de datos: {ex.Message}";
            }
            catch (Exception ex)
            {
                _responseService.Estado = false;
                _responseService.Error = ex.Message;
            }

            return _responseService;
        }

        public async Task<IResponseService> GetListAll()
        {
            try
            {
                using var connection = _context.CreateConnection();
                const string query = @"
                    SELECT 
                        m.id AS Id,
                        m.nombre AS Nombre,
                        m.creditos AS Creditos,
                        m.idProfesor AS IdProfesor,
                        CONCAT(p.nombres, ' ', p.apellidos) AS ProfesorNombre,
                        m.estado AS Estado,
                        m.fecharegistro AS FechaRegistro
                    FROM materia m
                    INNER JOIN profesores p ON m.idProfesor = p.id
                    ORDER BY m.id DESC;";

                var resultado = (await connection.QueryAsync<MateriaDTO>(query)).ToList();

                _responseService.EstablecerRespuestaLista(true, resultado, resultado.Count);
            }
            catch (SqlException ex)
            {
                _responseService.Estado = false;
                _responseService.Error = $"Error de base de datos: {ex.Message}";
            }
            catch (Exception ex)
            {
                _responseService.Estado = false;
                _responseService.Error = ex.Message;
            }

            return _responseService;
        }

        public async Task<IResponseService> GetByProfesor(int idProfesor)
        {
            try
            {
                using var connection = _context.CreateConnection();
                const string query = @"
                    SELECT 
                        m.id AS Id,
                        m.nombre AS Nombre,
                        m.creditos AS Creditos,
                        m.idProfesor AS IdProfesor,
                        CONCAT(p.nombres, ' ', p.apellidos) AS ProfesorNombre,
                        m.estado AS Estado,
                        m.fecharegistro AS FechaRegistro
                    FROM materia m
                    INNER JOIN profesores p ON m.idProfesor = p.id
                    WHERE m.idProfesor = @IdProfesor
                    ORDER BY m.id DESC;";

                var param = new DynamicParameters();
                param.Add("@IdProfesor", idProfesor, DbType.Int32, ParameterDirection.Input);

                var resultado = (await connection.QueryAsync<MateriaDTO>(query, param)).ToList();

                _responseService.EstablecerRespuestaLista(true, resultado, resultado.Count);
            }
            catch (SqlException ex)
            {
                _responseService.Estado = false;
                _responseService.Error = $"Error de base de datos: {ex.Message}";
            }
            catch (Exception ex)
            {
                _responseService.Estado = false;
                _responseService.Error = ex.Message;
            }

            return _responseService;
        }

        #endregion

        #region SERVICIOS POST - PUT - DELETE

        public async Task<IResponseService> Add(MateriaModel model)
        {
            try
            {
                using var connection = _context.CreateConnection();

                // Validar que el profesor exista
                const string checkProfesorQuery = "SELECT COUNT(1) FROM profesores WHERE id = @IdProfesor;";
                int existsProfesor = await connection.ExecuteScalarAsync<int>(checkProfesorQuery, new { model.IdProfesor });

                if (existsProfesor == 0)
                {
                    _responseService.Estado = false;
                    _responseService.Error = $"El profesor con ID {model.IdProfesor} no existe.";
                    return _responseService;
                }

                const string insertQuery = @"
                    INSERT INTO materia (
                        nombre,
                        creditos,
                        idProfesor,
                        estado,
                        fecharegistro
                    )
                    VALUES (
                        @Nombre,
                        @Creditos,
                        @IdProfesor,
                        @Estado,
                        GETDATE()
                    );
                    SELECT CAST(SCOPE_IDENTITY() AS INT);";

                var param = new DynamicParameters();
                param.Add("@Nombre", model.Nombre, DbType.String, ParameterDirection.Input, 100);
                param.Add("@Creditos", model.Creditos, DbType.Int32, ParameterDirection.Input);
                param.Add("@IdProfesor", model.IdProfesor, DbType.Int32, ParameterDirection.Input);
                param.Add("@Estado", model.Estado, DbType.Boolean, ParameterDirection.Input);

                int newId = await connection.ExecuteScalarAsync<int>(insertQuery, param);
                model.Id = newId;

                // Obtener datos del profesor para el DTO
                const string getProfesorQuery = "SELECT CONCAT(nombres, ' ', apellidos) FROM profesores WHERE id = @IdProfesor;";
                string? profesorNombre = await connection.ExecuteScalarAsync<string>(getProfesorQuery, new { model.IdProfesor });

                _responseService.EstablecerRespuesta(true, model.ConvertToDTO(profesorNombre));
            }
            catch (SqlException ex)
            {
                _responseService.Estado = false;
                _responseService.Error = $"Error de base de datos: {ex.Message}";
            }
            catch (Exception ex)
            {
                _responseService.Estado = false;
                _responseService.Error = ex.Message;
            }

            return _responseService;
        }

        public async Task<IResponseService> Edit(int id, MateriaModel model)
        {
            try
            {
                using var connection = _context.CreateConnection();

                // Validar que el profesor exista
                const string checkProfesorQuery = "SELECT COUNT(1) FROM profesores WHERE id = @IdProfesor;";
                int existsProfesor = await connection.ExecuteScalarAsync<int>(checkProfesorQuery, new { model.IdProfesor });

                if (existsProfesor == 0)
                {
                    _responseService.Estado = false;
                    _responseService.Error = $"El profesor con ID {model.IdProfesor} no existe.";
                    return _responseService;
                }

                const string updateQuery = @"
                    UPDATE materia
                    SET nombre = @Nombre,
                        creditos = @Creditos,
                        idProfesor = @IdProfesor,
                        estado = @Estado
                    WHERE id = @Id;";

                var param = new DynamicParameters();
                param.Add("@Id", id, DbType.Int32, ParameterDirection.Input);
                param.Add("@Nombre", model.Nombre, DbType.String, ParameterDirection.Input, 100);
                param.Add("@Creditos", model.Creditos, DbType.Int32, ParameterDirection.Input);
                param.Add("@IdProfesor", model.IdProfesor, DbType.Int32, ParameterDirection.Input);
                param.Add("@Estado", model.Estado, DbType.Boolean, ParameterDirection.Input);

                int rowsAffected = await connection.ExecuteAsync(updateQuery, param);

                if (rowsAffected > 0)
                {
                    model.Id = id;
                    const string getProfesorQuery = "SELECT CONCAT(nombres, ' ', apellidos) FROM profesores WHERE id = @IdProfesor;";
                    string? profesorNombre = await connection.ExecuteScalarAsync<string>(getProfesorQuery, new { model.IdProfesor });

                    _responseService.EstablecerRespuesta(true, model.ConvertToDTO(profesorNombre));
                }
                else
                {
                    _responseService.Estado = false;
                    _responseService.Error = Messages.RecordNotFound;
                }
            }
            catch (SqlException ex)
            {
                _responseService.Estado = false;
                _responseService.Error = $"Error de base de datos: {ex.Message}";
            }
            catch (Exception ex)
            {
                _responseService.Estado = false;
                _responseService.Error = ex.Message;
            }

            return _responseService;
        }

        public async Task<IResponseService> Delete(int id)
        {
            try
            {
                using var connection = _context.CreateConnection();
                const string deleteQuery = "DELETE FROM materia WHERE id = @Id;";

                var param = new DynamicParameters();
                param.Add("@Id", id, DbType.Int32, ParameterDirection.Input);

                int rowsAffected = await connection.ExecuteAsync(deleteQuery, param);

                if (rowsAffected > 0)
                {
                    _responseService.EstablecerRespuesta(true, new { id, mensaje = "Registro eliminado con éxito." });
                }
                else
                {
                    _responseService.Estado = false;
                    _responseService.Error = Messages.RecordNotFound;
                }
            }
            catch (SqlException ex)
            {
                _responseService.Estado = false;
                _responseService.Error = $"Error de base de datos: {ex.Message}";
            }
            catch (Exception ex)
            {
                _responseService.Estado = false;
                _responseService.Error = ex.Message;
            }

            return _responseService;
        }

        public async Task<IResponseService> ToggleEstado(int id)
        {
            try
            {
                using var connection = _context.CreateConnection();
                const string toggleQuery = @"
                    UPDATE materia
                    SET estado = CASE WHEN estado = 1 THEN 0 ELSE 1 END
                    WHERE id = @Id;
                    SELECT estado FROM materia WHERE id = @Id;";

                var param = new DynamicParameters();
                param.Add("@Id", id, DbType.Int32, ParameterDirection.Input);

                var nuevoEstado = await connection.ExecuteScalarAsync<bool?>(toggleQuery, param);

                if (nuevoEstado.HasValue)
                {
                    _responseService.EstablecerRespuesta(true, new { id, estado = nuevoEstado.Value });
                }
                else
                {
                    _responseService.Estado = false;
                    _responseService.Error = Messages.RecordNotFound;
                }
            }
            catch (SqlException ex)
            {
                _responseService.Estado = false;
                _responseService.Error = $"Error de base de datos: {ex.Message}";
            }
            catch (Exception ex)
            {
                _responseService.Estado = false;
                _responseService.Error = ex.Message;
            }

            return _responseService;
        }

        #endregion
    }
}
