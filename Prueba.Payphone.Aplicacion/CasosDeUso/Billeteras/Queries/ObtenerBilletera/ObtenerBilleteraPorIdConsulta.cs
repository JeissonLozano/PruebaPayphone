using MediatR;

namespace Prueba.Payphone.Aplicacion.CasosDeUso.Billeteras.Queries.ObtenerBilletera;

public record ObtenerBilleteraPorIdConsulta(int Id) : IRequest<BilleteraDto?>;