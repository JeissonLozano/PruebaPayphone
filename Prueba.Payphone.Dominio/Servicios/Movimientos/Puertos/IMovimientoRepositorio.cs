using Prueba.Payphone.Dominio.Entidades;
using Prueba.Payphone.Dominio.Enumeradores;

namespace Prueba.Payphone.Dominio.Servicios.Movimientos.Puertos;

public interface IMovimientoRepositorio
{
    Task<Movimiento> AgregarAsync(Movimiento movimiento);
    Task<List<Movimiento>> ObtenerPorBilleteraAsync(int billeteraId);
    Task<(List<Movimiento> Items, int TotalElementos, int TotalPaginas)> ObtenerPorBilleteraIdAsync(
        int billeteraId,
        int pagina,
        int elementosPorPagina,
        DateTime? fechaInicio = null,
        DateTime? fechaFin = null,
        TipoMovimiento? tipo = null);
    Task<Movimiento?> ObtenerPorIdAsync(int id);
}