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
    public class ComercialService : IComercialService
    {
        private readonly IGenericRepository<Comercial> _comercialRepository;
        private readonly IMapper _mapper;

        public ComercialService(IGenericRepository<Comercial> comercialRepository, IMapper mapper)
        {
            _comercialRepository = comercialRepository;
            _mapper = mapper;
        }

        public async Task<List<ComercialDto>> ObtenerComercialesActivosAsync()
        {
            try
            {
                var query = await _comercialRepository.Consultar(c => c.Activo);
                var comerciales = await query.OrderBy(c => c.Nombre).ToListAsync();

                return _mapper.Map<List<ComercialDto>>(comerciales);
            }
            catch (Exception ex)
            {
                return new List<ComercialDto>();
            }
        }

        public async Task<ApiResponse<ComercialDto>> ObtenerComercialPorIdAsync(int comercialId)
        {
            try
            {
                var comercial = await _comercialRepository.Obtener(c => c.ComercialId == comercialId && c.Activo);

                if (comercial == null)
                {
                    return ApiResponse<ComercialDto>.Error("Comercial no encontrado");
                }

                var response = _mapper.Map<ComercialDto>(comercial);
                return ApiResponse<ComercialDto>.Success(response);
            }
            catch (Exception ex)
            {
                return ApiResponse<ComercialDto>.Error($"Error al obtener comercial: {ex.Message}");
            }
        }
    }
}
