using ServiceMonitor.Domain.Common;
using ServiceMonitor.Domain.Entities;

namespace ServiceMonitor.Domain.Interfaces;

public interface IHourlyServiceCheckRepository
{
    void CreateRange(IEnumerable<HourlyServiceCheck> serviceCheck);

    Task<PagedList<HourlyServiceCheck>> GetAllFromToPaged(int page, int pageSize, Guid serviceId, DateTime from, DateTime to,
        CancellationToken cancellationToken);

    Task<List<HourlyServiceCheck>> GetAllFromTo(DateTime from, DateTime to,
        CancellationToken cancellationToken);

    Task<DateTime?> GetLastCheckedHour(CancellationToken cancellationToken);

    Task DeleteOlderThanAsync(DateTime hour, CancellationToken cancellationToken);
}
