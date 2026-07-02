using ServiceMonitor.Domain.Entities;

namespace ServiceMonitor.Domain.Interfaces;

public interface IServiceRepository
{ 
    Task<Service?> GetByIdAsync(int id, CancellationToken cancellationToken);
    Task CreateAsync(Service service, CancellationToken cancellationToken);
    Task Delete(Service service, CancellationToken cancellationToken);
    Task Save(CancellationToken cancellationToken);
    Task<IEnumerable<Service>> GetAllAsync(CancellationToken cancellationToken);
    Task<IEnumerable<Service>> GetServicesForCheck(CancellationToken cancellationToken);
}