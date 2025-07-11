using FyaCreditManagement.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FyaCreditManagement.BLL.Contrato
{
    public interface IEstadoService
    {
        Task<List<EstadoCreditoDto>> ObtenerEstadosActivosAsync();
    }

}
