using FyaCreditManagement.BLL.Contrato;
using FyaCreditManagement.DTO;
using FyaCreditManagement.Utility;
using Microsoft.AspNetCore.Mvc;

namespace FyaCreditManagement.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class CorreosController : ControllerBase
    {
        private readonly ICorreoService _correoService;

        public CorreosController(ICorreoService correoService)
        {
            _correoService = correoService;
        }

        /// <summary>
        /// Obtener historial de envíos de correo (Requisito 2: Monitoreo RPA)
        /// </summary>
        /// <returns>Lista de logs de envío de correos</returns>
        [HttpGet("logs")]
        [ProducesResponseType(typeof(List<LogEnvioCorreoResponse>), 200)]
        public async Task<ActionResult<List<LogEnvioCorreoResponse>>> ObtenerLogsEnvio()
        {
            try
            {
                var resultado = await _correoService.ObtenerLogEnviosAsync();
                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new List<LogEnvioCorreoResponse>());
            }
        }

        /// <summary>
        /// Procesar envíos pendientes manualmente (Para troubleshooting)
        /// </summary>
        /// <returns>Resultado del procesamiento</returns>
        [HttpPost("procesar-pendientes")]
        [ProducesResponseType(typeof(ApiResponse), 200)]
        public async Task<ActionResult<ApiResponse>> ProcesarEnviosPendientes()
        {
            try
            {
                var resultado = await _correoService.ProcesarEnviosPendientesAsync();

                if (resultado)
                {
                    return Ok(ApiResponse.Success("Envíos pendientes procesados exitosamente"));
                }

                return StatusCode(500, ApiResponse.Error("Error al procesar envíos pendientes"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse.Error($"Error interno del servidor: {ex.Message}"));
            }
        }
    }

}
