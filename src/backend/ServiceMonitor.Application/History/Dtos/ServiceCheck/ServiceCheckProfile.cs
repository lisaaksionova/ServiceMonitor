using AutoMapper;
using ServiceMonitor.Application.ServiceChecks.Dtos;

namespace ServiceMonitor.Application.History.Dtos.ServiceCheck;

public class ServiceCheckProfile : Profile
{
    public ServiceCheckProfile()
    {
        CreateMap<Domain.Entities.ServiceCheck, ServiceCheckDto>()
            .ForMember(
                dest => dest.CheckedAt,
                opts => opts.MapFrom(src => src.CheckedAt.ToString("O"))
            )
            .ForMember(
                dest => dest.Status,
                opts => opts.MapFrom(src => src.Status.ToString())
            )
            .ForMember(
                dest => dest.StatusCode,
                opts => opts.MapFrom(src => src.StatusCode)
            )
            .ForMember(
                dest => dest.ResponseTimeSeconds,
                opts => opts.MapFrom(src =>
                    TimeSpan.FromMilliseconds(src.ResponseTimeMs).TotalSeconds)
            );
    }
}
