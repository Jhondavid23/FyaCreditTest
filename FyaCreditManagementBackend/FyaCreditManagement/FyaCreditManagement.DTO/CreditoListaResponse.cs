using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FyaCreditManagement.DTO
{
    public class CreditoListaResponse
    {
        public int CreditoId { get; set; }
        public string NombreCliente { get; set; } = string.Empty;
        public string NumeroIdentificacion { get; set; } = string.Empty;
        public string NombreComercial { get; set; } = string.Empty;
        public string EstadoCredito { get; set; } = string.Empty;
        public decimal ValorCredito { get; set; }
        public string ValorCreditoFormateado => ValorCredito.ToString("C0", new System.Globalization.CultureInfo("es-CO"));
        public decimal TasaInteres { get; set; }
        public int PlazoMeses { get; set; }
        public decimal? ValorCuota { get; set; }
        public string ValorCuotaFormateado => ValorCuota?.ToString("C0", new System.Globalization.CultureInfo("es-CO")) ?? "N/A";
        public DateTime FechaRegistro { get; set; }
        public string FechaRegistroFormateada => FechaRegistro.ToString("dd/MM/yyyy");
        public DateTime? FechaAprobacion { get; set; }
        public string FechaAprobacionFormateada => FechaAprobacion?.ToString("dd/MM/yyyy") ?? "Pendiente";
    }
}
