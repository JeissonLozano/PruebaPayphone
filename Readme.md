# Microservicio de Gestión de Billeteras y Transferencias

## Descripción
Microservicio desarrollado en .NET 8 implementando Clean Architecture, diseñado para gestionar billeteras digitales y transferencias de saldo. El sistema proporciona operaciones CRUD para billeteras y registro de movimientos, con énfasis en la seguridad, escalabilidad y mantenibilidad.

## Arquitectura

### Clean Architecture
- **Dominio**: Entidades core, reglas de negocio e interfaces
- **Aplicación**: Casos de uso, DTOs y validaciones
- **Infraestructura**: Implementaciones técnicas y adaptadores
- **API**: Endpoints REST y configuración

### Patrones Implementados
- **CQRS**: Separación de comandos y consultas usando MediatR
- **Repository Pattern**: Abstracción de acceso a datos
- **Mediator**: Comunicación entre componentes
- **Unit of Work**: Gestión de transacciones
- **Specification Pattern**: Consultas complejas

## Características Técnicas

### Seguridad
- Autenticación JWT Bearer
- Autorización basada en roles
- Protección contra CSRF
- Rate Limiting configurado
- Headers de seguridad HTTP
- Validación de entrada

### Persistencia
- Entity Framework Core
- Migraciones automáticas
- Configuración de índices
- Manejo de concurrencia
- Auditoría de cambios

### Validación y Manejo de Errores
- FluentValidation para validación de entrada
- Middleware global de excepciones
- Respuestas HTTP estandarizadas
- Logging estructurado con Serilog

### Documentación API
- Swagger/OpenAPI
- Ejemplos de uso
- Documentación XML
- Postman Collection

## Estructura del Proyecto

```
Prueba.Payphone.sln
├── Prueba.Payphone.API
│   ├── Endpoints/
│   ├── Properties/
│   ├── appsettings.json
│   ├── appsettings.Development.json
│   ├── appsettings.Production.json
│   ├── appsettings.Testing.json
│   ├── Program.cs
│   └── Dockerfile
│
├── Prueba.Payphone.Aplicacion
│   ├── CasosDeUso/
│   ├── Mapeadores/
│   ├── Puertos/
│   └── ReferenciaEnsambladoAplicacion.cs
│
├── Prueba.Payphone.Dominio
│   ├── Entidades/
│   ├── Servicios/
│   └── Puertos/
│
├── Prueba.Payphone.Infraestructura
│   ├── Adaptadores/
│   ├── Configuracion/
│   ├── ConfiguracionOpciones/
│   ├── Extensiones/
│   ├── ManejadorExepciones/
│   ├── Persistencia/
│   ├── Puerto/
│   ├── Puertos/
│   └── Migrations/
│
├── Prueba.Payphone.API.PruebasIntegracion
│   └── [Pruebas de integración]
│
├── Prueba.Payphone.Dominio.PruebasUnitarias
│   └── [Pruebas unitarias]
```

## Endpoints API

### Billeteras
- POST /api/billeteras - Crear nueva billetera
- GET /api/billeteras - Listar billeteras
- GET /api/billeteras/{id} - Obtener billetera
- PUT /api/billeteras/{id} - Actualizar billetera
- DELETE /api/billeteras/{id} - Eliminar billetera

### Movimientos
- POST /api/billeteras/{id}/movimientos - Crear movimiento
- GET /api/billeteras/{id}/movimientos - Listar movimientos

## Configuración y Despliegue

### Requisitos
- .NET 8 SDK
- SQL Server 2019+
- Redis (opcional para caché)

### Variables de Entorno
- `ConnectionStrings__DefaultConnection`
- `JWT__SecretKey`
- `JWT__Issuer`
- `JWT__Audience`

### Comandos de Inicio
```bash
dotnet restore
dotnet build
dotnet run --project Prueba.Payphone.API
```

## Usuarios de Prueba

### Administrador
- **Usuario**: admin
- **Contraseña**: Admin123!
- **Correo**: admin@payphone.com
- **Rol**: admin
- **Permisos**: Acceso total al sistema

### Usuarios Regulares
1. **Usuario 1**
   - **Usuario**: usuario1
   - **Contraseña**: Usuario123!
   - **Correo**: juan.perez@payphone.com
   - **Rol**: usuario

