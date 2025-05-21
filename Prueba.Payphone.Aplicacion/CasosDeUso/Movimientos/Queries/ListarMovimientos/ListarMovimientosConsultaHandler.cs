using MediatR;
using Prueba.Payphone.Aplicacion.CasosDeUso.Comun;
using Prueba.Payphone.Dominio.Entidades;
using Prueba.Payphone.Dominio.Servicios.Billeteras.Puertos;
using Prueba.Payphone.Dominio.Servicios.Movimientos.Puertos;

namespace Prueba.Payphone.Aplicacion.CasosDeUso.Movimientos.Queries.ListarMovimientos;

public class ManejadorListarMovimientos(
    IMovimientoRepositorio repositorioMovimiento,
    IBilleteraRepositorio repositorioBilletera
) : IRequestHandler<ListarMovimientosConsulta, ResultadoPaginado<MovimientoDto>>
{
    public async Task<ResultadoPaginado<MovimientoDto>> Handle(
        ListarMovimientosConsulta consulta,
        CancellationToken cancellationToken)
    {
        Billetera? billetera = await repositorioBilletera.ObtenerPorIdAsync(consulta.BilleteraId);
        if (billetera == null)
        {
            return new ResultadoPaginado<MovimientoDto>
            {
                PaginaActual = consulta.Pagina,
                ElementosPorPagina = consulta.ElementosPorPagina
            };
        }

        (List<Movimiento> Items, int TotalElementos, int TotalPaginas) movimientos = await repositorioMovimiento.ObtenerPorBilleteraIdAsync(
            consulta.BilleteraId,
            consulta.Pagina,
            consulta.ElementosPorPagina,
            consulta.FechaInicio,
            consulta.FechaFin,
            consulta.TipoMovimiento);

        List<MovimientoDto> movimientosDto = [.. movimientos.Items.Select(m => new MovimientoDto
        {
            Id = m.Id,
            BilleteraId = m.BilleteraId,
            Monto = m.Monto,
            Tipo = m.Tipo,
            FechaCreacion = m.FechaCreacion
        })];

        return new ResultadoPaginado<MovimientoDto>
        {
            Items = movimientosDto,
            PaginaActual = consulta.Pagina,
            ElementosPorPagina = consulta.ElementosPorPagina,
            TotalElementos = movimientos.TotalElementos,
            TotalPaginas = movimientos.TotalPaginas
        };
    }
}