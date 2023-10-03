using MsaTec.Abstractions.Model.Base;
using MsaTec.Core.Exceptions;
using MsaTec.Core.Extensions;

namespace MsaTec.Model;

public class Cliente : EntityRootBase
{
    public required string Nome { get; set; } // Nome do cliente (máximo de 100 caracteres)
    public required string Email { get; set; } // E-mail do cliente (máximo de 255 caracteres, único)
    public DateTime? DataNascimento { get; set; } // Data de Nascimento do cliente (opcional)
    public required List<Telefone> Telefones { get; set; } = new List<Telefone>();// Lista de telefones do cliente
    //public string CPF { get; set; }
    public override void Validate()
    {
        if (Email.IsEmpty()) throw new ModelValidateException("Campo requerido: Email do Cliente.");
        if (Nome.IsEmpty()) throw new ModelValidateException("Campo requerido: Nome do Cliente.");
        //if (Telefones.Count == 0) throw new ModelValidateException("Campo requerido: Telefone do cliente.");
    }
}