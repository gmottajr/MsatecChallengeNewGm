using MsaTec.Abstractions.Model.Base;
using MsaTec.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MsaTec.Application.ViewModels;

public class ClienteViewModel : CrudViewModelBase
{
    public Guid? Id { get; set; }

    [Required]
    [Display(Name = "Nome")]
    public string Nome { get; set; } // Nome do cliente (máximo de 100 caracteres)


    [Required]
    [Display(Name = "E-Mail")]
    [EmailAddress]
    public string Email { get; set; } // E-mail do cliente (máximo de 255 caracteres, único)

    
    [Display(Name = "Data de Nascimento")]
    public DateTime? DataNascimento { get; set; } // Data de Nascimento do cliente (opcional)

    
    [Display(Name = "Telefones")]
    public List<TelefoneViewModel> Telefones { get; set; } = new List<TelefoneViewModel>();// Lista de telefones do cliente

}
