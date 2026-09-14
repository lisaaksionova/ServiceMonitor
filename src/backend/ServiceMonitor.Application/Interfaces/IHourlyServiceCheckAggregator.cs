namespace ServiceMonitor.Application.Interfaces;

public interface IHourlyServiceCheckAggregator
{
    Task AggregateAsync(CancellationToken cancellationToken);
}
