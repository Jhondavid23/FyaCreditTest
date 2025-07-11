using AutoMapper;
using FyaCreditManagement.BLL.Contrato;
using FyaCreditManagement.DAL.Repositorio.Contrato;
using FyaCreditManagement.DTO;
using FyaCreditManagement.Model;
using FyaCreditManagement.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;


namespace FyaCreditManagement.BLL
{
    public class CreditoService : ICreditoService
    {
        private readonly IGenericRepository<Credito> _creditoRepository;
        private readonly IGenericRepository<Cliente> _clienteRepository;
        private readonly IGenericRepository<Comercial> _comercialRepository;
        private readonly IGenericRepository<EstadosCredito> _estadoRepository;
        private readonly IServicioEmail _servicioEmail;
        private readonly IMapper _mapper;
        private readonly ICorreoService _correoService;
        private readonly ILogger<CreditoService> _logger;
        public CreditoService(
            IGenericRepository<Credito> creditoRepository,
            IGenericRepository<Cliente> clienteRepository,
            IGenericRepository<Comercial> comercialRepository,
            IGenericRepository<EstadosCredito> estadoRepository,
            IServicioEmail servicioEmail,
            IMapper mapper,
            ICorreoService correoService,
            ILogger<CreditoService> logger)
        {
            _creditoRepository = creditoRepository;
            _clienteRepository = clienteRepository;
            _comercialRepository = comercialRepository;
            _estadoRepository = estadoRepository;
            _mapper = mapper;
            _correoService = correoService;
            _servicioEmail = servicioEmail;
            _logger = logger;
        }

