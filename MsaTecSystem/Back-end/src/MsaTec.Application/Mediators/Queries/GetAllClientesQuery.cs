using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MsaTec.Application.ViewModels;
using MsaTec.DAL.Repositories.Contracts;

namespace MsaTec.Application.Mediators.Queries;

public class GetAllClientesQuery : IRequest<IEnumerable<ClienteViewModelForList>>
{
    public class Handler : IRequestHandler<GetAllClientesQuery, IEnumerable<ClienteViewModelForList>>
    {
        private readonly IMapper _mapper;
        private readonly IClienteRepository _clienteRepository;

        public Handler(IMapper mapper, IClienteRepository clienteRepository)
        {
            _mapper = mapper;
            _clienteRepository = clienteRepository;
        }

        public async Task<IEnumerable<ClienteViewModelForList>> Handle(GetAllClientesQuery request, CancellationToken cancellationToken)
        {
            var clientes = await _clienteRepository.QueryAsync(expression: null, 
                c => c.OrderBy(cl => cl.Nome), 
                include: clnt => clnt.Include(c => c.Telefones));
            return _mapper.Map<IEnumerable<ClienteViewModelForList>>(clientes);
        }
    }
}
