namespace Prueba.Payphone.Aplicacion.CasosDeUso.Billeteras;

public class BilleteraDto
{
    public int Id { get; set; }
    public string DocumentoIdentidad { get; set; } = null!;
    public string Nombre { get; set; } = null!;
    public decimal Saldo { get; set; }
    public DateTime FechaCreacion { get; set; }
    public DateTime FechaActualizacion { get; set; }
}