namespace Prueba.Payphone.API.Endpoints;

public static class EndpointExtensions
{
    public static IEndpointRouteBuilder MapEndpointDefinitions(
        this IEndpointRouteBuilder routes
    )
    {
        using (IServiceScope scope = routes.ServiceProvider.CreateScope())
        {
            IEnumerable<IEndpointRegistrar> servicios =
                scope.ServiceProvider.GetServices<IEndpointRegistrar>();

            foreach (IEndpointRegistrar registrar in servicios)
            {
                registrar.RegisterRoutes(routes);
            }
        }

        return routes;
    }
}
