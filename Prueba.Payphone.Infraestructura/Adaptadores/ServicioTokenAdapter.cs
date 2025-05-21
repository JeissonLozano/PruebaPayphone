using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Prueba.Payphone.Dominio.Entidades;
using Prueba.Payphone.Dominio.Servicios.Autenticacion.Puertos;
using Prueba.Payphone.Infraestructura.Configuracion;

namespace Prueba.Payphone.Infraestructura.Adaptadores;

public class ServicioTokenAdapter(IOptions<JwtConfiguracion> configuracionJwt) : IServicioToken
{
    private const int TamañoMinimoClaveBytes = 32;
    private readonly JwtConfiguracion configuracionJwt = configuracionJwt.Value;
    private readonly HashSet<string> tokenesRevocados = [];

    public string GenerarToken(Usuario usuario)
    {
        if (usuario == null)
        {
            throw new ArgumentNullException(nameof(usuario));
        }

        if (string.IsNullOrEmpty(configuracionJwt.ClaveSecreta))
        {
            throw new InvalidOperationException("La clave secreta JWT no está configurada.");
        }

        if (string.IsNullOrEmpty(configuracionJwt.Emisor))
        {
            throw new InvalidOperationException("El emisor JWT no está configurado.");
        }

        if (string.IsNullOrEmpty(configuracionJwt.Audiencia))
        {
            throw new InvalidOperationException("La audiencia JWT no está configurada.");
        }

        try
        {
            byte[] claveBytes = Encoding.UTF8.GetBytes(configuracionJwt.ClaveSecreta);
            if (claveBytes.Length < TamañoMinimoClaveBytes)
            {
                throw new InvalidOperationException($"La clave secreta debe tener al menos {TamañoMinimoClaveBytes} bytes de longitud.");
            }

            Claim[] reclamaciones =
            [
                new Claim(JwtRegisteredClaimNames.Sub, usuario.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName, usuario.NombreUsuario),
                new Claim(JwtRegisteredClaimNames.Email, usuario.CorreoElectronico),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Role, usuario.Rol),
                new Claim("rol", usuario.Rol)
            ];

            SymmetricSecurityKey clave = new(claveBytes);
            SigningCredentials credenciales = new(clave, SecurityAlgorithms.HmacSha256);
            DateTime expiracion = DateTime.UtcNow.AddMinutes(configuracionJwt.DuracionMinutos);

            JwtSecurityToken token = new(
                issuer: configuracionJwt.Emisor,
                audience: configuracionJwt.Audiencia,
                claims: reclamaciones,
                expires: expiracion,
                signingCredentials: credenciales
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Error al generar el token JWT.", ex);
        }
    }

    public bool ValidarToken(string token)
    {
        if (string.IsNullOrEmpty(token))
        {
            return false;
        }

        if (tokenesRevocados.Contains(token))
        {
            return false;
        }

        JwtSecurityTokenHandler manejadorToken = new();
        byte[] claveBytes = Encoding.UTF8.GetBytes(configuracionJwt.ClaveSecreta);

        try
        {
            manejadorToken.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(claveBytes),
                ValidateIssuer = true,
                ValidIssuer = configuracionJwt.Emisor,
                ValidateAudience = true,
                ValidAudience = configuracionJwt.Audiencia,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            }, out _);

            return true;
        }
        catch
        {
            return false;
        }
    }

    public void RevocarToken(string token)
    {
        if (!string.IsNullOrEmpty(token))
        {
            tokenesRevocados.Add(token);
        }
    }
}