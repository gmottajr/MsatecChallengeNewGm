using Microsoft.Extensions.DependencyInjection;
using MsaTec.IoC.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MsaTec.IoC
{
    public static class DataSower
    {
        public static IServiceCollection AddDbInitializer(this IServiceCollection services)
        {
            services.AddTransient<IDbInitializer, DbInitializer>();
            return services;
        }
    }
}
