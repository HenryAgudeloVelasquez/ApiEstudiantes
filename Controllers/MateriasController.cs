using Microsoft.AspNetCore.Mvc;
using ApiEstudiantes.Interfaces;
using ApiEstudiantes.Models;
using ApiEstudiantes.Resources;

namespace ApiEstudiantes.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class MateriasController(IServiceMateria serviceMateria, IResponseService responseService) : ControllerBase
    {
        private readonly IServiceMateria _serviceMateria = serviceMateria;
        private readonly IResponseService _responseService = responseService;

        #region SERVICIOS GET

        [HttpGet]
        public async Task<ActionResult> GetListAll()
        {
            var response = await _serviceMateria.GetListAll();
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
            var response = await _serviceMateria.Get(id);
            if (response.Estado && response.Resultado is MateriaDTO dto)
            {
                return Ok(dto);
            }

            return NotFound(new { message = response.Error ?? Messages.RecordNotFound });
        }

        [HttpGet("{idProfesor}")]
        public async Task<ActionResult> GetByProfesor(int idProfesor)
        {
            var response = await _serviceMateria.GetByProfesor(idProfesor);
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

        #region SERVICIOS POST - PUT - DELETE

        [HttpPost]
        public async Task<ActionResult<MateriaDTO>> Add([FromBody] MateriaModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var response = await _serviceMateria.Add(model);
            if (response.Estado && response.Resultado is MateriaDTO createdDto)
            {
                return CreatedAtAction(nameof(Get), new { id = createdDto.Id }, createdDto);
            }

            return BadRequest(new { message = response.Error });
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<MateriaDTO>> Edit(int id, [FromBody] MateriaModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var response = await _serviceMateria.Edit(id, model);
            if (response.Estado && response.Resultado is MateriaDTO updatedDto)
            {
                return Ok(updatedDto);
            }

            return BadRequest(new { message = response.Error });
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var response = await _serviceMateria.Delete(id);
            if (response.Estado)
            {
                return Ok(response.Resultado);
            }

            return BadRequest(new { message = response.Error });
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult> ToggleEstado(int id)
        {
            var response = await _serviceMateria.ToggleEstado(id);
            if (response.Estado)
            {
                return Ok(response.Resultado);
            }

            return BadRequest(new { message = response.Error });
        }

        #endregion
    }
}
