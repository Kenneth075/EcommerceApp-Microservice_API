using ECommerce.APP.SharedLibrary.AppResponses;
using System.Linq.Expressions;

namespace ECommerce.APP.SharedLibrary.Interfaces
{
    public interface IGenericInterface<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetAllAsync(Expression<Func<T, bool>> predicate);
        Task<T> FindByAsync(Guid id);
        Task<AppResponse> CreateAsync(T entity);
        Task<AppResponse> UpdateAsync(T entity);
        Task<AppResponse> DeleteAsync(Guid id);
    }
}
