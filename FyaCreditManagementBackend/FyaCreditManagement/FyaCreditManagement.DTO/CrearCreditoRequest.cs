using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FyaCreditManagement.DTO
{
    public class CrearCreditoRequest
    {
        [Required(ErrorMessage = "El nombre del cliente es requerido")]
        [StringLength(200, MinimumLength = 2, ErrorMessage = "El nombre debe tener entre 2 y 200 caracteres")]
        public string NombreCliente { get; set; } = string.Empty;

        [Required(ErrorMessage = "El número de identificación es requerido")]
        [StringLength(50, MinimumLength = 5, ErrorMessage = "La identificación debe tener entre 5 y 50 caracteres")]
        public string NumeroIdentificacion { get; set; } = string.Empty;

        [StringLength(20)]
        public string TipoIdentificacion { get; set; } = "CC";

        [EmailAddress(ErrorMessage = "Email inválido")]
        [StringLength(100)]
        public string? EmailCliente { get; set; }

        [Phone(ErrorMessage = "Teléfono inválido")]
        [StringLength(20)]
        public string? TelefonoCliente { get; set; }

        [StringLength(300)]
        public string? DireccionCliente { get; set; }

        [StringLength(100)]
        public string? CiudadCliente { get; set; }

        [Required(ErrorMessage = "El comercial es requerido")]
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un comercial válido")]
        public int ComercialId { get; set; }

        [Required(ErrorMessage = "El valor del crédito es requerido")]
        [Range(100000, 999999999999, ErrorMessage = "El valor debe estar entre $100,000 y $999,999,999,999")]
        public decimal ValorCredito { get; set; }

        [Required(ErrorMessage = "La tasa de interés es requerida")]
        [Range(0.01, 50.00, ErrorMessage = "La tasa debe estar entre 0.01% y 50%")]
        public decimal TasaInteres { get; set; }

        [Required(ErrorMessage = "El plazo es requerido")]
        [Range(1, 360, ErrorMessage = "El plazo debe estar entre 1 y 360 meses")]
        public int PlazoMeses { get; set; }

        public string? UsuarioCreacion { get; set; }
    }
}
