using Microsoft.AspNetCore.Mvc;
using ApiEstudiantes.Interfaces;
using ApiEstudiantes.Models;
using ApiEstudiantes.Resources;

namespace ApiEstudiantes.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ProfesoresController(IServiceProfesor serviceProfesor, IResponseService responseService) : ControllerBase
    {
        private readonly IServiceProfesor _serviceProfesor = serviceProfesor;
        private readonly IResponseService _responseService = responseService;

        #region SERVICIOS GET

        [HttpGet]
        public async Task<ActionResult> GetListAll()
        {
            var response = await _serviceProfesor.GetListAll();
            if (response.Estado && response.Resultado is IEnumerable<ProfesorModel> list)
            {
                var dtoList = list.Select(p => p.ConvertToDTO()).ToList();
                return Ok(new
                {
                    data = dtoList,
                    totalItems = response.TotalItems
                });
            }

            return BadRequest(new { message = response.Error });
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> Get(int id)
        {
            var response = await _serviceProfesor.Get(id);
            if (response.Estado && response.Resultado is ProfesorModel model)
            {
                return Ok(model.ConvertToDTO());
            }

            return NotFound(new { message = response.Error ?? Messages.RecordNotFound });
        }

        [HttpGet("{identificacion}")]
        public async Task<ActionResult> GetByIdentificacion(string identificacion)
        {
            var response = await _serviceProfesor.GetByIdentificacion(identificacion);
            if (response.Estado && response.Resultado is ProfesorModel model)
            {
                return Ok(model.ConvertToDTO());
            }

            return NotFound(new { message = response.Error ?? Messages.RecordNotFound });
        }

        #endregion

        #region SERVICIOS POST - PUT - DELETE

        [HttpPost]
        public async Task<ActionResult<ProfesorDTO>> Add([FromBody] ProfesorModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var response = await _serviceProfesor.Add(model);
            if (response.Estado && response.Resultado is ProfesorModel createdModel)
            {
                return CreatedAtAction(nameof(Get), new { id = createdModel.Id }, createdModel.ConvertToDTO());
            }

            return BadRequest(new { message = response.Error });
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ProfesorDTO>> Edit(int id, [FromBody] ProfesorModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var response = await _serviceProfesor.Edit(id, model);
            if (response.Estado && response.Resultado is ProfesorModel updatedModel)
            {
                return Ok(updatedModel.ConvertToDTO());
            }

            return BadRequest(new { message = response.Error });
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var response = await _serviceProfesor.Delete(id);
            if (response.Estado)
            {
                return Ok(response.Resultado);
            }

            return BadRequest(new { message = response.Error });
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult> ToggleEstado(int id)
        {
            var response = await _serviceProfesor.ToggleEstado(id);
            if (response.Estado)
            {
                return Ok(response.Resultado);
            }

            return BadRequest(new { message = response.Error });
        }

        #endregion
    }
}
