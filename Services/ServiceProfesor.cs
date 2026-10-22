using System.Data;
using Microsoft.Data.SqlClient;
using Dapper;
using ApiEstudiantes.Data;
using ApiEstudiantes.Interfaces;
using ApiEstudiantes.Models;
using ApiEstudiantes.Resources;

namespace ApiEstudiantes.Services
{
    public class ServiceProfesor(DataConnectionContext context, IResponseService responseService) : IServiceProfesor
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
                        id AS Id,
                        identificacion AS Identificacion,
                        nombres AS Nombres,
                        apellidos AS Apellidos,
                        estado AS Estado,
                        fecharegistro AS FechaRegistro,
                        fechamodifica AS FechaModifica
                    FROM profesores
                    WHERE id = @Id;";

                var param = new DynamicParameters();
                param.Add("@Id", id, DbType.Int32, ParameterDirection.Input);

                var resultado = await connection.QueryFirstOrDefaultAsync<ProfesorModel>(query, param);

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

        public async Task<IResponseService> GetByIdentificacion(string identificacion)
        {
            try
            {
                using var connection = _context.CreateConnection();
                const string query = @"
                    SELECT 
                        id AS Id,
                        identificacion AS Identificacion,
                        nombres AS Nombres,
                        apellidos AS Apellidos,
                        estado AS Estado,
                        fecharegistro AS FechaRegistro,
                        fechamodifica AS FechaModifica
                    FROM profesores
                    WHERE identificacion = @Identificacion;";

                var param = new DynamicParameters();
                param.Add("@Identificacion", identificacion, DbType.String, ParameterDirection.Input, 50);

                var resultado = await connection.QueryFirstOrDefaultAsync<ProfesorModel>(query, param);

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
                        id AS Id,
                        identificacion AS Identificacion,
                        nombres AS Nombres,
                        apellidos AS Apellidos,
                        estado AS Estado,
                        fecharegistro AS FechaRegistro,
                        fechamodifica AS FechaModifica
                    FROM profesores
                    ORDER BY id DESC;";

                var resultado = (await connection.QueryAsync<ProfesorModel>(query)).ToList();

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

        public async Task<IResponseService> Add(ProfesorModel model)
        {
            try
            {
                using var connection = _context.CreateConnection();

                const string checkQuery = "SELECT COUNT(1) FROM profesores WHERE identificacion = @Identificacion;";
                int exists = await connection.ExecuteScalarAsync<int>(checkQuery, new { model.Identificacion });

                if (exists > 0)
                {
                    _responseService.Estado = false;
                    _responseService.Error = $"Ya existe un profesor con la identificación '{model.Identificacion}'.";
                    return _responseService;
                }

                const string insertQuery = @"
                    INSERT INTO profesores (
                        identificacion,
                        nombres,
                        apellidos,
                        estado,
                        fecharegistro
                    )
                    VALUES (
                        @Identificacion,
                        @Nombres,
                        @Apellidos,
                        @Estado,
                        GETDATE()
                    );
                    SELECT CAST(SCOPE_IDENTITY() AS INT);";

                var param = new DynamicParameters();
                param.Add("@Identificacion", model.Identificacion, DbType.String, ParameterDirection.Input, 50);
                param.Add("@Nombres", model.Nombres, DbType.String, ParameterDirection.Input, 100);
                param.Add("@Apellidos", model.Apellidos, DbType.String, ParameterDirection.Input, 100);
                param.Add("@Estado", model.Estado, DbType.Boolean, ParameterDirection.Input);

                int newId = await connection.ExecuteScalarAsync<int>(insertQuery, param);
                model.Id = newId;

                _responseService.EstablecerRespuesta(true, model);
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

        public async Task<IResponseService> Edit(int id, ProfesorModel model)
        {
            try
            {
                using var connection = _context.CreateConnection();

                const string checkQuery = "SELECT COUNT(1) FROM profesores WHERE identificacion = @Identificacion AND id <> @Id;";
                int exists = await connection.ExecuteScalarAsync<int>(checkQuery, new { model.Identificacion, Id = id });

                if (exists > 0)
                {
                    _responseService.Estado = false;
                    _responseService.Error = $"Ya existe otro profesor con la identificación '{model.Identificacion}'.";
                    return _responseService;
                }

                const string updateQuery = @"
                    UPDATE profesores
                    SET identificacion = @Identificacion,
                        nombres = @Nombres,
                        apellidos = @Apellidos,
                        estado = @Estado,
                        fechamodifica = GETDATE()
                    WHERE id = @Id;";

                var param = new DynamicParameters();
                param.Add("@Id", id, DbType.Int32, ParameterDirection.Input);
                param.Add("@Identificacion", model.Identificacion, DbType.String, ParameterDirection.Input, 50);
                param.Add("@Nombres", model.Nombres, DbType.String, ParameterDirection.Input, 100);
                param.Add("@Apellidos", model.Apellidos, DbType.String, ParameterDirection.Input, 100);
                param.Add("@Estado", model.Estado, DbType.Boolean, ParameterDirection.Input);

                int rowsAffected = await connection.ExecuteAsync(updateQuery, param);

                if (rowsAffected > 0)
                {
                    model.Id = id;
                    _responseService.EstablecerRespuesta(true, model);
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

                // Verificar si tiene materias asignadas
                const string checkMateriasQuery = "SELECT COUNT(1) FROM materia WHERE idProfesor = @Id;";
                int materiasCount = await connection.ExecuteScalarAsync<int>(checkMateriasQuery, new { Id = id });

                if (materiasCount > 0)
                {
                    _responseService.Estado = false;
                    _responseService.Error = $"No se puede eliminar el profesor porque tiene {materiasCount} materia(s) asignada(s).";
                    return _responseService;
                }

                const string deleteQuery = "DELETE FROM profesores WHERE id = @Id;";

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
                    UPDATE profesores
                    SET estado = CASE WHEN estado = 1 THEN 0 ELSE 1 END,
                        fechamodifica = GETDATE()
                    WHERE id = @Id;
                    SELECT estado FROM profesores WHERE id = @Id;";

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
