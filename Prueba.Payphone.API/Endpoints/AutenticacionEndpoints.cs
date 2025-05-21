using MediatR;
using Prueba.Payphone.Aplicacion.CasosDeUso.Autenticacion.Commands.CerrarSesion;
using Prueba.Payphone.Aplicacion.CasosDeUso.Autenticacion.Commands.IniciarSesion;
using Prueba.Payphone.Aplicacion.CasosDeUso.Comun;

namespace Prueba.Payphone.API.Endpoints;

public class AutenticacionEndpoints : IEndpointRegistrar
{
    public void RegisterRoutes(IEndpointRouteBuilder routes)
    {
        RouteGroupBuilder grupo = routes.MapGroup("/api/auth")
            .WithTags("Autenticación")
            .WithOpenApi();

        grupo.MapPost("/login", async (IniciarSesionDto dto, ISender sender) =>
        {
            IniciarSesionComando comando = new(dto.NombreUsuario, dto.Clave);
            RespuestaAutenticacionDto resultado = await sender.Send(comando);

            return resultado.Exito
                ? Results.Ok(resultado)
                : Results.BadRequest(new { error = resultado.Mensaje });
        })
        .WithName("IniciarSesion")
        .WithSummary("Inicia sesión de usuario")
        .WithDescription("Autentica al usuario y devuelve un token JWT si las credenciales son válidas.")
        .Produces<RespuestaAutenticacionDto>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .AllowAnonymous();

        grupo.MapPost("/logout", async (HttpContext context, ISender sender) =>
        {
            string? authHeader = context.Request.Headers.Authorization;
            if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer "))
                return Results.BadRequest(new { error = "Token no proporcionado o formato inválido." });

            string tokenValue = authHeader["Bearer ".Length..].Trim();
            CerrarSesionComando comando = new(tokenValue);
            RespuestaAutenticacionDto resultado = await sender.Send(comando);

            return resultado.Exito
                ? Results.Ok(resultado)
                : Results.BadRequest(new { error = resultado.Mensaje });
        })
        .WithName("CerrarSesion")
        .WithSummary("Cierra la sesión del usuario")
        .WithDescription("Invalida el token JWT del usuario.")
        .Produces<RespuestaAutenticacionDto>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .RequireAuthorization();
    }
}