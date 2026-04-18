using ServiceMonitor.Domain.Entities;

namespace ServiceMonitor.Domain.Interfaces;

public interface IIncidentRepository
{
    Task<IEnumerable<Incident>> GetAllAsync();
    Task<Incident?> GetByIdAsync(int id);
    Task CreateAsync(Incident incident);
    Task DeleteAsync(Incident incident);
    Task SaveAsync();
}