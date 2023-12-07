using Microsoft.Extensions.DependencyInjection;

namespace OnlineStore.Domain.Extensions;
public static class DomainServiceCollectionExtension
{
    public static void AddDomain(this IServiceCollection services)
    {
        services.AddMediatR(options =>
        {
            options.RegisterServicesFromAssembly(typeof(DomainServiceCollectionExtension).Assembly);
        });

    }
}
