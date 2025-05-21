namespace Prueba.Payphone.Dominio.Entidades;

public class Usuario : EntidadDominio
{
    public string NombreUsuario { get; set; } = null!;
    public string CorreoElectronico { get; set; } = null!;
    public string HashClave { get; set; } = null!;
    public string Sal { get; set; } = null!;
    public string Rol { get; set; } = null!;
    public DateTime FechaCreacion { get; set; }
    public DateTime? UltimoAcceso { get; set; }
    public bool Activo { get; set; }
}