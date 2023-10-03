using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Configuration;
using MsaTec.Abstractions.Model.Base;
using MsaTec.Core;
using MsaTec.DAL.Data.Contracts;
using MsaTec.DAL.FluentApiMappingConfiguration;
using MsaTec.Model;

namespace MsaTec.DAL.Data;

public class DbContextMsaTec : DbContext//, IUnitOfWorks
{
    private readonly IConfiguration _configuration;
    private bool _isTesting;

    public DbSet<Cliente> Clientes { get; set; }
    public DbSet<Telefone> Telefones { get; set; }

    public DbContextMsaTec(DbContextOptions<DbContextMsaTec> options, IConfiguration configuration) : base(options)
    {
        _configuration = configuration;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
          optionsBuilder.UseNpgsql(_configuration.GetConnectionString(GetConnectionStringAlias()));
        //optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=MsatecDb;User Id=postgres;Password=XXXXX-XXX;");

    }

    private string GetConnectionStringAlias()
    {
        return _isTesting? MsatecConsts.ConnectionStringDbTestAlias : MsatecConsts.ConnectionStringAlias;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ClienteConfiguration).Assembly);
        //modelBuilder.ApplyConfiguration(new TelefoneConfiguration());
        //modelBuilder.ApplyConfiguration(new ClienteConfiguration());

        // Add configurations for other entities if needed.
    }

    public async Task<bool> Commit()
    {
        foreach (var entry in ChangeTracker.Entries().Where(entry => entry.Entity.GetType().GetProperty("CreatedWhen") != null))
        {
            //var brasilia = TimeZoneInfo.FindSystemTimeZoneById("Central Brazilian Standard Time");
            if (entry.State == EntityState.Added)
            {
                entry.Property("CreatedWhen").CurrentValue = DateTime.UtcNow;//TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, brasilia);
            }
            if (entry.State == EntityState.Modified)
            {
                entry.Property("UpdatedWhen").CurrentValue = DateTime.UtcNow; //TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, brasilia);
            }
        }

        //Calls entity's validates data method before persisting the object
        foreach (var entry in ChangeTracker.Entries().Where(entry => (entry.State == EntityState.Modified || entry.State == EntityState.Added) && entry.Entity is EntityBase))
        {
            ((EntityBase)entry.Entity).Validate();
        }

        return await this.SaveChangesAsync() > 0;
    }

    public void SetTestMode()
    {
        _isTesting = true;
    }
}

    
