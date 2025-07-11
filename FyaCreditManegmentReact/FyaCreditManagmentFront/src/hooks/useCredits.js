import { useState, useEffect } from 'react';
import { creditosAPI, comercialesAPI } from '../services/api';

const useCredits = () => {
  const [credits, setCredits] = useState([]);
  const [comerciales, setComerciales] = useState([]);
  const [dashboard, setDashboard] = useState({});
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState(null);

  // =============================================
  // FUNCIONES DE LA API
  // =============================================

  const fetchCredits = async (filtros = {}) => {
    try {
      setLoading(true);
      setError(null);
      
      const response = await creditosAPI.obtenerCreditos(filtros);
      
      // Manejar la estructura de respuesta del backend .NET
      if (response.creditos) {
        setCredits(response.creditos);
        return response;
      } else {
        setCredits([]);
        return { creditos: [], totalRegistros: 0 };
      }
    } catch (err) {
      setError(err.message || 'Error al cargar los créditos');
      console.error('Error fetching credits:', err);
      return { creditos: [], totalRegistros: 0 };
    } finally {
      setLoading(false);
    }
  };

  const fetchComerciales = async () => {
    try {
      const response = await comercialesAPI.obtenerComerciales();
      setComerciales(response || []);
      return response;
    } catch (err) {
      console.error('Error fetching comerciales:', err);
      setComerciales([]);
      return [];
    }
  };

  const fetchDashboard = async () => {
    try {
      const response = await creditosAPI.obtenerDashboard();
      setDashboard(response || {});
      return response;
    } catch (err) {
      console.error('Error fetching dashboard:', err);
      setDashboard({});
      return {};
    }
  };

  const createCredit = async (creditData) => {
    try {
      setLoading(true);
      setError(null);

      // Mapear datos del formulario al formato esperado por el backend
      const creditoRequest = {
        nombreCliente: creditData.clientName,
        numeroIdentificacion: creditData.clientId,
        tipoIdentificacion: 'CC', // Por defecto
        emailCliente: creditData.emailCliente || '',
        telefonoCliente: creditData.telefonoCliente || '',
        direccionCliente: creditData.direccionCliente || '',
        ciudadCliente: creditData.ciudadCliente || '',
        comercialId: getSalesRepId(creditData.salesRep),
        valorCredito: parseFloat(creditData.creditAmount),
        tasaInteres: parseFloat(creditData.interestRate),
        plazoMeses: parseInt(creditData.termMonths),
        usuarioCreacion: 'Sistema'
      };

      console.log('🔄 Enviando datos al backend:', creditoRequest);

      const response = await creditosAPI.crearCredito(creditoRequest);

      if (response.exitoso) {
        // Recargar la lista de créditos después de crear uno nuevo
        await fetchCredits();
        await fetchDashboard(); // Actualizar métricas también
        
        return { 
          success: true, 
          message: response.mensaje,
          data: response.datos
        };
      } else {
        return { 
          success: false, 
          message: response.mensaje, 
          errors: response.errores || []
        };
      }
    } catch (err) {
      console.error('Error creating credit:', err);
      
      let errorMessage = 'Error al registrar el crédito';
      let errors = [];

      if (err.response?.data) {
        errorMessage = err.response.data.mensaje || errorMessage;
        errors = err.response.data.errores || [err.message];
      } else {
        errors = [err.message];
      }

      return { 
        success: false, 
        message: errorMessage,
        errors: errors
      };
    } finally {
      setLoading(false);
    }
  };

  // Mapear nombres de comerciales a IDs (esto debería venir de la API)
  const getSalesRepId = (salesRepName) => {
    const salesRepsMap = {
      "Juan Carlos Martínez": 1,
      "María Elena González": 2,
      "Carlos Alberto Rodríguez": 3,
      "Ana Patricia Jiménez": 4,
      "Pedro Alejandro Sánchez": 5
    };
    return salesRepsMap[salesRepName] || 1;
  };

  // Mapear IDs de comerciales a nombres
  const getSalesRepName = (comercialId) => {
    const comercial = comerciales.find(c => c.comercialId === comercialId);
    return comercial?.nombre || 'No asignado';
  };

  // =============================================
  // FUNCIONES DE TRANSFORMACIÓN DE DATOS
  // =============================================

  const transformCreditData = (creditFromAPI) => {
    return {
      id: creditFromAPI.creditoId,
      clientName: creditFromAPI.cliente?.nombre || creditFromAPI.nombreCliente,
      clientId: creditFromAPI.cliente?.numeroIdentificacion || creditFromAPI.numeroIdentificacion,
      amount: creditFromAPI.valorCredito,
      interestRate: creditFromAPI.tasaInteres,
      termMonths: creditFromAPI.plazoMeses,
      salesRep: creditFromAPI.comercial?.nombre || creditFromAPI.nombreComercial,
      date: creditFromAPI.fechaRegistro,
      status: creditFromAPI.estado?.descripcion === 'Activo' ? 'active' : 'pending',
      valorCuota: creditFromAPI.valorCuota,
      fechaAprobacion: creditFromAPI.fechaAprobacion
    };
  };

  // =============================================
  // EFECTOS
  // =============================================

  useEffect(() => {
    // Cargar datos iniciales
    const loadInitialData = async () => {
      console.log('🚀 Cargando datos iniciales...');
      
      try {
        // Cargar comerciales primero
        await fetchComerciales();
        
        // Luego cargar créditos
        await fetchCredits();
        
        // Y finalmente métricas del dashboard
        await fetchDashboard();
        
        console.log('✅ Datos iniciales cargados');
      } catch (error) {
        console.error('❌ Error cargando datos iniciales:', error);
      }
    };

    loadInitialData();
  }, []);

  // =============================================
  // FUNCIONES AUXILIARES
  // =============================================

  const formatCreditsData = (creditsFromAPI) => {
    if (!creditsFromAPI || !Array.isArray(creditsFromAPI)) {
      return [];
    }
    
    return creditsFromAPI.map(transformCreditData);
  };

  // =============================================
  // RETURN DEL HOOK
  // =============================================

  return { 
    credits: formatCreditsData(credits),
    comerciales,
    dashboard,
    setCredits, 
    loading, 
    error, 
    fetchCredits, 
    fetchComerciales,
    fetchDashboard,
    createCredit,
    getSalesRepName,
    transformCreditData
  };
};

export default useCredits;