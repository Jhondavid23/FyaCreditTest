using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FyaCreditManagement.Utility
{
    public class ApiResponse<T>
    {
        public bool Exitoso { get; set; }
        public string Mensaje { get; set; } = string.Empty;
        public T? Datos { get; set; }
        public List<string> Errores { get; set; } = new();
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        public static ApiResponse<T> Success(T datos, string mensaje = "Operación exitosa")
        {
            return new ApiResponse<T>
            {
                Exitoso = true,
                Mensaje = mensaje,
                Datos = datos
            };
        }

        public static ApiResponse<T> Error(string mensaje, List<string>? errores = null)
        {
            return new ApiResponse<T>
            {
                Exitoso = false,
                Mensaje = mensaje,
                Errores = errores ?? new List<string>()
            };
        }
    }

    public class ApiResponse : ApiResponse<object>
    {
        public static ApiResponse Success(string mensaje = "Operación exitosa")
        {
            return new ApiResponse
            {
                Exitoso = true,
                Mensaje = mensaje
            };
        }

        public static new ApiResponse Error(string mensaje, List<string>? errores = null)
        {
            return new ApiResponse
            {
                Exitoso = false,
                Mensaje = mensaje,
                Errores = errores ?? new List<string>()
            };
        }
    }
}
