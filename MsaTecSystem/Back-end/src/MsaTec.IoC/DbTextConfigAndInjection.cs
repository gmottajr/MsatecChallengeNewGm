using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MsaTec.Core;
using MsaTec.DAL.Data;

namespace MsaTec.IoC;

public static class DbTextConfigAndInjection
{
    public static IServiceCollection SetupDbContext(this IServiceCollection services, IConfiguration config)
    {
        var auxConnectionString = config.GetConnectionString(MsatecConsts.ConnectionStringAlias);
        services.AddDbContext<DbContextMsaTec>(options => options.UseNpgsql(auxConnectionString).LogTo(Console.WriteLine, LogLevel.Information));
        return services;
    }
}
