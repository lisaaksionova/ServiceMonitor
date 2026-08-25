using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using ServiceMonitor.Domain.Interfaces;
using ServiceMonitor.Infrastructure.Persistence;

namespace ServiceMonitor.Infrastructure.Repositories;

public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : class
{
    protected MonitorDbContext _context;
    public RepositoryBase(MonitorDbContext context)
    {
        _context = context;
    }

    public void Create(T entity) => _context.Set<T>().Add(entity);
    public void Delete(T entity) => _context.Set<T>().Remove(entity);
    public IQueryable<T> GetAll() =>
      _context.Set<T>().AsNoTracking();
    public IQueryable<T> GetByCondition(Expression<Func<T, bool>> expression) =>
      _context.Set<T>().Where(expression).AsNoTracking();
    public void Update(T entity) => _context.Set<T>().Update(entity);
}
