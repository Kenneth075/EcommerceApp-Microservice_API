using ECommerce.APP.Domain.Conversion;
using ECommerce.APP.Domain.Dtos;
using ECommerce.APP.Domain.Entities;
using ECommerce.APP.Service.Interfaces;
using ECommerce.APP.SharedLibrary.AppResponses;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceApp.OrderApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrder orderInterface;

        public OrderController(IOrder orderInterface)
        {
            this.orderInterface = orderInterface;
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
    }
}