2. **Usuario 2**
   - **Usuario**: usuario2
   - **Contraseña**: Usuario123!
   - **Correo**: maria.garcia@payphone.com
   - **Rol**: usuario

## Billeteras de Prueba

El sistema incluye las siguientes billeteras predefinidas:

1. Juan Pérez
   - **Documento**: 1234567890
   - **Saldo Inicial**: $1,000.00

2. María García
   - **Documento**: 0987654321
   - **Saldo Inicial**: $500.00

3. Carlos Rodríguez
   - **Documento**: 1122334455
   - **Saldo Inicial**: $750.00

4. Ana Martínez
   - **Documento**: 5544332211
   - **Saldo Inicial**: $250.00

5. Luis González
   - **Documento**: 9988776655
   - **Saldo Inicial**: $1,500.00

## Ejemplos de Flujos

### 1. Autenticación y Obtención de Token

```bash
# Iniciar sesión como administrador
curl -X POST http://localhost:5000/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{
    "nombreUsuario": "admin",
    "clave": "Admin123!"
  }'

# Respuesta esperada:
{
  "exito": true,
  "mensaje": "Inicio de sesión exitoso",
  "token": "eyJhbG...",
  "usuario": {
    "id": 1,
    "nombreUsuario": "admin",
    "correoElectronico": "admin@payphone.com",
    "rol": "admin"
  }
}
```

### 2. Gestión de Billeteras

```bash
# Crear nueva billetera (requiere autenticación)
curl -X POST http://localhost:5000/api/billeteras \
  -H "Authorization: Bearer {token}" \
  -H "Content-Type: application/json" \
  -d '{
    "documentoIdentidad": "1234567890",
    "nombre": "Nuevo Usuario",
    "saldoInicial": 100.00
  }'

# Consultar saldo y movimientos (acceso público)
curl http://localhost:5000/api/billeteras/1/movimientos

# Actualizar datos de billetera (requiere autenticación)
curl -X PUT http://localhost:5000/api/billeteras/1 \
  -H "Authorization: Bearer {token}" \
  -H "Content-Type: application/json" \
  -d '{
    "documentoIdentidad": "1234567890",
    "nombre": "Nombre Actualizado"
  }'
```

### 3. Operaciones de Movimientos

```bash
# Realizar un débito
curl -X POST http://localhost:5000/api/billeteras/1/movimientos \
  -H "Authorization: Bearer {token}" \
  -H "Content-Type: application/json" \
  -d '{
    "tipo": "Debito",
    "monto": 50.00
  }'

# Realizar un crédito
curl -X POST http://localhost:5000/api/billeteras/1/movimientos \
  -H "Authorization: Bearer {token}" \
  -H "Content-Type: application/json" \
  -d '{
    "tipo": "Credito",
    "monto": 100.00
  }'
```

### 4. Consulta de Movimientos con Filtros

```bash
# Obtener movimientos paginados con filtros
curl "http://localhost:5000/api/billeteras/1/movimientos?pagina=1&elementosPorPagina=10&fechaInicio=2025-01-01&fechaFin=2025-12-31&tipo=Debito"
```

### Notas Importantes

1. **Seguridad**:
   - El token JWT debe incluirse en el header `Authorization: Bearer {token}`
   - Los tokens tienen una validez de 30 minutos
   - Las operaciones de escritura requieren autenticación
   - Las consultas de movimientos son públicas

2. **Manejo de Errores**:
   - El sistema retorna códigos HTTP estándar
   - Los errores incluyen mensajes descriptivos
   - Se implementa retry automático para operaciones fallidas

## Pruebas

### Tipos de Pruebas
- Unitarias (xUnit)
- Integración (TestServer)

### Ejecución
```bash
dotnet test
```

## Estado del Proyecto

### Implementado
- Estructura Clean Architecture
- Configuración de seguridad
- Documentación API
- Manejo de errores
- Autenticación y autorización
- Operaciones CRUD de billeteras
- Gestión de movimientos
- Pruebas unitarias e integración

### Pendiente
- Optimizaciones de rendimiento
- Mejoras en la documentación
- Implementación de caché distribuido
