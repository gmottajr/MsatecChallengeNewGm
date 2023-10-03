namespace MsaTec.Abstractions.Model.Contracts;

public interface IEntity
{
    public DateTime CreatedWhen { get; set; }
    public DateTime? UpdatedWhen { get; set; }
}
