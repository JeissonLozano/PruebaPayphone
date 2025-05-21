namespace Prueba.Payphone.Infraestructura.ConfiguracionOpciones;

public class CorsOpciones
{
    public const string SECCION = "Cors";

    public string OrigenesPermitidos { get; set; } = string.Empty;
    public List<string> MetodosPermitidos { get; set; } = new();
    public List<string> EncabezadosPermitidos { get; set; } = new();
    public bool PermitirCredenciales { get; set; }
}
