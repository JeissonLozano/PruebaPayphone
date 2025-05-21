using MediatR;
using Prueba.Payphone.Aplicacion.CasosDeUso.Comun;
using Prueba.Payphone.Aplicacion.Puertos;
using Prueba.Payphone.Dominio.Servicios.Billeteras.Puertos;

namespace Prueba.Payphone.Aplicacion.CasosDeUso.Billeteras.Commands.EliminarBilletera;

public class ManejadorEliminarBilletera(
    IServicioBilletera servicioBilletera,
    IUnidadDeTrabajo unidadDeTrabajo
) : IRequestHandler<EliminarBilleteraComando, ResultadoOperacion>
{
    public async Task<ResultadoOperacion> Handle(EliminarBilleteraComando comando, CancellationToken cancellationToken)
    {
        (bool exito, string mensaje) = await servicioBilletera.EliminarBilleteraAsync(comando.Id);

        await unidadDeTrabajo.GuardarCambiosAsync(cancellationToken);

        return new ResultadoOperacion
        {
            Exito = exito,
            Mensaje = mensaje
        };
    }
}