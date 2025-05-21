namespace Prueba.Payphone.Dominio.Servicios.Autenticacion.Puertos;

public interface IServicioPassword
{
    bool VerificarPassword(string password, string hashAlmacenado, string salAlmacenada);
    (string Hash, string Sal) HashearPassword(string password);
}