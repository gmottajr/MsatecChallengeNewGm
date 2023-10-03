using AutoMapper;
using MsaTec.Application.ViewModels;
using MsaTec.Model;

namespace MsaTec.Application.ProfilerMapping;

public class ClienteProfilerMapping : Profile
{
    public ClienteProfilerMapping()
    {
        // Cliente to ClienteViewModelList and vice-versa
        CreateMap<Cliente, ClienteViewModelList>()
            .ForMember(dest => dest.TelefonePrincipal, opt => 
               opt.MapFrom(src => src.Telefones.FirstOrDefault(t => t.Tipo == Core.Enums.TipoTelefoneEnum.Pessoal) != null ?
               src.Telefones.FirstOrDefault(t => t.Tipo == Core.Enums.TipoTelefoneEnum.Pessoal) :
               src.Telefones.FirstOrDefault()));
        CreateMap<ClienteViewModelList, Cliente>();

        // Cliente to ClienteViewModel and vice-versa
        CreateMap<Cliente, ClienteViewModel>()
            .ForMember(dest => dest.Telefones, opt => opt.MapFrom(src => src.Telefones));
        CreateMap<ClienteViewModel, Cliente>()
            .ForMember(dest => dest.Telefones, opt => opt.MapFrom(src => src.Telefones));

    }
}
