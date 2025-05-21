using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace Prueba.Payphone.Infraestructura.Extensiones;

public static class ExtensionConfiguracionAutorizacion
{
    public static IServiceCollection ConfigurarAutorizacion(this IServiceCollection services)
    {
        services.AddAuthorizationBuilder()
            .AddPolicy("RequiereAdmin", policy =>
                policy.RequireClaim("rol", "admin"))
            .AddPolicy("RequiereUsuario", policy =>
                policy.RequireClaim("rol", "usuario", "admin"))
            .AddPolicy("RequiereVerificado", policy =>
                policy.RequireClaim("email_verificado", "true"))
            .SetFallbackPolicy(new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .Build())
            .AddPolicy("RequiereAutenticacionReciente", policy =>
                policy.RequireAuthenticatedUser()
                    .AddAuthenticationSchemes("Bearer")
                    .RequireClaim("auth_time")
                    .AddRequirements(new MaximumAuthenticationAgeRequirement(TimeSpan.FromMinutes(30))));

        return services;
    }
}

public class MaximumAuthenticationAgeRequirement(TimeSpan maximumAge) : IAuthorizationRequirement
{
    public TimeSpan MaximumAge { get; } = maximumAge;
}

public class MaximumAuthenticationAgeHandler : AuthorizationHandler<MaximumAuthenticationAgeRequirement>
{
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        MaximumAuthenticationAgeRequirement requirement)
    {
        if (!context.User.HasClaim(c => c.Type == "auth_time"))
        {
            return Task.CompletedTask;
        }

        DateTimeOffset authTime = DateTimeOffset.FromUnixTimeSeconds(
            long.Parse(context.User.FindFirst("auth_time").Value));

        if (DateTimeOffset.UtcNow - authTime < requirement.MaximumAge)
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}