using FyaCreditManagement.DTO;
using FyaCreditManagement.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FyaCreditManagement.BLL.Contrato
{
    public interface IComercialService
    {
        Task<List<ComercialDto>> ObtenerComercialesActivosAsync();
        Task<ApiResponse<ComercialDto>> ObtenerComercialPorIdAsync(int comercialId);
    }

}
