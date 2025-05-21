using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;

namespace Prueba.Payphone.Infraestructura.Extensiones;

public static class ExtensionEncabezadosSeguridad
{
    public static void ConfiguracionEncabezadosSeguridad(this WebApplication app)
    {
        app.Use(async (context, next) =>
        {
            context.Response.Headers.Append("Content-Security-Policy",
            "default-src 'self'; script-src 'self'; object-src 'none'; img-src 'self' data:; style-src 'self'; font-src 'self'; connect-src 'self'");
            context.Response.Headers.Append("X-Content-Type-Options", "nosniff");
            context.Response.Headers.Append("X-Frame-Options", "DENY");
            context.Response.Headers.Append("Referrer-Policy", "strict-origin-when-cross-origin");
            context.Response.Headers.Append("Permissions-Policy", "geolocation=(), camera=()");
            if (context.Request.Path.StartsWithSegments("/api"))
            {
                context.Response.Headers[HeaderNames.CacheControl] = "no-store, max-age=0";
            }
            await next();
        });
    }
}
