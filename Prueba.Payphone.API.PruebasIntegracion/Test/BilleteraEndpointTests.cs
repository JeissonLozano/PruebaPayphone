using System.Net;
using System.Net.Http.Json;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Prueba.Payphone.API.PruebasIntegracion.Semillas;

namespace Prueba.Payphone.API.PruebasIntegracion.Test;

[TestClass]
public class BilleteraEndpointTests : BasePruebasIntegracion
{
    private HttpClient _cliente;
    private string _token;

    [TestInitialize]
    public async Task Inicializar()
    {
        _cliente = CrearCliente();
        _token = await ObtenerTokenAutenticacionAsync(
            SemillaPruebas.DatosPrueba.Usuarios.AdminNombreUsuario,
            SemillaPruebas.DatosPrueba.Usuarios.AdminClave);
        _cliente.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _token);
    }

    [TestMethod]
    public async Task CrearBilletera_DatosValidos_DebeCrearBilletera()
    {
        // Arrange
        var nuevaBilletera = new
        {
            DocumentoIdentidad = "9876543210",
            Nombre = "Nuevo Usuario",
            SaldoInicial = 100.00M
        };

        // Act
        HttpResponseMessage respuesta = await _cliente.PostAsJsonAsync("/api/billeteras", nuevaBilletera);
        string contenidoRespuesta = await respuesta.Content.ReadAsStringAsync();

        // Assert
        Assert.AreEqual(HttpStatusCode.OK, respuesta.StatusCode);
        Assert.IsTrue(contenidoRespuesta.Contains("Billetera creada exitosamente"));
    }

    [TestMethod]
    public async Task CrearBilletera_DocumentoExistente_DebeRetornarError()
    {
        // Arrange
        var billeteraExistente = new
        {
            DocumentoIdentidad = SemillaPruebas.DatosPrueba.Billeteras.DocumentoIdentidad1,
            Nombre = "Otro Nombre",
            SaldoInicial = 100.00M
        };

        // Act
        HttpResponseMessage respuesta = await _cliente.PostAsJsonAsync("/api/billeteras", billeteraExistente);

        // Assert
        Assert.AreEqual(HttpStatusCode.BadRequest, respuesta.StatusCode);
    }

    [TestMethod]
    public async Task ObtenerBilleteras_DebeRetornarListaDeBilleteras()
    {
        // Act
        HttpResponseMessage respuesta = await _cliente.GetAsync("/api/billeteras");
        string contenidoRespuesta = await respuesta.Content.ReadAsStringAsync();

        // Assert
        Assert.AreEqual(HttpStatusCode.OK, respuesta.StatusCode);
        Assert.IsTrue(contenidoRespuesta.Contains(SemillaPruebas.DatosPrueba.Billeteras.DocumentoIdentidad1));
        Assert.IsTrue(contenidoRespuesta.Contains(SemillaPruebas.DatosPrueba.Billeteras.DocumentoIdentidad2));
    }

    [TestMethod]
    public async Task ActualizarBilletera_DatosValidos_DebeActualizarBilletera()
    {
        // Arrange
        var actualizacion = new
        {
            DocumentoIdentidad = SemillaPruebas.DatosPrueba.Billeteras.DocumentoIdentidad1,
            Nombre = "Nombre Actualizado"
        };

        // Act
        HttpResponseMessage respuesta = await _cliente.PutAsJsonAsync("/api/billeteras/1", actualizacion);
        string contenidoRespuesta = await respuesta.Content.ReadAsStringAsync();

        // Assert
        Assert.AreEqual(HttpStatusCode.OK, respuesta.StatusCode);
        Assert.IsTrue(contenidoRespuesta.Contains("Billetera actualizada exitosamente"));
    }

    [TestMethod]
    public async Task EliminarBilletera_BilleteraExistente_DebeEliminarBilletera()
    {
        // Act
        HttpResponseMessage respuesta = await _cliente.DeleteAsync("/api/billeteras/2");
        string contenidoRespuesta = await respuesta.Content.ReadAsStringAsync();

        // Assert
        Assert.AreEqual(HttpStatusCode.OK, respuesta.StatusCode);
        Assert.IsTrue(contenidoRespuesta.Contains("Billetera eliminada exitosamente"));
    }

    [TestMethod]
    public async Task EliminarBilletera_BilleteraNoExistente_DebeRetornarError()
    {
        // Act
        HttpResponseMessage respuesta = await _cliente.DeleteAsync("/api/billeteras/999");

        // Assert
        Assert.AreEqual(HttpStatusCode.BadRequest, respuesta.StatusCode);
    }
} 