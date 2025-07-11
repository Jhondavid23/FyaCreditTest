const MetricsCards = ({ credits, dashboard }) => {
  // Usar datos del dashboard si están disponibles, sino calcular localmente
  const totalCredits = dashboard?.totalCreditos ?? credits.length;
  const totalAmount = dashboard?.valorTotalCreditos ?? credits.reduce((sum, credit) => sum + credit.amount, 0);
  const avgAmount = dashboard?.promedioCreditos ?? (totalCredits > 0 ? totalAmount / totalCredits : 0);
  const activeCredits = dashboard?.creditosActivos ?? credits.filter(credit => credit.status === 'active').length;

  const formatCurrency = (amount) => {
    return new Intl.NumberFormat('es-CO', {
      style: 'currency',
      currency: 'COP',
      minimumFractionDigits: 0,
      maximumFractionDigits: 0
    }).format(amount || 0);
  };

  const formatNumber = (number) => {
    return new Intl.NumberFormat('es-CO').format(number || 0);
  };

  return (
    <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-5 mb-6">
      <div className="bg-gradient-to-br from-blue-50 to-blue-100 border border-blue-200 rounded-lg p-4 text-center">
        <div className="text-2xl font-bold text-blue-800 mb-1">
          {formatNumber(totalCredits)}
        </div>
        <div className="text-xs text-blue-600 uppercase tracking-wide font-medium">
          Total Créditos
        </div>
        {dashboard?.totalCreditos && (
          <div className="text-xs text-blue-500 mt-1">
            📊 Datos del servidor
          </div>
        )}
      </div>
      
      <div className="bg-gradient-to-br from-green-50 to-green-100 border border-green-200 rounded-lg p-4 text-center">
        <div className="text-2xl font-bold text-green-800 mb-1">
          {formatCurrency(totalAmount)}
        </div>
        <div className="text-xs text-green-600 uppercase tracking-wide font-medium">
          Valor Total
        </div>
        {dashboard?.valorTotalCreditos && (
          <div className="text-xs text-green-500 mt-1">
            📊 Datos del servidor
          </div>
        )}
      </div>
      
      <div className="bg-gradient-to-br from-purple-50 to-purple-100 border border-purple-200 rounded-lg p-4 text-center">
        <div className="text-2xl font-bold text-purple-800 mb-1">
          {formatCurrency(avgAmount)}
        </div>
        <div className="text-xs text-purple-600 uppercase tracking-wide font-medium">
          Promedio por Crédito
        </div>
        {dashboard?.promedioCreditos && (
          <div className="text-xs text-purple-500 mt-1">
            📊 Datos del servidor
          </div>
        )}
      </div>
      
      <div className="bg-gradient-to-br from-amber-50 to-amber-100 border border-amber-200 rounded-lg p-4 text-center">
        <div className="text-2xl font-bold text-amber-800 mb-1">
          {formatNumber(activeCredits)}
        </div>
        <div className="text-xs text-amber-600 uppercase tracking-wide font-medium">
          Créditos Activos
        </div>
        {dashboard?.creditosActivos && (
          <div className="text-xs text-amber-500 mt-1">
            📊 Datos del servidor
          </div>
        )}
      </div>

      {/* Métricas adicionales si están disponibles en el dashboard */}
      {dashboard?.creditosPendientes !== undefined && (
        <div className="bg-gradient-to-br from-orange-50 to-orange-100 border border-orange-200 rounded-lg p-4 text-center">
          <div className="text-2xl font-bold text-orange-800 mb-1">
            {formatNumber(dashboard.creditosPendientes)}
          </div>
          <div className="text-xs text-orange-600 uppercase tracking-wide font-medium">
            Créditos Pendientes
          </div>
          <div className="text-xs text-orange-500 mt-1">
            📊 Datos del servidor
          </div>
        </div>
      )}

      {dashboard?.ultimoRegistro && (
        <div className="bg-gradient-to-br from-gray-50 to-gray-100 border border-gray-200 rounded-lg p-4 text-center">
          <div className="text-sm font-bold text-gray-800 mb-1">
            {new Date(dashboard.ultimoRegistro).toLocaleDateString('es-CO')}
          </div>
          <div className="text-xs text-gray-600 uppercase tracking-wide font-medium">
            Último Registro
          </div>
          <div className="text-xs text-gray-500 mt-1">
            📊 Datos del servidor
          </div>
        </div>
      )}
    </div>
  );
};

export default MetricsCards;