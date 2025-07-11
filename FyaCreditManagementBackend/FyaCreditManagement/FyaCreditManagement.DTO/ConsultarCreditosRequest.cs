using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FyaCreditManagement.DTO
{
    public class ConsultarCreditosRequest
    {
        public string? FiltroCliente { get; set; }
        public string? FiltroIdentificacion { get; set; }
        public string? FiltroComercial { get; set; }
        public int? EstadoId { get; set; }
        public DateTime? FechaDesde { get; set; }
        public DateTime? FechaHasta { get; set; }
        public decimal? ValorMinimo { get; set; }
        public decimal? ValorMaximo { get; set; }

        [StringLength(50)]
        public string OrdenarPor { get; set; } = "FechaRegistro";

        [StringLength(4)]
        public string OrdenDireccion { get; set; } = "DESC";

        [Range(1, 1000)]
        public int Pagina { get; set; } = 1;

        [Range(10, 100)]
        public int TamañoPagina { get; set; } = 20;
    }
}
