using MediatR;
using Microsoft.AspNetCore.Authorization;
using Prueba.Payphone.Aplicacion.CasosDeUso.Billeteras;
using Prueba.Payphone.Aplicacion.CasosDeUso.Billeteras.Commands.ActualizarBilletera;
using Prueba.Payphone.Aplicacion.CasosDeUso.Billeteras.Commands.CrearBilletera;
using Prueba.Payphone.Aplicacion.CasosDeUso.Billeteras.Commands.EliminarBilletera;
using Prueba.Payphone.Aplicacion.CasosDeUso.Billeteras.Queries.ListarBilleteras;
using Prueba.Payphone.Aplicacion.CasosDeUso.Billeteras.Queries.ObtenerBilletera;
using Prueba.Payphone.Aplicacion.CasosDeUso.Comun;

namespace Prueba.Payphone.API.Endpoints;

[Authorize]
public class BilleteraEndpoints : IEndpointRegistrar
{
    public void RegisterRoutes(IEndpointRouteBuilder routes)
    {
        RouteGroupBuilder grupo = routes.MapGroup("/api/billeteras")
            .WithTags("Billeteras")
            .WithOpenApi();

        grupo.MapPost("/", async (CrearBilleteraComando comando, ISender sender) =>
        {
            ResultadoOperacionBilletera resultado = await sender.Send(comando);
            return resultado.Exito ? Results.Ok(resultado) : Results.BadRequest(new { error = resultado.Mensaje });
        })
        .WithName("CrearBilletera")
        .WithSummary("Crea una nueva billetera")
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status401Unauthorized);

        grupo.MapGet("/", async (ISender sender) =>
        {
            IEnumerable<BilleteraDto> resultado = await sender.Send(new ListarBilleterasConsulta());
            return Results.Ok(resultado);
        })
        .WithName("ListarBilleteras")
        .WithSummary("Lista todas las billeteras")
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status401Unauthorized)
        .RequireAuthorization("RequiereAdmin");

        grupo.MapGet("/{id:int}", async (int id, ISender sender) =>
        {
            BilleteraDto resultado = await sender.Send(new ObtenerBilleteraPorIdConsulta(id));
            return resultado != null ? Results.Ok(resultado) : Results.NotFound();
        })
        .WithName("ObtenerBilleteraPorId")
        .WithSummary("Obtiene una billetera por su id")
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound)
        .Produces(StatusCodes.Status401Unauthorized);

        grupo.MapPut("/{id:int}", async (int id, ActualizarBilleteraComando comando, ISender sender) =>
        {
            ActualizarBilleteraComando comandoActualizado = comando with { Id = id };
            ResultadoOperacionBilletera resultado = await sender.Send(comandoActualizado);
            return resultado.Exito ? Results.Ok(resultado) : Results.BadRequest(new { error = resultado.Mensaje });
        })
        .WithName("ActualizarBilletera")
        .WithSummary("Actualiza una billetera existente")
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status401Unauthorized);

        grupo.MapDelete("/{id:int}", async (int id, ISender sender) =>
        {
            ResultadoOperacion resultado = await sender.Send(new EliminarBilleteraComando(id));
            return resultado.Exito ? Results.Ok(resultado) : Results.BadRequest(new { error = resultado.Mensaje });
        })
        .WithName("EliminarBilletera")
        .WithSummary("Elimina una billetera por su id")
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound)
        .Produces(StatusCodes.Status401Unauthorized)
        .RequireAuthorization("RequiereAdmin");
    }
}