using Prueba.Payphone.Dominio.Enumeradores;

namespace Prueba.Payphone.API.PruebasIntegracion.ConstructoresMock.DTOs;

public record ResultadoPaginado<T>
{
    public List<T> Elementos { get; init; } = new();
    public int PaginaActual { get; init; }
    public int ElementosPorPagina { get; init; }
    public int TotalElementos { get; init; }
    public int TotalPaginas { get; init; }
}

public record MovimientoDto
{
    public int Id { get; init; }
    public int BilleteraId { get; init; }
    public TipoMovimiento Tipo { get; init; }
    public decimal Monto { get; init; }
    public string Descripcion { get; init; } = string.Empty;
    public DateTime FechaCreacion { get; init; }
}

public record CrearMovimientoComando
{
    public int BilleteraId { get; init; }
    public TipoMovimiento Tipo { get; init; }
    public decimal Monto { get; init; }
    public string Descripcion { get; init; } = string.Empty;
} 