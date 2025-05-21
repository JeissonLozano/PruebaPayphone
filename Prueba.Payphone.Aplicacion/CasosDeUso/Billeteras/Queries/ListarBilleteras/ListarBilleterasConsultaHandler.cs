using MediatR;
using Prueba.Payphone.Dominio.Entidades;
using Prueba.Payphone.Dominio.Servicios.Billeteras.Puertos;

namespace Prueba.Payphone.Aplicacion.CasosDeUso.Billeteras.Queries.ListarBilleteras;

public class ManejadorListarBilleteras(IBilleteraRepositorio repositorioBilletera)
    : IRequestHandler<ListarBilleterasConsulta, List<BilleteraDto>>
{
    public async Task<List<BilleteraDto>> Handle(ListarBilleterasConsulta consulta, CancellationToken cancellationToken)
    {
        List<Billetera> billeteras = await repositorioBilletera.ObtenerTodosAsync();

        return [.. billeteras.Select(b => new BilleteraDto
        {
            Id = b.Id,
            DocumentoIdentidad = b.DocumentoIdentidad,
            Nombre = b.Nombre,
            Saldo = b.Saldo,
            FechaCreacion = b.FechaCreacion,
            FechaActualizacion = b.FechaActualizacion
        })];
    }
}