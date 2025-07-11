using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FyaCreditManagement.Utility
{
    public class ValidacionCreditoResponse
    {
        public bool EsValido { get; set; }
        public List<string> Errores { get; set; } = new();
        public List<string> Advertencias { get; set; } = new();
        public CalculoFinancieroDto? CalculoFinanciero { get; set; }
    }

    public class CalculoFinancieroDto
    {
        public decimal ValorCredito { get; set; }
        public decimal TasaInteres { get; set; }
        public int PlazoMeses { get; set; }
        public decimal ValorCuotaMensual { get; set; }
        public decimal ValorTotalAPagar { get; set; }
        public decimal TotalIntereses { get; set; }
        public List<CuotaDto> TablaCuotas { get; set; } = new();
    }

    public class CuotaDto
    {
        public int NumeroCuota { get; set; }
        public DateTime FechaCuota { get; set; }
        public decimal ValorCuota { get; set; }
        public decimal ValorCapital { get; set; }
        public decimal ValorInteres { get; set; }
        public decimal SaldoPendiente { get; set; }
    }
}
