using System.Security.Cryptography;
using Prueba.Payphone.Dominio.Servicios.Autenticacion.Puertos;

namespace Prueba.Payphone.Infraestructura.Adaptadores;

public class ServicioPasswordAdapter : IServicioPassword
{
    private const int IteracionesPBKDF2 = 10000;
    private const int TamañoHash = 32;

    public bool VerificarPassword(string password, string hashAlmacenado, string salAlmacenada)
    {
        byte[] sal = Convert.FromBase64String(salAlmacenada);
        using Rfc2898DeriveBytes pbkdf2 = new(password, sal, IteracionesPBKDF2, HashAlgorithmName.SHA256);
        string hash = Convert.ToBase64String(pbkdf2.GetBytes(TamañoHash));
        return hash == hashAlmacenado;
    }

    public (string Hash, string Sal) HashearPassword(string password)
    {
        byte[] sal = new byte[16];
        using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(sal);
        }

        using Rfc2898DeriveBytes pbkdf2 = new(password, sal, IteracionesPBKDF2, HashAlgorithmName.SHA256);
        string hash = Convert.ToBase64String(pbkdf2.GetBytes(TamañoHash));
        string salBase64 = Convert.ToBase64String(sal);

        return (hash, salBase64);
    }
}