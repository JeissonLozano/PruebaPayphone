namespace Prueba.Payphone.Aplicacion.CasosDeUso.Autenticacion.Commands.IniciarSesion;

public class IniciarSesionDto
{
    public string NombreUsuario { get; set; } = null!;
    public string Clave { get; set; } = null!;
}