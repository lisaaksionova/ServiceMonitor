using AutoMapper;
using MediatR;
using ServiceMonitor.Application.Services.Dtos;
using ServiceMonitor.Domain.Interfaces;

namespace ServiceMonitor.Application.Services.Queries.GetAllServices;

public class GetAllServicesQueryHandler(IServiceRepository repository,
    IMapper mapper) : IRequestHandler<GetAllServicesQuery, IEnumerable<ServiceDto>>
{
    public async Task<IEnumerable<ServiceDto>> Handle(GetAllServicesQuery request, CancellationToken cancellationToken)
    {
        var services = await repository.GetAllAsync();
        var serviceDtos = mapper.Map<IEnumerable<ServiceDto>>(services);
        return serviceDtos;
    }
}