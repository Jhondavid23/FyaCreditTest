import React, { useState, useEffect } from 'react';
import './App.css';

// Importar componentes
import Header from './components/common/Header';
import Navigation from './components/common/Navigation';
import RegistrationForm from './components/forms/RegistrationForm';
import QueryPanel from './components/panels/QueryPanel';
import { utilAPI } from './services/api';

// =============================================
// COMPONENTE DE ERROR BOUNDARY
// =============================================
class ErrorBoundary extends React.Component {
  constructor(props) {
    super(props);
    this.state = { hasError: false, error: null };
  }

  static getDerivedStateFromError(error) {
    return { hasError: true, error };
  }

  componentDidCatch(error, errorInfo) {
    console.error('Error capturado por ErrorBoundary:', error, errorInfo);
  }

  render() {
    if (this.state.hasError) {
      return (
        <div className="min-h-screen bg-gray-50 py-6">
          <div className="max-w-7xl mx-auto px-6">
            <div className="bg-white border border-red-200 rounded-xl p-8 shadow-sm">
              <div className="text-center">
                <div className="text-red-500 text-6xl mb-4">⚠️</div>
                <h2 className="text-2xl font-bold text-red-700 mb-2">Error de la Aplicación</h2>
                <p className="text-gray-600 mb-4">
                  Ha ocurrido un error inesperado. Por favor, recargue la página.
                </p>
                <button
                  onClick={() => window.location.reload()}
                  className="px-6 py-3 bg-red-600 text-white rounded-lg hover:bg-red-700 transition-colors"
                >
                  Recargar Página
                </button>
                {process.env.NODE_ENV === 'development' && (
                  <details className="mt-4 text-left">
                    <summary className="cursor-pointer text-sm text-gray-500">
                      Detalles del error (desarrollo)
                    </summary>
                    <pre className="mt-2 p-4 bg-gray-100 rounded text-xs overflow-auto">
                      {this.state.error?.toString()}
                    </pre>
                  </details>
                )}
              </div>
            </div>
          </div>
        </div>
      );
    }

    return this.props.children;
  }
}

// =============================================
// COMPONENTE DE VERIFICACIÓN DE CONEXIÓN
// =============================================
const ConnectionStatus = ({ isConnected, onRetry }) => {
  if (isConnected) return null;

  return (
    <div className="bg-red-50 border border-red-200 rounded-xl p-4 mb-6">
      <div className="flex items-center justify-between">
        <div className="flex items-center">
          <div className="text-red-500 mr-3">❌</div>
          <div>
            <h3 className="text-red-700 font-semibold">Sin conexión al servidor</h3>
            <p className="text-red-600 text-sm">
              No se puede conectar con el backend en https://localhost:7014/
            </p>
          </div>
        </div>
        <button
          onClick={onRetry}
          className="px-4 py-2 bg-red-600 text-white rounded-lg hover:bg-red-700 transition-colors text-sm"
        >
          Reintentar
        </button>
      </div>
    </div>
  );
};

// =============================================
// COMPONENTE PRINCIPAL
// =============================================
function App() {
  const [activeTab, setActiveTab] = useState('register');
  const [connectionStatus, setConnectionStatus] = useState({
    isConnected: false,
    isChecking: true,
    lastCheck: null
  });

  // Verificar conexión con el backend
  const checkConnection = async () => {
    try {
      setConnectionStatus(prev => ({ ...prev, isChecking: true }));
      
      const response = await utilAPI.verificarSalud();
      
      if (response && response.estado === 'saludable') {
        setConnectionStatus({
          isConnected: true,
          isChecking: false,
          lastCheck: new Date()
        });
        console.log('✅ Conexión con backend establecida:', response);
      } else {
        throw new Error('Respuesta inesperada del servidor');
      }
    } catch (error) {
      console.error('❌ Error de conexión con backend:', error);
      setConnectionStatus({
        isConnected: false,
        isChecking: false,
        lastCheck: new Date()
      });
    }
  };

  // Verificar conexión al cargar la app
  useEffect(() => {
    checkConnection();

    // Verificar conexión periódicamente
    const interval = setInterval(checkConnection, 30000); // Cada 30 segundos

    return () => clearInterval(interval);
  }, []);

  // Handler para reintentar conexión
  const handleRetryConnection = () => {
    console.log('🔄 Reintentando conexión...');
    checkConnection();
  };

  return (
    <ErrorBoundary>
      <div className="min-h-screen bg-gray-50 py-6">
        <div className="max-w-7xl mx-auto px-6">
          <Header />
          
          <ConnectionStatus 
            isConnected={connectionStatus.isConnected} 
            onRetry={handleRetryConnection}
          />
          
          <Navigation activeTab={activeTab} setActiveTab={setActiveTab} />
          
          {/* Mostrar indicador de estado de conexión */}
          {connectionStatus.isChecking && (
            <div className="bg-blue-50 border border-blue-200 rounded-xl p-4 mb-6">
              <div className="flex items-center">
                <div className="animate-spin rounded-full h-4 w-4 border-b-2 border-blue-600 mr-3"></div>
                <span className="text-blue-700 text-sm">
                  Verificando conexión con el servidor...
                </span>
              </div>
            </div>
          )}
          
          {/* Contenido principal */}
          {activeTab === 'register' ? (
            <RegistrationForm />
          ) : (
            <QueryPanel />
          )}

          {/* Footer con información de estado */}
          <div className="mt-8 p-4 bg-white border border-gray-200 rounded-xl shadow-sm">
            <div className="flex justify-between items-center text-xs text-gray-500">
              <div>
                <span className="font-medium">FYA Social Capital</span> - Sistema de Gestión de Créditos v1.0
              </div>
              <div className="flex items-center space-x-4">
                <span>
                  Estado: 
                  <span className={connectionStatus.isConnected ? 'text-green-600' : 'text-red-600'}>
                    {connectionStatus.isConnected ? ' ✅ Conectado' : ' ❌ Desconectado'}
                  </span>
                </span>
                {connectionStatus.lastCheck && (
                  <span>
                    Última verificación: {connectionStatus.lastCheck.toLocaleTimeString()}
                  </span>
                )}
                <span>
                  Backend: https://localhost:7014/
                </span>
              </div>
            </div>
          </div>
        </div>
      </div>
    </ErrorBoundary>
  );
}

export default App;