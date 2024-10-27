using ECommerce.APP.Domain.Dtos;
using ECommerce.APP.Domain.Entities;
using ECommerce.APP.Service.Interfaces;
using ECommerce.APP.SharedLibrary.AppResponses;
using EcommerceApp.ProductApi.Controllers;
using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApplicationTest
{
    public class ProductApiTest
    {
        private readonly IProductInterface _productInterface;
        private readonly ProductController _productController;

        public ProductApiTest()
        {
            //Set up dependency
            _productInterface = A.Fake<IProductInterface>();

            //Set up System Under Test -SUT
            _productController = new ProductController(_productInterface);
        }

        [Fact]
        public async Task WhenProductsIsNotNullReturnOK()
        {
            //Arrange
            var products = new List<Product>()
            {
                new(){Id = new Guid("e1a02edf-5c9c-4805-a75c-7ffd34330f19"), Name = "books", Price = 50m, Quantity = 5},
                new(){Id = new Guid("c910674c-b35e-4c6b-bc1f-ad4aaf1627fe"), Name = "pens", Price = 50m, Quantity = 5}
            };
            //Make a fake call.
            A.CallTo(() => _productInterface.GetAllAsync()).Returns(products);

            //Act
            var result = await _productController.GetAllProductAsync();

            //Assert
            var oKResult = result.Result as OkObjectResult;
            oKResult.Should().NotBeNull();
            oKResult!.StatusCode.Should().Be(StatusCodes.Status200OK);

            var returnProducts = oKResult.Value as IEnumerable<ProductDto>;
            returnProducts.Should().NotBeNull();
            returnProducts.Should().HaveCount(2);
            


        }

        [Fact]
        public async Task WhenProductsIsNullReturnNotFound()
        {
            //Arrange
            var products = new List<Product>();
            
            //Make a fake call.
            A.CallTo(() => _productInterface.GetAllAsync()).Returns(products);

            //Act
            var result = await _productController.GetAllProductAsync();

            //Assert
            var notFoundResult = result.Result as NotFoundResult;
            notFoundResult.Should().NotBeNull();
            notFoundResult!.StatusCode.Should().Be(StatusCodes.Status404NotFound);

           

        }

        [Fact]
        public async Task CreateProduct_WhenModelStateIsInvalid_ReturnBadRequest()
        {
            //Arrange
            var products = new ProdDto("Books", 50m, 5);
            _productController.ModelState.AddModelError("Name", "Required");

            //Act
            var result = await _productController.CreateProductAsync(products);

            //Assert
            var badRequestResult = result.Result as BadRequestResult;
            badRequestResult.Should().NotBeNull();
            badRequestResult!.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        }

        [Fact]
        public async Task CreateProduct_WhenIsSuccessful_ReturnOk()
        {
            //Arrange
            var products = new ProdDto("Books", 50m, 5);
            var response = new AppResponse(true, "Created");

            A.CallTo(() => _productInterface.CreateAsync(A<Product>.Ignored)).Returns(response);

            //Act
            var result = await _productController.CreateProductAsync(products);

            //Assert
            var okResult = result.Result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult!.StatusCode.Should().Be(StatusCodes.Status200OK);

            var returnRes = okResult.Value as AppResponse;
            returnRes!.Flag.Should().BeTrue();
            returnRes.Message.Should().Be("Created");
        }
    }
}
