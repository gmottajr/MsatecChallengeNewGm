using MsaTec.Abstractions.Model.Base;
using MsaTec.Core.Enums;
using MsaTec.Core.Exceptions;
using MsaTec.Core.Extensions;

namespace MsaTec.Model;

public class Telefone: EntityRootBase
{
    public required string Numero { get; set; } // Número de telefone
    public TipoTelefoneEnum Tipo { get; set; } = TipoTelefoneEnum.Pessoal;// Tipo de telefone (pessoal, comercial, residencial, etc.)
    public Guid ClienteId { get; set; }
    public Cliente Cliente { get; set; }

    public Telefone()
    {
        this.Id = Guid.NewGuid();
    }
    public override void Validate()
    {
        if (Numero.IsEmpty()) throw new ModelValidateException("Campo requerido: Numero do Telefone.");
        if (ClienteId == Guid.Empty) throw new ModelValidateException("Campo requerido: Id do Cliente.");
    }
}
