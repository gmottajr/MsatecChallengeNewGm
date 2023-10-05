using MsaTec.Abstractions.Model.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MsaTec.Application.ViewModels;

public class ClienteViewModelForList : ViewModelBase
{
    public Guid Id { get; set; }
    public  string Nome { get; set; } // Nome do cliente (máximo de 100 caracteres)
    public string Email { get; set; } // E-mail do cliente (máximo de 255 caracteres, único)
    
    public TelefoneViewModelForList TelefonePrincipal { get; set; } = new TelefoneViewModelForList();// Lista de telefones do cliente
}
