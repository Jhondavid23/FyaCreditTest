using AutoMapper;
using FyaCreditManagement.DTO;
using FyaCreditManagement.Model;
using Microsoft.Extensions.DependencyInjection;

namespace FyaCreditManagement.Utility
{
    // =============================================
    // PROFILE PRINCIPAL PARA CRÉDITOS
    // =============================================
    public class CreditoMappingProfile : Profile
    {
        public CreditoMappingProfile()
        {
            // =============================================
            // MAPEOS DE CRÉDITO
            // =============================================

            // Modelo -> DTO Response (Completo)
            CreateMap<Credito, CreditoResponse>()
                .ForMember(dest => dest.Cliente, opt => opt.MapFrom(src => src.Cliente))
                .ForMember(dest => dest.Comercial, opt => opt.MapFrom(src => src.Comercial))
                .ForMember(dest => dest.Estado, opt => opt.MapFrom(src => src.Estado));

            // Modelo -> DTO Lista (Simplificado)
            CreateMap<Credito, CreditoListaResponse>()
                .ForMember(dest => dest.NombreCliente, opt => opt.MapFrom(src => src.Cliente.Nombre))
                .ForMember(dest => dest.NumeroIdentificacion, opt => opt.MapFrom(src => src.Cliente.NumeroIdentificacion))
                .ForMember(dest => dest.NombreComercial, opt => opt.MapFrom(src => src.Comercial.Nombre))
                .ForMember(dest => dest.EstadoCredito, opt => opt.MapFrom(src => src.Estado.Descripcion));

            // Request -> Modelo (Para crear/actualizar)
            CreateMap<CrearCreditoRequest, Credito>()
                .ForMember(dest => dest.CreditoId, opt => opt.Ignore())
                .ForMember(dest => dest.ClienteId, opt => opt.Ignore()) // Se asigna en el servicio
                .ForMember(dest => dest.EstadoId, opt => opt.MapFrom(src => 1)) // Pendiente por defecto
                .ForMember(dest => dest.Cliente, opt => opt.Ignore())
                .ForMember(dest => dest.Comercial, opt => opt.Ignore())
                .ForMember(dest => dest.Estado, opt => opt.Ignore())
                .ForMember(dest => dest.ValorCuota, opt => opt.Ignore())
                .ForMember(dest => dest.ValorTotal, opt => opt.Ignore())
                .ForMember(dest => dest.FechaVencimiento, opt => opt.Ignore())
                .ForMember(dest => dest.FechaAprobacion, opt => opt.Ignore())
                .ForMember(dest => dest.LogEnvioCorreos, opt => opt.Ignore());

            // Request -> Cliente (Para extraer datos del cliente)
            CreateMap<CrearCreditoRequest, Cliente>()
                .ForMember(dest => dest.ClienteId, opt => opt.Ignore())
                .ForMember(dest => dest.Nombre, opt => opt.MapFrom(src => src.NombreCliente))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.EmailCliente))
                .ForMember(dest => dest.Telefono, opt => opt.MapFrom(src => src.TelefonoCliente))
                .ForMember(dest => dest.Direccion, opt => opt.MapFrom(src => src.DireccionCliente))
                .ForMember(dest => dest.Ciudad, opt => opt.MapFrom(src => src.CiudadCliente))
                .ForMember(dest => dest.Creditos, opt => opt.Ignore());

            // =============================================
            // MAPEOS DE ENTIDADES RELACIONADAS
            // =============================================

            // Cliente
            CreateMap<Cliente, ClienteDto>();
            CreateMap<ClienteDto, Cliente>()
                .ForMember(dest => dest.Creditos, opt => opt.Ignore())
                .ForMember(dest => dest.FechaCreacion, opt => opt.Ignore())
                .ForMember(dest => dest.FechaModificacion, opt => opt.Ignore());

            // Comercial
            CreateMap<Comercial, ComercialDto>();
            CreateMap<ComercialDto, Comercial>()
                .ForMember(dest => dest.Activo, opt => opt.Ignore())
                .ForMember(dest => dest.FechaCreacion, opt => opt.Ignore())
                .ForMember(dest => dest.FechaModificacion, opt => opt.Ignore())
                .ForMember(dest => dest.Creditos, opt => opt.Ignore());

