namespace Prueba.Payphone.Dominio.Comun;

public class ResultadoOperacion
{
    public bool EsExitoso { get; }
    public string? Mensaje { get; }

    private ResultadoOperacion(bool esExitoso, string? mensaje = null)
    {
        EsExitoso = esExitoso;
        Mensaje = mensaje;
    }

    public static ResultadoOperacion Ok() => new(true);
    public static ResultadoOperacion Fallo(string mensaje) => new(false, mensaje);
}
