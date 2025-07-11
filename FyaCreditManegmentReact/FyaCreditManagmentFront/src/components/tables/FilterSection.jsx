import { useState, useEffect } from "react";
import { comercialesAPI } from "../../services/api";

const FilterSection = ({ onFilterChange }) => {
  const [filters, setFilters] = useState({
    clientFilter: '',
    idFilter: '',
    salesRepFilter: '',
    sortBy: 'date-desc'
  });

  const [comerciales, setComerciales] = useState([]);
  const [loadingComerciales, setLoadingComerciales] = useState(true);

  // Cargar comerciales al montar el componente
  useEffect(() => {
    const fetchComerciales = async () => {
      try {
        setLoadingComerciales(true);
        const response = await comercialesAPI.obtenerComerciales();
        setComerciales(response || []);
      } catch (error) {
        console.error('Error cargando comerciales:', error);
        setComerciales([]);
      } finally {
        setLoadingComerciales(false);
      }
    };

    fetchComerciales();
  }, []);

  const updateFilters = (newFilters) => {
    const updatedFilters = { ...filters, ...newFilters };
    setFilters(updatedFilters);
    onFilterChange(updatedFilters);
  };

  const clearFilters = () => {
    const clearedFilters = {
      clientFilter: '',
      idFilter: '',
      salesRepFilter: '',
      sortBy: 'date-desc'
    };
    setFilters(clearedFilters);
    onFilterChange(clearedFilters);
  };

  const applyFilters = () => {
    onFilterChange(filters);
  };

  return (
    <div className="bg-gray-50 border border-gray-200 rounded-lg p-6 mb-6">
      <h3 className="text-base font-semibold text-gray-800 mb-4 pb-2 border-b border-gray-200">
        Filtros de Búsqueda
      </h3>
      <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-4 mb-4">
        <div>
          <label className="block mb-1.5 font-medium text-gray-700 text-sm">
            Filtrar por Cliente
          </label>
          <input
            type="text"
            value={filters.clientFilter}
            onChange={(e) => updateFilters({ clientFilter: e.target.value })}
            className="w-full p-3 border border-gray-300 rounded-lg text-sm transition-all duration-200 focus:outline-none focus:border-blue-800 focus:ring-3 focus:ring-blue-100"
            placeholder="Nombre del cliente"
          />
        </div>
        
        <div>
          <label className="block mb-1.5 font-medium text-gray-700 text-sm">
            Filtrar por Identificación
          </label>
          <input
            type="text"
            value={filters.idFilter}
            onChange={(e) => updateFilters({ idFilter: e.target.value })}
            className="w-full p-3 border border-gray-300 rounded-lg text-sm transition-all duration-200 focus:outline-none focus:border-blue-800 focus:ring-3 focus:ring-blue-100"
            placeholder="Número de documento"
          />
        </div>
        
        <div>
          <label className="block mb-1.5 font-medium text-gray-700 text-sm">
            Filtrar por Ejecutivo
          </label>
          <select
            value={filters.salesRepFilter}
            onChange={(e) => updateFilters({ salesRepFilter: e.target.value })}
            className="w-full p-3 border border-gray-300 rounded-lg text-sm transition-all duration-200 focus:outline-none focus:border-blue-800 focus:ring-3 focus:ring-blue-100"
            disabled={loadingComerciales}
          >
            <option value="">
              {loadingComerciales ? 'Cargando ejecutivos...' : 'Todos los ejecutivos'}
            </option>
            {comerciales.map((comercial) => (
              <option key={comercial.comercialId} value={comercial.nombre}>
                {comercial.nombre}
              </option>
            ))}
          </select>
          {loadingComerciales && (
            <div className="text-xs text-blue-600 mt-1">
              🔄 Cargando ejecutivos desde el servidor...
            </div>
          )}
        </div>
        
        <div>
          <label className="block mb-1.5 font-medium text-gray-700 text-sm">
            Ordenar por
          </label>
          <select
            value={filters.sortBy}
            onChange={(e) => updateFilters({ sortBy: e.target.value })}
            className="w-full p-3 border border-gray-300 rounded-lg text-sm transition-all duration-200 focus:outline-none focus:border-blue-800 focus:ring-3 focus:ring-blue-100"
          >
            <option value="date-desc">Fecha (Más reciente)</option>
            <option value="date-asc">Fecha (Más antigua)</option>
            <option value="amount-desc">Monto (Mayor a menor)</option>
            <option value="amount-asc">Monto (Menor a mayor)</option>
            <option value="client-asc">Cliente (A-Z)</option>
          </select>
        </div>
      </div>

      {/* Filtros adicionales */}
      <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-4 mb-4">
        <div>
          <label className="block mb-1.5 font-medium text-gray-700 text-sm">
            Valor Mínimo (COP)
          </label>
          <input
            type="number"
            value={filters.valorMinimo || ''}
            onChange={(e) => updateFilters({ valorMinimo: e.target.value ? parseFloat(e.target.value) : undefined })}
            className="w-full p-3 border border-gray-300 rounded-lg text-sm transition-all duration-200 focus:outline-none focus:border-blue-800 focus:ring-3 focus:ring-blue-100"
            placeholder="Valor mínimo"
            min="0"
            step="100000"
          />
        </div>
        
        <div>
          <label className="block mb-1.5 font-medium text-gray-700 text-sm">
            Valor Máximo (COP)
          </label>
          <input
            type="number"
            value={filters.valorMaximo || ''}
            onChange={(e) => updateFilters({ valorMaximo: e.target.value ? parseFloat(e.target.value) : undefined })}
            className="w-full p-3 border border-gray-300 rounded-lg text-sm transition-all duration-200 focus:outline-none focus:border-blue-800 focus:ring-3 focus:ring-blue-100"
            placeholder="Valor máximo"
            min="0"
            step="100000"
          />
        </div>
        
        <div>
          <label className="block mb-1.5 font-medium text-gray-700 text-sm">
            Fecha Desde
          </label>
          <input
            type="date"
            value={filters.fechaDesde || ''}
            onChange={(e) => updateFilters({ fechaDesde: e.target.value || undefined })}
            className="w-full p-3 border border-gray-300 rounded-lg text-sm transition-all duration-200 focus:outline-none focus:border-blue-800 focus:ring-3 focus:ring-blue-100"
          />
        </div>
        
        <div>
          <label className="block mb-1.5 font-medium text-gray-700 text-sm">
            Fecha Hasta
          </label>
          <input
            type="date"
            value={filters.fechaHasta || ''}
            onChange={(e) => updateFilters({ fechaHasta: e.target.value || undefined })}
            className="w-full p-3 border border-gray-300 rounded-lg text-sm transition-all duration-200 focus:outline-none focus:border-blue-800 focus:ring-3 focus:ring-blue-100"
          />
        </div>
      </div>
      
      <div className="flex justify-end gap-3">
        <button
          onClick={clearFilters}
          className="px-6 py-3 border border-gray-300 text-gray-700 font-semibold rounded-lg text-sm uppercase tracking-wide transition-all duration-200 hover:bg-gray-50 hover:border-gray-400"
        >
          Limpiar Filtros
        </button>
        <button
          onClick={applyFilters}
          className="px-6 py-3 bg-blue-800 text-white font-semibold rounded-lg text-sm uppercase tracking-wide transition-all duration-200 hover:bg-blue-900"
        >
          Aplicar Filtros
        </button>
      </div>

      {/* Información de API */}
      <div className="mt-4 p-3 bg-blue-50 border border-blue-200 rounded-lg">
        <div className="text-xs text-blue-700">
          <span className="font-medium">💡 Información:</span> Los filtros se aplican directamente en el servidor para optimizar el rendimiento.
          {comerciales.length > 0 && (
            <span className="ml-2">• {comerciales.length} ejecutivos disponibles</span>
          )}
        </div>
      </div>
    </div>
  );
};

export default FilterSection;