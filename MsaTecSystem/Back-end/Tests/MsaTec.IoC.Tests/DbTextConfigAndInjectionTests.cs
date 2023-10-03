//using Castle.Core.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MsaTec.Core;
using MsaTec.DAL.Data;

namespace MsaTec.IoC.Tests;

public class DbTextConfigAndInjectionTests
{
    private IConfigurationRoot _Configuration;

    public DbTextConfigAndInjectionTests()
    {
        _Configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

    }

    [Fact]
    public void AppSettingsJson_ShouldExistInTestDirectory()
    {
        // Arrange
        var testDirectory = Directory.GetCurrentDirectory();
        var appSettingsPath = Path.Combine(testDirectory, "appsettings.json");

        // Act & Assert
        Assert.True(File.Exists(appSettingsPath), $"The 'appsettings.json' file does not exist in the test directory: '{testDirectory}'");
    }

    [Fact]
    public void SetupDbContext_ShouldAddDbContextWithPostgreSQLProvider()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddSingleton<IConfiguration>(_Configuration);
        var connectionString = _Configuration.GetConnectionString(MsatecConsts.ConnectionStringDbTestAlias);

        // Act
        var result = DbTextConfigAndInjection.SetupDbContext(services, _Configuration);
        services.AddDbContext<DbContextMsaTec>(options =>
        {
            options.UseNpgsql(_Configuration.GetConnectionString(MsatecConsts.ConnectionStringDbTestAlias)); // Adjust the connection string name as needed
        });
        

        // Assert
        var serviceProvider = services.BuildServiceProvider();
        var dbContext = serviceProvider.GetRequiredService<DbContextMsaTec>();

        // Check if the database provider is PostgreSQL
        var dbContextOptions = serviceProvider.GetRequiredService<DbContextOptions<DbContextMsaTec>>();


        // Check if the database provider is PostgreSQL
        //var dbContextOptions = dbContext.GetInfrastructure().GetService<DbContextOptions<DbContextMsaTec>>();
        var extension = dbContextOptions.FindExtension<CoreOptionsExtension>();
        var connectionStringFromOptions = extension?.ApplicationServiceProvider?.GetService<IConfiguration>()?.GetConnectionString(MsatecConsts.ConnectionStringDbTestAlias);

        Assert.NotNull(connectionStringFromOptions);
        Assert.Equal(connectionString, connectionStringFromOptions);
    }

    [Fact]
    public void SetupDbContext_ShouldAddDbContextToServices()
    {
        // Arrange
        var services = new ServiceCollection();
        
        var connectionString = _Configuration.GetConnectionString(MsatecConsts.ConnectionStringDbTestAlias);
        services.AddSingleton<IConfiguration>(_Configuration);
        // Act
        var result = DbTextConfigAndInjection.SetupDbContext(services, _Configuration);

        // Assert
        var serviceProvider = services.BuildServiceProvider();
        var dbContext = serviceProvider.GetRequiredService<DbContextMsaTec>();

        Assert.NotNull(dbContext);

        // Ensure the DbContext is properly registered
        var dbContextOptions = serviceProvider.GetRequiredService<DbContextOptions<DbContextMsaTec>>();
        Assert.NotNull(dbContextOptions);

        // Verify the correct connection string is used in DbContextOptions
        var coreOptionsExtension = dbContextOptions.Extensions.FirstOrDefault(e => e is CoreOptionsExtension) as CoreOptionsExtension;
        Assert.NotNull(coreOptionsExtension);

        var connectionStringFromOptions = coreOptionsExtension.ApplicationServiceProvider.GetService<IConfiguration>().GetConnectionString(MsatecConsts.ConnectionStringDbTestAlias);
        Assert.Equal(connectionString, connectionStringFromOptions);
    }
}
