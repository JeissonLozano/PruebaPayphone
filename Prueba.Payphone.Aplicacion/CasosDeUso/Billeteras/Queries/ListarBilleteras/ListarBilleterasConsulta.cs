using MediatR;

namespace Prueba.Payphone.Aplicacion.CasosDeUso.Billeteras.Queries.ListarBilleteras;

public record ListarBilleterasConsulta : IRequest<List<BilleteraDto>>;