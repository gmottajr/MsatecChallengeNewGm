using MsaTec.Abstractions.Model.Contracts;

namespace MsaTec.Abstractions.Model.Base;

public abstract class EntityRootBase: EntityBase, IEntityRoot
{
    public Guid Id { get; set; }
    public EntityRootBase()
    {
        CreatedWhen = DateTime.UtcNow;
    }
}
