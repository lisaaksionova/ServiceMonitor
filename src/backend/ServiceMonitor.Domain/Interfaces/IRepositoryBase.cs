using System.Linq.Expressions;

namespace ServiceMonitor.Domain.Interfaces;

public interface IRepositoryBase<T>
{
    IQueryable<T> GetAllAsync(bool trackChanges);
    IQueryable<T> GetByConditionAsync(Expression<Func<T, bool>> expression, bool trackChanges);
    void CreateAsync(T entity);
    void UpdateAsync(T entity);
    void DeleteAsync(T entity);
}
