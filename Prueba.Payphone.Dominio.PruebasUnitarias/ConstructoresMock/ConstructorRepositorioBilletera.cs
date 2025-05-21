using NSubstitute;
using Prueba.Payphone.Dominio.Entidades;
using Prueba.Payphone.Dominio.Servicios.Billeteras.Puertos;

namespace Prueba.Payphone.Dominio.PruebasUnitarias.ConstructoresMock
{
    public class ConstructorRepositorioBilletera
    {
        private readonly IBilleteraRepositorio _repositorio;
        private readonly List<Billetera> _billeteras;

        public ConstructorRepositorioBilletera()
        {
            _repositorio = Substitute.For<IBilleteraRepositorio>();
            _billeteras = [];
        }

        public ConstructorRepositorioBilletera ConBilleteraExistente(Billetera billetera)
        {
            _billeteras.Add(billetera);
            _repositorio.ObtenerPorIdAsync(billetera.Id).Returns(billetera);
            _repositorio.ObtenerPorDocumentoIdentidadAsync(billetera.DocumentoIdentidad).Returns(billetera);
            return this;
        }

        public ConstructorRepositorioBilletera SimularAgregarBilletera()
        {
            _repositorio
                .AgregarAsync(Arg.Any<Billetera>())
                .Returns(callInfo =>
                {
                    Billetera billetera = callInfo.Arg<Billetera>();
                    _billeteras.Add(billetera);
                    return billetera;
                });
            return this;
        }

        public ConstructorRepositorioBilletera SimularObtenerTodos()
        {
            _repositorio.ObtenerTodosAsync().Returns(_billeteras);
            return this;
        }

        public IBilleteraRepositorio Construir()
        {
            return _repositorio;
        }
    }
} 