using AutoMapper;
using BarberBoss.Communication.Requests;
using BarberBoss.Communication.Responses;
using BarberBoss.Domain.Entities;

namespace BarberBoss.Application.AutoMapper;

public class AutoMapping : Profile
{
    public AutoMapping()
    {
        RequestToEntity();
        EntityToResponse();
    }

    private void RequestToEntity()
    {
        CreateMap<RequestBillingJson, Billing>();
    }
    
    private void EntityToResponse()
    {
        CreateMap<Billing, ResponseRegisteredBillingJson>();
        CreateMap<Billing, ResponseShortBillingJson>()
            .ForMember(
                dest => dest.Date, 
                opt => opt.MapFrom(src => src.Date.ToString("dd/MM/yyyy HH:mm:ss"))
            );
        CreateMap<Billing, ResponseBillingJson>()
            .ForMember(
                dest => dest.Date,
                opt => opt.MapFrom(src => src.Date.ToString("dd/MM/yyyy HH:mm:ss"))
            )
            .ForMember(
                dest => dest.CreatedAt,
                opt => opt.MapFrom(src => src.CreatedAt.ToString("dd/MM/yyyy HH:mm:ss"))
            )
            .ForMember(
                dest => dest.UpdatedAt,
                opt => opt.MapFrom(src => src.UpdatedAt.ToString("dd/MM/yyyy HH:mm:ss"))
            );
    }
}
