using System.Net;
using System.Net.Http.Json;
using Prueba.Payphone.API.PruebasIntegracion.ConstructoresMock;
using Prueba.Payphone.API.PruebasIntegracion.ConstructoresMock.DTOs;
using Prueba.Payphone.API.PruebasIntegracion.Semillas;
using Prueba.Payphone.Dominio.Enumeradores;

namespace Prueba.Payphone.API.PruebasIntegracion.Test;

[TestClass]
public class MovimientoEndpointTests : BasePruebasIntegracion
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
    public async Task ListarMovimientos_DebeRetornarMovimientosDeBilletera()
    {
        // Act
        HttpResponseMessage respuesta = await _cliente.GetAsync("/api/billeteras/1/movimientos");

        // Assert
        Assert.AreEqual(HttpStatusCode.OK, respuesta.StatusCode);
        var resultado = await respuesta.Content.ReadFromJsonAsync<ResultadoPaginado<MovimientoDto>>();
        Assert.IsNotNull(resultado);
        Assert.IsTrue(resultado.TotalElementos > 0);
    }

    [TestMethod]
    public async Task ListarMovimientos_ConFiltros_DebeRetornarMovimientosFiltrados()
    {
        // Arrange
        string fechaInicio = DateTime.UtcNow.AddDays(-1).ToString("yyyy-MM-dd");
        string fechaFin = DateTime.UtcNow.ToString("yyyy-MM-dd");

        // Act
        HttpResponseMessage respuesta = await _cliente.GetAsync(
            $"/api/billeteras/1/movimientos?pagina=1&elementosPorPagina=5&fechaInicio={fechaInicio}&fechaFin={fechaFin}&tipo={TipoMovimiento.Credito}");

        // Assert
        Assert.AreEqual(HttpStatusCode.OK, respuesta.StatusCode);
        ResultadoPaginado<MovimientoDto>? resultado = await respuesta.Content.ReadFromJsonAsync<ResultadoPaginado<MovimientoDto>>();
        Assert.IsNotNull(resultado);
        Assert.IsTrue(resultado.Elementos.All(m => m.Tipo == TipoMovimiento.Credito));
    }

    [TestMethod]
    public async Task CrearMovimiento_Credito_DebeCrearMovimientoYActualizarSaldo()
    {
        // Arrange
        Aplicacion.CasosDeUso.Movimientos.Commands.CrearMovimiento.CrearMovimientoComando nuevoMovimiento = MovimientoTestBuilder.CrearComandoCredito();

        // Act
        HttpResponseMessage respuesta = await _cliente.PostAsJsonAsync("/api/billeteras/1/movimientos", nuevoMovimiento);

        // Assert
        Assert.AreEqual(HttpStatusCode.Created, respuesta.StatusCode);
        MovimientoDto? resultado = await respuesta.Content.ReadFromJsonAsync<MovimientoDto>();
        Assert.IsNotNull(resultado);
        Assert.AreEqual(nuevoMovimiento.Monto, resultado.Monto);
        Assert.AreEqual(nuevoMovimiento.Tipo, resultado.Tipo);
    }

    [TestMethod]
    public async Task CrearMovimiento_Debito_DebeCrearMovimientoYActualizarSaldo()
    {
        // Arrange
        Aplicacion.CasosDeUso.Movimientos.Commands.CrearMovimiento.CrearMovimientoComando nuevoMovimiento = MovimientoTestBuilder.CrearComandoDebito();

        // Act
        HttpResponseMessage respuesta = await _cliente.PostAsJsonAsync("/api/billeteras/1/movimientos", nuevoMovimiento);

        // Assert
        Assert.AreEqual(HttpStatusCode.Created, respuesta.StatusCode);
        MovimientoDto? resultado = await respuesta.Content.ReadFromJsonAsync<MovimientoDto>();
        Assert.IsNotNull(resultado);
        Assert.AreEqual(nuevoMovimiento.Monto, resultado.Monto);
        Assert.AreEqual(nuevoMovimiento.Tipo, resultado.Tipo);
    }

    [TestMethod]
    public async Task CrearMovimiento_MontoInvalido_DebeRetornarError()
    {
        // Arrange
        Aplicacion.CasosDeUso.Movimientos.Commands.CrearMovimiento.CrearMovimientoComando nuevoMovimiento = MovimientoTestBuilder.CrearComandoDebito(monto: -50.00M);

        // Act
        HttpResponseMessage respuesta = await _cliente.PostAsJsonAsync("/api/billeteras/1/movimientos", nuevoMovimiento);

        // Assert
        Assert.AreEqual(HttpStatusCode.BadRequest, respuesta.StatusCode);
    }

    [TestMethod]
    public async Task CrearMovimiento_BilleteraNoExiste_DebeRetornarError()
    {
        // Arrange
        Aplicacion.CasosDeUso.Movimientos.Commands.CrearMovimiento.CrearMovimientoComando nuevoMovimiento = MovimientoTestBuilder.CrearComandoCredito(billeteraId: 999);

        // Act
        HttpResponseMessage respuesta = await _cliente.PostAsJsonAsync("/api/billeteras/999/movimientos", nuevoMovimiento);

        // Assert
        Assert.AreEqual(HttpStatusCode.BadRequest, respuesta.StatusCode);
    }
}
