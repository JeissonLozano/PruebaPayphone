using Prueba.Payphone.Dominio.Entidades;

namespace Prueba.Payphone.Dominio.PruebasUnitarias.ConstructoresMock
{
    public class ConstructorBilletera
    {
        private string _documentoIdentidad = "1234567890";
        private string _nombre = "Usuario Prueba";
        private int _id = 1;

        public ConstructorBilletera ConDocumentoIdentidad(string documentoIdentidad)
        {
            _documentoIdentidad = documentoIdentidad;
            return this;
        }

        public ConstructorBilletera ConNombre(string nombre)
        {
            _nombre = nombre;
            return this;
        }

        public ConstructorBilletera ConId(int id)
        {
            _id = id;
            return this;
        }

        public Billetera Construir()
        {
            Billetera billetera = new(_documentoIdentidad, _nombre);
            typeof(EntidadDominio).GetProperty("Id")?.SetValue(billetera, _id);
            return billetera;
        }
    }
} 