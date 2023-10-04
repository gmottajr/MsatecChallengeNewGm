using MsaTec.Abstractions.Model.Base;

namespace MsaTec.Application.ViewModels;

public class TelefoneViewModelList : ViewModelBase
{
    public Guid? Id { get; set; }
    public Guid? ClienteId { get; set; }
    public string Numero { get; set; }
    public string Tipo { get; set; }
}
