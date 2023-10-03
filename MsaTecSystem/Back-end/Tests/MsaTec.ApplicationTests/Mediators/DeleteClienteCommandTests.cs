using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MsaTec.Abstractions.Application.Contract;
using MsaTec.Application.Mediators.Commands;
using MsaTec.Application.Mediators.Queries;
using MsaTec.Application.ViewModels;
using MsaTec.DAL.Data;
using MsaTec.DAL.Repositories.Contracts;
using MsaTec.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;

namespace MsaTec.Application.Tests.Mediators;

public class DeleteClienteCommandTests
{
    private IMediator _Mediator;
    private ServiceProvider _ServiceProvider;

    public DeleteClienteCommandTests()
    {
        _ServiceProvider = new ServiceCollection()
            .AddDbContext<DbContextMsaTec>(options => options.UseInMemoryDatabase(databaseName: "InMemoryDatabase"))
            .AddScoped<IClienteRepository, ClientesRepository>() // Replace YourClienteRepository with your actual repository implementation
            .AddAutoMapper(typeof(InsertClienteCommand)) // Assuming AutoMapper profiles are configured in Startup class
            .AddScoped<IRequestHandler<InsertClienteCommand, ICommandResult>, InsertClienteCommand.Handler>()
            .AddScoped<IRequestHandler<GetClienteByIdQuery, ClienteViewModel>, GetClienteByIdQuery.Handler>()
            .AddScoped<IRequestHandler<DeleteClienteCommand, ICommandResult>, DeleteClienteCommand.Handler>()
            .AddScoped<IMediator, Mediator>()
            .BuildServiceProvider();
    }

    [Fact]
    public async Task Mediator_ShouldDeleteClient()
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
            }

            foreach (var clienteViewModel in dataList)
            {
                // Act
                var deleteCommand = new DeleteClienteCommand(clienteViewModel.Id);
                var deleteResult = await _Mediator.Send(deleteCommand, CancellationToken.None);

                var qry = new GetClienteByIdQuery(clienteViewModel.Id);
                var clienteInserted = await _Mediator.Send(qry, CancellationToken.None);

                // Assert
                deleteResult.Should().NotBeNull();
                deleteResult.IsSuccess.Should().BeTrue();
                clienteInserted.Should().BeNull();
            }

        }
    }
}
