namespace Prueba.Payphone.Aplicacion.CasosDeUso.Comun;

public class ResultadoPaginado<T>
{
    public List<T> Items { get; set; } = new();
    public int PaginaActual { get; set; }
    public int TotalPaginas { get; set; }
    public int ElementosPorPagina { get; set; }
    public int TotalElementos { get; set; }
    public bool TienePaginaAnterior => PaginaActual > 1;
    public bool TienePaginaSiguiente => PaginaActual < TotalPaginas;
}