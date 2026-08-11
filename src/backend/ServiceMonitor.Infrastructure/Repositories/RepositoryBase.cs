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

    public void CreateAsync(T entity) => _context.Set<T>().Add(entity);
    public void DeleteAsync(T entity) => _context.Set<T>().Remove(entity);
    public IQueryable<T> GetAllAsync(bool trackChanges) =>
      !trackChanges ? _context.Set<T>().AsNoTracking() : _context.Set<T>();
    public IQueryable<T> GetByConditionAsync(Expression<Func<T, bool>> expression, bool trackChanges) =>
      !trackChanges ? _context.Set<T>().Where(expression).AsNoTracking() : _context.Set<T>().Where(expression);
    public void UpdateAsync(T entity) => _context.Set<T>().Update(entity);
}
