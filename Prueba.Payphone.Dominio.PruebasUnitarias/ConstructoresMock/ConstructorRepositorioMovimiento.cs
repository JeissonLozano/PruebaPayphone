using NSubstitute;
using Prueba.Payphone.Dominio.Entidades;
using Prueba.Payphone.Dominio.Servicios.Movimientos.Puertos;

namespace Prueba.Payphone.Dominio.PruebasUnitarias.ConstructoresMock
{
    public class ConstructorRepositorioMovimiento
    {
        private readonly IMovimientoRepositorio _repositorio;
        private readonly List<Movimiento> _movimientos;

        public ConstructorRepositorioMovimiento()
        {
            _repositorio = Substitute.For<IMovimientoRepositorio>();
            _movimientos = [];
        }

        public ConstructorRepositorioMovimiento SimularAgregarMovimiento()
        {
            _repositorio
                .AgregarAsync(Arg.Any<Movimiento>())
                .Returns(callInfo =>
                {
                    Movimiento movimiento = callInfo.Arg<Movimiento>();
                    _movimientos.Add(movimiento);
                    return movimiento;
                });
            return this;
        }

        public ConstructorRepositorioMovimiento ConMovimientosExistentes(int billeteraId, List<Movimiento> movimientos)
        {
            _movimientos.AddRange(movimientos);
            _repositorio.ObtenerPorBilleteraAsync(billeteraId).Returns(
                movimientos.Where(m => m.BilleteraId == billeteraId).ToList());
            return this;
        }

        public IMovimientoRepositorio Construir()
        {
            return _repositorio;
        }
    }
} 