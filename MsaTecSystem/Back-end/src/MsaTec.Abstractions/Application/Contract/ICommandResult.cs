namespace MsaTec.Abstractions.Application.Contract;

public interface ICommandResult
{
    dynamic Result { get; set; }
    List<string> MessageError { get; set; }
    public bool HasErrors { get => MessageError.Count > 0;}
    bool IsSuccess { get; set; }
}
