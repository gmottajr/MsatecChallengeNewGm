using MsaTec.Abstractions.Model.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MsaTec.Application.ViewModels;

public class TelefoneViewModel : CrudViewModelBase
{
    
    public Guid Id { get; set; }

    
    public Guid ClienteId { get; set; }

    [Required]
    [Display(Name = "Número do Telefone")]
    public string Numero { get; set; }

    [Required]
    public int Tipo { get; set; }

    [Display(Name = "Tipo")]
    public string TipoDescricao { get; set; }
}
