using MsaTec.Abstractions.Model.Contracts;

namespace MsaTec.Abstractions.Model.Base;

public abstract class EntityBase : IEntity, IEntityValidator
{
    public DateTime CreatedWhen { get; set; }
    public DateTime? UpdatedWhen { get; set; }

    public abstract void Validate();
}
