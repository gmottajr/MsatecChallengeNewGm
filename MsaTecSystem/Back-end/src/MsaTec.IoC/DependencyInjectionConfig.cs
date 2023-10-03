using AutoMapper;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MsaTec.Application.Mediators.Queries;
using MsaTec.Application.ProfilerMapping;
using MsaTec.DAL.Repositories;
using MsaTec.DAL.Repositories.Contracts;
using System.Reflection;

namespace MsaTec.IoC;

public static class DependencyInjectionConfig
{
    public static IServiceCollection ResolveDependencies(this IServiceCollection services)
    {
        services.AddScoped<IClienteRepository, ClientesRepository>();

        //services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetClienteByIdQuery).Assembly));
        //services.AddMediatR(typeof(GetClienteByIdQuery).GetTypeInfo().Assembly);
        //services.AddMediatR(typeof(DependencyInjectionConfig));

        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(GetClienteByIdQuery).GetTypeInfo().Assembly));

        //services.AddScoped<IRequestHandler<GetAllClientesQuery, IEnumerable<ClienteViewModelList>>, GetAllClientesQuery.Handler>();
        //services.AddScoped<IRequestHandler<GetClienteByIdQuery, ClienteViewModel>, GetClienteByIdQuery.Handler>();
        //services.AddScoped<IRequestHandler<InsertClienteCommand, ICommandResult>, InsertClienteCommand.Handler>();
        //services.AddScoped<IRequestHandler<UpdateClienteCommand, ICommandResult>, UpdateClienteCommand.Handler>();
        //services.AddScoped<IRequestHandler<DeleteClienteCommand, ICommandResult>, DeleteClienteCommand.Handler>();

        // AutoMapper configuration
        var mapperConfig = new MapperConfiguration(configItem =>
        {
            configItem.AddProfile(new ClienteProfilerMapping());
            configItem.AddProfile(new TelefoneProfilerMapping());
        });

        IMapper mapper = mapperConfig.CreateMapper();
        services.AddSingleton(mapper); // Register the IMapper instance in the IoC container


        return services;
    }
}
