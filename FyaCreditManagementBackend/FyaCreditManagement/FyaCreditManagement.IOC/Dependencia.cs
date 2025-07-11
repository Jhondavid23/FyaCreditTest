using FyaCreditManagement.BLL.Contrato;
using FyaCreditManagement.BLL;
using FyaCreditManagement.Model;
using FyaCreditManagement.Utility;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FyaCreditManagement.DAL.Repositorio.Contrato;
using FyaCreditManagement.DAL.Repositorio;

namespace FyaCreditManagement.IOC
{
    public static class Dependencia
    {
        public static void InyectarDependencias(this IServiceCollection services, IConfiguration configuration)
        {
            // Inyectar el contexto de la base de datos
            services.AddDbContext<FyaCreditManagementContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("CadenaSql"), sqlOptions =>
                {
                    
                    sqlOptions.CommandTimeout(120); 
                    sqlOptions.EnableRetryOnFailure(
                        maxRetryCount: 3,
                        maxRetryDelay: TimeSpan.FromSeconds(30),
                        errorNumbersToAdd: null);
                });

            });
            services.AddScoped<IGenericRepository<Credito>, GenericRepository<Credito>>();
            services.AddScoped<IGenericRepository<Cliente>, GenericRepository<Cliente>>();
            services.AddScoped<IGenericRepository<Comercial>, GenericRepository<Comercial>>();
            services.AddScoped<IGenericRepository<EstadosCredito>, GenericRepository<EstadosCredito>>();
            services.AddScoped<IGenericRepository<LogEnvioCorreo>, GenericRepository<LogEnvioCorreo>>();
            services.AddScoped<IGenericRepository<LogEnvioCorreo>, GenericRepository<LogEnvioCorreo>>();

            services.AddAutoMapper(typeof(CreditoMappingProfile));
            services.AddTransient<IServicioEmail, ServicioEmail>();
            services.AddScoped<ICreditoService, CreditoService>();
            services.AddScoped<ICorreoService, CorreoService>();
            services.AddScoped<IComercialService, ComercialService>();
            services.AddScoped<IEstadoService, EstadoService>();
        }
    }
}
