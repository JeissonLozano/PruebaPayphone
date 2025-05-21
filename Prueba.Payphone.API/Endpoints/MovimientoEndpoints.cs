using MediatR;
using Prueba.Payphone.Aplicacion.CasosDeUso.Comun;
using Prueba.Payphone.Aplicacion.CasosDeUso.Movimientos.Commands.CrearMovimiento;
using Prueba.Payphone.Aplicacion.CasosDeUso.Movimientos.Queries.ListarMovimientos;
using Prueba.Payphone.Dominio.Enumeradores;

namespace Prueba.Payphone.API.Endpoints;


public class MovimientoEndpoints : IEndpointRegistrar
{
    public void RegisterRoutes(IEndpointRouteBuilder routes)
    {
        RouteGroupBuilder grupo = routes.MapGroup("/api/billeteras/{billeteraId:int}/movimientos")
            .WithTags("Movimientos")
            .WithOpenApi();

        grupo.MapGet("/", async (
            int billeteraId,
            int? pagina,
            int? elementosPorPagina,
            DateTime? fechaInicio,
            DateTime? fechaFin,
            TipoMovimiento? tipo,
            ISender sender) =>
        {
            ListarMovimientosConsulta consulta = new(
                billeteraId,
                pagina ?? 1,
                elementosPorPagina ?? 10,
                fechaInicio,
                fechaFin,
                tipo);

            ResultadoPaginado<MovimientoDto> resultado = await sender.Send(consulta);
            return Results.Ok(resultado);
        })
        .WithName("ListarMovimientos")
        .WithSummary("Lista los movimientos de una billetera")
        .WithDescription("Obtiene el historial de movimientos de una billetera con paginación y filtros opcionales.")
        .Produces<ResultadoPaginado<MovimientoDto>>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest);

        grupo.MapPost("/", async (
            int billeteraId,
            CrearMovimientoComando comando,
            ISender sender) =>
        {
            if (billeteraId != comando.BilleteraId)
            {
                return Results.BadRequest(new { error = "El ID de la billetera en la ruta no coincide con el ID en el cuerpo de la petición." });
            }

            try
            {
                MovimientoDto resultado = await sender.Send(comando);
                return Results.Created($"/api/billeteras/{billeteraId}/movimientos/{resultado.Id}", resultado);
            }
            catch (InvalidOperationException ex)
            {
                return Results.BadRequest(new { error = ex.Message });
            }
        })
        .WithName("CrearMovimiento")
        .WithSummary("Crea un nuevo movimiento en la billetera")
        .WithDescription("Crea un nuevo movimiento (débito o crédito) y actualiza el saldo de la billetera.")
        .Produces<MovimientoDto>(StatusCodes.Status201Created)
        .Produces(StatusCodes.Status400BadRequest)
        .RequireRateLimiting("financial_operations");
    }
}