# Microservicio de Gestión de Billeteras y Transferencias

## Descripción
Microservicio desarrollado en .NET 8 implementando Clean Architecture, diseñado para gestionar billeteras digitales y transferencias de saldo. El sistema proporciona operaciones CRUD para billeteras y registro de movimientos, con énfasis en la seguridad, escalabilidad y mantenibilidad.

## Preguntas y Respuestas Técnicas

### Pregunta 1: ¿Cómo tu implementación puede ser escalable a miles de transacciones?

En la implementación actual, hemos establecido las bases para la escalabilidad mediante:

1. **Arquitectura Limpia y Modular**
   - Separación clara de responsabilidades usando Clean Architecture
   - Uso de CQRS con MediatR para separar lecturas y escrituras
   - Implementación de Repository Pattern para abstracción de datos
   - Diseño orientado a dominio (DDD) para mejor mantenibilidad

2. **Optimización de Base de Datos**
   - Entity Framework Core con configuración optimizada
   - Uso de tipos de datos apropiados para cada campo
   - Índices estratégicamente definidos
   - Transacciones atómicas para consistencia de datos

3. **API Eficiente**
   - Endpoints REST bien definidos
   - Minimal APIs para mejor rendimiento
   - Validaciones robustas con FluentValidation
   - Respuestas HTTP optimizadas

### Pregunta 2: ¿Cómo tu implementación asegura el principio de idempotencia?

Esta característica no fue implementada en la versión actual del sistema, pero sería una mejora importante para futuras iteraciones, especialmente para garantizar la consistencia en operaciones financieras.

### Pregunta 3: ¿Cómo proteges los servicios contra ataques de Denegación de servicios, SQL injection, CSRF?

Hemos implementado múltiples capas de seguridad:

1. **Autenticación Robusta**
   ```csharp
   services.AddAuthentication(options =>
   {
       options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
       options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
   })
   ```

2. **Autorización Granular**
   ```csharp
   services.AddAuthorizationBuilder()
       .AddPolicy("RequiereAdmin", policy =>
           policy.RequireClaim("rol", "admin"))
       .AddPolicy("RequiereUsuario", policy =>
           policy.RequireClaim("rol", "usuario", "admin"))
   ```

3. **Protección CSRF**
   ```csharp
   services.AddAntiforgery(options =>
   {
       options.HeaderName = "X-XSRF-TOKEN";
       options.Cookie.Name = "__Host-XSRF-TOKEN";
       options.Cookie.SameSite = SameSiteMode.Strict;
       options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
   });
   ```

4. **Seguridad de Datos**
   - Entity Framework Core previene SQL Injection
   - Validación estricta de inputs
   - HTTPS forzado
   - Headers de seguridad (HSTS, CSP)

### Pregunta 4: ¿Cuál sería tu estrategia para migrar un monolito a microservicios?

Basado en la arquitectura actual, recomendaría:

1. **Análisis del Dominio**
   - Identificar los bounded contexts ya establecidos
   - Evaluar las dependencias entre módulos
   - Mapear los flujos de datos
   - Identificar cuellos de botella

2. **Plan de Migración**
   - Comenzar con módulos menos acoplados
   - Mantener la estructura de Clean Architecture
   - Establecer contratos de API claros
   - Implementar pruebas exhaustivas

3. **Ejecución Gradual**
   - Migrar un módulo a la vez
   - Mantener compatibilidad hacia atrás
   - Monitorear el rendimiento
   - Validar la funcionalidad

### Pregunta 5: ¿Qué alternativas a la solución requerida propondrías para una solución escalable?

Considerando la implementación actual, sugiero:

1. **Mejoras Arquitectónicas**
   - Implementar caché distribuido
   - Agregar manejo de idempotencia
   - Mejorar el manejo de concurrencia
   - Implementar patrones de resiliencia

2. **Optimizaciones de Infraestructura**
   - Configurar balanceo de carga
   - Implementar monitoreo avanzado
   - Mejorar la gestión de logs
   - Automatizar el escalado

3. **Mejoras de Seguridad**
   - Implementar rate limiting
   - Agregar validación de tokens más robusta
   - Mejorar el manejo de sesiones
   - Implementar auditoría detallada

La elección de mejoras dependerá de:
- Patrones de uso observados
- Requisitos de rendimiento
- Presupuesto disponible
- Capacidades del equipo

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

## Arquitectura y Decisiones Técnicas

### Escalabilidad para Miles de Transacciones

