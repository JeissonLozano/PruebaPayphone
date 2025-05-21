using Microsoft.EntityFrameworkCore;
using Prueba.Payphone.Dominio.Entidades;
using Prueba.Payphone.Dominio.Enumeradores;
using Prueba.Payphone.Dominio.Servicios.Movimientos.Puertos;
using Prueba.Payphone.Infraestructura.Puerto;

namespace Prueba.Payphone.Infraestructura.Adaptadores;

public class MovimientoRepositorio(
    IRepositorioGenerico<Movimiento> repositorioGenerico
) : IMovimientoRepositorio
{
    public async Task<Movimiento> AgregarAsync(Movimiento movimiento)
    {
        await repositorioGenerico.AgregarAsync(movimiento);
        return movimiento;
    }

    public async Task<List<Movimiento>> ObtenerPorBilleteraAsync(int billeteraId)
    {
        List<Movimiento> movimientos = [.. (await repositorioGenerico.ObtenerTodosAsync(
            filtro: m => m.BilleteraId == billeteraId))
            .OrderByDescending(m => m.FechaCreacion)];
        return movimientos;
    }

    public async Task<(List<Movimiento> Items, int TotalElementos, int TotalPaginas)> ObtenerPorBilleteraIdAsync(
        int billeteraId,
        int pagina,
        int elementosPorPagina,
        DateTime? fechaInicio = null,
        DateTime? fechaFin = null,
        TipoMovimiento? tipo = null)
    {
        IQueryable<Movimiento> query = (await repositorioGenerico.ObtenerTodosAsync(
            filtro: m => m.BilleteraId == billeteraId &&
                        (!fechaInicio.HasValue || m.FechaCreacion >= fechaInicio) &&
                        (!fechaFin.HasValue || m.FechaCreacion <= fechaFin) &&
                        (!tipo.HasValue || m.Tipo == tipo)))
            .AsQueryable();

        int totalElementos = query.Count();
        int totalPaginas = (int)Math.Ceiling(totalElementos / (double)elementosPorPagina);

        List<Movimiento> items = query
            .OrderByDescending(m => m.FechaCreacion)
            .Skip((pagina - 1) * elementosPorPagina)
            .Take(elementosPorPagina)
            .ToList();

        return (items, totalElementos, totalPaginas);
    }

    public async Task<Movimiento?> ObtenerPorIdAsync(int id)
    {
        Movimiento? movimiento = await repositorioGenerico.ObtenerPorIdAsync(id);
        return movimiento;
    }
}