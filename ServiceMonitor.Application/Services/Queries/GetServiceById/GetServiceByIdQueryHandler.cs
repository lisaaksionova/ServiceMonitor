using MediatR;
using ServiceMonitor.Application.Services.Dtos;

namespace ServiceMonitor.Application.Services.Queries.GetServiceById;

public class GetServiceByIdQueryHandler : IRequestHandler<GetServiceByIdQuery,  ServiceDto>
{
    public Task<ServiceDto> Handle(GetServiceByIdQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}