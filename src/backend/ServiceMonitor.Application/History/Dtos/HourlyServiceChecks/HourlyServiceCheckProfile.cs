using AutoMapper;
using ServiceMonitor.Domain.Entities;

namespace ServiceMonitor.Application.History.Dtos.HourlyServiceChecks;

public class HourlyServiceCheckProfile : Profile
{
    public HourlyServiceCheckProfile()
    {
        CreateMap<HourlyServiceCheck, HourlyServiceCheckDto>();
    }
}
