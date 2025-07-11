using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FyaCreditManagement.DTO
{
    public class DashboardResponse
    {
        public int TotalCreditos { get; set; }
        public decimal ValorTotalCreditos { get; set; }
        public string ValorTotalFormateado => ValorTotalCreditos.ToString("C0", new System.Globalization.CultureInfo("es-CO"));
        public decimal PromedioCreditos { get; set; }
        public string PromedioCreditosFormateado => PromedioCreditos.ToString("C0", new System.Globalization.CultureInfo("es-CO"));
        public int CreditosActivos { get; set; }
        public int CreditosPendientes { get; set; }
        public DateTime? UltimoRegistro { get; set; }
        public string UltimoRegistroFormateado => UltimoRegistro?.ToString("dd/MM/yyyy HH:mm") ?? "N/A";

        // Métricas adicionales
        public List<EstadisticaPorEstado> CreditosPorEstado { get; set; } = new();
        public List<EstadisticaPorComercial> CreditosPorComercial { get; set; } = new();
        public List<EstadisticaPorMes> CreditosPorMes { get; set; } = new();
    }
    public class EstadisticaPorEstado
    {
        public string EstadoDescripcion { get; set; } = string.Empty;
        public int Cantidad { get; set; }
        public decimal ValorTotal { get; set; }
        public double Porcentaje { get; set; }
    }

    public class EstadisticaPorComercial
    {
        public string NombreComercial { get; set; } = string.Empty;
        public int CantidadCreditos { get; set; }
        public decimal ValorTotal { get; set; }
        public decimal ValorPromedio { get; set; }
    }

    public class EstadisticaPorMes
    {
        public int Año { get; set; }
        public int Mes { get; set; }
        public string MesNombre { get; set; } = string.Empty;
        public int CantidadCreditos { get; set; }
        public decimal ValorTotal { get; set; }
    }
}
