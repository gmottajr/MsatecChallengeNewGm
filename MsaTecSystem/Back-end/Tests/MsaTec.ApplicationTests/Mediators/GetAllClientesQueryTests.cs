using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MsaTec.Abstractions.Application.Contract;
using MsaTec.Application.Mediators.Commands;
using MsaTec.Application.ProfilerMapping;
using MsaTec.DAL.Data;
using MsaTec.DAL.Repositories.Contracts;
using MsaTec.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using MsaTec.Abstractions.Persistence.Contracts;
using MsaTec.Application.ViewModels;
using MsaTec.Application.Mediators.Queries;

namespace MsaTec.Application.Tests.Mediators;

public class GetAllClientesQueryTests
{
    private IMediator _Mediator;
    private ServiceProvider _ServiceProvider;

    public GetAllClientesQueryTests()
    {
        _ServiceProvider = new ServiceCollection()
            .AddDbContext<DbContextMsaTec>(options => options.UseInMemoryDatabase(databaseName: "InMemoryDatabase"))
            .AddScoped<IClienteRepository, ClientesRepository>() // Replace YourClienteRepository with your actual repository implementation
            .AddAutoMapper(typeof(InsertClienteCommand)) // Assuming AutoMapper profiles are configured in Startup class
            .AddScoped<IRequestHandler<InsertClienteCommand, ICommandResult>, InsertClienteCommand.Handler>()
            .AddScoped<IRequestHandler<GetAllClientesQuery, IEnumerable<ClienteViewModelList>>, GetAllClientesQuery.Handler>()
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
            }

            // Act
            var qry = new GetAllClientesQuery();
            var clientesInserted = await _Mediator.Send(qry, CancellationToken.None);
            // Assert
            clientesInserted.Should().NotBeNull();
            clientesInserted.Should().HaveCount(dataList.Count);
        }
    }

}
