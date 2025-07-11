using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FyaCreditManagement.DTO
{
    public class ClienteDto
    {
        public int ClienteId { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string NumeroIdentificacion { get; set; } = string.Empty;
        public string TipoIdentificacion { get; set; } = string.Empty;
        public string? Email { get; set; }
        public string? Telefono { get; set; }
        public string? Direccion { get; set; }
        public string? Ciudad { get; set; }
    }
}
