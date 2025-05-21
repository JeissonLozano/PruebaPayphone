using MediatR;
using Prueba.Payphone.Aplicacion.Puertos;
using Prueba.Payphone.Dominio.Servicios.Billeteras.Puertos;

namespace Prueba.Payphone.Aplicacion.CasosDeUso.Billeteras.Commands.ActualizarBilletera;

public class ManejadorActualizarBilletera(
    IServicioBilletera servicioBilletera,
    IUnidadDeTrabajo unidadDeTrabajo
) : IRequestHandler<ActualizarBilleteraComando, ResultadoOperacionBilletera>
{
    public async Task<ResultadoOperacionBilletera> Handle(ActualizarBilleteraComando comando, CancellationToken cancellationToken)
    {
        (bool exito, string mensaje, Dominio.Entidades.Billetera billetera) = await servicioBilletera.ActualizarBilleteraAsync(
            comando.Id,
            comando.DocumentoIdentidad,
            comando.Nombre);

        if (!exito)
        {
            return new ResultadoOperacionBilletera
            {
                Exito = false,
                Mensaje = mensaje
            };
        }

        await unidadDeTrabajo.GuardarCambiosAsync(cancellationToken);

        return new ResultadoOperacionBilletera
        {
            Exito = true,
            Mensaje = mensaje,
            Billetera = new BilleteraDto
            {
                Id = billetera!.Id,
                DocumentoIdentidad = billetera.DocumentoIdentidad,
                Nombre = billetera.Nombre,
                Saldo = billetera.Saldo,
                FechaCreacion = billetera.FechaCreacion,
                FechaActualizacion = billetera.FechaActualizacion
            }
        };
    }
}