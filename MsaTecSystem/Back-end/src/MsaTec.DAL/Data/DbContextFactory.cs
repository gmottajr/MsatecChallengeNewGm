using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MsaTec.DAL.Data
{
    public class DbContextFactory : IDesignTimeDbContextFactory<DbContextMsaTec>
    {
        public DbContextMsaTec CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<DbContextMsaTec>();

            var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json") // Provide the path to your appsettings.json file
            .Build();

            return new DbContextMsaTec(optionsBuilder.Options, configuration);
        }
    }
}