        /// <summary>
        /// Crear un nuevo crédito (Requisito 1: Formulario de Registro)
        /// </summary>
        public async Task<ApiResponse<CreditoResponse>> CrearCreditoAsync(CrearCreditoRequest request)
        {
            try
            {

                // 1. Validar comercial existe
                var comercial = await _comercialRepository.Obtener(c => c.ComercialId == request.ComercialId && c.Activo);
                if (comercial == null)
                {
                    return ApiResponse<CreditoResponse>.Error("El comercial seleccionado no existe o está inactivo");
                }

                // 2. Buscar o crear cliente
                var cliente = await _clienteRepository.Obtener(c => c.NumeroIdentificacion == request.NumeroIdentificacion);
                if (cliente == null)
                {
                    // Crear nuevo cliente usando AutoMapper
                    cliente = _mapper.Map<Cliente>(request);
                    cliente = await _clienteRepository.Crear(cliente);
                }
                else
                {
                }

                // 3. Crear crédito usando AutoMapper
                var credito = _mapper.Map<Credito>(request);
                credito.ClienteId = cliente.ClienteId;
                credito.UsuarioCreacion = request.UsuarioCreacion ?? "System";
                credito.UsuarioModificacion = request.UsuarioCreacion ?? "System";

                credito = await _creditoRepository.Crear(credito);


                // 4. Cargar datos relacionados para el response
                credito.Cliente = cliente;
                credito.Comercial = comercial;
                credito.Estado = await _estadoRepository.Obtener(e => e.EstadoId == credito.EstadoId);

                // 5. ENVIAR CORREO AUTOMÁTICAMENTE (REQUISITO 2: RPA)

                // NOTA IMPORTANTE: El envío se hace de forma asíncrona en segundo plano
                // Si falla el correo, NO falla el registro del crédito
                _ = Task.Run(async () =>
                {
                    try
                    {
                        var emailEnviado = await _servicioEmail.EnviarCorreoRegistroCreditoAsync(credito);
                        if (emailEnviado)
                        {
                            _logger.LogInformation($"Correo enviado exitosamente para crédito {credito.CreditoId}");
                        }
                        else
                        {
                            _logger.LogWarning($"No se pudo enviar correo para crédito {credito.CreditoId}");
                        }
                    }
                    catch (Exception emailEx)
                    {
                        _logger.LogError(emailEx, $"Error al enviar correo para crédito {credito.CreditoId}");
                    }
                });

                // 6. Convertir a DTO de respuesta
                var response = _mapper.Map<CreditoResponse>(credito);

                _logger.LogInformation($"🎉 Registro de crédito completado exitosamente - ID: {credito.CreditoId}");

                return ApiResponse<CreditoResponse>.Success(response, "Crédito registrado exitosamente. Se enviará una notificación por correo electrónico.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"❌ Error al crear crédito para cliente: {request.NombreCliente}");
                return ApiResponse<CreditoResponse>.Error($"Error al crear crédito: {ex.Message}");
            }
        }

        /// <summary>
        /// Consultar créditos con filtros (Requisito 3: Módulo de Consulta)
        /// </summary>
        public async Task<ConsultarCreditosResponse> ConsultarCreditosAsync(ConsultarCreditosRequest request)
        {
            try
            {
                // Obtener todos los créditos
                var query = await _creditoRepository.Consultar();

                // Incluir datos relacionados
                query = query.Include(c => c.Cliente)
                           .Include(c => c.Comercial)
                           .Include(c => c.Estado);

                // Aplicar filtros
                if (!string.IsNullOrEmpty(request.FiltroCliente))
                {
                    query = query.Where(c => c.Cliente.Nombre.Contains(request.FiltroCliente));
                }

                if (!string.IsNullOrEmpty(request.FiltroIdentificacion))
                {
                    query = query.Where(c => c.Cliente.NumeroIdentificacion.Contains(request.FiltroIdentificacion));
                }

                if (!string.IsNullOrEmpty(request.FiltroComercial))
                {
                    query = query.Where(c => c.Comercial.Nombre.Contains(request.FiltroComercial));
                }

                if (request.EstadoId.HasValue)
                {
                    query = query.Where(c => c.EstadoId == request.EstadoId.Value);
                }

                if (request.FechaDesde.HasValue)
                {
                    query = query.Where(c => c.FechaRegistro >= request.FechaDesde.Value);
                }

                if (request.FechaHasta.HasValue)
                {
                    query = query.Where(c => c.FechaRegistro <= request.FechaHasta.Value);
                }

                if (request.ValorMinimo.HasValue)
                {
                    query = query.Where(c => c.ValorCredito >= request.ValorMinimo.Value);
                }

                if (request.ValorMaximo.HasValue)
                {
                    query = query.Where(c => c.ValorCredito <= request.ValorMaximo.Value);
                }

                // Contar total antes de paginación
                var totalRegistros = await query.CountAsync();

                // Aplicar ordenamiento
                query = request.OrdenarPor.ToLower() switch
                {
                    "fecha" or "fecharegistro" => request.OrdenDireccion.ToUpper() == "ASC"
                        ? query.OrderBy(c => c.FechaRegistro)
                        : query.OrderByDescending(c => c.FechaRegistro),
                    "valor" or "valorcredito" => request.OrdenDireccion.ToUpper() == "ASC"
                        ? query.OrderBy(c => c.ValorCredito)
                        : query.OrderByDescending(c => c.ValorCredito),
                    "cliente" => request.OrdenDireccion.ToUpper() == "ASC"
                        ? query.OrderBy(c => c.Cliente.Nombre)
                        : query.OrderByDescending(c => c.Cliente.Nombre),
                    _ => query.OrderByDescending(c => c.FechaRegistro)
                };

                // Aplicar paginación
                var creditos = await query
                    .Skip((request.Pagina - 1) * request.TamañoPagina)
                    .Take(request.TamañoPagina)
                    .ToListAsync();

                // Usar AutoMapper para crear respuesta paginada
                return _mapper.MapearRespuestaPaginada(creditos, totalRegistros, request.Pagina, request.TamañoPagina);
            }
            catch (Exception ex)
            {
                return new ConsultarCreditosResponse
                {
                    Creditos = new List<CreditoListaResponse>(),
                    TotalRegistros = 0,
                    Pagina = request.Pagina,
                    TamañoPagina = request.TamañoPagina
                };
            }
        }

        /// <summary>
        /// Obtener crédito por ID
        /// </summary>
        public async Task<ApiResponse<CreditoResponse>> ObtenerCreditoPorIdAsync(int creditoId)
        {
            try
            {
                var query = await _creditoRepository.Consultar(c => c.CreditoId == creditoId);
                var credito = await query
                    .Include(c => c.Cliente)
                    .Include(c => c.Comercial)
                    .Include(c => c.Estado)
                    .FirstOrDefaultAsync();

                if (credito == null)
                {
                    return ApiResponse<CreditoResponse>.Error("Crédito no encontrado");
                }

                var response = _mapper.Map<CreditoResponse>(credito);
                return ApiResponse<CreditoResponse>.Success(response);
            }
            catch (Exception ex)
            {
                return ApiResponse<CreditoResponse>.Error($"Error al obtener crédito: {ex.Message}");
            }
        }

        /// <summary>
        /// Actualizar crédito existente
        /// </summary>
        public async Task<ApiResponse<CreditoResponse>> ActualizarCreditoAsync(ActualizarCreditoRequest request)
        {
            try
            {
                var credito = await _creditoRepository.Obtener(c => c.CreditoId == request.CreditoId);
                if (credito == null)
                {
                    return ApiResponse<CreditoResponse>.Error("Crédito no encontrado");
                }

                // Actualizar solo los campos que vienen en el request
                if (request.EstadoId.HasValue)
                    credito.EstadoId = request.EstadoId.Value;

                if (request.ValorCredito.HasValue)
                    credito.ValorCredito = request.ValorCredito.Value;

                if (request.TasaInteres.HasValue)
                    credito.TasaInteres = request.TasaInteres.Value;

                if (request.PlazoMeses.HasValue)
                    credito.PlazoMeses = request.PlazoMeses.Value;

                if (request.FechaAprobacion.HasValue)
                    credito.FechaAprobacion = request.FechaAprobacion.Value;

                credito.UsuarioModificacion = request.UsuarioModificacion ?? "System";

                await _creditoRepository.Editar(credito);

                // Cargar datos relacionados para el response
                credito.Cliente = await _clienteRepository.Obtener(c => c.ClienteId == credito.ClienteId);
                credito.Comercial = await _comercialRepository.Obtener(c => c.ComercialId == credito.ComercialId);
                credito.Estado = await _estadoRepository.Obtener(e => e.EstadoId == credito.EstadoId);

                var response = _mapper.Map<CreditoResponse>(credito);
                return ApiResponse<CreditoResponse>.Success(response, "Crédito actualizado exitosamente");
            }
            catch (Exception ex)
            {
                return ApiResponse<CreditoResponse>.Error($"Error al actualizar crédito: {ex.Message}");
            }
        }

        /// <summary>
        /// Obtener métricas del dashboard
        /// </summary>
        public async Task<DashboardResponse> ObtenerDashboardAsync()
        {
            try
            {
                var creditosQuery = await _creditoRepository.Consultar();
                var creditos = await creditosQuery.ToListAsync();

                var totalCreditos = creditos.Count;
                var valorTotal = creditos.Sum(c => c.ValorCredito);
                var promedio = totalCreditos > 0 ? valorTotal / totalCreditos : 0;
                var activos = creditos.Count(c => c.EstadoId == 3); // Estado Activo
                var pendientes = creditos.Count(c => c.EstadoId == 1); // Estado Pendiente
                var ultimoRegistro = creditos.Any() ? creditos.Max(c => c.FechaRegistro) : (DateTime?)null;

                return new DashboardResponse
                {
                    TotalCreditos = totalCreditos,
                    ValorTotalCreditos = valorTotal,
                    PromedioCreditos = promedio,
                    CreditosActivos = activos,
                    CreditosPendientes = pendientes,
                    UltimoRegistro = ultimoRegistro
                };
            }
            catch (Exception ex)
            {
                return new DashboardResponse();
            }
        }
    }
}
