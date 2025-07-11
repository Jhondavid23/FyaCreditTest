using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FyaCreditManagement.DTO
{
    public class ActualizarCreditoRequest
    {
        [Required]
        public int CreditoId { get; set; }

        [Range(1, int.MaxValue)]
        public int? EstadoId { get; set; }

        [Range(100000, 999999999999)]
        public decimal? ValorCredito { get; set; }

        [Range(0.01, 50.00)]
        public decimal? TasaInteres { get; set; }

        [Range(1, 360)]
        public int? PlazoMeses { get; set; }

        public DateTime? FechaAprobacion { get; set; }

        public string? UsuarioModificacion { get; set; }
    }
}
