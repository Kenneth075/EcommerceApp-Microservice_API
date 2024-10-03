using ECommerce.APP.Domain.Dtos;

namespace ECommerce.APP.Service.OrderServices
{
    public interface IOrderService
    {
        Task<IEnumerable<OrderDto>> GetOrderByClientAsync(Guid clientId);
        Task<OrderDetailsDto> GetOrderDetailsAsync(Guid orderId);
    }
}
