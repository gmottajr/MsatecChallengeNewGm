using System.ComponentModel.DataAnnotations;

namespace MsaTec.Abstractions.Model.Base;

public abstract class CrudViewModelBase : ViewModelBase
{
    public Guid Id { get; set; }
    public bool IsInserting { get; set; }

    [Display(Name = "Criado em:")]
    public DateTime CreatedWhen { get; set; }

    [Display(Name = "Atualzado em:")]
    public DateTime? UpdatedWhen { get; set; }
}
