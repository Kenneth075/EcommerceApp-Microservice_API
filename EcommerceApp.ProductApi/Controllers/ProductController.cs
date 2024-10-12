using ECommerce.APP.Domain.Conversion;
using ECommerce.APP.Domain.Dtos;
using ECommerce.APP.Domain.Entities;
using ECommerce.APP.Service.Interfaces;
using ECommerce.APP.SharedLibrary.AppResponses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceApp.ProductApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductInterface productInterface;

        public ProductController(IProductInterface productInterface)
        {
            this.productInterface = productInterface;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetAllProductAsync()
        {
            var response = await productInterface.GetAllAsync();
            var (_, list) = ClassConversion.FromEntity(null, response);
            if (list.Any())
                return Ok(list);
            return NotFound();
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<Product>> GetById(Guid id)
        {
            var result = await productInterface.FindByAsync(id);
            var (prod,_) = ClassConversion.FromEntity(result, null!);
            if(prod != null)
                return Ok(prod);
            return NotFound("Product not found");
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<AppResponse>> CreateProductAsync(ProdDto prodDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var prod = ClassConversion.ToEntity(prodDto);
            var result = await productInterface.CreateAsync(prod);
            if(result.Flag == true)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpPut]
        [Authorize]
        public async Task<ActionResult<AppResponse>> UpdateAsync(ProductDto productDto)
        {
            if (!ModelState.IsValid) 
                return BadRequest();

            var prod = ClassConversion.ToEntity(productDto);
            var result = await productInterface.UpdateAsync(prod);
            if (result.Flag == true)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpDelete("{id:guid}")]
        [Authorize]
        public async Task<ActionResult<AppResponse>> DeleteAsync(Guid id)
        {
            var result = await productInterface.DeleteAsync(id);
            if(result.Flag == true)
                return Ok(result);
            return BadRequest(result);

        }
    }
}
