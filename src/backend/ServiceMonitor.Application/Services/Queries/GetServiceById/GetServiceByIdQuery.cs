using MediatR;
using ServiceMonitor.Application.Services.Dtos;

namespace ServiceMonitor.Application.Services.Queries.GetServiceById;

public class GetServiceByIdQuery(Guid id) : IRequest<ServiceDto>
{
    public Guid Id { get; set; } = id;
}
