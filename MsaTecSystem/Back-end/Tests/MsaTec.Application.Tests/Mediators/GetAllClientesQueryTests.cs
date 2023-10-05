using FluentAssertions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MsaTec.Abstractions.Application.Contract;
using MsaTec.Application.Mediators.Commands;
using MsaTec.Application.Mediators.Queries;
using MsaTec.Application.ViewModels;
using MsaTec.DAL.Data;
using MsaTec.DAL.Repositories;
using MsaTec.DAL.Repositories.Contracts;

namespace MsaTec.Application.Tests.Mediators;

public class GetAllClientesQueryTests
{
    private IMediator _Mediator;
    private ServiceProvider _ServiceProvider;

    public GetAllClientesQueryTests()
    {
        var basePath = AppContext.BaseDirectory;
        // Set up configuration and services here 
        var configuration = new ConfigurationBuilder()
            .SetBasePath(basePath)
            .Build();

        _ServiceProvider = new ServiceCollection()
            .AddSingleton<IConfiguration>(configuration)
            .AddDbContext<DbContextMsaTec>(options => options.UseInMemoryDatabase(databaseName: "InMemoryDatabase"))
            .AddScoped<IClienteRepository, ClientesRepository>() // Replace YourClienteRepository with your actual repository implementation
            .AddAutoMapper(typeof(InsertClienteCommand)) // Assuming AutoMapper profiles are configured in Startup class
            .AddScoped<IRequestHandler<InsertClienteCommand, ICommandResult>, InsertClienteCommand.Handler>()
            .AddScoped<IRequestHandler<GetAllClientesQuery, IEnumerable<ClienteViewModelForList>>, GetAllClientesQuery.Handler>()
            .AddScoped<IMediator, Mediator>()
            .BuildServiceProvider();
    }

    [Fact]
    public async Task Handle_ReturnsClientViewModelList()
    {
        // Arrange
        using (var scope = _ServiceProvider.CreateScope())
        {
            var dataList = TestHelper.LoadClienteViewModelsData();
            using (var dbContext = scope.ServiceProvider.GetRequiredService<DbContextMsaTec>())
            {
                _Mediator = _ServiceProvider.GetRequiredService<IMediator>();
                
                foreach (var clienteViewModel in dataList)
                {
                    // Act
                    var command = new InsertClienteCommand(clienteViewModel);
                    var rst = await _Mediator.Send(command, CancellationToken.None);
                    rst.IsSuccess.Should().BeTrue();
                };

                // Act
                var qry = new GetAllClientesQuery();
                var clientesInserted = await _Mediator.Send(qry, CancellationToken.None);
                // Assert
                clientesInserted.Should().NotBeNull();
                clientesInserted.Should().HaveCount(dataList.Count);
            }
        }
    }

}
