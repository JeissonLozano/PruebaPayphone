using MediatR;
using Prueba.Payphone.Aplicacion.Puertos;
using Prueba.Payphone.Dominio.Entidades;
using Prueba.Payphone.Dominio.Servicios.Billeteras.Puertos;

namespace Prueba.Payphone.Aplicacion.CasosDeUso.Billeteras.Commands.CrearBilletera;

public class ManejadorCrearBilletera(
    IServicioBilletera servicioBilletera,
    IUnidadDeTrabajo unidadDeTrabajo
) : IRequestHandler<CrearBilleteraComando, ResultadoOperacionBilletera>
{
    public async Task<ResultadoOperacionBilletera> Handle(CrearBilleteraComando peticion, CancellationToken cancellationToken)
    {
        (bool exito, string mensaje, Billetera? billetera) = await servicioBilletera.CrearBilleteraAsync(
            peticion.DocumentoIdentidad,
            peticion.Nombre,
            peticion.SaldoInicial);

        if (!exito)
        {
            return new ResultadoOperacionBilletera
            {
                Exito = false,
                Mensaje = mensaje
            };
        }

        BilleteraDto billeteraDto = new()
        {
            Id = billetera!.Id,
            DocumentoIdentidad = billetera.DocumentoIdentidad,
            Nombre = billetera.Nombre,
            Saldo = billetera.Saldo,
            FechaCreacion = billetera.FechaCreacion,
            FechaActualizacion = billetera.FechaActualizacion
        };

        await unidadDeTrabajo.GuardarCambiosAsync(cancellationToken);

        return new ResultadoOperacionBilletera
        {
            Exito = true,
            Mensaje = mensaje,
            Billetera = billeteraDto
        };
    }
}