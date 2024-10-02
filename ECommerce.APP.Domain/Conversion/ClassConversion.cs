using ECommerce.APP.Domain.Dtos;
using ECommerce.APP.Domain.Entities;

namespace ECommerce.APP.Domain.Conversion
{
    public class ClassConversion
    {
        public static Product ToEntity(ProductDto productDto)
        {
            return new Product
            {
                Id = productDto.Id,
                Name = productDto.Name,
                Price = productDto.Price,
                Quantity = productDto.Quantity,
            };

        }

        public static Product ToEntity(ProdDto productDto)
        {
            return new Product
            {
                Name = productDto.Name,
                Price = productDto.Price,
                Quantity = productDto.Quantity,
            };

        }

        public static Order ToEntityOrder(OrderDto orderDto)
        {
            return new Order
            {
                Id= orderDto.Id,
                ProductId= orderDto.ProductId,
                ClientId= orderDto.ClientId,
                PurchaseQuantity = orderDto.purchasedQuantity
            };

        }

        public static Order ToEntityOrder(CreateOrderDto createOrderDto)
        {
            return new Order
            {
                ProductId = createOrderDto.ProductId,
                PurchaseQuantity = createOrderDto.purchasedQuantity
            };
        }

        public static (ProductDto,IEnumerable<ProductDto>) FromEntity(Product product, IEnumerable<Product> products)
        {
            //Return single product
            if(product != null || products == null)
            {
                var singleProduct = new ProductDto(
                    product.Id,
                    product.Name,
                    product.Price,
                    product.Quantity
                    );
                return (singleProduct, null!);
            }

            //Return all products
            if(product == null || products != null)
            {
                var prod = products.Select(p => new ProductDto(p.Id,p.Name, p.Price, p.Quantity)).ToList();
                return(null!, prod);
            }

            return(null!, null!);
        }

        public static (OrderDto, IEnumerable<OrderDto>) FromEntityOrder(Order order, IEnumerable<Order> orders)
        {
            //Return single product
            if (order != null || orders == null)
            {
                var singleProduct = new OrderDto(
                    order.Id,
                    order.ProductId,
                    order.ClientId,
                    order.PurchaseQuantity
                    );
                return (singleProduct, null!);
            }

            //Return all products
            if (order == null || orders != null)
            {
                var orderResult = orders.Select(p => new OrderDto(p.Id, p.ProductId, p.ClientId, p.PurchaseQuantity )).ToList();
                return (null!, orderResult);
            }

            return (null!, null!);
        }
    }
}
