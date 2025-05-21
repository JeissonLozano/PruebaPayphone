using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using Prueba.Payphone.Dominio.Entidades;
using Prueba.Payphone.Dominio.Enumeradores;
using Prueba.Payphone.Dominio.PruebasUnitarias.ConstructoresMock;
using Prueba.Payphone.Dominio.Servicios.Billeteras.Puertos;
using Prueba.Payphone.Dominio.Servicios.Movimientos;
using Prueba.Payphone.Dominio.Servicios.Movimientos.Puertos;

namespace Prueba.Payphone.Dominio.PruebasUnitarias.Pruebas
{
    [TestClass]
    public class ServicioMovimientoPruebas
    {
        private ConstructorBilletera _constructorBilletera;
        private ConstructorMovimiento _constructorMovimiento;
        private ConstructorRepositorioBilletera _constructorRepositorioBilletera;
        private ConstructorRepositorioMovimiento _constructorRepositorioMovimiento;
        private ServicioMovimiento _servicioMovimiento;
        private IBilleteraRepositorio _repositorioBilletera;
        private IMovimientoRepositorio _repositorioMovimiento;

        [TestInitialize]
        public void Inicializar()
        {
            _constructorBilletera = new ConstructorBilletera();
            _constructorMovimiento = new ConstructorMovimiento();
            _constructorRepositorioBilletera = new ConstructorRepositorioBilletera();
            _constructorRepositorioMovimiento = new ConstructorRepositorioMovimiento();

            _repositorioBilletera = _constructorRepositorioBilletera.Construir();
            _repositorioMovimiento = _constructorRepositorioMovimiento.Construir();

            _servicioMovimiento = new ServicioMovimiento(_repositorioMovimiento, _repositorioBilletera);
        }

        [TestMethod]
        public async Task CrearDebito_ConDatosValidos_DebeCrearDebitoExitosamente()
        {
            // Arrange
            Billetera billetera = _constructorBilletera
                .ConId(1)
                .ConDocumentoIdentidad("1234567890")
                .ConNombre("Usuario Test")
                .Construir();
            billetera.Acreditar(200.00M);

            _constructorRepositorioBilletera.ConBilleteraExistente(billetera);
            _constructorRepositorioMovimiento.SimularAgregarMovimiento();

            decimal montoDebito = 100.00M;

            // Act
            (bool Exito, string Mensaje, Movimiento? Movimiento) resultado = await _servicioMovimiento.CrearDebitoAsync(billetera.Id, montoDebito);

            // Assert
            Assert.IsTrue(resultado.Exito);
            Assert.IsNotNull(resultado.Movimiento);
            Assert.AreEqual(montoDebito, resultado.Movimiento.Monto);
            Assert.AreEqual(TipoMovimiento.Debito, resultado.Movimiento.Tipo);
            await _repositorioMovimiento.Received(1).AgregarAsync(Arg.Any<Movimiento>());
            await _repositorioBilletera.Received(1).ActualizarAsync(Arg.Any<Billetera>());
        }

        [TestMethod]
        public async Task CrearDebito_SaldoInsuficiente_DebeRetornarError()
        {
            // Arrange
            Billetera billetera = _constructorBilletera
                .ConId(1)
                .ConDocumentoIdentidad("1234567890")
                .ConNombre("Usuario Test")
                .Construir();
            billetera.Acreditar(50.00M);

            _constructorRepositorioBilletera.ConBilleteraExistente(billetera);

            decimal montoDebito = 100.00M;

            // Act
            (bool Exito, string Mensaje, Movimiento? Movimiento) resultado = await _servicioMovimiento.CrearDebitoAsync(billetera.Id, montoDebito);

            // Assert
            Assert.IsFalse(resultado.Exito);
            Assert.IsNull(resultado.Movimiento);
            Assert.AreEqual("Saldo insuficiente para realizar la operación.", resultado.Mensaje);
            await _repositorioMovimiento.DidNotReceive().AgregarAsync(Arg.Any<Movimiento>());
            await _repositorioBilletera.DidNotReceive().ActualizarAsync(Arg.Any<Billetera>());
        }

