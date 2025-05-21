using Prueba.Payphone.Dominio.Entidades;

namespace Prueba.Payphone.Dominio.Servicios.Autenticacion.Puertos;

public interface IServicioToken
{
    string GenerarToken(Usuario usuario);
    bool ValidarToken(string token);
    void RevocarToken(string token);
}