using Prueba.Payphone.Dominio.Entidades;
using Prueba.Payphone.Dominio.Enumeradores;

namespace Prueba.Payphone.Dominio.PruebasUnitarias.ConstructoresMock
{
    public class ConstructorMovimiento
    {
        private int _billeteraId = 1;
        private decimal _monto = 100.00M;
        private TipoMovimiento _tipo = TipoMovimiento.Credito;

        public ConstructorMovimiento ConBilleteraId(int billeteraId)
        {
            _billeteraId = billeteraId;
            return this;
        }

        public ConstructorMovimiento ConMonto(decimal monto)
        {
            _monto = monto;
            return this;
        }

        public Movimiento ConstruirCredito()
        {
            return Movimiento.CrearCredito(_billeteraId, _monto);
        }

        public Movimiento ConstruirDebito()
        {
            return Movimiento.CrearDebito(_billeteraId, _monto);
        }
    }
} 