using Prueba.Payphone.Dominio.Enumeradores;

namespace Prueba.Payphone.Aplicacion.CasosDeUso.Movimientos.Queries.ListarMovimientos;

public class MovimientoDto
{
    public int Id { get; set; }
    public int BilleteraId { get; set; }
    public decimal Monto { get; set; }
    public TipoMovimiento Tipo { get; set; }
    public DateTime FechaCreacion { get; set; }
}