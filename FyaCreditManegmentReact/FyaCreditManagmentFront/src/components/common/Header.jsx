import React from 'react';

const Header = () => (
  <div className="bg-white border border-gray-200 rounded-xl p-8 mb-6 shadow-sm">
    <div className="flex items-center justify-between flex-wrap gap-5">
      <div className="flex items-center">
        <div className="w-14 h-14 bg-blue-800 rounded-lg flex items-center justify-center mr-4">
          <svg width="24" height="24" viewBox="0 0 24 24" fill="white">
            <path d="M3 6h18v2H3V6zm0 5h18v2H3v-2zm0 5h18v2H3v-2z"/>
            <path d="M7 6v12h2V6H7zm4 0v12h2V6h-2zm4 0v12h2V6h-2z"/>
          </svg>
        </div>
        <div>
          <h1 className="text-3xl font-bold text-gray-800 mb-1">FYA SOCIAL CAPITAL</h1>
          <p className="text-gray-600 text-sm font-medium">SOLUCIONES FINANCIERAS CORPORATIVAS</p>
        </div>
      </div>
      <div className="text-right">
        <div className="text-xl font-semibold text-blue-800 mb-1">Sistema de Gestión de Créditos</div>
        <div className="text-gray-600 text-sm">Versión 1.0 • Proceso: Evaluación Técnica</div>
      </div>
    </div>
  </div>
);
export default Header;