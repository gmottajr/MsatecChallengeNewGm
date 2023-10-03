using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MsaTec.Abstractions.Application.Contract;
using MsaTec.Application.Mediators.Commands;
using MsaTec.DAL.Data;
using MsaTec.DAL.Repositories.Contracts;
using MsaTec.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MsaTec.Application.ProfilerMapping;
using FluentAssertions;
using MsaTec.Application.ViewModels;

namespace MsaTec.Application.Tests.Mediators;

public class UpdateClienteCommandHandlerTests
{
    private IMapper _Mapper;
    private ServiceProvider _ServiceProvider;
    private IMediator _Mediator;
    private IClienteRepository _Repository;

    public UpdateClienteCommandHandlerTests()
    {
        // Configure AutoMapper with the MappingProfile
        var mapperConfig = new MapperConfiguration(config =>
        {
            config.AddProfile(new ClienteProfilerMapping());
            config.AddProfile(new TelefoneProfilerMapping());
        });

        _Mapper = mapperConfig.CreateMapper();

        _ServiceProvider = new ServiceCollection()
            .AddDbContext<DbContextMsaTec>(options => options.UseInMemoryDatabase(databaseName: "InMemoryDatabase"))
            .AddScoped<IClienteRepository, ClientesRepository>() // Replace YourClienteRepository with your actual repository implementation
            .AddAutoMapper(typeof(InsertClienteCommand)) // Assuming AutoMapper profiles are configured in Startup class
            .AddScoped<IRequestHandler<InsertClienteCommand, ICommandResult>, InsertClienteCommand.Handler>()
            .AddScoped<IRequestHandler<UpdateClienteCommand, ICommandResult>, UpdateClienteCommand.Handler>()
            .AddScoped<IMediator, Mediator>()
            .BuildServiceProvider();

    }

    [Fact]
    public async Task Handle_ValidRequest_InsertsUpdatesClienteSuccessfully_UsingMediator()
    {
        // Arrange
        using (var scope = _ServiceProvider.CreateScope())
        {
            using (var dbContext = scope.ServiceProvider.GetRequiredService<DbContextMsaTec>())
            {
                _Mediator = _ServiceProvider.GetRequiredService<IMediator>();
                _Repository = _ServiceProvider.GetRequiredService<IClienteRepository>();
                foreach (var clienteViewModel in TestHelper.LoadClienteViewModelsData())
                {
                    // Act
                    var commandInsert = new InsertClienteCommand(clienteViewModel);
                    var rst = await _Mediator.Send(commandInsert, CancellationToken.None);
                    Guid clienteId = (Guid)rst.Result;

                    // Retrieve the inserted entity from the in-memory database context for additional assertions if needed
                    var insertedCliente = await _Repository.QuerySingleAsync(c => c.Id == clienteId, include: c => c.Include(t => t.Telefones));//dbContext.Clientes.FirstOrDefaultAsync(c => c.Id == clienteId);

                    var updatingMapped = _Mapper.Map<ClienteViewModel>(insertedCliente);
                    var nomeUpdating = $"{insertedCliente.Nome} Almeida Test";
                    updatingMapped.Nome = nomeUpdating;
                    var commandUpdate = new UpdateClienteCommand(updatingMapped);
                    var updateRst = await _Mediator.Send(commandUpdate, CancellationToken.None);

                    // Assert
                    updateRst.Should().NotBeNull();
                    updateRst.IsSuccess.Should().BeTrue();
                    updateRst.MessageError.Count.Should().Be(0);
                    var updatedCliente = await dbContext.Clientes.FirstOrDefaultAsync(c => c.Id == clienteId);
                    updatedCliente.Should().NotBeNull();
                    if(updatedCliente != null)
                      updatedCliente.Nome.Should().Be(nomeUpdating);
                };
            }
        }

    }
}
