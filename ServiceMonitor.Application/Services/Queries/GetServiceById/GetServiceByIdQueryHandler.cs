using AutoMapper;
using MediatR;
using ServiceMonitor.Application.Services.Dtos;
using ServiceMonitor.Domain.Interfaces;

namespace ServiceMonitor.Application.Services.Queries.GetServiceById;

public class GetServiceByIdQueryHandler(IServiceRepository repository,
    IMapper mapper) : IRequestHandler<GetServiceByIdQuery,  ServiceDto>
{
    public async Task<ServiceDto> Handle(GetServiceByIdQuery request, CancellationToken cancellationToken)
    {
        var service = await repository.GetByIdAsync(request.Id);
        var serviceDto = mapper.Map<ServiceDto>(service);
        return serviceDto;
    }
}