namespace Prueba.Payphone.Aplicacion.CasosDeUso.Billeteras;

public class ResultadoOperacionBilletera
{
    public bool Exito { get; set; }
    public string Mensaje { get; set; } = string.Empty;
    public BilleteraDto? Billetera { get; set; }
}