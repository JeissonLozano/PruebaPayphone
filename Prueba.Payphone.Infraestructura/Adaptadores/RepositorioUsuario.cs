using Prueba.Payphone.Dominio.Entidades;
using Prueba.Payphone.Dominio.Servicios.Autenticacion.Puertos;
using Prueba.Payphone.Infraestructura.Puerto;

namespace Prueba.Payphone.Infraestructura.Adaptadores;

public class RepositorioUsuario(
    IRepositorioGenerico<Usuario> repositorioGenerico
) : IRepositorioUsuario
{
    public async Task<Usuario?> ObtenerPorIdAsync(int id)
    {
        return await repositorioGenerico.ObtenerPorIdAsync(id);
    }

    public async Task<Usuario?> ObtenerPorNombreUsuarioAsync(string nombreUsuario)
    {
        IEnumerable<Usuario> usuarios = await repositorioGenerico.ObtenerTodosAsync(
            filtro: u => u.NombreUsuario == nombreUsuario);
        return usuarios.FirstOrDefault();
    }

    public async Task<Usuario?> ObtenerPorCorreoElectronicoAsync(string correoElectronico)
    {
        IEnumerable<Usuario> usuarios = await repositorioGenerico.ObtenerTodosAsync(
            filtro: u => u.CorreoElectronico == correoElectronico);
        return usuarios.FirstOrDefault();
    }

    public async Task<Usuario> AgregarAsync(Usuario usuario)
    {
        await repositorioGenerico.AgregarAsync(usuario);
        return usuario;
    }

    public async Task ActualizarAsync(Usuario usuario)
    {
        await repositorioGenerico.ActualizarAsync(usuario);
    }

    public async Task EliminarAsync(int id)
    {
        Usuario usuario = await repositorioGenerico.ObtenerPorIdAsync(id);
        if (usuario != null)
        {
            await repositorioGenerico.EliminarAsync(usuario);
        }
    }
}