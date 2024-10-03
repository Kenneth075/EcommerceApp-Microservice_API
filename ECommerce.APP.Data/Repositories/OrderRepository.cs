using ECommerce.APP.Domain.Entities;
using ECommerce.APP.Service.Interfaces;
using ECommerce.APP.SharedLibrary.AppResponses;
using ECommerce.APP.SharedLibrary.Logs;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ECommerce.APP.Data.Repositories
{
    public class OrderRepository : IOrder
    {
        private readonly AppDbContext dbContext;

        public OrderRepository(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<AppResponse> CreateAsync(Order entity)
        {
            try
            {
                var product = await dbContext.Orders.AddAsync(entity);
                await dbContext.SaveChangesAsync();
                if(product != null)
                    return new AppResponse(true, "Order added successfully");
                return null!;

            }
            catch (Exception ex)
            {

                LogExceptions.LogExcep(ex);
                return new AppResponse(false, "An error occur while placing order");
            }
        }

        public async Task<AppResponse> DeleteAsync(Guid id)
        {
            try
            {
                var order = await FindByAsync(id);
                if (order == null)
                    return new AppResponse(false, "Order does not exist");
                dbContext.Orders.Remove(order);
                await dbContext.SaveChangesAsync();
                return new AppResponse(true, "Order deleted successfully");
            }
            catch (Exception ex)
            {

                LogExceptions.LogExcep(ex);
                return new AppResponse(false, "An error occur while deleting order");
            }
        }

        public async Task<Order> FindByAsync(Guid id)
        {
            try
            {
                var order = await dbContext.Orders.FindAsync(id);
                if (order != null)
                    return order;
                return null!;
            }
            catch (Exception ex)
            {
                LogExceptions.LogExcep(ex);
                throw new Exception("Unable to retrieve order");

            }
        }

        public async Task<IEnumerable<Order>> GetAllAsync()
        {
            try
            {
                var orders = await dbContext.Orders.AsNoTracking().ToListAsync();
                if(orders != null)
                    return orders;
                return null!;
            }
            catch (Exception ex)
            {

                LogExceptions.LogExcep(ex);
                throw new Exception("Unable to retrieve orders");
            }
        }

        public async Task<Order> GetAllAsync(Expression<Func<Order, bool>> predicate)
        {
            try
            {
                var order = await dbContext.Orders.Where(predicate).FirstOrDefaultAsync();
                if (order != null)
                    return order;
                return null!;
            }
            catch (Exception ex)
            {

                LogExceptions.LogExcep(ex);
                throw new Exception("Unable to retrieve order"); 
            }
        }

        public async Task<IEnumerable<Order>> GetClientOrders(Expression<Func<Order, bool>> predicate)
        {
            try
            {
                var clientOrder = await dbContext.Orders.Where(predicate).ToListAsync();
                if (clientOrder.Any())
                    return clientOrder;
                return null!;

            }
            catch (Exception ex)
            {
                LogExceptions.LogExcep(ex);
                throw new Exception("Failed. An error occur while retrieving client order");
            }
        }

        public async Task<AppResponse> UpdateAsync(Order entity)
        {
            try
            {
                var order = await FindByAsync(entity.Id);
                if (order == null)
                    return new AppResponse(false, "Order not find");

                dbContext.Entry(order).State = EntityState.Detached;
                dbContext.Orders.Update(entity);
                await dbContext.SaveChangesAsync();

                return new AppResponse(true, "Order updated successfully");
            }
            catch (Exception ex)
            {
                LogExceptions.LogExcep(ex);
                return new AppResponse(false, "Unable to update order");
            }
        }
    }
}
