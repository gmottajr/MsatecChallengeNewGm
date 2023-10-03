using MsaTec.Abstractions.Model.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MsaTec.Application.ViewModels;

public class TelefoneViewModelList : ViewModelBase
{
    public Guid Id { get; set; }
    public Guid ClienteId { get; set; }
    public string Numero { get; set; }
    public string Tipo { get; set; }
}
