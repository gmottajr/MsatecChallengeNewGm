using AutoMapper;
using MsaTec.Application.ViewModels;
using MsaTec.Core.Enums;
using MsaTec.Model;

namespace MsaTec.Application.ProfilerMapping;

public class TelefoneProfilerMapping : Profile
{
    public TelefoneProfilerMapping()
    {
        // Telefone to TelefoneViewModelList and vice-versa
        CreateMap<Telefone, TelefoneViewModelList>();
        CreateMap<TelefoneViewModelList, Telefone>();

        // Telefone to TelefoneViewModel and vice-versa
        CreateMap<Telefone, TelefoneViewModel>()
            .ForMember(dest => dest.TipoDescricao, opt => opt.MapFrom(src => Enum.GetName(typeof(TipoTelefoneEnum), src.Tipo)));
        CreateMap<TelefoneViewModel, Telefone>();
    }
}