        [TestMethod]
        public async Task CrearCredito_ConDatosValidos_DebeCrearCreditoExitosamente()
        {
            // Arrange
            Billetera billetera = _constructorBilletera
                .ConId(1)
                .ConDocumentoIdentidad("1234567890")
                .ConNombre("Usuario Test")
                .Construir();

            _constructorRepositorioBilletera.ConBilleteraExistente(billetera);
            _constructorRepositorioMovimiento.SimularAgregarMovimiento();

            decimal montoCredito = 100.00M;

            // Act
            (bool Exito, string Mensaje, Movimiento? Movimiento) resultado = await _servicioMovimiento.CrearCreditoAsync(billetera.Id, montoCredito);

            // Assert
            Assert.IsTrue(resultado.Exito);
            Assert.IsNotNull(resultado.Movimiento);
            Assert.AreEqual(montoCredito, resultado.Movimiento.Monto);
            Assert.AreEqual(TipoMovimiento.Credito, resultado.Movimiento.Tipo);
            await _repositorioMovimiento.Received(1).AgregarAsync(Arg.Any<Movimiento>());
            await _repositorioBilletera.Received(1).ActualizarAsync(Arg.Any<Billetera>());
        }

        [TestMethod]
        public async Task CrearCredito_BilleteraNoExistente_DebeRetornarError()
        {
            // Arrange
            int billeteraIdInexistente = 999;
            decimal montoCredito = 100.00M;

            // Act
            (bool Exito, string Mensaje, Movimiento? Movimiento) resultado = await _servicioMovimiento.CrearCreditoAsync(billeteraIdInexistente, montoCredito);

            // Assert
            Assert.IsFalse(resultado.Exito);
            Assert.IsNull(resultado.Movimiento);
            Assert.AreEqual("La billetera no existe.", resultado.Mensaje);
            await _repositorioMovimiento.DidNotReceive().AgregarAsync(Arg.Any<Movimiento>());
            await _repositorioBilletera.DidNotReceive().ActualizarAsync(Arg.Any<Billetera>());
        }

        [TestMethod]
        public async Task ObtenerPorBilletera_DebeRetornarMovimientosDeBilletera()
        {
            // Arrange
            Billetera billetera = _constructorBilletera
                .ConId(1)
                .ConDocumentoIdentidad("1234567890")
                .ConNombre("Usuario Test")
                .Construir();

            List<Movimiento> movimientos =
            [
                _constructorMovimiento.ConBilleteraId(billetera.Id).ConMonto(100.00M).ConstruirCredito(),
                _constructorMovimiento.ConBilleteraId(billetera.Id).ConMonto(50.00M).ConstruirDebito()
            ];

            _constructorRepositorioMovimiento.ConMovimientosExistentes(billetera.Id, movimientos);

            // Act
            List<Movimiento> resultado = await _servicioMovimiento.ObtenerPorBilleteraAsync(billetera.Id);

            // Assert
            Assert.AreEqual(2, resultado.Count);
            Assert.IsTrue(resultado.Any(m => m.Tipo == TipoMovimiento.Credito && m.Monto == 100.00M));
            Assert.IsTrue(resultado.Any(m => m.Tipo == TipoMovimiento.Debito && m.Monto == 50.00M));
        }

        [TestMethod]
        public async Task CrearDebito_MontoNegativo_DebeLanzarExcepcion()
        {
            // Arrange
            Billetera billetera = _constructorBilletera
                .ConId(1)
                .ConDocumentoIdentidad("1234567890")
                .ConNombre("Usuario Test")
                .Construir();

            _constructorRepositorioBilletera.ConBilleteraExistente(billetera);
            decimal montoNegativo = -100.00M;

            // Act & Assert
            await Assert.ThrowsExactlyAsync<ArgumentException>(
                async () => await _servicioMovimiento.CrearDebitoAsync(billetera.Id, montoNegativo));
        }

        [TestMethod]
        public async Task CrearCredito_MontoNegativo_DebeLanzarExcepcion()
        {
            // Arrange
            Billetera billetera = _constructorBilletera
                .ConId(1)
                .ConDocumentoIdentidad("1234567890")
                .ConNombre("Usuario Test")
                .Construir();

            _constructorRepositorioBilletera.ConBilleteraExistente(billetera);
            decimal montoNegativo = -100.00M;

            // Act & Assert
            await Assert.ThrowsExactlyAsync<ArgumentException>(
                async () => await _servicioMovimiento.CrearCreditoAsync(billetera.Id, montoNegativo));
        }
    }
}