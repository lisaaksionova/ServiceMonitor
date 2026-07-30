using AutoMapper;
using ServiceMonitor.Application.Incidents.Commands.CreateIncident;
using ServiceMonitor.Application.Incidents.Commands.UpdateIncident;
using ServiceMonitor.Domain.Entities;
using ServiceMonitor.Domain.Enums;

namespace ServiceMonitor.Application.Incidents.Dtos;

public class IncidentProfile : Profile
{
    public IncidentProfile()
    {
        CreateMap<Incident, IncidentDto>();
        CreateMap<CreateIncidentCommand, Incident>()
            .ForMember(x => x.Date, opt => opt.MapFrom(_ => DateTime.UtcNow))
            .ForMember(x => x.Status, opt => opt.MapFrom(_ => IncidentStatus.Open));
        CreateMap<UpdateIncidentCommand, Incident>()
            .ForMember(x => x.Description,
                opts => opts.MapFrom((s, d) => s.Description ?? d.Description))
            .ForMember(x => x.Status,
                opt => opt.MapFrom((s, d) => s.Status != null ? Enum.Parse<IncidentStatus>(s.Status, true) : d.Status));


    }
}
