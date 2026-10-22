namespace ServiceMonitor.Application.History.Dtos.HourlyServiceChecks;

public class HourlyServiceCheckProfile : Profile
{
    public HourlyServiceCheckProfile()
    {
        CreateMap<HourlyServiceCheck, HourlyServiceCheckDto>();
    }
}
