using Microsoft.VisualStudio.TestTools.UnitTesting;
using Prueba.Payphone.Dominio.Entidades;
using Prueba.Payphone.Dominio.Enumeradores;
using Prueba.Payphone.Dominio.PruebasUnitarias.ConstructoresMock;

namespace Prueba.Payphone.Dominio.PruebasUnitarias.Pruebas
{
    [TestClass]
    public class MovimientoPruebas
    {
        private ConstructorMovimiento _constructorMovimiento;

        [TestInitialize]
        public void Inicializar()
        {
            _constructorMovimiento = new ConstructorMovimiento();
        }

        [TestMethod]
        public void CrearCredito_ConDatosValidos_DebeCrearseCorrectamente()
        {
            // Arrange
            int billeteraId = 1;
            decimal monto = 100.00M;

            // Act
            Movimiento movimiento = _constructorMovimiento
                .ConBilleteraId(billeteraId)
                .ConMonto(monto)
                .ConstruirCredito();

            // Assert
            Assert.IsNotNull(movimiento);
            Assert.AreEqual(billeteraId, movimiento.BilleteraId);
            Assert.AreEqual(monto, movimiento.Monto);
            Assert.AreEqual(TipoMovimiento.Credito, movimiento.Tipo);
        }

        [TestMethod]
        public void CrearDebito_ConDatosValidos_DebeCrearseCorrectamente()
        {
            // Arrange
            int billeteraId = 1;
            decimal monto = 100.00M;

            // Act
            Movimiento movimiento = _constructorMovimiento
                .ConBilleteraId(billeteraId)
                .ConMonto(monto)
                .ConstruirDebito();

            // Assert
            Assert.IsNotNull(movimiento);
            Assert.AreEqual(billeteraId, movimiento.BilleteraId);
            Assert.AreEqual(monto, movimiento.Monto);
            Assert.AreEqual(TipoMovimiento.Debito, movimiento.Tipo);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CrearMovimiento_ConBilleteraIdInvalido_DebeLanzarExcepcion()
        {
            // Arrange
            int billeteraIdInvalido = 0;

            // Act
            _constructorMovimiento
                .ConBilleteraId(billeteraIdInvalido)
                .ConstruirCredito();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CrearMovimiento_ConMontoInvalido_DebeLanzarExcepcion()
        {
            // Arrange
            decimal montoInvalido = 0;

            // Act
            _constructorMovimiento
                .ConMonto(montoInvalido)
                .ConstruirCredito();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CrearMovimiento_ConMontoNegativo_DebeLanzarExcepcion()
        {
            // Arrange
            decimal montoNegativo = -100.00M;

            // Act
            _constructorMovimiento
                .ConMonto(montoNegativo)
                .ConstruirCredito();
        }
    }
} 