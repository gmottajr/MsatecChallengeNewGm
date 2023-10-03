using AutoMapper;
using MediatR;
using MsaTec.Application.ViewModels;
using MsaTec.DAL.Repositories.Contracts;

namespace MsaTec.Application.Mediators.Queries;

public class GetAllClientesQuery : IRequest<IEnumerable<ClienteViewModelList>>
{
    public class Handler : IRequestHandler<GetAllClientesQuery, IEnumerable<ClienteViewModelList>>
    {
        private readonly IMapper _mapper;
        private readonly IClienteRepository _clienteRepository;

        public Handler(IMapper mapper, IClienteRepository clienteRepository)
        {
            _mapper = mapper;
            _clienteRepository = clienteRepository;
        }

        public async Task<IEnumerable<ClienteViewModelList>> Handle(GetAllClientesQuery request, CancellationToken cancellationToken)
        {
            var clientes = await _clienteRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<ClienteViewModelList>>(clientes);
        }
    }
}
