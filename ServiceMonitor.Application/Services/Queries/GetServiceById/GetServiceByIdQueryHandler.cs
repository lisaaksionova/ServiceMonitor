using AutoMapper;
using MediatR;
using ServiceMonitor.Application.Services.Dtos;
using ServiceMonitor.Application.SharedServices;
using ServiceMonitor.Domain.Entities;
using ServiceMonitor.Domain.Exceptions;
using ServiceMonitor.Domain.Interfaces;

namespace ServiceMonitor.Application.Services.Queries.GetServiceById;

public class GetServiceByIdQueryHandler(
    IServiceRepository repository,
    IMapper mapper,
    IAuthenticatedUser authenticatedUser) : IRequestHandler<GetServiceByIdQuery, ServiceDto>
{
    public async Task<ServiceDto> Handle(GetServiceByIdQuery request, CancellationToken cancellationToken)
    {
        var service = await repository.GetByIdAsync(request.Id, authenticatedUser.UserId, cancellationToken) ??
                      throw new NotFoundException(nameof(Service), request.Id.ToString());
        var serviceDto = mapper.Map<ServiceDto>(service);
        return serviceDto;
    }
}
