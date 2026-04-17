using MediatR;
using ServiceMonitor.Application.Services.Dtos;

namespace ServiceMonitor.Application.Services.Queries.GetServiceById;

public class GetServiceByIdQuery(int id) : IRequest<ServiceDto>
{
    public int Id { get; set; } = id;
}