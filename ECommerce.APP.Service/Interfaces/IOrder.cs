using ECommerce.APP.Domain.Entities;
using ECommerce.APP.SharedLibrary.Interfaces;
using System.Linq.Expressions;

namespace ECommerce.APP.Service.Interfaces
{
    public interface IOrder : IGenericInterface<Order>
    {
        Task<IEnumerable<Order>> GetClientOrders(Expression<Func<Order, bool>> predicate);
    }
}
