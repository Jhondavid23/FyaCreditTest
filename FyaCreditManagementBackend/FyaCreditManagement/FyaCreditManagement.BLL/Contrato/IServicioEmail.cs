using FyaCreditManagement.Model;

namespace FyaCreditManagement.BLL.Contrato
{
    public interface IServicioEmail
    {
        Task EnviarEmail(string emailReceptor, string tema, string cuerpo);
        Task<bool> EnviarCorreoRegistroCreditoAsync(Credito credito);
        Task<bool> EnviarCorreoPersonalizadoAsync(string destinatario, string asunto, string mensaje);
    }
}