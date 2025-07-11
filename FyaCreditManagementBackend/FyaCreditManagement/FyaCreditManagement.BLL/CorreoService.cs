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

namespace FyaCreditManagement.BLL
{
    public class CorreoService : ICorreoService
    {
        private readonly IGenericRepository<LogEnvioCorreo> _logRepository;
        private readonly IMapper _mapper;
        private const string EMAIL_DESTINO = "fyasocialcapital@gmail.com";

        public CorreoService(IGenericRepository<LogEnvioCorreo> logRepository, IMapper mapper)
        {
            _logRepository = logRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Enviar correo de registro automáticamente (Requisito 2: RPA)
        /// </summary>
        public async Task<bool> EnviarCorreoRegistroAsync(Credito credito)
        {
            try
            {
                // Crear log de envío
                var logCorreo = new LogEnvioCorreo
                {
                    CreditoId = credito.CreditoId,
                    DestinatarioEmail = EMAIL_DESTINO,
                    AsuntoCorreo = $"Nuevo Crédito Registrado - ID: {credito.CreditoId}",
                    CuerpoCorreo = ConstruirCuerpoCorreo(credito),
                    EstadoEnvio = "PENDIENTE"
                };

                await _logRepository.Crear(logCorreo);

                // Simular envío (aquí integrarías con SendGrid, Mailgun, etc.)
                var enviado = await SimularEnvioCorreoAsync(logCorreo);

                if (enviado)
                {
                    logCorreo.EstadoEnvio = "ENVIADO";
                    logCorreo.FechaEnvio = DateTime.UtcNow;
                }
                else
                {
                    logCorreo.EstadoEnvio = "ERROR";
                    logCorreo.MensajeError = "Error simulado en el envío";
                }

                await _logRepository.Editar(logCorreo);
                return enviado;
            }
            catch (Exception ex)
            {
                // Log del error pero no fallar la creación del crédito
                Console.WriteLine($"Error al enviar correo: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Obtener logs de envíos de correo
        /// </summary>
        public async Task<List<LogEnvioCorreoResponse>> ObtenerLogEnviosAsync()
        {
            try
            {
                var query = await _logRepository.Consultar();
                var logs = await query
                    .Include(l => l.Credito)
                    .ThenInclude(c => c.Cliente)
                    .OrderByDescending(l => l.FechaIntento)
                    .ToListAsync();

                return _mapper.Map<List<LogEnvioCorreoResponse>>(logs);
            }
            catch (Exception ex)
            {
                return new List<LogEnvioCorreoResponse>();
            }
        }

        /// <summary>
        /// Procesar envíos pendientes (para jobs en segundo plano)
        /// </summary>
        public async Task<bool> ProcesarEnviosPendientesAsync()
        {
            try
            {
                var query = await _logRepository.Consultar(l => l.EstadoEnvio == "PENDIENTE");
                var pendientes = await query
                    .Include(l => l.Credito)
                    .ThenInclude(c => c.Cliente)
                    .Include(l => l.Credito)
                    .ThenInclude(c => c.Comercial)
                    .Include(l => l.Credito)
                    .ThenInclude(c => c.Estado)
                    .ToListAsync();

                foreach (var log in pendientes)
                {
                    var enviado = await SimularEnvioCorreoAsync(log);

                    if (enviado)
                    {
                        log.EstadoEnvio = "ENVIADO";
                        log.FechaEnvio = DateTime.UtcNow;
                    }
                    else
                    {
                        log.EstadoEnvio = "ERROR";
                        log.MensajeError = "Error en el envío automático";
                    }

                    await _logRepository.Editar(log);
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al procesar envíos pendientes: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Simular envío de correo (aquí integrarías con servicio real)
        /// </summary>
        private async Task<bool> SimularEnvioCorreoAsync(LogEnvioCorreo log)
        {
            // Simular delay de envío
            await Task.Delay(100);

            // Simular 90% de éxito
            var random = new Random();
            var exito = random.Next(1, 11) <= 9;

            return exito;
        }

        /// <summary>
        /// Construir cuerpo del correo
        /// </summary>
        private string ConstruirCuerpoCorreo(Credito credito)
        {
            return $@"
Nuevo Crédito Registrado en el Sistema FYA Social Capital

Información del Cliente:
- Nombre: {credito.Cliente.Nombre}
- Identificación: {credito.Cliente.NumeroIdentificacion}

Información del Crédito:
- Valor: {credito.ValorCredito:C0}
- Tasa de Interés: {credito.TasaInteres:F2}%
- Plazo: {credito.PlazoMeses} meses

Información Comercial:
- Ejecutivo: {credito.Comercial.Nombre}
- Email: {credito.Comercial.Email}

Fecha de Registro: {credito.FechaRegistro:dd/MM/yyyy HH:mm}
Estado: {credito.Estado.Descripcion}

---
Sistema de Gestión de Créditos FYA Social Capital
Generado automáticamente el {DateTime.Now:dd/MM/yyyy HH:mm}
            ".Trim();
        }
    }
}
