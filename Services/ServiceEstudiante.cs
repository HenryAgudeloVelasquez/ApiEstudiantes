using System.Data;
using Microsoft.Data.SqlClient;
using Dapper;
using ApiEstudiantes.Data;
using ApiEstudiantes.Interfaces;
using ApiEstudiantes.Models;
using ApiEstudiantes.Resources;

namespace ApiEstudiantes.Services
{
    public class ServiceEstudiante(DataConnectionContext context, IResponseService responseService) : IServiceEstudiante
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
                        tipo_documento AS TipoDocumento,
                        identificacion AS Identificacion,
                        nombres AS Nombres,
                        apellidos AS Apellidos,
                        correo_electronico AS CorreoElectronico,
                        password AS Password,
                        fecha_nacimiento AS FechaNacimiento,
                        ciudad_residencia AS CiudadResidencia,
                        estado AS Estado,
                        fecharegistro AS FechaRegistro,
                        fechamodifica AS FechaModifica
                    FROM estudiantes
                    WHERE id = @Id;";

                var param = new DynamicParameters();
                param.Add("@Id", id, DbType.Int32, ParameterDirection.Input);

                var resultado = await connection.QueryFirstOrDefaultAsync<EstudianteModel>(query, param);

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
                        tipo_documento AS TipoDocumento,
                        identificacion AS Identificacion,
                        nombres AS Nombres,
                        apellidos AS Apellidos,
                        correo_electronico AS CorreoElectronico,
                        password AS Password,
                        fecha_nacimiento AS FechaNacimiento,
                        ciudad_residencia AS CiudadResidencia,
                        estado AS Estado,
                        fecharegistro AS FechaRegistro,
                        fechamodifica AS FechaModifica
                    FROM estudiantes
                    WHERE identificacion = @Identificacion;";

                var param = new DynamicParameters();
                param.Add("@Identificacion", identificacion, DbType.String, ParameterDirection.Input, 50);

                var resultado = await connection.QueryFirstOrDefaultAsync<EstudianteModel>(query, param);

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

