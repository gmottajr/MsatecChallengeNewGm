using AutoMapper;
using FluentAssertions;
using FluentAssertions.Common;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MsaTec.Abstractions.Application.Contract;
using MsaTec.Application.Mediators.Commands;
using MsaTec.Application.Mediators.Queries;
using MsaTec.Application.ProfilerMapping;
using MsaTec.Application.ViewModels;
using MsaTec.DAL.Data;
using MsaTec.DAL.Repositories;
using MsaTec.DAL.Repositories.Contracts;
using MsaTec.WebApi.Controllers;

namespace MsaTec.Presentation.Tests.Controllers;

public class ClienteControllerTests
{
    private readonly ServiceProvider _ServiceProvider;
    private readonly IMapper _Mapper;
    private IMediator _Mediator;
    public ClienteControllerTests()
    {
        var basePath = AppContext.BaseDirectory;
        // Set up configuration and services here 
        var configuration = new ConfigurationBuilder()
            .SetBasePath(basePath)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

        // Configure AutoMapper with the MappingProfile
        var mapperConfig = new MapperConfiguration(config =>
        {
            config.AddProfile(new ClienteProfilerMapping());
            config.AddProfile(new TelefoneProfilerMapping());
        });

        _Mapper = mapperConfig.CreateMapper();
        _ServiceProvider = new ServiceCollection()
            .AddDbContext<DbContextMsaTec>(options => options.UseInMemoryDatabase(databaseName: "InMemoryDatabase"))
            .AddAutoMapper(typeof(ClienteControllerTests))
            .AddScoped<IClienteRepository, ClientesRepository>()
            .AddScoped<IRequestHandler<GetAllClientesQuery, IEnumerable<ClienteViewModelList>>, GetAllClientesQuery.Handler>()
            .AddScoped<IRequestHandler<GetClienteByIdQuery, ClienteViewModel>, GetClienteByIdQuery.Handler>()
            .AddScoped<IRequestHandler<InsertClienteCommand, ICommandResult>, InsertClienteCommand.Handler>()
            .AddScoped<IRequestHandler<UpdateClienteCommand, ICommandResult>, UpdateClienteCommand.Handler>()
            .AddScoped<IRequestHandler<DeleteClienteCommand, ICommandResult>, DeleteClienteCommand.Handler>()
            .AddScoped<IMediator, Mediator>()
            .AddSingleton(_Mapper)
            .AddSingleton<IConfiguration>(configuration)
            .BuildServiceProvider();

        InsertDataAsync();
    }

    [Fact]
    public async Task Get_ShouldReturnsSuccessResultClientList()
    {
        // Arrange
        using (var scope = _ServiceProvider.CreateScope())
        {
            var controller = new ClienteController(scope.ServiceProvider.GetRequiredService<IMediator>());

            // Act
            var result = await controller.Get();

            // Assert
            var okObjectResult = result.Should().BeOfType<OkObjectResult>().Subject;
            okObjectResult.Value.Should().NotBeNull();
            okObjectResult.StatusCode.Should().Be(200);
        }
    }

    [Fact]
    public async Task GetBy_WithValidId_ShouldReturnsSuccessResult()
    {
        // Arrange
        using (var scope = _ServiceProvider.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<DbContextMsaTec>();

            var controller = new ClienteController(scope.ServiceProvider.GetRequiredService<IMediator>());
            
            foreach(var clientitem in dbContext.Clientes)
            {
                // Act
                var result = await controller.Get(clientitem.Id);

                // Assert
                var okObjectResult = result.Should().BeOfType<OkObjectResult>().Subject;
                okObjectResult.Value.Should().NotBeNull();
                okObjectResult.StatusCode.Should().Be(200);
            }
        }
    }

    [Fact]
    public async Task CreateCliente_ShouldReturnsSuccessResult()
    {
        // Arrange
        using (var scope = _ServiceProvider.CreateScope())
        {
            var controller = new ClienteController(scope.ServiceProvider.GetRequiredService<IMediator>());
            // Arrange
            foreach (var addingClienteViewModel in TestHelper.LoadClienteViewModelsDataForInsert()) 
            {
                // Act
                var result = await controller.CreateCliente(addingClienteViewModel);

                // Assert
                var createdAtActionResult = result.Should().BeOfType<CreatedAtActionResult>().Subject;
                createdAtActionResult.ActionName.Should().Be(nameof(ClienteController.Get));
                createdAtActionResult.Value.Should().Be(addingClienteViewModel);
            }
           
        }
    }

    [Fact]
    public async Task UpdateCliente_ShouldReturnsSuccessResult()
    {
        
        using (var scope = _ServiceProvider.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<DbContextMsaTec>();
            
            var controller = new ClienteController(scope.ServiceProvider.GetRequiredService<IMediator>());

            foreach(var cliente in dbContext.Clientes)
            {
                var mappedCliente = _Mapper.Map<ClienteViewModel>(cliente);
                var updatingNome = $"{mappedCliente.Nome} da Silva Test";
                mappedCliente.Nome = updatingNome;
                // Act
                var result = await controller.UpdateCliente(mappedCliente);

                // Assert
                var createdAtActionResult = result.Should().BeOfType<CreatedAtActionResult>().Subject;
                createdAtActionResult.ActionName.Should().Be(nameof(ClienteController.Get));
                createdAtActionResult.Value.Should().Be(mappedCliente);
            }
        }
    }

    [Fact]
    public async Task Delete_ShouldReturnsSuccessResult()
    {
        //Arrange
        using (var scope = _ServiceProvider.CreateScope())
        {
            var controller = new ClienteController(scope.ServiceProvider.GetRequiredService<IMediator>());
            var dbContext = scope.ServiceProvider.GetRequiredService<DbContextMsaTec>();
            foreach (var cliente in dbContext.Clientes)
            {
                // Act
                var result = await controller.Delete(cliente.Id);

                // Assert
                var okObjectResult = result.Should().BeOfType<OkObjectResult>().Subject;
                okObjectResult.Value.Should().Be($"Cliente excluido: {cliente.Id}");
            }
        }
    }

    [Fact]
    public async Task Get_WithInvalidId_ShouldReturnsNotFoundResult()
    {
        // Arrange
        var invalidClientId = Guid.NewGuid(); // Provide an invalid client ID for testing

        using (var scope = _ServiceProvider.CreateScope())
        {
            var controller = new ClienteController(scope.ServiceProvider.GetRequiredService<IMediator>());

            // Act
            var result = await controller.Get(invalidClientId);

            // Assert
            result.Should().BeOfType<NotFoundResult>();
        }
    }

    private async Task InsertDataAsync()
    {
        using (var scope = _ServiceProvider.CreateScope())
        {
            using (var dbContext = scope.ServiceProvider.GetRequiredService<DbContextMsaTec>())
            {
                _Mediator = _ServiceProvider.GetRequiredService<IMediator>();

                foreach (var clienteViewModel in TestHelper.LoadClienteViewModelsData())
                {
                    var command = new InsertClienteCommand(clienteViewModel);
                    var rst = await _Mediator.Send(command, CancellationToken.None);

                };
            }
        }
    }
}
