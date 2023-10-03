using AutoMapper;
using MediatR;
using MsaTec.Abstractions.Application.Contract;
using MsaTec.Application.ViewModels;
using MsaTec.Core;
using MsaTec.Core.Extensions;
using MsaTec.DAL.Repositories.Contracts;
using MsaTec.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MsaTec.Application.Mediators.Commands;

public class UpdateClienteCommand : IRequest<ICommandResult>
{
    public ClienteViewModel ClienteViewModel { get; }

    public UpdateClienteCommand(ClienteViewModel clienteViewModel)
    {
        ClienteViewModel = clienteViewModel;
    }

    public class Handler : IRequestHandler<UpdateClienteCommand, ICommandResult>
    {
        private readonly IClienteRepository _clienteRepository;
        private readonly IMapper _mapper;

        public Handler(IClienteRepository clienteRepository, IMapper mapper)
        {
            _clienteRepository = clienteRepository;
            _mapper = mapper;
        }

        public async Task<ICommandResult> Handle(UpdateClienteCommand request, CancellationToken cancellationToken)
        {
            var result = new CommandResult();
            try
            {
                var gotEntity = await _clienteRepository.GetByIdAsync(request.ClienteViewModel.Id);
                if (gotEntity == null)
                {
                    result.MessageError.Add($"Cliente with ID {request.ClienteViewModel.Id} not found.");
                    return result;
                }

                // Update the entity properties from the view model
                _mapper.Map(request.ClienteViewModel, gotEntity);

                await _clienteRepository.Update(gotEntity);
                result.IsSuccess = true;
            }
            catch (Exception ex)
            {
                var msgError = $"Error updating Cliente: {ex.ExtractInfoAll()}";
                result.MessageError.Add(msgError);
            }

            return result;
        }
    }
}
