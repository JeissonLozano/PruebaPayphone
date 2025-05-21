using System.Text.Json;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Http;

namespace Prueba.Payphone.Infraestructura.Extensiones
{
    public static class ExtensionSaludApi
    {
        public static void ConfigurarMetricasSaludApi(this WebApplication app)
        {
            app.MapHealthChecks("/health", new HealthCheckOptions
            {
                ResponseWriter = async (context, report) =>
                {
                    context.Response.ContentType = "application/json";
                    string resultado = JsonSerializer.Serialize(new
                    {
                        estado = report.Status.ToString(),
                        verificaciones = report.Entries.Select(e => new
                        {
                            nombre = e.Key,
                            estado = e.Value.Status.ToString(),
                            descripcion = e.Value.Description,
                            duracion = e.Value.Duration.TotalMilliseconds
                        }),
                        duracionTotal = report.TotalDuration.TotalMilliseconds
                    });
                    await context.Response.WriteAsync(resultado);
                }
            });
        }
    }
}
