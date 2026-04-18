using AutoMapper;
using ServiceMonitor.Application.Services.Commands.CreateService;
using ServiceMonitor.Application.Services.Commands.UpdateService;
using ServiceMonitor.Domain.Entities;
using ServiceMonitor.Domain.Enums;

namespace ServiceMonitor.Application.Services.Dtos;

public class ServiceProfile : Profile
{
    public ServiceProfile()
    {
        CreateMap<Service, ServiceDto>()
            .ForMember(s => s.Incidents, opt => opt.MapFrom(src => src.Incidents));

        CreateMap<UpdateServiceCommand, Service>()
            .ForMember(s => s.Name, opts => opts.MapFrom((s, d) => s.Name ?? d.Name))
            .ForMember(s => s.Endpoint, opts => opts.MapFrom((s, d) => s.Endpoint ?? d.Endpoint))
            .ForMember(s => s.Status,
                opt => opt.MapFrom((s, d) => s.Status != null ? Enum.Parse<ServiceStatus>(s.Status, true) : d.Status));
        
        CreateMap<CreateServiceCommand, Service>()
            .ForMember(s => s.Status,
                opt => opt.MapFrom(src => ServiceStatus.Healthy));

    }
}