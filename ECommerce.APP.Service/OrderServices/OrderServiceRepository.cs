using ECommerce.APP.Domain.Dtos;
using ECommerce.APP.Domain.Entities;
using ECommerce.APP.Service.Interfaces;
using ECommerce.APP.SharedLibrary.Logs;
using Polly.Registry;
using System.Net.Http.Json;

namespace ECommerce.APP.Service.OrderServices
{
    public class OrderServiceRepository : IOrderService
    {
        private readonly HttpClient httpClient;
        private readonly IOrder orderInterface;
        private readonly ResiliencePipelineProvider<string> resiliencePipeline;

        public OrderServiceRepository(HttpClient httpClient, IOrder orderInterface, ResiliencePipelineProvider<string> resiliencePipeline)
        {
            this.httpClient = httpClient;
            this.orderInterface = orderInterface;
            this.resiliencePipeline = resiliencePipeline;
        }
        
        public async Task<IEnumerable<OrderDto>> GetOrderByClientAsync(Guid clientId)
        {
            var order = await orderInterface.GetClientOrders(x => x.ClientId == clientId);
            if (order == null)
                return null!;

            return (IEnumerable<OrderDto>)order;

        }


        public async Task<OrderDetailsDto> GetOrderDetailsAsync(Guid orderId)
        {
            try
            {
                var order = await orderInterface.FindByAsync(orderId);
                if (order == null)
                    return null!;

                //Set retry pipeline
                var retrypipeline = resiliencePipeline.GetPipeline("my-retry-pipeline");

                //Get Product details from product api call.
                var products = await retrypipeline.ExecuteAsync(async token => await GetProduct(order.ProductId));
                if (products == null)
                    return null!;

                

                //Get App user from app user api call.
                var appUser = await retrypipeline.ExecuteAsync(async token => await GetUser(order.ClientId));

                //Return order details.
                return new OrderDetailsDto(
                     order.Id,
                     products.Id,
                     appUser.Id,
                     appUser.Name,
                     appUser.Email,
                     appUser.PhoneNumber,
                     products.Name,
                     order.PurchaseQuantity,
                     products.Price,
                     products.Quantity * order.PurchaseQuantity,
                     order.OrderDate
                    );
                
            }
            catch (Exception ex)
            {

                LogExceptions.LogExcep(ex);
                throw new Exception("An error occur");
            }
        }

        private async Task<ProductDto> GetProduct(Guid productId)
        {
            //Get product Api using HttpClient.
            //Redirect this call to the Api Gateway since product api does not response to outside Api call.

            var getProduct = await httpClient.GetAsync($"api/product/{productId}");
            if (!getProduct.IsSuccessStatusCode)
            {
                return null!;
            }
            var product = await getProduct.Content.ReadFromJsonAsync<ProductDto>();
            return product!;
        }

        private async Task<AppUserDto> GetUser(Guid userid)
        {
            var getUser = await httpClient.GetAsync($"api/auth/{userid}");
            if(!getUser.IsSuccessStatusCode)
                return null!;
            var user = await getUser.Content.ReadFromJsonAsync<AppUserDto>();
            return user!;
        }
    }
}
