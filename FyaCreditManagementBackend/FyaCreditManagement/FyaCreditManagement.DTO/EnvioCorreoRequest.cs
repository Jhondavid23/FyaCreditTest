using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FyaCreditManagement.DTO
{
    public class EnvioCorreoRequest
    {
        [Required]
        public int CreditoId { get; set; }

        [Required]
        [EmailAddress]
        public string DestinatarioEmail { get; set; } = string.Empty;

        [StringLength(200)]
        public string? AsuntoPersonalizado { get; set; }

        [StringLength(1000)]
        public string? MensajeAdicional { get; set; }
    }

    public class LogEnvioCorreoResponse
    {
        public int LogId { get; set; }
        public int CreditoId { get; set; }
        public string NombreCliente { get; set; } = string.Empty;
        public string DestinatarioEmail { get; set; } = string.Empty;
        public string AsuntoCorreo { get; set; } = string.Empty;
        public string EstadoEnvio { get; set; } = string.Empty;
        public string? MensajeError { get; set; }
        public DateTime FechaIntento { get; set; }
        public DateTime? FechaEnvio { get; set; }
        public string FechaIntentoFormateada => FechaIntento.ToString("dd/MM/yyyy HH:mm");
        public string FechaEnvioFormateada => FechaEnvio?.ToString("dd/MM/yyyy HH:mm") ?? "Pendiente";
    }
}
