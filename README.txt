# Sistema de Gestión de Créditos - FYA Social Capital

**Versión:** 1.0.0  
**Fecha:** Julio 2025  
**Propósito:** Evaluación Técnica - Proceso de Contratación

## Descripción del Proyecto

Sistema web completo para la gestión de créditos que permite registrar, consultar y administrar créditos de manera eficiente. El sistema incluye funcionalidades de envío automático de notificaciones por correo electrónico y un módulo de consulta avanzado con filtros y ordenamiento.

## Tecnologías Utilizadas

### Backend
- **.NET 8.0** - Framework principal
- **ASP.NET Core Web API** - API REST
- **Entity Framework Core** - ORM para acceso a datos
- **AutoMapper** - Mapeo de objetos
- **SQL Server** - Base de datos relacional

### Frontend
- **React 18** - Framework de interfaz de usuario
- **Vite** - Herramienta de desarrollo
- **Tailwind CSS** - Framework de estilos
- **React Hook Form** - Manejo de formularios
- **Axios** - Cliente HTTP

### Características Adicionales
- **Logging integrado** - Seguimiento de operaciones
- **Validación de datos** - Frontend y backend
- **Envío de correos SMTP** - Notificaciones automáticas
- **Arquitectura en capas** - Repository Pattern + Services
- **Auditoría de cambios** - Trazabilidad completa

## Funcionalidades Implementadas

### 1. Registro de Créditos
- Formulario completo con validación
- Información del cliente (nombre, identificación, contacto)
- Detalles del crédito (valor, tasa, plazo)
- Selección de ejecutivo comercial
- Cálculo automático de cuotas y valor total

### 2. Envío Automático de Correos (RPA)
- Notificación automática al registrar un crédito
- Envío a fyasocialcapital@gmail.com
- Formato HTML profesional con toda la información
- Log de envíos para trazabilidad
- Tolerante a fallos (no bloquea el registro)

### 3. Módulo de Consulta
- Visualización de todos los créditos en tabla
- Filtros por cliente, identificación y comercial
- Ordenamiento por fecha y valor
- Paginación de resultados
- Métricas y estadísticas generales

### 4. Características Técnicas
- API REST documentada
- Manejo de errores centralizado
- Validaciones exhaustivas
- Auditoría automática de cambios
- Arquitectura escalable

## Requisitos del Sistema

### Software Necesario
- **SQL Server** (LocalDB, Express o completo)
- **.NET 8.0 SDK** o superior
- **Node.js** 18+ y npm
- **Visual Studio 2022** o VS Code (recomendado)
- **SQL Server Management Studio** (opcional)

### Hardware Mínimo
- 4 GB RAM
- 2 GB espacio en disco
- Procesador dual core

## Instalación y Configuración

### 1. Clonar el Repositorio
```bash
git clone [URL_DEL_REPOSITORIO]
cd FyaCreditManagement
```

### 2. Configurar la Base de Datos

#### Opción A: SQL Server Local (LocalDB)
1. Abrir SQL Server Management Studio
2. Conectarse a `(localdb)\MSSQLLocalDB`
3. Ejecutar el script de la base de datos ubicado en la raíz del proyecto

#### Opción B: SQL Server en Servidor
1. Conectarse a su instancia de SQL Server
2. Ejecutar el script completo de la base de datos
3. Anotar la cadena de conexión

#### Script de Base de Datos
El archivo de script SQL está incluido en el proyecto y contiene:
- Creación de todas las tablas
- Índices optimizados
- Triggers de auditoría
- Constraints y validaciones

### 3. Configurar el Backend

#### 3.1 Cadena de Conexión
Editar `appsettings.json` en el proyecto de la API:

```json
{
  "ConnectionStrings": {
    "CadenaSql": "Server=(local);Database=FyaCreditManagement;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}
```

**Ejemplos de cadenas de conexión:**
- **LocalDB:** `Server=(localdb)\\MSSQLLocalDB;Database=FyaCreditManagement;Trusted_Connection=True;`
- **SQL Server local:** `Server=localhost;Database=FyaCreditManagement;Trusted_Connection=True;TrustServerCertificate=True;`
- **SQL Server remoto:** `Server=IP_SERVIDOR;Database=FyaCreditManagement;User Id=usuario;Password=contraseña;TrustServerCertificate=True;`

#### 3.2 Configurar Secretos de Usuario (Correo Electrónico)

**Paso 1:** En Visual Studio, hacer clic derecho sobre el proyecto de la API

**Paso 2:** Seleccionar "Administrar secretos de usuario"

**Paso 3:** Se abrirá el archivo `secrets.json`, configurar con sus datos:

```json
{
  "EmailSettings": {
    "EmailEmisor": "su_email@gmail.com",
    "PasswordEmisor": "contraseña_de_aplicacion_16_caracteres"
  }
}
```

#### 3.3 Configurar Gmail para Envío de Correos

El sistema utiliza **Gmail SMTP** para enviar las notificaciones automáticas. Debe configurar una **contraseña de aplicación** en su cuenta de Gmail:

**Pasos para crear la contraseña de aplicación:**

