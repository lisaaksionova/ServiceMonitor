using AutoMapper;
using ServiceMonitor.Domain.Entities;

namespace ServiceMonitor.Application.ServiceChecks.Dtos;

public class ServiceCheckProfile : Profile
{
    public ServiceCheckProfile()
    {
        CreateMap<ServiceCheck, ServiceCheckDto>()
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
                opts => opts.MapFrom(src => src.StatusCode.HasValue
                    ? (int)src.StatusCode.Value
                    : 0)
            )
            .ForMember(
                dest => dest.ResponseTimeSeconds,
                opts => opts.MapFrom(src =>
                    TimeSpan.FromMilliseconds(src.ResponseTimeMs).TotalSeconds)
            );
    }
}
