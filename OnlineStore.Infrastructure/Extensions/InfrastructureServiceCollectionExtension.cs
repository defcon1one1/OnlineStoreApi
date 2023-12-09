using Microsoft.Extensions.DependencyInjection;
using OnlineStore.Domain.Interfaces.Shared;
using OnlineStore.Domain.Interfaces.Repositories;
using OnlineStore.Infrastructure.DAL;
using OnlineStore.Infrastructure.DAL.Repositories;
using OnlineStore.Infrastructure.Services;


namespace OnlineStore.Infrastructure.Extensions;
public static class InfrastructureServiceCollectionExtension
{
    public static void AddInfrastructure(this IServiceCollection services)
    {
        services.AddDbContext<AppDbContext>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<ITransactionRepository, TransactionRepository>();
        services.AddScoped<IDataSeederService, DataSeederService>();
    }
}
