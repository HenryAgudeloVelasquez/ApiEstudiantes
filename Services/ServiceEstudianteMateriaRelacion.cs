using System.Data;
using Microsoft.Data.SqlClient;
using Dapper;
using ApiEstudiantes.Data;
using ApiEstudiantes.Interfaces;
using ApiEstudiantes.Models;
using ApiEstudiantes.Resources;

namespace ApiEstudiantes.Services
{
    public class ServiceEstudianteMateriaRelacion(DataConnectionContext context, IResponseService responseService) : IServiceEstudianteMateriaRelacion
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
                        r.id AS Id,
                        r.idEstudiante AS IdEstudiante,
                        CONCAT(e.nombres, ' ', e.apellidos) AS EstudianteNombre,
                        e.identificacion AS EstudianteIdentificacion,
                        e.correo_electronico AS EstudianteCorreo,
                        r.idMateria AS IdMateria,
                        m.nombre AS MateriaNombre,
                        m.creditos AS MateriaCreditos,
                        CONCAT(p.nombres, ' ', p.apellidos) AS ProfesorNombre,
                        r.fecharegistro AS FechaRegistro
                    FROM estudiante_materia_relacion r
                    INNER JOIN estudiantes e ON r.idEstudiante = e.id
                    INNER JOIN materia m ON r.idMateria = m.id
                    INNER JOIN profesores p ON m.idProfesor = p.id
                    WHERE r.id = @Id;";

                var param = new DynamicParameters();
                param.Add("@Id", id, DbType.Int32, ParameterDirection.Input);

                var resultado = await connection.QueryFirstOrDefaultAsync<EstudianteMateriaRelacionDTO>(query, param);

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
                        r.id AS Id,
                        r.idEstudiante AS IdEstudiante,
                        CONCAT(e.nombres, ' ', e.apellidos) AS EstudianteNombre,
                        e.identificacion AS EstudianteIdentificacion,
                        e.correo_electronico AS EstudianteCorreo,
                        r.idMateria AS IdMateria,
                        m.nombre AS MateriaNombre,
                        m.creditos AS MateriaCreditos,
                        CONCAT(p.nombres, ' ', p.apellidos) AS ProfesorNombre,
                        r.fecharegistro AS FechaRegistro
                    FROM estudiante_materia_relacion r
                    INNER JOIN estudiantes e ON r.idEstudiante = e.id
                    INNER JOIN materia m ON r.idMateria = m.id
                    INNER JOIN profesores p ON m.idProfesor = p.id
                    ORDER BY r.id DESC;";

                var resultado = (await connection.QueryAsync<EstudianteMateriaRelacionDTO>(query)).ToList();

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

