using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using Prueba.Payphone.Dominio.Entidades;
using Prueba.Payphone.Dominio.Servicios.Billeteras;
using Prueba.Payphone.Dominio.Servicios.Billeteras.Puertos;
using Prueba.Payphone.Dominio.PruebasUnitarias.ConstructoresMock;

namespace Prueba.Payphone.Dominio.PruebasUnitarias.Pruebas
{
    [TestClass]
    public class ServicioBilleteraPruebas
    {
        private ConstructorBilletera _constructorBilletera;
        private ConstructorRepositorioBilletera _constructorRepositorio;
        private ServicioBilletera _servicioBilletera;
        private IBilleteraRepositorio _repositorioBilletera;

        [TestInitialize]
        public void Inicializar()
        {
            _constructorBilletera = new ConstructorBilletera();
            _constructorRepositorio = new ConstructorRepositorioBilletera();
            _repositorioBilletera = _constructorRepositorio.Construir();
            _servicioBilletera = new ServicioBilletera(_repositorioBilletera);
        }

        [TestMethod]
        public async Task CrearBilletera_ConDatosValidos_DebeCrearBilleteraExitosamente()
        {
            // Arrange
            string documentoIdentidad = "1234567890";
            string nombre = "Juan Pérez";
            decimal saldoInicial = 100.00M;
            _constructorRepositorio.SimularAgregarBilletera();

            // Act
            (bool Exito, string Mensaje, Billetera? Billetera) resultado = await _servicioBilletera
                .CrearBilleteraAsync(documentoIdentidad, nombre, saldoInicial);

            // Assert
            Assert.IsTrue(resultado.Exito);
            Assert.IsNotNull(resultado.Billetera);
            Assert.AreEqual(documentoIdentidad, resultado.Billetera.DocumentoIdentidad);
            Assert.AreEqual(nombre, resultado.Billetera.Nombre);
            Assert.AreEqual(saldoInicial, resultado.Billetera.Saldo);
            await _repositorioBilletera.Received(1).AgregarAsync(Arg.Any<Billetera>());
        }

        [TestMethod]
        public async Task CrearBilletera_ConDocumentoExistente_DebeRetornarError()
        {
            // Arrange
            Billetera billeteraExistente = _constructorBilletera
                .ConDocumentoIdentidad("1234567890")
                .ConNombre("Usuario Existente")
                .Construir();

            _constructorRepositorio.ConBilleteraExistente(billeteraExistente);

            // Act
            (bool Exito, string Mensaje, Billetera? Billetera) resultado = await _servicioBilletera.CrearBilleteraAsync(
                billeteraExistente.DocumentoIdentidad,
                "Nuevo Usuario",
                100.00M);

            // Assert
            Assert.IsFalse(resultado.Exito);
            Assert.IsNull(resultado.Billetera);
            Assert.AreEqual("Ya existe una billetera con ese documento.", resultado.Mensaje);
            await _repositorioBilletera.DidNotReceive().AgregarAsync(Arg.Any<Billetera>());
        }

        [TestMethod]
        public async Task ObtenerTodas_DebeRetornarListaDeBilleteras()
        {
            // Arrange
            Billetera billetera1 = _constructorBilletera
                .ConDocumentoIdentidad("1111111111")
                .ConNombre("Usuario 1")
                .Construir();

            Billetera billetera2 = _constructorBilletera
                .ConDocumentoIdentidad("2222222222")
                .ConNombre("Usuario 2")
                .Construir();

            _constructorRepositorio
                .ConBilleteraExistente(billetera1)
                .ConBilleteraExistente(billetera2)
                .SimularObtenerTodos();

            // Act
            List<Billetera> billeteras = await _servicioBilletera.ObtenerTodasAsync();

            // Assert
            Assert.AreEqual(2, billeteras.Count);
            Assert.IsTrue(billeteras.Any(b => b.DocumentoIdentidad == "1111111111"));
            Assert.IsTrue(billeteras.Any(b => b.DocumentoIdentidad == "2222222222"));
        }

        [TestMethod]
        public async Task ActualizarBilletera_ConDatosValidos_DebeActualizarExitosamente()
        {
            // Arrange
            Billetera billeteraExistente = _constructorBilletera
                .ConDocumentoIdentidad("1234567890")
                .ConNombre("Nombre Original")
                .Construir();

            _constructorRepositorio.ConBilleteraExistente(billeteraExistente);

            string nuevoNombre = "Nombre Actualizado";

            // Act
            (bool Exito, string Mensaje, Billetera? Billetera) resultado = await _servicioBilletera.ActualizarBilleteraAsync(
                billeteraExistente.Id,
                billeteraExistente.DocumentoIdentidad,
                nuevoNombre);

            // Assert
            Assert.IsTrue(resultado.Exito);
            Assert.IsNotNull(resultado.Billetera);
            Assert.AreEqual(nuevoNombre, resultado.Billetera.Nombre);
            await _repositorioBilletera.Received(1).ActualizarAsync(Arg.Any<Billetera>());
        }

        [TestMethod]
        public async Task EliminarBilletera_BilleteraExistente_DebeEliminarExitosamente()
        {
            // Arrange
            Billetera billeteraExistente = _constructorBilletera
                .ConDocumentoIdentidad("1234567890")
                .ConNombre("Usuario a Eliminar")
                .Construir();

            _constructorRepositorio.ConBilleteraExistente(billeteraExistente);

            // Act
            (bool Exito, string Mensaje) resultado = await _servicioBilletera.EliminarBilleteraAsync(billeteraExistente.Id);

            // Assert
            Assert.IsTrue(resultado.Exito);
            Assert.AreEqual("Billetera eliminada exitosamente.", resultado.Mensaje);
            await _repositorioBilletera.Received(1).EliminarAsync(billeteraExistente.Id);
        }

        [TestMethod]
        public async Task EliminarBilletera_BilleteraNoExistente_DebeRetornarError()
        {
            // Arrange
            int idNoExistente = 999;

            // Act
            (bool Exito, string Mensaje) resultado = await _servicioBilletera.EliminarBilleteraAsync(idNoExistente);

            // Assert
            Assert.IsFalse(resultado.Exito);
            Assert.AreEqual("La billetera no existe.", resultado.Mensaje);
            await _repositorioBilletera.DidNotReceive().EliminarAsync(Arg.Any<int>());
        }
    }
} 