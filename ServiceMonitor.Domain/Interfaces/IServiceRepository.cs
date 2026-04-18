using ServiceMonitor.Domain.Entities;

namespace ServiceMonitor.Domain.Interfaces;

public interface IServiceRepository
{ 
    Task<Service?> GetByIdAsync(int id);
    Task CreateAsync(Service service);
    Task Delete(Service service);
    Task Save();
    Task<IEnumerable<Service>> GetAllAsync();
}