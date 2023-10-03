using AutoMapper;
using MediatR;
using MsaTec.Application.ViewModels;
using MsaTec.DAL.Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MsaTec.Application.Mediators.Queries;

public class GetClienteByIdQuery : IRequest<ClienteViewModel>
{
    public Guid ClienteId { get; }

    public GetClienteByIdQuery(Guid clienteId)
    {
        ClienteId = clienteId;
    }

    public class Handler : IRequestHandler<GetClienteByIdQuery, ClienteViewModel>
    {
        private readonly IMapper _mapper;
        private readonly IClienteRepository _clienteRepository;

        public Handler(IMapper mapper, IClienteRepository clienteRepository)
        {
            _mapper = mapper;
            _clienteRepository = clienteRepository;
        }

        public async Task<ClienteViewModel> Handle(GetClienteByIdQuery request, CancellationToken cancellationToken)
        {
            var cliente = await _clienteRepository.GetByIdAsync(request.ClienteId);
            if (cliente == null)
            {
                return null;
            }

            return _mapper.Map<ClienteViewModel>(cliente);
        }
    }
}