namespace Prueba.Payphone.Aplicacion.CasosDeUso.Comun;

public class UsuarioDto
{
    public int Id { get; set; }
    public string NombreUsuario { get; set; } = null!;
    public string CorreoElectronico { get; set; } = null!;
    public DateTime FechaCreacion { get; set; }
    public DateTime? UltimoAcceso { get; set; }
}