        public async Task<IResponseService> GetByCorreo(string correo)
        {
            try
            {
                using var connection = _context.CreateConnection();
                const string query = @"
                    SELECT 
                        id AS Id,
                        tipo_documento AS TipoDocumento,
                        identificacion AS Identificacion,
                        nombres AS Nombres,
                        apellidos AS Apellidos,
                        correo_electronico AS CorreoElectronico,
                        password AS Password,
                        fecha_nacimiento AS FechaNacimiento,
                        ciudad_residencia AS CiudadResidencia,
                        estado AS Estado,
                        fecharegistro AS FechaRegistro,
                        fechamodifica AS FechaModifica
                    FROM estudiantes
                    WHERE correo_electronico = @Correo;";

                var param = new DynamicParameters();
                param.Add("@Correo", correo, DbType.String, ParameterDirection.Input, 150);

                var resultado = await connection.QueryFirstOrDefaultAsync<EstudianteModel>(query, param);

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
                        tipo_documento AS TipoDocumento,
                        identificacion AS Identificacion,
                        nombres AS Nombres,
                        apellidos AS Apellidos,
                        correo_electronico AS CorreoElectronico,
                        password AS Password,
                        fecha_nacimiento AS FechaNacimiento,
                        ciudad_residencia AS CiudadResidencia,
                        estado AS Estado,
                        fecharegistro AS FechaRegistro,
                        fechamodifica AS FechaModifica
                    FROM estudiantes
                    ORDER BY id DESC;";

                var resultado = (await connection.QueryAsync<EstudianteModel>(query)).ToList();

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

        public async Task<IResponseService> Add(EstudianteModel model)
        {
            try
            {
                using var connection = _context.CreateConnection();

                const string checkIdentificacionQuery = "SELECT COUNT(1) FROM estudiantes WHERE identificacion = @Identificacion;";
                int existsIdentificacion = await connection.ExecuteScalarAsync<int>(checkIdentificacionQuery, new { model.Identificacion });

                if (existsIdentificacion > 0)
                {
                    _responseService.Estado = false;
                    _responseService.Error = $"Ya existe un estudiante con la identificación '{model.Identificacion}'.";
                    return _responseService;
                }

                const string checkCorreoQuery = "SELECT COUNT(1) FROM estudiantes WHERE correo_electronico = @CorreoElectronico;";
                int existsCorreo = await connection.ExecuteScalarAsync<int>(checkCorreoQuery, new { model.CorreoElectronico });

                if (existsCorreo > 0)
                {
                    _responseService.Estado = false;
                    _responseService.Error = $"Ya existe un estudiante con el correo '{model.CorreoElectronico}'.";
                    return _responseService;
                }

                const string insertQuery = @"
                    INSERT INTO estudiantes (
                        tipo_documento,
                        identificacion,
                        nombres,
                        apellidos,
                        correo_electronico,
                        password,
                        fecha_nacimiento,
                        ciudad_residencia,
                        estado,
                        fecharegistro
                    )
                    VALUES (
                        @TipoDocumento,
                        @Identificacion,
                        @Nombres,
                        @Apellidos,
                        @CorreoElectronico,
                        @Password,
                        @FechaNacimiento,
                        @CiudadResidencia,
                        @Estado,
                        GETDATE()
                    );
                    SELECT CAST(SCOPE_IDENTITY() AS INT);";

                var param = new DynamicParameters();
                param.Add("@TipoDocumento", model.TipoDocumento, DbType.String, ParameterDirection.Input, 20);
                param.Add("@Identificacion", model.Identificacion, DbType.String, ParameterDirection.Input, 50);
                param.Add("@Nombres", model.Nombres, DbType.String, ParameterDirection.Input, 100);
                param.Add("@Apellidos", model.Apellidos, DbType.String, ParameterDirection.Input, 100);
                param.Add("@CorreoElectronico", model.CorreoElectronico, DbType.String, ParameterDirection.Input, 150);
                param.Add("@Password", model.Password, DbType.String, ParameterDirection.Input, 255);
                param.Add("@FechaNacimiento", model.FechaNacimiento, DbType.Date, ParameterDirection.Input);
                param.Add("@CiudadResidencia", model.CiudadResidencia, DbType.String, ParameterDirection.Input, 100);
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

        public async Task<IResponseService> Edit(int id, EstudianteModel model)
        {
            try
            {
                using var connection = _context.CreateConnection();

                const string checkIdentificacionQuery = "SELECT COUNT(1) FROM estudiantes WHERE identificacion = @Identificacion AND id <> @Id;";
                int existsIdentificacion = await connection.ExecuteScalarAsync<int>(checkIdentificacionQuery, new { model.Identificacion, Id = id });

                if (existsIdentificacion > 0)
                {
                    _responseService.Estado = false;
                    _responseService.Error = $"Ya existe otro estudiante con la identificación '{model.Identificacion}'.";
                    return _responseService;
                }

                const string checkCorreoQuery = "SELECT COUNT(1) FROM estudiantes WHERE correo_electronico = @CorreoElectronico AND id <> @Id;";
                int existsCorreo = await connection.ExecuteScalarAsync<int>(checkCorreoQuery, new { model.CorreoElectronico, Id = id });

                if (existsCorreo > 0)
                {
                    _responseService.Estado = false;
                    _responseService.Error = $"Ya existe otro estudiante con el correo '{model.CorreoElectronico}'.";
                    return _responseService;
                }

                // Si no se envía contraseña nueva, conservamos la actual
                string updateQuery;
                var param = new DynamicParameters();
                param.Add("@Id", id, DbType.Int32, ParameterDirection.Input);
                param.Add("@TipoDocumento", model.TipoDocumento, DbType.String, ParameterDirection.Input, 20);
                param.Add("@Identificacion", model.Identificacion, DbType.String, ParameterDirection.Input, 50);
                param.Add("@Nombres", model.Nombres, DbType.String, ParameterDirection.Input, 100);
                param.Add("@Apellidos", model.Apellidos, DbType.String, ParameterDirection.Input, 100);
                param.Add("@CorreoElectronico", model.CorreoElectronico, DbType.String, ParameterDirection.Input, 150);
                param.Add("@FechaNacimiento", model.FechaNacimiento, DbType.Date, ParameterDirection.Input);
                param.Add("@CiudadResidencia", model.CiudadResidencia, DbType.String, ParameterDirection.Input, 100);
                param.Add("@Estado", model.Estado, DbType.Boolean, ParameterDirection.Input);

                if (!string.IsNullOrWhiteSpace(model.Password))
                {
                    updateQuery = @"
                        UPDATE estudiantes
                        SET tipo_documento = @TipoDocumento,
                            identificacion = @Identificacion,
                            nombres = @Nombres,
                            apellidos = @Apellidos,
                            correo_electronico = @CorreoElectronico,
                            password = @Password,
                            fecha_nacimiento = @FechaNacimiento,
                            ciudad_residencia = @CiudadResidencia,
                            estado = @Estado,
                            fechamodifica = GETDATE()
                        WHERE id = @Id;";
                    param.Add("@Password", model.Password, DbType.String, ParameterDirection.Input, 255);
                }
                else
                {
                    updateQuery = @"
                        UPDATE estudiantes
                        SET tipo_documento = @TipoDocumento,
                            identificacion = @Identificacion,
                            nombres = @Nombres,
                            apellidos = @Apellidos,
                            correo_electronico = @CorreoElectronico,
                            fecha_nacimiento = @FechaNacimiento,
                            ciudad_residencia = @CiudadResidencia,
                            estado = @Estado,
                            fechamodifica = GETDATE()
                        WHERE id = @Id;";
                }

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
                const string deleteQuery = "DELETE FROM estudiantes WHERE id = @Id;";

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
                    UPDATE estudiantes
                    SET estado = CASE WHEN estado = 1 THEN 0 ELSE 1 END,
                        fechamodifica = GETDATE()
                    WHERE id = @Id;
                    SELECT estado FROM estudiantes WHERE id = @Id;";

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
