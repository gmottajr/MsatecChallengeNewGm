using AutoMapper;
using MediatR;
using MsaTec.Abstractions.Application.Contract;
using MsaTec.Application.ViewModels;
using MsaTec.Core;
using MsaTec.Core.Extensions;
using MsaTec.DAL.Repositories.Contracts;
using MsaTec.Model;

namespace MsaTec.Application.Mediators.Commands;
public class InsertClienteCommand : IRequest<ICommandResult>
{
    public ClienteViewModel ClienteViewModel { get; }

    public InsertClienteCommand(ClienteViewModel clienteViewModel)
    {
        ClienteViewModel = clienteViewModel;
    }

    public class Handler : IRequestHandler<InsertClienteCommand, ICommandResult>
    {
        private readonly IClienteRepository _clienteRepository;
        private readonly IMapper _mapper;

        public Handler(IClienteRepository clienteRepository, IMapper mapper)
        {
            _clienteRepository = clienteRepository;
            _mapper = mapper;
        }

        public async Task<ICommandResult> Handle(InsertClienteCommand request, CancellationToken cancellationToken)
        {
            var result = new CommandResult();
            try
            {
                var cliente = _mapper.Map<Cliente>(request.ClienteViewModel);

                await _clienteRepository.InsertAsync(cliente);
                result.Result = cliente.Id;
                result.IsSuccess = true;
            }
            catch (Exception ex)
            {
                var msgError = $"Error inserting Cliente: {ex.ExtractInfoAll()}";
                result.MessageError.Add(msgError);
            }

            // Assuming cliente.Id is the newly generated ID after insertion
            return result;
        }
    }
}