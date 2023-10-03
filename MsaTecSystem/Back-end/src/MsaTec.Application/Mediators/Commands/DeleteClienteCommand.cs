using MediatR;
using MsaTec.Abstractions.Application.Contract;
using MsaTec.Core;
using MsaTec.Core.Extensions;
using MsaTec.DAL.Repositories.Contracts;

namespace MsaTec.Application.Mediators.Commands;

public class DeleteClienteCommand : IRequest<ICommandResult>
{
    public Guid ClienteId { get; private set; }

    public DeleteClienteCommand(Guid clienteId)
    {
        ClienteId = clienteId;
    }

    public class Handler : IRequestHandler<DeleteClienteCommand, ICommandResult>
    {
        private readonly IClienteRepository _clienteRepository;

        public Handler(IClienteRepository clienteRepository)
        {
            _clienteRepository = clienteRepository;
        }

        public async Task<ICommandResult> Handle(DeleteClienteCommand request, CancellationToken cancellationToken)
        {
            var result = new CommandResult();
            try
            {
                var gotEntity = await _clienteRepository.GetByIdAsync(request.ClienteId);
                if (gotEntity == null)
                {
                    result.MessageError.Add($"Delete error: Cliente with ID {request.ClienteId} not found.");
                    return result;
                }

                await _clienteRepository.DeleteAsync(request.ClienteId);
                result.IsSuccess = true;
            }
            catch (Exception ex)
            {
                var msgError = $"Error deleting Cliente: {ex.ExtractInfoAll()}";
                result.MessageError.Add(msgError);
            }

            return result;
        }
    }
}
