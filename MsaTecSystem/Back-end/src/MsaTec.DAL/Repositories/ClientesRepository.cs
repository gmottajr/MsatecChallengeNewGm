using MsaTec.DAL.Data;
using MsaTec.DAL.Repositories.Base;
using MsaTec.DAL.Repositories.Contracts;
using MsaTec.Model;

namespace MsaTec.DAL.Repositories;

public class ClientesRepository : RepositoryBase<Cliente>, IClienteRepository
{
    public ClientesRepository(DbContextMsaTec dbContext) : base(dbContext)
    {
        _DbSet = dbContext.Clientes;
    }

    public override async Task DeleteAsync(Guid id)
    {
        var cliente = await GetByIdAsync(id);

        if (cliente.Telefones.Count > 0)
            ((DbContextMsaTec)_DbContext).Telefones.RemoveRange(cliente.Telefones);

        await base.DeleteAsync(id);
    }

    public override async Task<bool> SaveChangesAsync()
    {
        return await ((DbContextMsaTec)_DbContext).Commit();
    }
}
