namespace Prueba.Payphone.Aplicacion.CasosDeUso.Comun;

public class RespuestaAutenticacionDto
{
    public bool Exito { get; set; }
    public string Mensaje { get; set; } = string.Empty;
    public string? Token { get; set; }
    public DateTime? Expiracion { get; set; }
    public UsuarioDto? Usuario { get; set; }
}