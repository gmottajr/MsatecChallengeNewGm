using FluentAssertions;
using Microsoft.Extensions.Configuration;
using MsaTec.Core;

namespace MsaTec.DAL.Tests;

public class DbConnectionTests
{
    private readonly IConfiguration _configuration;

    public DbConnectionTests()
    {
        var basePath = AppContext.BaseDirectory;

        // Read configuration from appsettings.json
        _configuration = new ConfigurationBuilder()
        .SetBasePath(basePath)
        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
        .Build();
    }

    [Fact]
    public void TestPostgresConnection_ShouldSucceedWithValidConnectionString()
    {
        // Arrange
        string? postgresConnectionString = _configuration.GetConnectionString(MsatecConsts.ConnectionStringDbTestAlias);

        // Act
        var connectionResult = DbConnectionTester.TestPostgresConnection(postgresConnectionString);

        // Assert
        connectionResult.Should().BeTrue("because a valid PostgreSQL connection string should succeed");
    }

    [Fact]
    public void TestPostgresConnection_ShouldFailWithInvalidConnectionString()
    {
        // Arrange
        var invalidPostgresConnectionString = "invalid_connection_string";

        // Act
        var connectionResult = DbConnectionTester.TestPostgresConnection(invalidPostgresConnectionString);

        // Assert
        connectionResult.Should().BeFalse("because an invalid PostgreSQL connection string should fail");
    }
}