            // Estado Crédito
            CreateMap<EstadosCredito, EstadoCreditoDto>();
            CreateMap<EstadoCreditoDto, EstadosCredito>()
                .ForMember(dest => dest.Activo, opt => opt.Ignore())
                .ForMember(dest => dest.Creditos, opt => opt.Ignore());


            // =============================================
            // MAPEOS DE SISTEMA RPA DE CORREOS
            // =============================================
            CreateMap<LogEnvioCorreo, LogEnvioCorreoResponse>()
                .ForMember(dest => dest.NombreCliente, opt => opt.MapFrom(src => src.Credito.Cliente.Nombre));

            CreateMap<EnvioCorreoRequest, LogEnvioCorreo>()
                .ForMember(dest => dest.LogId, opt => opt.Ignore())
                .ForMember(dest => dest.AsuntoCorreo, opt => opt.MapFrom(src =>
                    !string.IsNullOrEmpty(src.AsuntoPersonalizado)
                        ? src.AsuntoPersonalizado
                        : $"Nuevo Crédito Registrado - ID: {src.CreditoId}"))
                .ForMember(dest => dest.CuerpoCorreo, opt => opt.Ignore()) // Se construye en el servicio
                .ForMember(dest => dest.EstadoEnvio, opt => opt.MapFrom(src => "PENDIENTE"))
                .ForMember(dest => dest.FechaEnvio, opt => opt.Ignore())
                .ForMember(dest => dest.MensajeError, opt => opt.Ignore())
                .ForMember(dest => dest.Credito, opt => opt.Ignore());

            // =============================================
            // MAPEOS PARA CÁLCULOS FINANCIEROS
            // =============================================
            CreateMap<Credito, CalculoFinancieroDto>()
                .ForMember(dest => dest.ValorCuotaMensual, opt => opt.MapFrom(src => src.ValorCuota ?? 0))
                .ForMember(dest => dest.ValorTotalAPagar, opt => opt.MapFrom(src => src.ValorTotal ?? 0))
                .ForMember(dest => dest.TotalIntereses, opt => opt.MapFrom(src =>
                    (src.ValorTotal ?? 0) - src.ValorCredito))
                .ForMember(dest => dest.TablaCuotas, opt => opt.Ignore()); // Se calcula en el servicio

