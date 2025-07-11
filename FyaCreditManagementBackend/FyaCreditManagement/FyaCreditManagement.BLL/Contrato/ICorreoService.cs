using FyaCreditManagement.DTO;
using FyaCreditManagement.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FyaCreditManagement.BLL.Contrato
{
    public interface ICorreoService
    {
        Task<bool> EnviarCorreoRegistroAsync(Credito credito);
        Task<List<LogEnvioCorreoResponse>> ObtenerLogEnviosAsync();
        Task<bool> ProcesarEnviosPendientesAsync();
    }
}
