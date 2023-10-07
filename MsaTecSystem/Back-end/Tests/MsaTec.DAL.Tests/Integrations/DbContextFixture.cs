using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MsaTec.Core;
using MsaTec.DAL.Data;
using MsaTec.DAL.Repositories.Contracts;
using MsaTec.DAL.Repositories;

namespace MsaTec.DAL.Tests.Integrations;

[Collection("Non-Parallel Collection")]
public class DbContextFixture : IDisposable
{
    public ServiceProvider ServiceProvider { get; private set; }

    public DbContextFixture()
    {
        var basePath = AppContext.BaseDirectory;
        // Set up configuration and services here 
        var configuration = new ConfigurationBuilder()
            .SetBasePath(basePath)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

        var services = new ServiceCollection();

        services.AddSingleton(configuration);
        services.AddDbContext<DbContextMsaTec>(options =>
        {
            options.UseNpgsql(configuration.GetConnectionString(MsatecConsts.ConnectionStringDbTestAlias)); // Adjust the connection string name as needed
        });
        services.AddSingleton<IConfiguration>(configuration);
        services.AddScoped<IClienteRepository, ClientesRepository>();
        ServiceProvider = services.BuildServiceProvider();
    }

    public void Dispose()
    {
        // Clean up any resources after the tests are run
        ServiceProvider.Dispose();
    }
}
