using MediatR;
using ServiceMonitor.Application.Services.Dtos;

namespace ServiceMonitor.Application.Services.Queries.GetAllServices;

public class GetAllServicesQuery : IRequest<IEnumerable<ServiceDto>>
{
    
}