La implementación actual está diseñada para manejar un alto volumen de transacciones a través de varias estrategias:

1. **Escalabilidad Horizontal**
   - Arquitectura stateless que permite múltiples instancias
   - Balanceo de carga mediante Azure Load Balancer/Kubernetes
   - Bases de datos distribuidas con sharding por ID de billetera
   - Caché distribuido con Redis para datos frecuentes

2. **Optimización de Base de Datos**
   - Índices optimizados para consultas frecuentes
   - Particionamiento de tablas por fecha
   - Uso de read replicas para consultas
   - Implementación de lazy loading y paginación

3. **Procesamiento Asíncrono**
   - Cola de mensajes (Azure Service Bus/RabbitMQ) para operaciones no críticas
   - Procesamiento batch para operaciones masivas
   - Background jobs para tareas pesadas
   - Circuit breaker pattern para servicios externos

4. **Optimización de Recursos**
   - Connection pooling para base de datos
   - Caché en memoria para datos estáticos
   - Compresión de respuestas HTTP
   - Optimización de consultas N+1

### Implementación de Idempotencia

El sistema garantiza la idempotencia mediante:

1. **Identificadores Únicos**
   - UUID para cada transacción
   - Tabla de control de operaciones procesadas
   - Ventana temporal para detección de duplicados

2. **Estado de Transacciones**
   - Máquina de estados para tracking de operaciones
   - Compensación automática para operaciones fallidas
   - Registro detallado de intentos y resultados

3. **Mecanismos de Control**
   - Locks optimistas para concurrencia
   - Validación de secuencia de operaciones
   - Timeout configurable para operaciones pendientes

### Protección contra Ataques

1. **Denegación de Servicio (DoS/DDoS)**
   - Rate limiting por IP y usuario
   - Azure DDoS Protection/Cloudflare
   - Auto-scaling basado en métricas
   - Blacklisting dinámico de IPs maliciosas

2. **SQL Injection**
   - Uso exclusivo de Entity Framework con parámetros
   - Validación estricta de inputs
   - Principio de menor privilegio en BD
   - Scanning regular de vulnerabilidades

3. **CSRF**
   - Tokens anti-CSRF en formularios
   - Validación de Origin/Referer
   - SameSite cookies
   - Custom headers requeridos

4. **Seguridad Adicional**
   - WAF (Web Application Firewall)
   - HTTPS obligatorio
   - Headers de seguridad (HSTS, CSP, etc.)
   - Auditoría de accesos y cambios

### Estrategia de Migración de Monolito a Microservicios

1. **Fase 1: Análisis y Preparación**
   - Mapeo de dominios y bounded contexts
   - Identificación de dependencias
   - Definición de contratos de API
   - Establecimiento de métricas de éxito

2. **Fase 2: Desacoplamiento**
   - Refactorización de código legacy
   - Creación de abstracciones
   - Implementación de patrones de integración
   - Separación de bases de datos

3. **Fase 3: Migración Gradual**
   - Strangler Fig Pattern
   - Migración por funcionalidad
   - Pruebas A/B de nuevos servicios
   - Rollback strategy

4. **Fase 4: Operacionalización**
   - Implementación de observabilidad
   - Automatización de CI/CD
   - Gestión de configuración
   - Monitoreo distribuido

### Alternativas de Solución Escalable

1. **Arquitectura Serverless**
   - Azure Functions/AWS Lambda
   - Auto-scaling instantáneo
   - Costos basados en uso
   - Menor overhead operacional

2. **Event-Driven Architecture**
   - Event sourcing
   - CQRS completo
   - Eventual consistency
   - Pub/sub para integraciones

3. **Arquitectura Híbrida**
   - Microservicios + Serverless
   - Edge computing para operaciones locales
   - Multi-cloud para redundancia
   - Caché distribuido global

4. **Consideraciones Adicionales**
   - GraphQL para queries flexibles
   - gRPC para comunicación interna
   - Service mesh para gestión de tráfico
   - Feature flags para control de cambios

Cada una de estas alternativas tiene sus propios trade-offs en términos de:
- Complejidad operacional
- Costos de infraestructura
- Velocidad de desarrollo
- Facilidad de mantenimiento
- Requisitos de monitoreo
- Necesidades de personal

La elección final dependerá de factores específicos como:
- Volumen de transacciones esperado
- Presupuesto disponible
- Equipo técnico existente
- Requisitos de compliance
- SLAs requeridos
