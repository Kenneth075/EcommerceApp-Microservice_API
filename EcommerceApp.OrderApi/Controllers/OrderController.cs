using ECommerce.APP.Domain.Conversion;
using ECommerce.APP.Domain.Dtos;
using ECommerce.APP.Domain.Entities;
using ECommerce.APP.Service.Interfaces;
using ECommerce.APP.Service.OrderServices;
using ECommerce.APP.SharedLibrary.AppResponses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceApp.OrderApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class OrderController : ControllerBase
    {
        private readonly IOrder orderInterface;
        private readonly IOrderService orderService;

        public OrderController(IOrder orderInterface, IOrderService orderService)
        {
            this.orderInterface = orderInterface;
            this.orderService = orderService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Order>>> GetAllOrders()
        {
            var orders = await orderInterface.GetAllAsync();

            var (_, result) = ClassConversion.FromEntityOrder(null!, orders);
            if (result != null)
                return Ok(result);
            return NotFound();
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<Order>> GetById(Guid id)
        {
            var order = await orderInterface.FindByAsync(id);
            var (result,_) = ClassConversion.FromEntityOrder(order,null!);
            if(result != null)
            {
                return Ok(result);
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<ActionResult<AppResponse>> CreateOrder(CreateOrderDto createOrder)
        {
            var order = ClassConversion.ToEntityOrder(createOrder);
            var result = await orderInterface.CreateAsync(order);
            if (result.Flag == true)
                return Ok(result);
            return BadRequest(result);
            
        }

        [HttpPut]
        public async Task<ActionResult<AppResponse>> UpdateOrder(OrderDto updateOrder)
        {
            var order = ClassConversion.ToEntityOrder(updateOrder);
            var orderResult = await orderInterface.UpdateAsync(order);
            if (orderResult.Flag == true)
                return Ok(orderResult);
            return BadRequest(orderResult);
        }

        [HttpDelete("{id:guid}")]
        public async Task<ActionResult<AppResponse>> DeleteOrder(Guid id)
        {
            var order = await orderInterface.DeleteAsync(id);
            if (order.Flag == true)
                return Ok(order);
            return BadRequest(order);
        }

        [HttpGet("Client/{clientId:Guid}")]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetOrderByClientAync(Guid clientId)
        {
            var orders = await orderService.GetOrderByClientAsync(clientId);
            if (orders == null)
                return NotFound(orders);
            return Ok(orders);
        }

        [HttpGet("{OrderId:Guid}")]
        public async Task<ActionResult<OrderDetailsDto>> GetOrderDetailsAsync(Guid OrderId)
        {
            var order = await orderService.GetOrderDetailsAsync(OrderId);
            if (order == null)
                return NotFound(order);
            return Ok(order);
        }

    }
}
