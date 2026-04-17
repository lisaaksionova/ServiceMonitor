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
        CreateMap<Service, ServiceDto>();
        CreateMap<UpdateServiceCommand, Service>()
            .ForMember(s => s.Status,
                opt => opt.MapFrom(src => Enum.Parse<ServiceStatus>(src.Status, true)));
        CreateMap<CreateServiceCommand, Service>()
            .ForMember(s => s.Status,
                opt => opt.MapFrom(src => ServiceStatus.Healthy));

    }
}