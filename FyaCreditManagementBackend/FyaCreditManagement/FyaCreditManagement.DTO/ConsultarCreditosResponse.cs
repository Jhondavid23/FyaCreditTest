using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FyaCreditManagement.DTO
{
    public class ConsultarCreditosResponse
    {
        public List<CreditoListaResponse> Creditos { get; set; } = new();
        public int TotalRegistros { get; set; }
        public int Pagina { get; set; }
        public int TamañoPagina { get; set; }
        public int TotalPaginas => (int)Math.Ceiling((double)TotalRegistros / TamañoPagina);
        public bool TienePaginaAnterior => Pagina > 1;
        public bool TienePaginaSiguiente => Pagina < TotalPaginas;
    }
}
