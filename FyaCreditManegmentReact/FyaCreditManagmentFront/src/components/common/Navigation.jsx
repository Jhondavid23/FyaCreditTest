const Navigation = ({ activeTab, setActiveTab }) => (
  <div className="bg-white border border-gray-200 rounded-xl mb-6 overflow-hidden">
    <div className="flex bg-gray-50">
      <button
        className={`flex-1 py-4 px-6 text-sm font-semibold uppercase tracking-wide transition-all duration-200 border-r border-gray-200 ${
          activeTab === 'register'
            ? 'bg-blue-800 text-white'
            : 'text-gray-600 hover:bg-gray-200 hover:text-gray-800'
        }`}
        onClick={() => setActiveTab('register')}
      >
        Registro de Créditos
      </button>
      <button
        className={`flex-1 py-4 px-6 text-sm font-semibold uppercase tracking-wide transition-all duration-200 ${
          activeTab === 'query'
            ? 'bg-blue-800 text-white'
            : 'text-gray-600 hover:bg-gray-200 hover:text-gray-800'
        }`}
        onClick={() => setActiveTab('query')}
      >
        Consulta y Reportes
      </button>
    </div>
  </div>
);
export default Navigation;