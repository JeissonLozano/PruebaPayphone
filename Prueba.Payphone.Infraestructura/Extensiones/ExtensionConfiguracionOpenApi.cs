using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Prueba.Payphone.Infraestructura.ConfiguracionOpciones;

namespace Prueba.Payphone.Infraestructura.Extensiones
{
    public static class ExtensionConfiguracionOpenApi
    {
        private const string VERSION_API = "v1";
        private const string ESQUEMA_SEGURIDAD = "Bearer";
        private const string DESCRIPCION_SEGURIDAD = "Autenticación JWT usando el esquema Bearer. \nEjemplo: \"Authorization: Bearer {token}\"";

        public static void ConfigurarOpenApi(this WebApplicationBuilder builder)
        {
            OpenApiOpciones opcionesOpenApi = builder.Configuration
               .GetSection(OpenApiOpciones.SECCION)
               .Get<OpenApiOpciones>() ?? new OpenApiOpciones();

            builder.Services.AddSwaggerGen(configuracion =>
            {
                OpenApiInfo informacionApi = new()
                {
                    Title = opcionesOpenApi.Titulo ?? "API de Billeteras Digitales",
                    Version = VERSION_API,
                    Description = "Esta API RESTful permite gestionar billeteras digitales y realizar transferencias de saldo entre usuarios. " +
                                "Soporta operaciones CRUD sobre billeteras, registro y consulta de movimientos, validaciones de negocio, " +
                                "manejo de errores y está diseñada bajo principios de Clean Architecture y buenas prácticas de seguridad. " +
                                "Incluye autenticación JWT, pruebas automatizadas y documentación interactiva.",
                    Contact = new OpenApiContact
                    {
                        Name = "Jeisson Julian Lozano Ortiz",
                        Email = "ing.jeissonlozano@hotmail.com"
                    }
                };

                configuracion.SwaggerDoc(VERSION_API, informacionApi);

                OpenApiSecurityScheme esquemaSeguridad = new()
                {
                    Description = DESCRIPCION_SEGURIDAD,
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = ESQUEMA_SEGURIDAD
                };

                configuracion.AddSecurityDefinition(ESQUEMA_SEGURIDAD, esquemaSeguridad);

                OpenApiSecurityScheme esquemaReferencia = new()
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = ESQUEMA_SEGURIDAD
                    },
                    Scheme = "oauth2",
                    Name = ESQUEMA_SEGURIDAD,
                    In = ParameterLocation.Header
                };

                OpenApiSecurityRequirement requerimientoSeguridad = new()
                {
                    { esquemaReferencia, new List<string>() }
                };

                configuracion.AddSecurityRequirement(requerimientoSeguridad);
            });
        }
    }
}
