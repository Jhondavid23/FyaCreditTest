using AutoMapper;
using FyaCreditManagement.BLL.Contrato;
using FyaCreditManagement.DAL.Repositorio.Contrato;
using FyaCreditManagement.DTO;
using FyaCreditManagement.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace FyaCreditManagement.BLL
{
    public class EstadoService : IEstadoService
    {
        private readonly IGenericRepository<EstadosCredito> _estadoRepository;
        private readonly IMapper _mapper;

        public EstadoService(IGenericRepository<EstadosCredito> estadoRepository, IMapper mapper)
        {
            _estadoRepository = estadoRepository;
            _mapper = mapper;
        }

        public async Task<List<EstadoCreditoDto>> ObtenerEstadosActivosAsync()
        {
            try
            {
                var query = await _estadoRepository.Consultar(e => e.Activo);
                var estados = await query.OrderBy(e => e.EstadoId).ToListAsync();

                return _mapper.Map<List<EstadoCreditoDto>>(estados);
            }
            catch (Exception ex)
            {
                return new List<EstadoCreditoDto>();
            }
        }
    }
}
