using AutoMapper;
using FluentAssertions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MsaTec.Abstractions.Application.Contract;
using MsaTec.Application.Mediators.Commands;
using MsaTec.Application.ProfilerMapping;
using MsaTec.DAL.Data;
using MsaTec.DAL.Repositories;
using MsaTec.DAL.Repositories.Contracts;

namespace MsaTec.Application.Tests.Mediators;

public class InsertClienteCommandHandlerTests
{
    private IMapper _Mapper;
    private IMediator _Mediator;
    private DbContextOptions<DbContextMsaTec> _DbContextOptions;
    private ServiceProvider _ServiceProvider;
    private IConfigurationRoot _Configuration;

    public InsertClienteCommandHandlerTests()
    {
        // Configure AutoMapper with the MappingProfile
        var mapperConfig = new MapperConfiguration(config =>
        {
            config.AddProfile(new ClienteProfilerMapping());
            config.AddProfile(new TelefoneProfilerMapping());
        });

        _Mapper = mapperConfig.CreateMapper();

        _DbContextOptions = new DbContextOptionsBuilder<DbContextMsaTec>()
            .UseInMemoryDatabase(databaseName: "InMemoryDatabase")
            .Options;

        _Configuration = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .Build();

        _ServiceProvider = new ServiceCollection()
            .AddSingleton<IConfiguration>(_Configuration)
            .AddDbContext<DbContextMsaTec>(options => options.UseInMemoryDatabase(databaseName: "InMemoryDatabase"))
            .AddScoped<IClienteRepository, ClientesRepository>() // Replace YourClienteRepository with your actual repository implementation
            .AddAutoMapper(typeof(InsertClienteCommand)) // Assuming AutoMapper profiles are configured in Startup class
            .AddScoped<IRequestHandler<InsertClienteCommand, ICommandResult>, InsertClienteCommand.Handler>()
            .AddScoped<IMediator, Mediator>()
            .BuildServiceProvider();        
    }

    [Fact]
    public async Task Handle_ValidRequest_InsertsClienteSuccessfully()
    {
        using (var dbContext = new DbContextMsaTec(_DbContextOptions, _Configuration))
        {
            
            var repository = new ClientesRepository(dbContext); // Replace YourClienteRepository with your actual repository implementation
            var handler = new InsertClienteCommand.Handler(repository, _Mapper);

            // Arrange
            foreach (var clienteViewModel in TestHelper.LoadClienteViewModelsData())
            {
                // Act
                var result = await handler.Handle(new InsertClienteCommand(clienteViewModel), CancellationToken.None);

                // Assert
                Assert.NotNull(result);
                Assert.True(result.IsSuccess);
                Assert.NotNull(result.Result); // Assuming cliente.Id is set in the Handler
                Assert.Empty(result.MessageError);
                Guid clienteId = (Guid)result.Result;
                // Retrieve the inserted entity from the in-memory database context for additional assertions if needed
                var insertedCliente = await dbContext.Clientes.FirstOrDefaultAsync(c => c.Id == clienteId);
                insertedCliente.Should().NotBeNull();
                if (insertedCliente != null)
                    insertedCliente.Nome.Should().Be(clienteViewModel.Nome);
            }
        }
    }

    [Fact]
    public async Task Handle_ValidRequest_InsertsClienteSuccessfully_UsingMediator()
    {
        // Arrange
        using (var scope = _ServiceProvider.CreateScope()) 
        {
            using (var dbContext = scope.ServiceProvider.GetRequiredService<DbContextMsaTec>())
            {
                _Mediator = _ServiceProvider.GetRequiredService<IMediator>();

                foreach (var clienteViewModel in TestHelper.LoadClienteViewModelsData())
                {
                    // Act
                    var command = new InsertClienteCommand(clienteViewModel);
                    var rst = await _Mediator.Send(command, CancellationToken.None);

                    // Assert
                    rst.Should().NotBeNull();
                    rst.IsSuccess.Should().BeTrue();
                    rst.MessageError.Count.Should().Be(0);
                    Guid clienteId = (Guid)rst.Result;

                    // Retrieve the inserted entity from the in-memory database context for additional assertions if needed
                    var insertedCliente = await dbContext.Clientes.FirstOrDefaultAsync(c => c.Id == clienteId);
                    Assert.NotNull(insertedCliente);
                };
            }
        }
        
    }
}
