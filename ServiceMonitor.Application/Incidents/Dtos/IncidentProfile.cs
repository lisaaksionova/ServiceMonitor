using AutoMapper;
using ServiceMonitor.Application.Incidents.Commands.CreateIncident;
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
        
    }
}