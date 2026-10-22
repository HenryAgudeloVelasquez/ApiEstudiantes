using Microsoft.AspNetCore.Mvc;
using ApiEstudiantes.Interfaces;
using ApiEstudiantes.Models;
using ApiEstudiantes.Resources;

namespace ApiEstudiantes.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class InscripcionesController(IServiceEstudianteMateriaRelacion serviceInscripcion, IResponseService responseService) : ControllerBase
    {
        private readonly IServiceEstudianteMateriaRelacion _serviceInscripcion = serviceInscripcion;
        private readonly IResponseService _responseService = responseService;

        #region SERVICIOS GET

        [HttpGet]
        public async Task<ActionResult> GetListAll()
        {
            var response = await _serviceInscripcion.GetListAll();
            if (response.Estado)
            {
                return Ok(new
                {
                    data = response.Resultado,
                    totalItems = response.TotalItems
                });
            }

            return BadRequest(new { message = response.Error });
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> Get(int id)
        {
            var response = await _serviceInscripcion.Get(id);
            if (response.Estado && response.Resultado is EstudianteMateriaRelacionDTO dto)
            {
                return Ok(dto);
            }

            return NotFound(new { message = response.Error ?? Messages.RecordNotFound });
        }

        [HttpGet("{idEstudiante}")]
        public async Task<ActionResult> GetByEstudiante(int idEstudiante)
        {
            var response = await _serviceInscripcion.GetByEstudiante(idEstudiante);
            if (response.Estado)
            {
                return Ok(new
                {
                    data = response.Resultado,
                    totalItems = response.TotalItems
                });
            }

            return BadRequest(new { message = response.Error });
        }

        [HttpGet("{idMateria}")]
        public async Task<ActionResult> GetByMateria(int idMateria)
        {
            var response = await _serviceInscripcion.GetByMateria(idMateria);
            if (response.Estado)
            {
                return Ok(new
                {
                    data = response.Resultado,
                    totalItems = response.TotalItems
                });
            }

            return BadRequest(new { message = response.Error });
        }

        #endregion

        #region SERVICIOS POST - DELETE

        [HttpPost]
        public async Task<ActionResult<EstudianteMateriaRelacionDTO>> Add([FromBody] EstudianteMateriaRelacionModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var response = await _serviceInscripcion.Add(model);
            if (response.Estado && response.Resultado is EstudianteMateriaRelacionDTO createdDto)
            {
                return CreatedAtAction(nameof(Get), new { id = createdDto.Id }, createdDto);
            }

            return BadRequest(new { message = response.Error });
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var response = await _serviceInscripcion.Delete(id);
            if (response.Estado)
            {
                return Ok(response.Resultado);
            }

            return BadRequest(new { message = response.Error });
        }

        #endregion
    }
}
