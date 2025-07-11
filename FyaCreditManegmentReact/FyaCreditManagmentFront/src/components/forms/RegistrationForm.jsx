import React, { useState, useEffect } from 'react';
import { useForm } from 'react-hook-form';
import useCredits from '../../hooks/useCredits';
import Alert from '../common/Alert';
import Loading from '../common/Loading';

const RegistrationForm = () => {
  const { register, handleSubmit, reset, formState: { errors } } = useForm();
  const { createCredit, loading, comerciales, fetchComerciales } = useCredits();
  const [alert, setAlert] = useState(null);

  // Cargar comerciales al montar el componente
  useEffect(() => {
    fetchComerciales();
  }, []);

  const onSubmit = async (data) => {
    console.log('📝 Datos del formulario:', data);
    
    const result = await createCredit(data);
    
    if (result.success) {
      setAlert({ 
        type: 'success', 
        message: `${result.message}. Se ha enviado una notificación automática por correo electrónico.`
      });
      reset();
      
      // Auto-ocultar mensaje después de 10 segundos
      setTimeout(() => setAlert(null), 10000);
      
      // Scroll al mensaje de éxito
      setTimeout(() => {
        const alertElement = document.querySelector('[role="alert"]');
        if (alertElement) {
          alertElement.scrollIntoView({ behavior: 'smooth', block: 'center' });
        }
      }, 100);
    } else {
      const errorMessage = result.errors && result.errors.length > 0 
        ? result.errors.join(', ') 
        : result.message;
      
      setAlert({ type: 'error', message: errorMessage });
      setTimeout(() => setAlert(null), 8000);
    }
  };

  const clearForm = () => {
    reset();
    setAlert(null);
  };

  return (
    <div className="bg-white border border-gray-200 rounded-xl p-8 shadow-sm">
      <div className="border-b-2 border-gray-100 pb-4 mb-8">
        <h2 className="text-2xl font-bold text-gray-800 mb-2">Registro de Nuevo Crédito</h2>
        <p className="text-gray-600 text-sm">
          Complete la información requerida para registrar un nuevo crédito en el sistema. 
          Todos los campos marcados con (*) son obligatorios.
        </p>
      </div>

      {alert && (
        <Alert 
          type={alert.type} 
          message={alert.message} 
          onClose={() => setAlert(null)} 
        />
      )}

      {loading && <Loading />}

      <form onSubmit={handleSubmit(onSubmit)} className={loading ? 'opacity-50 pointer-events-none' : ''}>
        {/* Información del Cliente */}
        <div className="mb-8">
          <h3 className="text-base font-semibold text-gray-800 mb-4 pb-2 border-b border-gray-200">
            Información del Cliente
          </h3>
          <div className="grid grid-cols-1 lg:grid-cols-2 gap-5 mb-6">
            <div>
              <label className="block mb-1.5 font-medium text-gray-700 text-sm">
                Nombre Completo del Cliente <span className="text-red-600">*</span>
              </label>
              <input
                type="text"
                {...register('clientName', { 
                  required: 'El nombre del cliente es requerido',
                  minLength: { value: 2, message: 'El nombre debe tener al menos 2 caracteres' },
                  maxLength: { value: 200, message: 'El nombre no puede exceder 200 caracteres' }
                })}
                className="w-full p-3 border border-gray-300 rounded-lg text-sm transition-all duration-200 focus:outline-none focus:border-blue-800 focus:ring-3 focus:ring-blue-100"
                placeholder="Ingrese el nombre completo"
              />
              {errors.clientName && (
                <p className="text-red-600 text-xs mt-1">{errors.clientName.message}</p>
              )}
            </div>
            
            <div>
              <label className="block mb-1.5 font-medium text-gray-700 text-sm">
                Número de Identificación <span className="text-red-600">*</span>
              </label>
              <input
                type="text"
                {...register('clientId', { 
                  required: 'El número de identificación es requerido',
                  minLength: { value: 5, message: 'La identificación debe tener al menos 5 caracteres' },
                  maxLength: { value: 50, message: 'La identificación no puede exceder 50 caracteres' }
                })}
                className="w-full p-3 border border-gray-300 rounded-lg text-sm transition-all duration-200 focus:outline-none focus:border-blue-800 focus:ring-3 focus:ring-blue-100"
                placeholder="Cédula, NIT o documento de identidad"
              />
              {errors.clientId && (
                <p className="text-red-600 text-xs mt-1">{errors.clientId.message}</p>
              )}
            </div>
          </div>

          {/* Campos adicionales del cliente (opcionales) */}
          <div className="grid grid-cols-1 lg:grid-cols-2 gap-5 mb-6">
            <div>
              <label className="block mb-1.5 font-medium text-gray-700 text-sm">
                Email del Cliente
              </label>
              <input
                type="email"
                {...register('emailCliente', {
                  pattern: {
                    value: /^[^\s@]+@[^\s@]+\.[^\s@]+$/,
                    message: 'Ingrese un email válido'
                  },
                  maxLength: { value: 100, message: 'El email no puede exceder 100 caracteres' }
                })}
                className="w-full p-3 border border-gray-300 rounded-lg text-sm transition-all duration-200 focus:outline-none focus:border-blue-800 focus:ring-3 focus:ring-blue-100"
                placeholder="email@ejemplo.com"
              />
              {errors.emailCliente && (
                <p className="text-red-600 text-xs mt-1">{errors.emailCliente.message}</p>
              )}
            </div>
            
            <div>
              <label className="block mb-1.5 font-medium text-gray-700 text-sm">
                Teléfono del Cliente
              </label>
              <input
                type="tel"
                {...register('telefonoCliente', {
                  maxLength: { value: 20, message: 'El teléfono no puede exceder 20 caracteres' }
                })}
                className="w-full p-3 border border-gray-300 rounded-lg text-sm transition-all duration-200 focus:outline-none focus:border-blue-800 focus:ring-3 focus:ring-blue-100"
                placeholder="Número de teléfono"
              />
              {errors.telefonoCliente && (
                <p className="text-red-600 text-xs mt-1">{errors.telefonoCliente.message}</p>
              )}
            </div>
          </div>

          <div className="grid grid-cols-1 lg:grid-cols-2 gap-5 mb-6">
            <div>
              <label className="block mb-1.5 font-medium text-gray-700 text-sm">
                Dirección del Cliente
              </label>
              <input
                type="text"
                {...register('direccionCliente', {
                  maxLength: { value: 300, message: 'La dirección no puede exceder 300 caracteres' }
                })}
                className="w-full p-3 border border-gray-300 rounded-lg text-sm transition-all duration-200 focus:outline-none focus:border-blue-800 focus:ring-3 focus:ring-blue-100"
                placeholder="Dirección completa"
              />
              {errors.direccionCliente && (
                <p className="text-red-600 text-xs mt-1">{errors.direccionCliente.message}</p>
              )}
            </div>
            
            <div>
              <label className="block mb-1.5 font-medium text-gray-700 text-sm">
                Ciudad del Cliente
              </label>
              <input
                type="text"
                {...register('ciudadCliente', {
                  maxLength: { value: 100, message: 'La ciudad no puede exceder 100 caracteres' }
                })}
                className="w-full p-3 border border-gray-300 rounded-lg text-sm transition-all duration-200 focus:outline-none focus:border-blue-800 focus:ring-3 focus:ring-blue-100"
                placeholder="Ciudad de residencia"
              />
              {errors.ciudadCliente && (
                <p className="text-red-600 text-xs mt-1">{errors.ciudadCliente.message}</p>
              )}
            </div>
          </div>
        </div>

        {/* Detalles del Crédito */}
        <div className="mb-8">
          <h3 className="text-base font-semibold text-gray-800 mb-4 pb-2 border-b border-gray-200">
            Detalles del Crédito
          </h3>
          <div className="grid grid-cols-1 lg:grid-cols-3 gap-5 mb-6">
            <div>
              <label className="block mb-1.5 font-medium text-gray-700 text-sm">
                Valor del Crédito (COP) <span className="text-red-600">*</span>
              </label>
              <input
                type="number"
                {...register('creditAmount', { 
                  required: 'El valor del crédito es requerido',
                  min: { value: 100000, message: 'El valor mínimo es $100,000' },
                  max: { value: 999999999999, message: 'El valor máximo es $999,999,999,999' }
                })}
                className="w-full p-3 border border-gray-300 rounded-lg text-sm transition-all duration-200 focus:outline-none focus:border-blue-800 focus:ring-3 focus:ring-blue-100"
                placeholder="100000"
                min="100000"
                step="1000"
              />
              {errors.creditAmount && (
                <p className="text-red-600 text-xs mt-1">{errors.creditAmount.message}</p>
              )}
            </div>
            
            <div>
              <label className="block mb-1.5 font-medium text-gray-700 text-sm">
                Tasa de Interés (%) <span className="text-red-600">*</span>
              </label>
              <input
                type="number"
                {...register('interestRate', { 
                  required: 'La tasa de interés es requerida',
                  min: { value: 0.01, message: 'La tasa mínima es 0.01%' },
                  max: { value: 50, message: 'La tasa máxima es 50%' }
                })}
                className="w-full p-3 border border-gray-300 rounded-lg text-sm transition-all duration-200 focus:outline-none focus:border-blue-800 focus:ring-3 focus:ring-blue-100"
                placeholder="2.00"
                min="0.01"
                max="50"
                step="0.01"
              />
              {errors.interestRate && (
                <p className="text-red-600 text-xs mt-1">{errors.interestRate.message}</p>
              )}
            </div>
            
            <div>
              <label className="block mb-1.5 font-medium text-gray-700 text-sm">
                Plazo (Meses) <span className="text-red-600">*</span>
              </label>
              <input
                type="number"
                {...register('termMonths', { 
                  required: 'El plazo es requerido',
                  min: { value: 1, message: 'El plazo mínimo es 1 mes' },
                  max: { value: 360, message: 'El plazo máximo es 360 meses' }
                })}
                className="w-full p-3 border border-gray-300 rounded-lg text-sm transition-all duration-200 focus:outline-none focus:border-blue-800 focus:ring-3 focus:ring-blue-100"
                placeholder="12"
                min="1"
                max="360"
              />
              {errors.termMonths && (
                <p className="text-red-600 text-xs mt-1">{errors.termMonths.message}</p>
              )}
            </div>
          </div>
        </div>

        {/* Información Comercial */}
        <div className="mb-8">
          <h3 className="text-base font-semibold text-gray-800 mb-4 pb-2 border-b border-gray-200">
            Información Comercial
          </h3>
          <div className="grid grid-cols-1 lg:grid-cols-2 gap-5 mb-6">
            <div>
              <label className="block mb-1.5 font-medium text-gray-700 text-sm">
                Ejecutivo Comercial <span className="text-red-600">*</span>
              </label>
              <select
                {...register('salesRep', { required: 'Debe seleccionar un ejecutivo comercial' })}
                className="w-full p-3 border border-gray-300 rounded-lg text-sm transition-all duration-200 focus:outline-none focus:border-blue-800 focus:ring-3 focus:ring-blue-100"
              >
                <option value="">Seleccione un ejecutivo comercial</option>
                {comerciales.map((comercial) => (
                  <option key={comercial.comercialId} value={comercial.nombre}>
                    {comercial.nombre}
                  </option>
                ))}
              </select>
              {errors.salesRep && (
                <p className="text-red-600 text-xs mt-1">{errors.salesRep.message}</p>
              )}
              {comerciales.length === 0 && (
                <p className="text-amber-600 text-xs mt-1">
                  ⚠️ Cargando ejecutivos comerciales...
                </p>
              )}
            </div>
          </div>
        </div>

        {/* Botones */}
        <div className="flex justify-end gap-4 mt-8 pt-6 border-t border-gray-200">
          <button
            type="button"
            onClick={clearForm}
            disabled={loading}
            className="px-6 py-3 border border-gray-300 text-gray-700 font-semibold rounded-lg text-sm uppercase tracking-wide transition-all duration-200 hover:bg-gray-50 hover:border-gray-400 disabled:opacity-50 disabled:cursor-not-allowed"
          >
            Limpiar Formulario
          </button>
          <button
            type="submit"
            disabled={loading || comerciales.length === 0}
            className="px-6 py-3 bg-blue-800 text-white font-semibold rounded-lg text-sm uppercase tracking-wide transition-all duration-200 hover:bg-blue-900 disabled:opacity-50 disabled:cursor-not-allowed"
          >
            {loading ? 'Registrando...' : 'Registrar Crédito'}
          </button>
        </div>
      </form>
    </div>
  );
};

export default RegistrationForm;