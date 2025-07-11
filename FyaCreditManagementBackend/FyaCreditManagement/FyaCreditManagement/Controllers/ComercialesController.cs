using FyaCreditManagement.BLL.Contrato;
using FyaCreditManagement.DTO;
using FyaCreditManagement.Utility;
using Microsoft.AspNetCore.Mvc;

namespace FyaCreditManagement.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class ComercialesController : ControllerBase
    {
        private readonly IComercialService _comercialService;

        public ComercialesController(IComercialService comercialService)
        {
            _comercialService = comercialService;
        }

        /// <summary>
        /// Obtener lista de comerciales activos (Para dropdown del formulario)
        /// </summary>
        /// <returns>Lista de comerciales disponibles</returns>
        [HttpGet]
        [ProducesResponseType(typeof(List<ComercialDto>), 200)]
        public async Task<ActionResult<List<ComercialDto>>> ObtenerComerciales()
        {
            try
            {
                var resultado = await _comercialService.ObtenerComercialesActivosAsync();
                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new List<ComercialDto>());
            }
        }

        /// <summary>
        /// Obtener información de un comercial específico
        /// </summary>
        /// <param name="id">ID del comercial</param>
        /// <returns>Información del comercial</returns>
        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(ApiResponse<ComercialDto>), 200)]
        [ProducesResponseType(typeof(ApiResponse<ComercialDto>), 404)]
        public async Task<ActionResult<ApiResponse<ComercialDto>>> ObtenerComercial([FromRoute] int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest(ApiResponse<ComercialDto>.Error("ID de comercial inválido"));
                }

                var resultado = await _comercialService.ObtenerComercialPorIdAsync(id);

                if (resultado.Exitoso)
                {
                    return Ok(resultado);
                }

                return NotFound(resultado);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<ComercialDto>.Error($"Error interno del servidor: {ex.Message}"));
            }
        }
    }
}
