using Prueba.Payphone.Dominio.Entidades;

namespace Prueba.Payphone.Dominio.Servicios.Autenticacion.Puertos;

public interface IServicioAutenticacion
{
    Task<(bool Exito, string Mensaje, string? Token, Usuario? Usuario)> IniciarSesionAsync(
        string nombreUsuario,
        string clave);

    Task<(bool Exito, string Mensaje)> CerrarSesionAsync(string token);

    Task<bool> ValidarTokenAsync(string token);
}