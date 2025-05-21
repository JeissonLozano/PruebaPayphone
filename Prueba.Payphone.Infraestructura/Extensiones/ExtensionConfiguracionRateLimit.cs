using System.Threading.RateLimiting;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Prueba.Payphone.Infraestructura.Extensiones;

public static class ExtensionConfiguracionRateLimit
{
    public static IServiceCollection ConfigurarRateLimit(this IServiceCollection services)
    {
        services.AddRateLimiter(options =>
        {
            // Limiter global
            options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(context =>
            {
                return RateLimitPartition.GetFixedWindowLimiter(
                    partitionKey: context.User.Identity?.Name ?? context.Request.Headers.Host.ToString(),
                    factory: partition => new FixedWindowRateLimiterOptions
                    {
                        AutoReplenishment = true,
                        PermitLimit = 100,
                        Window = TimeSpan.FromMinutes(1)
                    });
            });

            // Política para operaciones financieras
            options.AddPolicy("financial_operations", context =>
            {
                return RateLimitPartition.GetFixedWindowLimiter(
                    partitionKey: context.User.Identity?.Name ?? context.Connection.RemoteIpAddress?.ToString() ?? "anónimo",
                    factory: _ => new FixedWindowRateLimiterOptions
                    {
                        AutoReplenishment = true,
                        PermitLimit = 30,         // 30 operaciones
                        Window = TimeSpan.FromHours(1) // por hora
                    });
            });

            options.OnRejected = async (context, cancellationToken) =>
            {
                context.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;
                await context.HttpContext.Response.WriteAsync(
                    "Has excedido el límite de operaciones permitidas. Por favor, intenta más tarde.",
                    cancellationToken);
            };
        });

        return services;
    }
}