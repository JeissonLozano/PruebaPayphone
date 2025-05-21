using Prueba.Payphone.Dominio.Entidades;
using Prueba.Payphone.Dominio.Servicios.Autenticacion.Puertos;

namespace Prueba.Payphone.Dominio.Servicios.Autenticacion;

public class ServicioAutenticacion(
    IRepositorioUsuario repositorioUsuario,
    IServicioToken servicioToken,
    IServicioPassword servicioPassword) : IServicioAutenticacion
{
    public async Task<(bool Exito, string Mensaje, string? Token, Usuario? Usuario)> IniciarSesionAsync(
        string nombreUsuario,
        string clave)
    {
        Usuario? usuario = await repositorioUsuario.ObtenerPorNombreUsuarioAsync(nombreUsuario);
        if (usuario == null)
        {
            return (false, "Usuario o contraseña incorrectos.", null, null);
        }

        if (!servicioPassword.VerificarPassword(clave, usuario.HashClave, usuario.Sal))
        {
            return (false, "Usuario o contraseña incorrectos.", null, null);
        }

        if (!usuario.Activo)
        {
            return (false, "La cuenta está desactivada.", null, null);
        }

        string token = servicioToken.GenerarToken(usuario);
        usuario.UltimoAcceso = DateTime.UtcNow;
        await repositorioUsuario.ActualizarAsync(usuario);

        return (true, "Inicio de sesión exitoso.", token, usuario);
    }

    public Task<(bool Exito, string Mensaje)> CerrarSesionAsync(string token)
    {
        servicioToken.RevocarToken(token);
        return Task.FromResult((true, "Sesión cerrada exitosamente."));
    }

    public Task<bool> ValidarTokenAsync(string token)
    {
        return Task.FromResult(servicioToken.ValidarToken(token));
    }
}