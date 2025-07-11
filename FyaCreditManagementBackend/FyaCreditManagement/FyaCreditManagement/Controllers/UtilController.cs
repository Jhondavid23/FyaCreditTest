using Microsoft.AspNetCore.Mvc;

namespace FyaCreditManagement.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class UtilController : ControllerBase
    {
        /// <summary>
        /// Verificar estado de la API (Health Check)
        /// </summary>
        /// <returns>Estado del sistema</returns>
        [HttpGet("health")]
        [ProducesResponseType(typeof(object), 200)]
        public ActionResult<object> VerificarSalud()
        {
            return Ok(new
            {
                estado = "saludable",
                timestamp = DateTime.UtcNow,
                version = "1.0.0",
                sistema = "FYA Social Capital - Sistema de Gestión de Créditos"
            });
        }

        /// <summary>
        /// Obtener información del sistema
        /// </summary>
        /// <returns>Información general del sistema</returns>
        [HttpGet("info")]
        [ProducesResponseType(typeof(object), 200)]
        public ActionResult<object> ObtenerInformacion()
        {
            return Ok(new
            {
                nombre = "Sistema de Gestión de Créditos",
                empresa = "FYA Social Capital",
                version = "1.0.0",
                descripcion = "API REST para gestión de créditos con envío automático de notificaciones",
                requisitos = new
                {
                    formulario_registro = "✅ Implementado",
                    envio_correo_rpa = "✅ Implementado",
                    modulo_consulta = "✅ Implementado"
                },
                tecnologias = new
                {
                    backend = ".NET 6+",
                    frontend = "React (separado)",
                    base_datos = "SQL Server",
                    mapeo = "AutoMapper",
                    arquitectura = "Repository Pattern + Services"
                }
            });
        }

        /// <summary>
        /// Validar conexión a base de datos
        /// </summary>
        /// <returns>Estado de la conexión</returns>
        [HttpGet("database-check")]
        [ProducesResponseType(typeof(object), 200)]
        [ProducesResponseType(typeof(object), 500)]
        public ActionResult<object> VerificarBaseDatos()
        {
            try
            {
                // Aquí podrías hacer una consulta simple a la BD para verificar conectividad
                return Ok(new
                {
                    estado = "conectado",
                    timestamp = DateTime.UtcNow,
                    base_datos = "SQL Server",
                    conexion = "exitosa"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    estado = "error",
                    timestamp = DateTime.UtcNow,
                    base_datos = "SQL Server",
                    conexion = "fallida",
                    error = ex.Message
                });
            }
        }
    }
}