        public async Task<IResponseService> GetByEstudiante(int idEstudiante)
        {
            try
            {
                using var connection = _context.CreateConnection();
                const string query = @"
                    SELECT 
                        r.id AS Id,
                        r.idEstudiante AS IdEstudiante,
                        CONCAT(e.nombres, ' ', e.apellidos) AS EstudianteNombre,
                        e.identificacion AS EstudianteIdentificacion,
                        e.correo_electronico AS EstudianteCorreo,
                        r.idMateria AS IdMateria,
                        m.nombre AS MateriaNombre,
                        m.creditos AS MateriaCreditos,
                        CONCAT(p.nombres, ' ', p.apellidos) AS ProfesorNombre,
                        r.fecharegistro AS FechaRegistro
                    FROM estudiante_materia_relacion r
                    INNER JOIN estudiantes e ON r.idEstudiante = e.id
                    INNER JOIN materia m ON r.idMateria = m.id
                    INNER JOIN profesores p ON m.idProfesor = p.id
                    WHERE r.idEstudiante = @IdEstudiante
                    ORDER BY r.id DESC;";

                var param = new DynamicParameters();
                param.Add("@IdEstudiante", idEstudiante, DbType.Int32, ParameterDirection.Input);

                var resultado = (await connection.QueryAsync<EstudianteMateriaRelacionDTO>(query, param)).ToList();

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

        public async Task<IResponseService> GetByMateria(int idMateria)
        {
            try
            {
                using var connection = _context.CreateConnection();
                const string query = @"
                    SELECT 
                        r.id AS Id,
                        r.idEstudiante AS IdEstudiante,
                        CONCAT(e.nombres, ' ', e.apellidos) AS EstudianteNombre,
                        e.identificacion AS EstudianteIdentificacion,
                        e.correo_electronico AS EstudianteCorreo,
                        r.idMateria AS IdMateria,
                        m.nombre AS MateriaNombre,
                        m.creditos AS MateriaCreditos,
                        CONCAT(p.nombres, ' ', p.apellidos) AS ProfesorNombre,
                        r.fecharegistro AS FechaRegistro
                    FROM estudiante_materia_relacion r
                    INNER JOIN estudiantes e ON r.idEstudiante = e.id
                    INNER JOIN materia m ON r.idMateria = m.id
                    INNER JOIN profesores p ON m.idProfesor = p.id
                    WHERE r.idMateria = @IdMateria
                    ORDER BY r.id DESC;";

                var param = new DynamicParameters();
                param.Add("@IdMateria", idMateria, DbType.Int32, ParameterDirection.Input);

                var resultado = (await connection.QueryAsync<EstudianteMateriaRelacionDTO>(query, param)).ToList();

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

        #region SERVICIOS POST - DELETE

        public async Task<IResponseService> Add(EstudianteMateriaRelacionModel model)
        {
            try
            {
                using var connection = _context.CreateConnection();

                // 1. Validar que el estudiante exista
                const string checkEstudianteQuery = "SELECT COUNT(1) FROM estudiantes WHERE id = @IdEstudiante;";
                int existsEstudiante = await connection.ExecuteScalarAsync<int>(checkEstudianteQuery, new { model.IdEstudiante });

                if (existsEstudiante == 0)
                {
                    _responseService.Estado = false;
                    _responseService.Error = $"El estudiante con ID {model.IdEstudiante} no existe.";
                    return _responseService;
                }

                // 2. Validar que la materia exista
                const string checkMateriaQuery = "SELECT COUNT(1) FROM materia WHERE id = @IdMateria;";
                int existsMateria = await connection.ExecuteScalarAsync<int>(checkMateriaQuery, new { model.IdMateria });

                if (existsMateria == 0)
                {
                    _responseService.Estado = false;
                    _responseService.Error = $"La materia con ID {model.IdMateria} no existe.";
                    return _responseService;
                }

                // 3. Validar si ya está inscrito
                const string checkInscripcionQuery = @"
                    SELECT COUNT(1) 
                    FROM estudiante_materia_relacion 
                    WHERE idEstudiante = @IdEstudiante AND idMateria = @IdMateria;";
                int existsInscripcion = await connection.ExecuteScalarAsync<int>(checkInscripcionQuery, new { model.IdEstudiante, model.IdMateria });

                if (existsInscripcion > 0)
                {
                    _responseService.Estado = false;
                    _responseService.Error = "El estudiante ya se encuentra inscrito en esta materia.";
                    return _responseService;
                }

                const string insertQuery = @"
                    INSERT INTO estudiante_materia_relacion (
                        idEstudiante,
                        idMateria,
                        fecharegistro
                    )
                    VALUES (
                        @IdEstudiante,
                        @IdMateria,
                        GETDATE()
                    );
                    SELECT CAST(SCOPE_IDENTITY() AS INT);";

                var param = new DynamicParameters();
                param.Add("@IdEstudiante", model.IdEstudiante, DbType.Int32, ParameterDirection.Input);
                param.Add("@IdMateria", model.IdMateria, DbType.Int32, ParameterDirection.Input);

                int newId = await connection.ExecuteScalarAsync<int>(insertQuery, param);
                model.Id = newId;

                // Devolver el DTO completo con los datos relacionados
                var responseDTO = await Get(newId);
                return responseDTO;
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
                const string deleteQuery = "DELETE FROM estudiante_materia_relacion WHERE id = @Id;";

                var param = new DynamicParameters();
                param.Add("@Id", id, DbType.Int32, ParameterDirection.Input);

                int rowsAffected = await connection.ExecuteAsync(deleteQuery, param);

                if (rowsAffected > 0)
                {
                    _responseService.EstablecerRespuesta(true, new { id, mensaje = "Inscripción eliminada con éxito." });
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
