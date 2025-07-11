using FyaCreditManagement.DTO;
using FyaCreditManagement.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FyaCreditManagement.BLL.Contrato
{
    public interface ICreditoService
    {
        Task<ApiResponse<CreditoResponse>> CrearCreditoAsync(CrearCreditoRequest request);
        Task<ConsultarCreditosResponse> ConsultarCreditosAsync(ConsultarCreditosRequest request);
        Task<ApiResponse<CreditoResponse>> ObtenerCreditoPorIdAsync(int creditoId);
        Task<ApiResponse<CreditoResponse>> ActualizarCreditoAsync(ActualizarCreditoRequest request);
        Task<DashboardResponse> ObtenerDashboardAsync();
    }
}
