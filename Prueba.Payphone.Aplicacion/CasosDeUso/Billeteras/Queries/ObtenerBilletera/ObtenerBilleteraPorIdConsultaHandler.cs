using MediatR;
using Prueba.Payphone.Dominio.Entidades;
using Prueba.Payphone.Dominio.Servicios.Billeteras.Puertos;

namespace Prueba.Payphone.Aplicacion.CasosDeUso.Billeteras.Queries.ObtenerBilletera;

public class ManejadorObtenerBilleteraPorId(IBilleteraRepositorio repositorioBilletera)
    : IRequestHandler<ObtenerBilleteraPorIdConsulta, BilleteraDto?>
{
    public async Task<BilleteraDto?> Handle(ObtenerBilleteraPorIdConsulta consulta, CancellationToken cancellationToken)
    {
        Billetera? billetera = await repositorioBilletera.ObtenerPorIdAsync(consulta.Id);

        if (billetera == null)
        {
            return null;
        }

        return new BilleteraDto
        {
            Id = billetera.Id,
            DocumentoIdentidad = billetera.DocumentoIdentidad,
            Nombre = billetera.Nombre,
            Saldo = billetera.Saldo,
            FechaCreacion = billetera.FechaCreacion,
            FechaActualizacion = billetera.FechaActualizacion
        };
    }
}