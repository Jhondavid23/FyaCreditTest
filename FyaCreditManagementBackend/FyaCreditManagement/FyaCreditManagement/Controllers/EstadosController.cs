using FyaCreditManagement.BLL.Contrato;
using FyaCreditManagement.DTO;
using Microsoft.AspNetCore.Mvc;

namespace FyaCreditManagement.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class EstadosController : ControllerBase
    {
        private readonly IEstadoService _estadoService;

        public EstadosController(IEstadoService estadoService)
        {
            _estadoService = estadoService;
        }

        /// <summary>
        /// Obtener lista de estados de crédito (Para filtros y selección)
        /// </summary>
        /// <returns>Lista de estados disponibles</returns>
        [HttpGet]
        [ProducesResponseType(typeof(List<EstadoCreditoDto>), 200)]
        public async Task<ActionResult<List<EstadoCreditoDto>>> ObtenerEstados()
        {
            try
            {
                var resultado = await _estadoService.ObtenerEstadosActivosAsync();
                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new List<EstadoCreditoDto>());
            }
        }
    }

}
