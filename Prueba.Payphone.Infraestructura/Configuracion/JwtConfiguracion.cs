namespace Prueba.Payphone.Infraestructura.Configuracion;

public class JwtConfiguracion
{
    public string ClaveSecreta { get; set; } = null!;
    public string Emisor { get; set; } = null!;
    public string Audiencia { get; set; } = null!;
    public int DuracionMinutos { get; set; }
}