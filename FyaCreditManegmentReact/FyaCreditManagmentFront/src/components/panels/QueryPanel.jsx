import React, { useState, useEffect } from 'react';
import useCredits from '../../hooks/useCredits';
import MetricsCards from '../tables/MetricsCards';
import FilterSection from '../tables/FilterSection';
import CreditsTable from '../tables/CreditsTable';

const QueryPanel = () => {
  const { credits, dashboard, fetchCredits, fetchDashboard, loading, error } = useCredits();
  const [filteredCredits, setFilteredCredits] = useState([]);
  const [currentFilters, setCurrentFilters] = useState({});

  useEffect(() => {
    setFilteredCredits(credits);
  }, [credits]);

  useEffect(() => {
    // Cargar datos iniciales del panel
    const loadData = async () => {
      await fetchCredits();
      await fetchDashboard();
    };
    
    loadData();
  }, []);

  const handleFilterChange = async (filters) => {
    setCurrentFilters(filters);
    
    // Construir parámetros para la API basados en los filtros
    const apiFilters = {
      filtroCliente: filters.clientFilter || undefined,
      filtroIdentificacion: filters.idFilter || undefined,
      filtroComercial: filters.salesRepFilter || undefined,
      ordenarPor: mapSortByToAPI(filters.sortBy),
      ordenDireccion: getSortDirection(filters.sortBy),
      pagina: 1,
      tamañoPagina: 100 // Por ahora traemos todos
    };

    // Llamar a la API con los filtros
    try {
      const response = await fetchCredits(apiFilters);
      if (response && response.creditos) {
        setFilteredCredits(response.creditos.map(transformCreditFromAPI));
      }
    } catch (error) {
      console.error('Error aplicando filtros:', error);
      // Si hay error, filtrar localmente como fallback
      applyLocalFilters(filters);
    }
  };

  // Mapear opciones de ordenamiento del frontend al backend
  const mapSortByToAPI = (sortBy) => {
    const sortMap = {
      'date-desc': 'FechaRegistro',
      'date-asc': 'FechaRegistro',
      'amount-desc': 'ValorCredito',
      'amount-asc': 'ValorCredito',
      'client-asc': 'Cliente'
    };
    return sortMap[sortBy] || 'FechaRegistro';
  };

  const getSortDirection = (sortBy) => {
    return sortBy.includes('-desc') ? 'DESC' : 'ASC';
  };

  // Transformar datos de la API al formato del frontend
  const transformCreditFromAPI = (creditFromAPI) => {
    return {
      id: creditFromAPI.creditoId,
      clientName: creditFromAPI.nombreCliente,
      clientId: creditFromAPI.numeroIdentificacion,
      amount: creditFromAPI.valorCredito,
      interestRate: creditFromAPI.tasaInteres,
      termMonths: creditFromAPI.plazoMeses,
      salesRep: creditFromAPI.nombreComercial,
      date: creditFromAPI.fechaRegistro,
      status: creditFromAPI.estadoCredito === 'Activo' ? 'active' : 'pending',
      valorCuota: creditFromAPI.valorCuota,
      fechaAprobacion: creditFromAPI.fechaAprobacion
    };
  };

  // Filtrado local como fallback
  const applyLocalFilters = (filters) => {
    let filtered = credits.filter(credit => {
      return (
        (!filters.clientFilter || credit.clientName.toLowerCase().includes(filters.clientFilter.toLowerCase())) &&
        (!filters.idFilter || credit.clientId.includes(filters.idFilter)) &&
        (!filters.salesRepFilter || credit.salesRep === filters.salesRepFilter)
      );
    });

    // Aplicar ordenamiento local
    filtered.sort((a, b) => {
      switch(filters.sortBy) {
        case 'date-desc':
          return new Date(b.date).getTime() - new Date(a.date).getTime();
        case 'date-asc':
          return new Date(a.date).getTime() - new Date(b.date).getTime();
        case 'amount-desc':
          return b.amount - a.amount;
        case 'amount-asc':
          return a.amount - b.amount;
        case 'client-asc':
          return a.clientName.localeCompare(b.clientName);
        default:
          return 0;
      }
    });

    setFilteredCredits(filtered);
  };

  const refreshData = async () => {
    await fetchCredits(currentFilters);
    await fetchDashboard();
  };

  if (error) {
    return (
      <div className="bg-white border border-gray-200 rounded-xl p-8 shadow-sm">
        <div className="text-center py-10">
          <div className="text-red-500 text-5xl mb-4">⚠️</div>
          <h3 className="text-red-700 text-lg font-semibold mb-2">Error al cargar los datos</h3>
          <p className="text-gray-600 mb-4">{error}</p>
          <button
            onClick={refreshData}
            className="px-4 py-2 bg-blue-800 text-white rounded-lg hover:bg-blue-900 transition-colors"
          >
            Reintentar
          </button>
        </div>
      </div>
    );
  }

  return (
    <div className="bg-white border border-gray-200 rounded-xl p-8 shadow-sm">
      <div className="border-b-2 border-gray-100 pb-4 mb-8">
        <div className="flex justify-between items-start">
          <div>
            <h2 className="text-2xl font-bold text-gray-800 mb-2">Consulta y Reportes de Créditos</h2>
            <p className="text-gray-600 text-sm">
              Visualice, filtre y ordene todos los créditos registrados en el sistema. 
              Utilice los filtros para realizar búsquedas específicas.
            </p>
          </div>
          <button
            onClick={refreshData}
            disabled={loading}
            className="px-4 py-2 bg-gray-100 text-gray-700 rounded-lg hover:bg-gray-200 transition-colors disabled:opacity-50 disabled:cursor-not-allowed text-sm font-medium"
          >
            {loading ? '🔄 Actualizando...' : '🔄 Actualizar'}
          </button>
        </div>
      </div>

      {/* Mostrar indicador de carga */}
      {loading && (
        <div className="mb-6 p-4 bg-blue-50 border border-blue-200 rounded-lg">
          <div className="flex items-center">
            <div className="animate-spin rounded-full h-4 w-4 border-b-2 border-blue-800 mr-3"></div>
            <span className="text-blue-800 text-sm font-medium">Cargando datos desde el servidor...</span>
          </div>
        </div>
      )}

      <MetricsCards credits={filteredCredits} dashboard={dashboard} />
      <FilterSection onFilterChange={handleFilterChange} />
      <CreditsTable credits={filteredCredits} />

      {/* Información de conexión */}
      <div className="mt-6 p-4 bg-gray-50 border border-gray-200 rounded-lg">
        <div className="flex items-center justify-between text-xs text-gray-600">
          <div>
            <span className="font-medium">Estado de conexión:</span>
            <span className={`ml-2 ${error ? 'text-red-600' : 'text-green-600'}`}>
              {error ? '❌ Error de conexión' : '✅ Conectado al servidor'}
            </span>
          </div>
          <div>
            <span className="font-medium">Última actualización:</span>
            <span className="ml-2">{new Date().toLocaleTimeString()}</span>
          </div>
        </div>
      </div>
    </div>
  );
};

export default QueryPanel;