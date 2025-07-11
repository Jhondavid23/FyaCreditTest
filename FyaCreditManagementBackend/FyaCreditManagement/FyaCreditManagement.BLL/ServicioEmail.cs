using FyaCreditManagement.BLL.Contrato;
using FyaCreditManagement.DAL.Repositorio.Contrato;
using FyaCreditManagement.Model;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Net.Mail;

namespace FyaCreditManagement.BLL
{
    public class ServicioEmail : IServicioEmail
    {
        private readonly IConfiguration _configuration;
        private readonly IServiceProvider _serviceProvider; 
        private readonly ILogger<ServicioEmail> _logger;

        public ServicioEmail(
            IConfiguration configuration,
            IServiceProvider serviceProvider, 
            ILogger<ServicioEmail> logger)
        {
            _configuration = configuration;
            _serviceProvider = serviceProvider; 
            _logger = logger;
        }

        /// <summary>
        /// Método original - mantener para compatibilidad
        /// </summary>
        public async Task EnviarEmail(string emailReceptor, string tema, string cuerpo)
        {
            var emailEmisor = _configuration["EmailSettings:EmailEmisor"];
            var passwordEmisor = _configuration["EmailSettings:PasswordEmisor"];
            var host = _configuration["EmailSettings:Host"];
            var smtpPort = int.Parse(_configuration["EmailSettings:SmtpPort"]);

            var smtpClient = new SmtpClient(host, smtpPort);
            smtpClient.EnableSsl = true;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = new System.Net.NetworkCredential(emailEmisor, passwordEmisor);

            var message = new MailMessage(emailEmisor, emailReceptor, tema, cuerpo);
            await smtpClient.SendMailAsync(message);
        }

        /// <summary>
        /// Enviar correo de registro automáticamente (Requisito 2: RPA)
        /// SOLUCIÓN: Crear scope independiente para evitar disposed context
        /// </summary>
        public async Task<bool> EnviarCorreoRegistroCreditoAsync(Credito credito)
        {
            try
            {
                // CREAR UN SCOPE INDEPENDIENTE para el DbContext
                using var scope = _serviceProvider.CreateScope();
                var logRepository = scope.ServiceProvider.GetRequiredService<IGenericRepository<LogEnvioCorreo>>();

                // 1. Obtener email destino desde configuración
                var emailDestino = _configuration["EmailSettings:EmailDestino"] ?? "fyasocialcapital@gmail.com";

                // 2. Crear log de intento
                var logCorreo = new LogEnvioCorreo
                {
                    CreditoId = credito.CreditoId,
                    DestinatarioEmail = emailDestino,
                    AsuntoCorreo = $"🏦 Nuevo Crédito Registrado - ID: {credito.CreditoId}",
                    CuerpoCorreo = ConstruirCuerpoCorreo(credito),
                    EstadoEnvio = "PENDIENTE"
                };

                await logRepository.Crear(logCorreo);

                // 3. Enviar correo usando el método base con HTML
                await EnviarEmailConHtml(
                    emailDestino,
                    logCorreo.AsuntoCorreo,
                    logCorreo.CuerpoCorreo,
                    ConstruirHtmlCorreo(credito)
                );

                // 4. Actualizar log como exitoso
                logCorreo.EstadoEnvio = "ENVIADO";
                logCorreo.FechaEnvio = DateTime.UtcNow;
                await logRepository.Editar(logCorreo);

                _logger.LogInformation($" Correo enviado exitosamente para crédito {credito.CreditoId}");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $" Error al enviar correo para crédito {credito.CreditoId}");

                // Intentar actualizar log como error con scope independiente
                try
                {
                    using var scope = _serviceProvider.CreateScope();
                    var logRepository = scope.ServiceProvider.GetRequiredService<IGenericRepository<LogEnvioCorreo>>();

                    // Buscar el log por CreditoId y estado PENDIENTE
                    var query = await logRepository.Consultar(l => l.CreditoId == credito.CreditoId && l.EstadoEnvio == "PENDIENTE");
                    var logError = query.FirstOrDefault();
                    if (logError != null)
                    {
                        logError.EstadoEnvio = "ERROR";
                        logError.MensajeError = ex.Message;
                        await logRepository.Editar(logError);
                    }
                }
                catch (Exception logEx)
                {
                    _logger.LogError(logEx, "Error adicional al actualizar log de error");
                }

                return false;
            }
        }

