using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Prueba.Payphone.API.PruebasIntegracion.Semillas;

namespace Prueba.Payphone.API.PruebasIntegracion.Test;

[TestClass]
public class AutenticacionEndpointTests : BasePruebasIntegracion
{
    private HttpClient _cliente;

    [TestInitialize]
    public void Inicializar()
    {
        _cliente = CrearCliente();
    }

    [TestMethod]
    public async Task IniciarSesion_CredencialesValidas_DebeRetornarToken()
    {
        // Arrange
        var credenciales = new
        {
            NombreUsuario = SemillaPruebas.DatosPrueba.Usuarios.AdminNombreUsuario,
            Clave = SemillaPruebas.DatosPrueba.Usuarios.AdminClave
        };

        // Act
        HttpResponseMessage respuesta = await _cliente.PostAsJsonAsync("/api/auth/login", credenciales);
        string contenidoRespuesta = await respuesta.Content.ReadAsStringAsync();
        RespuestaAutenticacionDto? resultado = JsonSerializer.Deserialize<RespuestaAutenticacionDto>(
            contenidoRespuesta,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        // Assert
        Assert.AreEqual(HttpStatusCode.OK, respuesta.StatusCode);
        Assert.IsNotNull(resultado);
        Assert.IsTrue(resultado.Exito);
        Assert.IsNotNull(resultado.Token);
    }

    [TestMethod]
    public async Task IniciarSesion_CredencialesInvalidas_DebeRetornarError()
    {
        // Arrange
        var credenciales = new
        {
            NombreUsuario = "usuarioInexistente",
            Clave = "claveIncorrecta"
        };

        // Act
        HttpResponseMessage respuesta = await _cliente.PostAsJsonAsync("/api/auth/login", credenciales);

        // Assert
        Assert.AreEqual(HttpStatusCode.BadRequest, respuesta.StatusCode);
    }

    [TestMethod]
    public async Task CerrarSesion_TokenValido_DebeRetornarExito()
    {
        // Arrange
        string token = await ObtenerTokenAutenticacionAsync(
            SemillaPruebas.DatosPrueba.Usuarios.AdminNombreUsuario,
            SemillaPruebas.DatosPrueba.Usuarios.AdminClave);

        _cliente.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        // Act
        HttpResponseMessage respuesta = await _cliente.PostAsync("/api/auth/logout", null);
        string contenidoRespuesta = await respuesta.Content.ReadAsStringAsync();
        RespuestaAutenticacionDto? resultado = JsonSerializer.Deserialize<RespuestaAutenticacionDto>(
            contenidoRespuesta,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        // Assert
        Assert.AreEqual(HttpStatusCode.OK, respuesta.StatusCode);
        Assert.IsNotNull(resultado);
        Assert.IsTrue(resultado.Exito);
    }

    [TestMethod]
    public async Task CerrarSesion_SinToken_DebeRetornarError()
    {
        // Act
        HttpResponseMessage respuesta = await _cliente.PostAsync("/api/auth/logout", null);

        // Assert
        Assert.AreEqual(HttpStatusCode.Unauthorized, respuesta.StatusCode);
    }
}