1. **Activar verificación en 2 pasos:**
   - Ir a [myaccount.google.com](https://myaccount.google.com)
   - Seguridad → Verificación en 2 pasos → Activar

2. **Generar contraseña de aplicación:**
   - En la misma sección de Seguridad
   - Buscar "Contraseñas de aplicaciones" o "App passwords"
   - Seleccionar aplicación: "Correo" 
   - Seleccionar dispositivo: "Windows Computer" o "Other"
   - Google generará una contraseña de **16 caracteres** (ejemplo: abcd efgh ijkl mnop)

3. **Copiar la contraseña** y usarla en el campo `PasswordEmisor` del archivo secrets.json

**Importante:** No usar su contraseña normal de Gmail, debe ser la contraseña de aplicación de 16 caracteres que genera Google.

### 4. Configurar el Frontend

#### 4.1 Instalar Dependencias
```bash
cd FyaCreditManagementFront
npm install
```

#### 4.2 Configurar URL del Backend
Editar `src/services/api.js` si es necesario:

```javascript
const api = axios.create({
  baseURL: 'https://localhost:7001/api', // Ajustar puerto si es diferente
  timeout: 10000
});
```

### 5. Insertar Datos de Prueba

Ejecutar estos scripts SQL para tener datos iniciales:

```sql
-- Comerciales
INSERT INTO Comerciales (Nombre, Email, Telefono) VALUES
('Juan Carlos Martínez', 'juan.martinez@fyasocialcapital.com', '3001234567'),
('María Elena González', 'maria.gonzalez@fyasocialcapital.com', '3007654321'),
('Carlos Alberto Rodríguez', 'carlos.rodriguez@fyasocialcapital.com', '3009876543'),
('Ana Patricia Jiménez', 'ana.jimenez@fyasocialcapital.com', '3005432109'),
('Pedro Alejandro Sánchez', 'pedro.sanchez@fyasocialcapital.com', '3008765432');

-- Estados de Crédito
INSERT INTO EstadosCredito (Codigo, Descripcion) VALUES
('PENDIENTE', 'Pendiente de Aprobación'),
('APROBADO', 'Aprobado'),
('ACTIVO', 'Activo'),
('RECHAZADO', 'Rechazado'),
('CANCELADO', 'Cancelado');
```

## Ejecución del Sistema

### 1. Ejecutar el Backend
```bash
# Opción 1: Desde Visual Studio
# - Abrir la solución
# - Establecer el proyecto API como startup
# - Presionar F5 o Ctrl+F5

# Opción 2: Desde terminal
cd FyaCreditManagement.API
dotnet run
```

La API estará disponible en: `https://localhost:7001`

### 2. Ejecutar el Frontend
```bash
cd FyaCreditManagementFront
npm run dev
```

La aplicación web estará disponible en: `http://localhost:5173`

### 3. Verificar Funcionamiento

#### Probar Envío de Correos
1. Navegar a: `https://localhost:7001/api/Emails/enviar`
2. Debe retornar: `{"exitoso": true, "mensaje": "Correo de prueba enviado exitosamente"}`
3. Verificar que llegue el correo a fyasocialcapital@gmail.com

#### Probar Registro de Crédito
1. Abrir la aplicación web en el navegador
2. Llenar el formulario de registro con datos de prueba
3. Verificar que se registre exitosamente
4. Confirmar que llegue el correo automático

## Estructura del Proyecto

```
FyaCreditManagement/
├── FyaCreditManagement.API/          # Proyecto Web API
├── FyaCreditManagement.BLL/          # Lógica de negocio
├── FyaCreditManagement.DAL/          # Acceso a datos
├── FyaCreditManagement.DTO/          # Objetos de transferencia
├── FyaCreditManagement.IOC/          # Inyección de dependencias
├── FyaCreditManagement.Model/        # Modelos de Entity Framework
├── FyaCreditManagement.Utility/      # Utilidades y helpers
├── FyaCreditManagementFront/         # Aplicación React
├── script_basedatos.sql              # Script de base de datos
└── README.md                         # Este archivo
```

## Solución de Problemas Comunes

### Error de Conexión a Base de Datos
- Verificar que SQL Server esté ejecutándose
- Confirmar la cadena de conexión en appsettings.json
- Verificar que la base de datos exista

### Error de Envío de Correos
- Verificar configuración en secrets.json
- Confirmar que la contraseña de aplicación sea correcta
- Revisar logs de la aplicación para detalles del error

### Error de CORS en Frontend
- Verificar que la URL del backend sea correcta
- Confirmar que el backend esté ejecutándose
- Revisar la configuración de CORS en el API

### Puerto Ocupado
- Cambiar el puerto en launchSettings.json (backend)
- Cambiar el puerto en vite.config.js (frontend)
- Actualizar las URLs correspondientes

## Contacto y Soporte

Para preguntas técnicas sobre la implementación o problemas durante la configuración, revisar:

1. **Logs de la aplicación** - Información detallada en consola
2. **Documentación de la API** - Swagger UI en `https://localhost:7001/swagger`
3. **Archivo de configuración** - Verificar appsettings.json y secrets.json

## Notas Técnicas

- El sistema está configurado para desarrollo local
- Los cálculos financieros se realizan automáticamente en la base de datos
- El envío de correos es asíncrono y no bloquea operaciones
- Todos los cambios en créditos son auditados automáticamente
- La aplicación incluye logging detallado para troubleshooting

---

**Sistema desarrollado para FYA Social Capital - Evaluación Técnica 2025**