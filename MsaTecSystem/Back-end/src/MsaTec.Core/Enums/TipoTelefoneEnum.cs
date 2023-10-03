using System.ComponentModel;

namespace MsaTec.Core.Enums;

public enum TipoTelefoneEnum
{
    [Description("Pessoal")]
    Pessoal = 1,
    [Description("Comercial")]
    Comercial = 2,
    [Description("Residencial")]
    Residencial = 3,
    [Description("Outros")]
    Outros = 4
}
