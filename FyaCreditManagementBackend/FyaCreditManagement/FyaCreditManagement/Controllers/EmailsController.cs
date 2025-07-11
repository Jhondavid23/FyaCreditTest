using FyaCreditManagement.BLL.Contrato;
using Microsoft.AspNetCore.Mvc;

namespace FyaCreditManagement.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmailsController : ControllerBase
    {
        private readonly IServicioEmail _servicioEmail;
        public EmailsController(IServicioEmail servicioEmail)
        {
            _servicioEmail = servicioEmail;
        }

        [HttpPost("enviar")]
        public async Task<ActionResult> Enviar(string email, string tema, string cuerpo)
        {
            await _servicioEmail.EnviarEmail(email, tema, cuerpo);
            return Ok(new { mensaje = "Email enviado correctamente" });
        }
    }
}
