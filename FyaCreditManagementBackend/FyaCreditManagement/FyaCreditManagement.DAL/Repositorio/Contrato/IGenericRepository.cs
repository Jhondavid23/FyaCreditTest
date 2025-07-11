using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace FyaCreditManagement.DAL.Repositorio.Contrato
{
    public interface IGenericRepository<TModelo> where TModelo : class
    {
        Task<IQueryable<TModelo>> Consultar(Expression<Func<TModelo, bool>> filtro = null);
        Task<TModelo> Obtener(Expression<Func<TModelo, bool>> filtro);
        Task<TModelo> Crear(TModelo modelo);
        Task<bool> Editar(TModelo modelo);
        Task<bool> Eliminar(TModelo modelo);
        Task<bool> Eliminar(IQueryable<TModelo> modelo);
    }
}
