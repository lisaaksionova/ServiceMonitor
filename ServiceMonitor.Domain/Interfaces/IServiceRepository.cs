using ServiceMonitor.Domain.Entities;

namespace ServiceMonitor.Domain.Interfaces;

public interface IServiceRepository
{ 
    Task<Service?> GetByIdAsync(int id);
    Task CreateAsync(Service service);
    Task UpdateAsync(Service service);
    Task Delete(int id);
}