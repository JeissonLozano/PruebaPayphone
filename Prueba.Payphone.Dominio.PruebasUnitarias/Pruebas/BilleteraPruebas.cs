using Microsoft.VisualStudio.TestTools.UnitTesting;
using Prueba.Payphone.Dominio.Entidades;
using Prueba.Payphone.Dominio.PruebasUnitarias.ConstructoresMock;

namespace Prueba.Payphone.Dominio.PruebasUnitarias.Pruebas
{
    [TestClass]
    public class BilleteraPruebas
    {
        private ConstructorBilletera _constructorBilletera;

        [TestInitialize]
        public void Inicializar()
        {
            _constructorBilletera = new ConstructorBilletera();
        }

        [TestMethod]
        public void CrearBilletera_ConDatosValidos_DebeCrearseCorrectamente()
        {
            // Arrange
            string documentoIdentidad = "1234567890";
            string nombre = "Juan Pérez";

            // Act
            Billetera billetera = _constructorBilletera
                .ConDocumentoIdentidad(documentoIdentidad)
                .ConNombre(nombre)
                .Construir();

            // Assert
            Assert.IsNotNull(billetera);
            Assert.AreEqual(documentoIdentidad, billetera.DocumentoIdentidad);
            Assert.AreEqual(nombre, billetera.Nombre);
            Assert.AreEqual(0, billetera.Saldo);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CrearBilletera_ConDocumentoIdentidadVacio_DebeLanzarExcepcion()
        {
            // Arrange
            string documentoIdentidad = "";
            string nombre = "Juan Pérez";

            // Act
            _constructorBilletera
                .ConDocumentoIdentidad(documentoIdentidad)
                .ConNombre(nombre)
                .Construir();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CrearBilletera_ConNombreVacio_DebeLanzarExcepcion()
        {
            // Arrange
            string documentoIdentidad = "1234567890";
            string nombre = "";

            // Act
            _constructorBilletera
                .ConDocumentoIdentidad(documentoIdentidad)
                .ConNombre(nombre)
                .Construir();
        }

        [TestMethod]
        public void Acreditar_MontoValido_DebeIncrementarSaldo()
        {
            // Arrange
            Billetera billetera = _constructorBilletera.Construir();
            decimal montoAcreditar = 100.00M;

            // Act
            billetera.Acreditar(montoAcreditar);

            // Assert
            Assert.AreEqual(montoAcreditar, billetera.Saldo);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Acreditar_MontoNegativo_DebeLanzarExcepcion()
        {
            // Arrange
            Billetera billetera = _constructorBilletera.Construir();
            decimal montoNegativo = -100.00M;

            // Act
            billetera.Acreditar(montoNegativo);
        }

        [TestMethod]
        public void Debitar_MontoValidoYSaldoSuficiente_DebeDecrementarSaldo()
        {
            // Arrange
            Billetera billetera = _constructorBilletera.Construir();
            decimal montoInicial = 200.00M;
            decimal montoDebitar = 100.00M;
            billetera.Acreditar(montoInicial);

            // Act
            billetera.Debitar(montoDebitar);

            // Assert
            Assert.AreEqual(montoInicial - montoDebitar, billetera.Saldo);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Debitar_SaldoInsuficiente_DebeLanzarExcepcion()
        {
            // Arrange
            Billetera billetera = _constructorBilletera.Construir();
            decimal montoDebitar = 100.00M;

            // Act
            billetera.Debitar(montoDebitar);
        }

        [TestMethod]
        public void ActualizarNombre_NombreValido_DebeActualizarNombre()
        {
            // Arrange
            Billetera billetera = _constructorBilletera.Construir();
            string nuevoNombre = "María López";

            // Act
            billetera.ActualizarNombre(nuevoNombre);

            // Assert
            Assert.AreEqual(nuevoNombre, billetera.Nombre);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ActualizarNombre_NombreVacio_DebeLanzarExcepcion()
        {
            // Arrange
            Billetera billetera = _constructorBilletera.Construir();
            string nombreVacio = "";

            // Act
            billetera.ActualizarNombre(nombreVacio);
        }
    }
} 