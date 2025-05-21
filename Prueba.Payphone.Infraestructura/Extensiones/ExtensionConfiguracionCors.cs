using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Prueba.Payphone.Infraestructura.ConfiguracionOpciones;

namespace Prueba.Payphone.Infraestructura.Extensiones
{
    public static class ExtensionConfiguracionCors
    {
        public static void ConfigurarCors(this WebApplicationBuilder builder)
        {
            CorsOpciones? corsOptions = builder.Configuration
                .GetSection(CorsOpciones.SECCION)
                .Get<CorsOpciones>();

            builder.Services.AddCors(opciones =>
            {
                opciones.AddPolicy("PoliticaCors", politica =>
                {
                    politica.WithOrigins(corsOptions!.OrigenesPermitidos
                        .Split(",", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
                        .WithMethods(corsOptions.MetodosPermitidos.ToArray())
                        .WithHeaders(corsOptions.EncabezadosPermitidos.ToArray());

                    if (corsOptions.PermitirCredenciales)
                        politica.AllowCredentials();
                });
            });
        }
    }
}
