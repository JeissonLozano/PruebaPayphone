using Prueba.Payphone.Dominio.Entidades;

namespace Prueba.Payphone.Dominio.Servicios.Autenticacion.Puertos;

public interface IRepositorioUsuario
{
    Task<Usuario?> ObtenerPorIdAsync(int id);
    Task<Usuario?> ObtenerPorNombreUsuarioAsync(string nombreUsuario);
    Task<Usuario?> ObtenerPorCorreoElectronicoAsync(string correoElectronico);
    Task<Usuario> AgregarAsync(Usuario usuario);
    Task ActualizarAsync(Usuario usuario);
    Task EliminarAsync(int id);
}