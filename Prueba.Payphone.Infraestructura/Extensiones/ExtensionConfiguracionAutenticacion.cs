using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Prueba.Payphone.Infraestructura.Configuracion;

namespace Prueba.Payphone.Infraestructura.Extensiones;

public static class ExtensionConfiguracionAutenticacion
{
    public static IServiceCollection ConfigurarAutenticacion(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        JwtConfiguracion jwtConfig = configuration.GetSection("JWT").Get<JwtConfiguracion>();

        if (string.IsNullOrEmpty(jwtConfig?.ClaveSecreta))
            throw new InvalidOperationException("La configuración JWT es requerida");

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtConfig.Emisor,
                ValidAudience = jwtConfig.Audiencia,
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(jwtConfig.ClaveSecreta)
                ),
                ClockSkew = TimeSpan.Zero
            };

            options.Events = new JwtBearerEvents
            {
                OnAuthenticationFailed = context =>
                {
                    if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                    {
                        context.Response.Headers.Add("Token-Expired", "true");
                    }
                    return Task.CompletedTask;
                }
            };
        });

        return services;
    }
}