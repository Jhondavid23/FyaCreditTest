const CreditsTable = ({ credits }) => {
  const formatCurrency = (amount) => {
    return new Intl.NumberFormat('es-CO', {
      style: 'currency',
      currency: 'COP',
      minimumFractionDigits: 0,
      maximumFractionDigits: 0
    }).format(amount || 0);
  };

  const formatDate = (dateString) => {
    if (!dateString) return 'N/A';
    
    try {
      const date = new Date(dateString);
      return date.toLocaleDateString('es-CO', {
        year: 'numeric',
        month: '2-digit',
        day: '2-digit'
      });
    } catch (error) {
      return 'Fecha inválida';
    }
  };

  const formatDateTime = (dateString) => {
    if (!dateString) return 'N/A';
    
    try {
      const date = new Date(dateString);
      return date.toLocaleString('es-CO', {
        year: 'numeric',
        month: '2-digit',
        day: '2-digit',
        hour: '2-digit',
        minute: '2-digit'
      });
    } catch (error) {
      return 'Fecha inválida';
    }
  };

  const getStatusBadge = (status) => {
    const statusMap = {
      'Activo': { class: 'bg-green-100 text-green-800', label: 'Activo' },
      'Pendiente': { class: 'bg-yellow-100 text-yellow-800', label: 'Pendiente' },
      'Aprobado': { class: 'bg-blue-100 text-blue-800', label: 'Aprobado' },
      'Rechazado': { class: 'bg-red-100 text-red-800', label: 'Rechazado' },
      'active': { class: 'bg-green-100 text-green-800', label: 'Activo' },
      'pending': { class: 'bg-yellow-100 text-yellow-800', label: 'Pendiente' }
    };

    const statusInfo = statusMap[status] || { class: 'bg-gray-100 text-gray-800', label: status || 'Desconocido' };
    
    return (
      <span className={`px-2 py-1 rounded text-xs font-semibold uppercase tracking-wide ${statusInfo.class}`}>
        {statusInfo.label}
      </span>
    );
  };

  if (!credits || credits.length === 0) {
    return (
      <div className="border border-gray-200 rounded-lg overflow-hidden bg-white">
        <div className="bg-gray-50 px-6 py-4 border-b border-gray-200 flex justify-between items-center">
          <div className="font-semibold text-gray-800">Registro de Créditos</div>
          <div className="bg-gray-100 text-gray-600 px-2 py-1 rounded text-xs font-semibold">
            0 registros
          </div>
        </div>
        
        <div className="text-center py-20 text-gray-600">
          <div className="text-5xl mb-4 opacity-50">📋</div>
          <h3 className="text-gray-700 mb-2 text-lg font-semibold">No hay créditos registrados</h3>
          <p className="text-sm">No se encontraron registros que coincidan con los criterios de búsqueda.</p>
          <p className="text-xs text-gray-500 mt-2">
            Verifique los filtros aplicados o intente cargar los datos nuevamente.
          </p>
        </div>
      </div>
    );
  }

  return (
    <div className="border border-gray-200 rounded-lg overflow-hidden bg-white shadow-sm">
      <div className="bg-gray-50 px-6 py-4 border-b border-gray-200 flex justify-between items-center">
        <div className="font-semibold text-gray-800">Registro de Créditos</div>
        <div className="bg-blue-100 text-blue-800 px-2 py-1 rounded text-xs font-semibold">
          {credits.length} registro{credits.length !== 1 ? 's' : ''}
        </div>
      </div>
      
      <div className="overflow-x-auto">
        <table className="w-full border-collapse min-w-[1200px]">
          <thead>
            <tr className="bg-gray-50">
              <th className="text-left py-3 px-4 text-gray-700 font-semibold text-xs uppercase tracking-wide border-b border-gray-200 hover:bg-gray-100 cursor-pointer transition-colors">
                ID
              </th>
              <th className="text-left py-3 px-4 text-gray-700 font-semibold text-xs uppercase tracking-wide border-b border-gray-200 hover:bg-gray-100 cursor-pointer transition-colors">
                Cliente
              </th>
              <th className="text-left py-3 px-4 text-gray-700 font-semibold text-xs uppercase tracking-wide border-b border-gray-200 hover:bg-gray-100 cursor-pointer transition-colors">
                Identificación
              </th>
              <th className="text-left py-3 px-4 text-gray-700 font-semibold text-xs uppercase tracking-wide border-b border-gray-200 hover:bg-gray-100 cursor-pointer transition-colors">
                Valor Crédito
              </th>
              <th className="text-left py-3 px-4 text-gray-700 font-semibold text-xs uppercase tracking-wide border-b border-gray-200 hover:bg-gray-100 cursor-pointer transition-colors">
                Tasa %
              </th>
              <th className="text-left py-3 px-4 text-gray-700 font-semibold text-xs uppercase tracking-wide border-b border-gray-200 hover:bg-gray-100 cursor-pointer transition-colors">
                Plazo
              </th>
              <th className="text-left py-3 px-4 text-gray-700 font-semibold text-xs uppercase tracking-wide border-b border-gray-200 hover:bg-gray-100 cursor-pointer transition-colors">
                Cuota Mensual
              </th>
              <th className="text-left py-3 px-4 text-gray-700 font-semibold text-xs uppercase tracking-wide border-b border-gray-200 hover:bg-gray-100 cursor-pointer transition-colors">
                Valor Total
              </th>
              <th className="text-left py-3 px-4 text-gray-700 font-semibold text-xs uppercase tracking-wide border-b border-gray-200 hover:bg-gray-100 cursor-pointer transition-colors">
                Ejecutivo Comercial
              </th>
              <th className="text-left py-3 px-4 text-gray-700 font-semibold text-xs uppercase tracking-wide border-b border-gray-200 hover:bg-gray-100 cursor-pointer transition-colors">
                Fecha Registro
              </th>
              <th className="text-left py-3 px-4 text-gray-700 font-semibold text-xs uppercase tracking-wide border-b border-gray-200">
                Estado
              </th>
            </tr>
          </thead>
          <tbody>
            {credits.map((credit, index) => (
              <tr key={credit.creditoId || credit.id || index} className="hover:bg-gray-50 transition-colors duration-150">
                <td className="py-3 px-4 border-b border-gray-100 text-sm">
                  <span className="font-mono text-blue-600 font-semibold">
                    #{credit.creditoId || credit.id}
                  </span>
                </td>
                <td className="py-3 px-4 border-b border-gray-100 text-sm">
                  <div>
                    <div className="font-semibold text-gray-900">
                      {credit.nombreCliente || credit.clientName}
                    </div>
                  </div>
                </td>
                <td className="py-3 px-4 border-b border-gray-100 text-sm">
                  <span className="font-mono text-gray-700">
                    {credit.numeroIdentificacion || credit.clientId}
                  </span>
                </td>
                <td className="py-3 px-4 border-b border-gray-100 text-sm">
                  <div className="font-semibold text-green-600">
                    {formatCurrency(credit.valorCredito || credit.amount)}
                  </div>
                </td>
                <td className="py-3 px-4 border-b border-gray-100 text-sm">
                  <span className="font-semibold text-blue-600">
                    {(credit.tasaInteres || credit.interestRate)?.toFixed(2)}%
                  </span>
                </td>
                <td className="py-3 px-4 border-b border-gray-100 text-sm">
                  <span className="font-medium text-gray-700">
                    {credit.plazoMeses || credit.termMonths} meses
                  </span>
                </td>
                <td className="py-3 px-4 border-b border-gray-100 text-sm">
                  {(credit.valorCuota || credit.valorCuotaMensual) ? (
                    <div className="font-semibold text-purple-600">
                      {formatCurrency(credit.valorCuota || credit.valorCuotaMensual)}
                    </div>
                  ) : (
                    <span className="text-gray-400 text-xs italic">Calculando...</span>
                  )}
                </td>
                <td className="py-3 px-4 border-b border-gray-100 text-sm">
                  {(credit.valorTotal || credit.valorTotalAPagar) ? (
                    <div className="font-semibold text-orange-600">
                      {formatCurrency(credit.valorTotal || credit.valorTotalAPagar)}
                    </div>
                  ) : (
                    <span className="text-gray-400 text-xs italic">Calculando...</span>
                  )}
                </td>
                <td className="py-3 px-4 border-b border-gray-100 text-sm">
                  <div className="font-medium text-gray-800">
                    {credit.nombreComercial || credit.salesRep}
                  </div>
                </td>
                <td className="py-3 px-4 border-b border-gray-100 text-sm">
                  <div className="text-gray-600 text-xs">
                    {formatDate(credit.fechaRegistro || credit.date)}
                  </div>
                  <div className="text-gray-400 text-xs">
                    {formatDateTime(credit.fechaRegistro || credit.date).split(' ')[1]}
                  </div>
                </td>
                <td className="py-3 px-4 border-b border-gray-100 text-sm">
                  {getStatusBadge(credit.estadoCredito || credit.status)}
                  {(credit.fechaAprobacion || credit.fechaAprobacion) && (
                    <div className="text-xs text-gray-500 mt-1">
                      Aprobado: {formatDate(credit.fechaAprobacion)}
                    </div>
                  )}
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>

      {/* Footer de la tabla con información adicional */}
      <div className="bg-gray-50 px-6 py-3 border-t border-gray-200">
        <div className="flex justify-between items-center text-xs text-gray-600">
          <div className="flex items-center space-x-4">
            <span>📊 Mostrando {credits.length} registro{credits.length !== 1 ? 's' : ''}</span>
            <span>💰 Total: {formatCurrency(credits.reduce((sum, credit) => sum + (credit.valorCredito || credit.amount || 0), 0))}</span>
          </div>
          <div className="flex items-center space-x-2">
            <span className="inline-block w-2 h-2 bg-green-500 rounded-full"></span>
            <span>Datos del servidor en tiempo real</span>
          </div>
        </div>
      </div>
    </div>
  );
};

export default CreditsTable;