using System.Diagnostics;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.DependencyInjection;
using Prueba.Payphone.Infraestructura.ManejadorExepciones;

namespace Prueba.Payphone.Infraestructura.Extensiones
{
    public static class ExtensionConfigurarFiltroExepciones
    {
        public static void ConfigurarFiltroExepciones(this WebApplicationBuilder builder)
        {
            builder.Services.AddProblemDetails(options =>
            {
                options.CustomizeProblemDetails = context =>
                {
                    context.ProblemDetails.Instance = $"{context.HttpContext.Request.Method} {context.HttpContext.Request.Path}";
                    context.ProblemDetails.Extensions.TryAdd("requestId", context.HttpContext.TraceIdentifier);
                    Activity? activity = context.HttpContext.Features.Get<IHttpActivityFeature>()?.Activity;
                    if (activity != null)
                    {
                        context.ProblemDetails.Extensions.TryAdd("traceId", activity.Id);
                    }
                };
            });

            builder.Services.AddExceptionHandler<ManejadorProblemasExcepciones>();
        }

    }
}