        /// <summary>
        /// Enviar correo personalizado con manejo de errores
        /// </summary>
        public async Task<bool> EnviarCorreoPersonalizadoAsync(string destinatario, string asunto, string mensaje)
        {
            try
            {
                await EnviarEmail(destinatario, asunto, mensaje);
                _logger.LogInformation($"✅ Correo personalizado enviado a {destinatario}");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"❌ Error al enviar correo personalizado a {destinatario}");
                return false;
            }
        }

        /// <summary>
        /// Método privado para enviar email con HTML
        /// </summary>
        private async Task EnviarEmailConHtml(string emailReceptor, string tema, string cuerpoTexto, string cuerpoHtml)
        {
            var emailEmisor = _configuration["EmailSettings:EmailEmisor"];
            var passwordEmisor = _configuration["EmailSettings:PasswordEmisor"];
            var host = _configuration["EmailSettings:Host"];
            var smtpPort = int.Parse(_configuration["EmailSettings:SmtpPort"]);

            using var smtpClient = new SmtpClient(host, smtpPort)
            {
                EnableSsl = true,
                UseDefaultCredentials = false,
                Credentials = new System.Net.NetworkCredential(emailEmisor, passwordEmisor)
            };

            using var message = new MailMessage(emailEmisor, emailReceptor, tema, cuerpoHtml)
            {
                IsBodyHtml = true
            };

            // Agregar versión en texto plano como alternativa
            var plainView = AlternateView.CreateAlternateViewFromString(cuerpoTexto, null, "text/plain");
            var htmlView = AlternateView.CreateAlternateViewFromString(cuerpoHtml, null, "text/html");

            message.AlternateViews.Add(plainView);
            message.AlternateViews.Add(htmlView);

            await smtpClient.SendMailAsync(message);
        }

        // ... resto de métodos (ConstruirCuerpoCorreo, ConstruirHtmlCorreo) igual que antes
        private string ConstruirCuerpoCorreo(Credito credito)
        {
            return $@"
=== NUEVO CRÉDITO REGISTRADO ===
Sistema de Gestión de Créditos FYA Social Capital

INFORMACIÓN DEL CLIENTE:
• Nombre: {credito.Cliente.Nombre}
• Identificación: {credito.Cliente.NumeroIdentificacion}
• Email: {credito.Cliente.Email ?? "No registrado"}
• Teléfono: {credito.Cliente.Telefono ?? "No registrado"}

INFORMACIÓN DEL CRÉDITO:
• ID del Crédito: {credito.CreditoId}
• Valor Solicitado: {credito.ValorCredito:C0}
• Tasa de Interés: {credito.TasaInteres:F2}%
• Plazo: {credito.PlazoMeses} meses
• Cuota Estimada: {credito.ValorCuota:C0}
• Valor Total a Pagar: {credito.ValorTotal:C0}

INFORMACIÓN COMERCIAL:
• Ejecutivo: {credito.Comercial.Nombre}
• Email Comercial: {credito.Comercial.Email}
• Teléfono Comercial: {credito.Comercial.Telefono ?? "No registrado"}

DETALLES DE REGISTRO:
• Fecha de Registro: {credito.FechaRegistro:dd/MM/yyyy HH:mm}
• Estado: {credito.Estado.Descripcion}
• Usuario que Registró: {credito.UsuarioCreacion}

---
Este correo ha sido generado automáticamente por el Sistema de Gestión de Créditos.
FYA Social Capital - {DateTime.Now:dd/MM/yyyy HH:mm}

📧 Enviado desde: {_configuration["EmailSettings:EmailEmisor"]}
🔒 Conexión segura SMTP
            ".Trim();
        }

        private string ConstruirHtmlCorreo(Credito credito)
        {
            return $@"
<!DOCTYPE html>
<html>
<head>
    <meta charset='utf-8'>
    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
    <style>
        body {{ font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif; margin: 0; padding: 20px; background-color: #f0f2f5; }}
        .container {{ max-width: 650px; margin: 0 auto; background-color: white; border-radius: 12px; padding: 0; box-shadow: 0 4px 6px rgba(0,0,0,0.1); overflow: hidden; }}
        .header {{ background: linear-gradient(135deg, #1e40af 0%, #3b82f6 100%); color: white; padding: 30px; text-align: center; }}
        .header h1 {{ margin: 0; font-size: 28px; font-weight: bold; }}
        .header p {{ margin: 10px 0 0 0; opacity: 0.9; font-size: 16px; }}
        .content {{ padding: 30px; }}
        .section {{ margin-bottom: 30px; }}
        .section h3 {{ color: #1e40af; font-size: 18px; border-bottom: 3px solid #e5e7eb; padding-bottom: 8px; margin-bottom: 15px; }}
        .info-item {{ margin-bottom: 10px; }}
        .label {{ font-weight: 600; color: #374151; display: inline-block; min-width: 140px; }}
        .value {{ color: #6b7280; }}
        .amount {{ font-size: 20px; font-weight: bold; color: #059669; background: #ecfdf5; padding: 2px 8px; border-radius: 4px; }}
        .footer {{ background: #f8fafc; margin: 30px -30px -30px -30px; padding: 25px 30px; border-top: 1px solid #e5e7eb; text-align: center; font-size: 13px; color: #6b7280; }}
    </style>
</head>
<body>
    <div class='container'>
        <div class='header'>
            <h1>🏦 FYA SOCIAL CAPITAL</h1>
            <p>Nuevo Crédito Registrado - ID: {credito.CreditoId}</p>
        </div>
        <div class='content'>
            <div class='section'>
                <h3>👤 Cliente: {credito.Cliente.Nombre}</h3>
                <div class='info-item'><span class='label'>Identificación:</span> <span class='value'>{credito.Cliente.NumeroIdentificacion}</span></div>
            </div>
            <div class='section'>
                <h3>💰 Crédito: <span class='amount'>{credito.ValorCredito:C0}</span></h3>
                <div class='info-item'><span class='label'>Tasa:</span> <span class='value'>{credito.TasaInteres:F2}%</span></div>
                <div class='info-item'><span class='label'>Plazo:</span> <span class='value'>{credito.PlazoMeses} meses</span></div>
                <div class='info-item'><span class='label'>Comercial:</span> <span class='value'>{credito.Comercial.Nombre}</span></div>
            </div>
        </div>
        <div class='footer'>
            <p>FYA Social Capital - {DateTime.Now:dd/MM/yyyy HH:mm}</p>
        </div>
    </div>
</body>
</html>
            ".Trim();
        }
    }
}
