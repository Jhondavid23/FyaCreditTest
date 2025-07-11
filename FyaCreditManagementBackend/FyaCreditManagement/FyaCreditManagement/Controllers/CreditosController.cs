using FyaCreditManagement.BLL.Contrato;
using FyaCreditManagement.DTO;
using FyaCreditManagement.Utility;
using Microsoft.AspNetCore.Mvc;

namespace FyaCreditManagement.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class CreditosController : ControllerBase
    {
        private readonly ICreditoService _creditoService;

        public CreditosController(ICreditoService creditoService)
        {
            _creditoService = creditoService;
        }

        /// <summary>
        /// Registrar un nuevo crédito (Requisito 1: Formulario de Registro)
        /// </summary>
        /// <param name="request">Datos del nuevo crédito</param>
        /// <returns>Información del crédito creado</returns>
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<CreditoResponse>), 200)]
        [ProducesResponseType(typeof(ApiResponse<CreditoResponse>), 400)]
        public async Task<ActionResult<ApiResponse<CreditoResponse>>> RegistrarCredito([FromBody] CrearCreditoRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errores = ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)
                        .ToList();

                    return BadRequest(ApiResponse<CreditoResponse>.Error("Datos inválidos", errores));
                }

                var resultado = await _creditoService.CrearCreditoAsync(request);

                if (resultado.Exitoso)
                {
                    return Ok(resultado);
                }

                return BadRequest(resultado);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<CreditoResponse>.Error($"Error interno del servidor: {ex.Message}"));
            }
        }

        /// <summary>
        /// Consultar créditos con filtros (Requisito 3: Módulo de Consulta)
        /// </summary>
        /// <param name="request">Parámetros de consulta y filtros</param>
        /// <returns>Lista paginada de créditos</returns>
        [HttpGet]
        [ProducesResponseType(typeof(ConsultarCreditosResponse), 200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<ConsultarCreditosResponse>> ConsultarCreditos([FromQuery] ConsultarCreditosRequest request)
        {
            try
            {
                var resultado = await _creditoService.ConsultarCreditosAsync(request);
                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ConsultarCreditosResponse
                {
                    Creditos = new List<CreditoListaResponse>(),
                    TotalRegistros = 0,
                    Pagina = request.Pagina,
                    TamañoPagina = request.TamañoPagina
                });
            }
        }

        /// <summary>
        /// Obtener un crédito específico por ID
        /// </summary>
        /// <param name="id">ID del crédito</param>
        /// <returns>Información completa del crédito</returns>
        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(ApiResponse<CreditoResponse>), 200)]
        [ProducesResponseType(typeof(ApiResponse<CreditoResponse>), 404)]
        public async Task<ActionResult<ApiResponse<CreditoResponse>>> ObtenerCredito([FromRoute] int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest(ApiResponse<CreditoResponse>.Error("ID de crédito inválido"));
                }

                var resultado = await _creditoService.ObtenerCreditoPorIdAsync(id);

                if (resultado.Exitoso)
                {
                    return Ok(resultado);
                }

                return NotFound(resultado);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<CreditoResponse>.Error($"Error interno del servidor: {ex.Message}"));
            }
        }

        /// <summary>
        /// Actualizar un crédito existente
        /// </summary>
        /// <param name="request">Datos a actualizar del crédito</param>
        /// <returns>Información del crédito actualizado</returns>
        [HttpPut]
        [ProducesResponseType(typeof(ApiResponse<CreditoResponse>), 200)]
        [ProducesResponseType(typeof(ApiResponse<CreditoResponse>), 400)]
        [ProducesResponseType(typeof(ApiResponse<CreditoResponse>), 404)]
        public async Task<ActionResult<ApiResponse<CreditoResponse>>> ActualizarCredito([FromBody] ActualizarCreditoRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errores = ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)
                        .ToList();

                    return BadRequest(ApiResponse<CreditoResponse>.Error("Datos inválidos", errores));
                }

                var resultado = await _creditoService.ActualizarCreditoAsync(request);

                if (resultado.Exitoso)
                {
                    return Ok(resultado);
                }

                return NotFound(resultado);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<CreditoResponse>.Error($"Error interno del servidor: {ex.Message}"));
            }
        }

        /// <summary>
        /// Obtener métricas del dashboard
        /// </summary>
        /// <returns>Estadísticas generales de créditos</returns>
        [HttpGet("dashboard")]
        [ProducesResponseType(typeof(DashboardResponse), 200)]
        public async Task<ActionResult<DashboardResponse>> ObtenerDashboard()
        {
            try
            {
                var resultado = await _creditoService.ObtenerDashboardAsync();
                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new DashboardResponse());
            }
        }
    }
}
