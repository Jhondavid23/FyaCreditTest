using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FyaCreditManagement.DTO
{
    public class CreditoResponse
    {
        public int CreditoId { get; set; }

        // Información del cliente
        public ClienteDto Cliente { get; set; } = new();

        // Información del comercial
        public ComercialDto Comercial { get; set; } = new();

        // Información del estado
        public EstadoCreditoDto Estado { get; set; } = new();

        // Datos del crédito
        public decimal ValorCredito { get; set; }
        public decimal TasaInteres { get; set; }
        public int PlazoMeses { get; set; }
        public decimal? ValorCuota { get; set; }
        public decimal? ValorTotal { get; set; }

        // Fechas
        public DateTime FechaRegistro { get; set; }
        public DateTime? FechaAprobacion { get; set; }
        public DateTime? FechaVencimiento { get; set; }

        // Auditoría
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaModificacion { get; set; }
        public string UsuarioCreacion { get; set; } = string.Empty;
        public string UsuarioModificacion { get; set; } = string.Empty;
    }
}
