using ECommerce.APP.Data;
using ECommerce.APP.Domain.Entities;
using ECommerce.APP.Service.Interfaces;
using ECommerce.APP.SharedLibrary.AppResponses;
using ECommerce.APP.SharedLibrary.Logs;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Linq.Expressions;

namespace ECommerce.APP.Service.Repositories
{
    public class ProductRepository : IProductInterface
    {
        private readonly AppDbContext dbContext;

        public ProductRepository(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<AppResponse> CreateAsync(Product entity)
        {
            try
            {
                var product = await GetAllAsync(p => p.Name.Equals(entity.Name));
                if (product != null && !string.IsNullOrEmpty(entity.Name))
                    return new AppResponse(false, $"{entity.Id} already exist");

                var prod = await dbContext.Products.AddAsync(entity);
                await dbContext.SaveChangesAsync();

                if (prod != null)
                {
                    return new AppResponse(true, $"{entity.Name} added successfully");
                }
                else
                {
                    return new AppResponse(false, "An error occur while adding product");
                }

            }
            catch (Exception ex)
            {
                LogExceptions.LogExcep(ex);
                return new AppResponse(false, "Fail to add product, an error occur");
            }
            
        }

        public async Task<AppResponse> DeleteAsync(Guid id)
        {
            try
            {
                var product = await FindByAsync(id);
                if (product == null)
                    return new AppResponse(false, "Product does not exist");

                dbContext.Products.Remove(product);
                await dbContext.SaveChangesAsync();
                return new AppResponse(true, $"{product.Id} successfully deleted");
            }
            catch (Exception ex)
            {
                LogExceptions.LogExcep(ex);
                return new AppResponse(false, "Fail to delete product, an error occur");
            }
        }

        public async Task<Product> FindByAsync(Guid id)
        {
            try
            {
                var product = await dbContext.Products.FindAsync(id);
                if (product == null)
                    return null!;
                return product;

            }
            catch (Exception ex)
            {

                LogExceptions.LogExcep(ex);
                throw new Exception("Fail to retrive product, an error occur");
            }
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            try
            {
                var products = await dbContext.Products.AsNoTracking().ToListAsync();
                return products != null ? products : Enumerable.Empty<Product>();
            }
            catch (Exception ex)
            {

                LogExceptions.LogExcep(ex);
                throw new Exception("Fail to retrive products, an error occur");
            }
        }

        public async Task<Product> GetAllAsync(Expression<Func<Product, bool>> predicate)
        {
            try
            {
                var products = await dbContext.Products.Where(predicate).FirstOrDefaultAsync();
                return products != null ? products : null!;
            }
            catch (Exception ex)
            {

                LogExceptions.LogExcep(ex);
                throw new Exception("Fail to retrive products, an error occur");
            }
        }

        public async Task<AppResponse> UpdateAsync(Product entity)
        {
            try
            {
                var product = await FindByAsync(entity.Id);
                if (product == null)
                    return new AppResponse(false, "product does not exist");

                dbContext.Entry(product).State = EntityState.Detached;
                dbContext.Products.Update(entity);
                await dbContext.SaveChangesAsync();

                return new AppResponse(true, "Product updated successfully");
            }
            catch (Exception ex)
            {

                LogExceptions.LogExcep(ex);
                throw new Exception("Fail to update product");
            }
        }
    }
}
