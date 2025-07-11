import axios from 'axios';

// =============================================
// CONFIGURACIÓN DE AXIOS PARA BACKEND REAL
// =============================================
const api = axios.create({
  baseURL: 'https://localhost:7014/api',
  timeout: 30000,
  headers: {
    'Content-Type': 'application/json',
    'Accept': 'application/json',
  },
});

// Interceptor para requests (agregar logs de debugging)
api.interceptors.request.use(
  (config) => {
    console.log(`🔄 API Request: ${config.method?.toUpperCase()} ${config.url}`, config.data);
    return config;
  },
  (error) => {
    console.error('❌ Request Error:', error);
    return Promise.reject(error);
  }
);

// Interceptor para responses (manejo de errores y logs)
api.interceptors.response.use(
  (response) => {
    console.log(`✅ API Response: ${response.config.url}`, response.data);
    return response;
  },
  (error) => {
    console.error('❌ API Error:', error);
    
    // Manejo específico de errores de la API
    if (error.response) {
      // Error con respuesta del servidor
      const { status, data } = error.response;
      console.error(`HTTP ${status}:`, data);
      
      // Estructura de error de tu backend .NET
      if (data && !data.exitoso) {
        error.message = data.mensaje || 'Error del servidor';
        error.details = data.errores || [];
      }
    } else if (error.request) {
      // Error de red/conexión
      console.error('Network Error:', error.request);
      error.message = 'Error de conexión con el servidor';
    }
    
    return Promise.reject(error);
  }
);

// =============================================
// SERVICIOS DE LA API
// =============================================

export const creditosAPI = {
  // Obtener todos los créditos con filtros
  obtenerCreditos: async (filtros = {}) => {
    const params = new URLSearchParams();
    
    if (filtros.filtroCliente) params.append('filtroCliente', filtros.filtroCliente);
    if (filtros.filtroIdentificacion) params.append('filtroIdentificacion', filtros.filtroIdentificacion);
    if (filtros.filtroComercial) params.append('filtroComercial', filtros.filtroComercial);
    if (filtros.estadoId) params.append('estadoId', filtros.estadoId);
    if (filtros.fechaDesde) params.append('fechaDesde', filtros.fechaDesde);
    if (filtros.fechaHasta) params.append('fechaHasta', filtros.fechaHasta);
    if (filtros.valorMinimo) params.append('valorMinimo', filtros.valorMinimo);
    if (filtros.valorMaximo) params.append('valorMaximo', filtros.valorMaximo);
    if (filtros.ordenarPor) params.append('ordenarPor', filtros.ordenarPor);
    if (filtros.ordenDireccion) params.append('ordenDireccion', filtros.ordenDireccion);
    if (filtros.pagina) params.append('pagina', filtros.pagina);
    if (filtros.tamañoPagina) params.append('tamañoPagina', filtros.tamañoPagina);
    
    const queryString = params.toString();
    const url = queryString ? `/creditos?${queryString}` : '/creditos';
    
    const response = await api.get(url);
    return response.data;
  },

  // Crear nuevo crédito
  crearCredito: async (creditoData) => {
    const response = await api.post('/creditos', creditoData);
    return response.data;
  },

  // Obtener crédito por ID
  obtenerCreditoPorId: async (creditoId) => {
    const response = await api.get(`/creditos/${creditoId}`);
    return response.data;
  },

  // Actualizar crédito
  actualizarCredito: async (creditoData) => {
    const response = await api.put('/creditos', creditoData);
    return response.data;
  },

  // Obtener métricas del dashboard
  obtenerDashboard: async () => {
    const response = await api.get('/creditos/dashboard');
    return response.data;
  }
};

export const comercialesAPI = {
  // Obtener comerciales activos
  obtenerComerciales: async () => {
    const response = await api.get('/comerciales');
    return response.data;
  },

  // Obtener comercial por ID
  obtenerComercialPorId: async (comercialId) => {
    const response = await api.get(`/comerciales/${comercialId}`);
    return response.data;
  }
};

export const estadosAPI = {
  // Obtener estados de crédito
  obtenerEstados: async () => {
    const response = await api.get('/estados');
    return response.data;
  }
};

export const correosAPI = {
  // Obtener logs de envío de correos
  obtenerLogsEnvio: async () => {
    const response = await api.get('/correos/logs');
    return response.data;
  },

  // Procesar envíos pendientes
  procesarEnviosPendientes: async () => {
    const response = await api.post('/correos/procesar-pendientes');
    return response.data;
  }
};

export const utilAPI = {
  // Health check
  verificarSalud: async () => {
    const response = await api.get('/util/health');
    return response.data;
  },

  // Información del sistema
  obtenerInformacion: async () => {
    const response = await api.get('/util/info');
    return response.data;
  },

  // Verificar conexión a base de datos
  verificarBaseDatos: async () => {
    const response = await api.get('/util/database-check');
    return response.data;
  }
};

export default api;