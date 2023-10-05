using AutoMapper;
using MsaTec.Application.ViewModels;
using MsaTec.Core.Enums;
using MsaTec.Model;

namespace MsaTec.Application.ProfilerMapping;

public class ClienteProfilerMapping : Profile
{
    public ClienteProfilerMapping()
    {
        // Cliente to ClienteViewModelList and vice-versa
        CreateMap<Cliente, ClienteViewModelForList>()
    .ForMember(dest => dest.TelefonePrincipal, opt =>
        opt.MapFrom(src => src.Telefones.FirstOrDefault(t => t.Tipo == TipoTelefoneEnum.Pessoal) ?? src.Telefones.FirstOrDefault()));

        CreateMap<ClienteViewModelForList, Cliente>();

        // Cliente to ClienteViewModel and vice-versa
        CreateMap<Cliente, ClienteViewModel>()
            .ForMember(dest => dest.Telefones, opt => opt.MapFrom(src => src.Telefones));
        CreateMap<ClienteViewModel, Cliente>()
            .ForMember(dest => dest.Telefones, opt => opt.MapFrom(src => src.Telefones));

    }
}