            CreateMap<CrearCreditoRequest, CalculoFinancieroDto>()
                .ForMember(dest => dest.ValorCuotaMensual, opt => opt.Ignore()) // Se calcula
                .ForMember(dest => dest.ValorTotalAPagar, opt => opt.Ignore()) // Se calcula
                .ForMember(dest => dest.TotalIntereses, opt => opt.Ignore()) // Se calcula
                .ForMember(dest => dest.TablaCuotas, opt => opt.Ignore()); // Se calcula
        }
    }

    // =============================================
    // CONFIGURACIÓN CENTRALIZADA DE AUTOMAPPER
    // =============================================
    public static class AutoMapperConfig
    {
        /// <summary>
        /// Configura todos los profiles de AutoMapper
        /// </summary>
        public static MapperConfiguration CreateConfiguration()
        {
            return new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<CreditoMappingProfile>();

                // Configuraciones globales
                cfg.AllowNullCollections = true;
                cfg.AllowNullDestinationValues = true;

                // Configurar formato de fechas globalmente
                cfg.ValueTransformers.Add<DateTime>(val => val.ToLocalTime());
            });
        }

        /// <summary>
        /// Crea una instancia de IMapper configurada
        /// </summary>
        public static IMapper CreateMapper()
        {
            var config = CreateConfiguration();
            return config.CreateMapper();
        }
    }

    // =============================================
    // EXTENSIONES PARA INYECCIÓN DE DEPENDENCIAS
    // =============================================
    public static class AutoMapperExtensions
    {
        /// <summary>
        /// Registra AutoMapper en el contenedor de DI
        /// </summary>
        public static void AddAutoMapperProfiles(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(CreditoMappingProfile));
        }

        /// <summary>
        /// Registra AutoMapper con configuración personalizada
        /// </summary>
        public static void AddAutoMapperWithCustomConfig(this IServiceCollection services)
        {
            var config = AutoMapperConfig.CreateConfiguration();
            services.AddSingleton(config);
            services.AddSingleton<IMapper>(provider => new Mapper(config, provider.GetService));
        }
    }

    // =============================================
    // MAPEOS PERSONALIZADOS Y HELPERS
    // =============================================
    public static class MappingHelpers
    {
        /// <summary>
        /// Mapea lista de créditos con información completa
        /// </summary>
        public static List<CreditoListaResponse> MapearListaCreditos(
            this IMapper mapper,
            IEnumerable<Credito> creditos)
        {
            return mapper.Map<List<CreditoListaResponse>>(creditos);
        }

        /// <summary>
        /// Mapea respuesta paginada de créditos
        /// </summary>
        public static ConsultarCreditosResponse MapearRespuestaPaginada(
            this IMapper mapper,
            IEnumerable<Credito> creditos,
            int totalRegistros,
            int pagina,
            int tamañoPagina)
        {
            return new ConsultarCreditosResponse
            {
                Creditos = mapper.Map<List<CreditoListaResponse>>(creditos),
                TotalRegistros = totalRegistros,
                Pagina = pagina,
                TamañoPagina = tamañoPagina
            };
        }

        /// <summary>
        /// Mapea respuesta paginada desde vista ResumenCredito
        /// </summary>
       

        /// <summary>
        /// Construye el cuerpo del correo para RPA
        /// </summary>
        public static string ConstruirCuerpoCorreo(Credito credito, string? mensajeAdicional = null)
        {
            var mensaje = $@"
                Nuevo Crédito Registrado en el Sistema FYA Social Capital
                
                Información del Cliente:
                - Nombre: {credito.Cliente.Nombre}
                - Identificación: {credito.Cliente.NumeroIdentificacion}
                
                Información del Crédito:
                - Valor: {credito.ValorCredito:C0}
                - Tasa de Interés: {credito.TasaInteres:F2}%
                - Plazo: {credito.PlazoMeses} meses
                - Cuota Estimada: {credito.ValorCuota:C0}
                
                Información Comercial:
                - Ejecutivo: {credito.Comercial.Nombre}
                - Email: {credito.Comercial.Email}
                
                Fecha de Registro: {credito.FechaRegistro:dd/MM/yyyy HH:mm}
                Estado: {credito.Estado.Descripcion}
                
                {(!string.IsNullOrEmpty(mensajeAdicional) ? $"Mensaje Adicional:\n{mensajeAdicional}" : "")}
                
                ---
                Sistema de Gestión de Créditos FYA Social Capital
                Generado automáticamente el {DateTime.Now:dd/MM/yyyy HH:mm}
            ";

            return mensaje.Trim();
        }
    }

    // =============================================
    // VALIDADORES DE MAPEO (PARA TESTING)
    // =============================================
    public static class MappingValidators
    {
        /// <summary>
        /// Valida que todas las configuraciones de mapeo sean correctas
        /// </summary>
        public static void ValidateAllMappings()
        {
            var config = AutoMapperConfig.CreateConfiguration();
            config.AssertConfigurationIsValid();
        }

        /// <summary>
        /// Prueba de mapeo específico para debugging
        /// </summary>
        public static void TestCreditoMapping()
        {
            var mapper = AutoMapperConfig.CreateMapper();

            // Crear datos de prueba
            var credito = new Credito
            {
                CreditoId = 1,
                ValorCredito = 1000000,
                TasaInteres = 2.5m,
                PlazoMeses = 12,
                Cliente = new Cliente { Nombre = "Test Cliente", NumeroIdentificacion = "123456789" },
                Comercial = new Comercial { Nombre = "Test Comercial" },
                Estado = new EstadosCredito { Descripcion = "Activo" }
            };

            // Probar mapeos
            var response = mapper.Map<CreditoResponse>(credito);
            var lista = mapper.Map<CreditoListaResponse>(credito);

            // Validar que los mapeos funcionan
            System.Diagnostics.Debug.Assert(response.CreditoId == credito.CreditoId);
            System.Diagnostics.Debug.Assert(lista.NombreCliente == credito.Cliente.Nombre);
        }
    }
}