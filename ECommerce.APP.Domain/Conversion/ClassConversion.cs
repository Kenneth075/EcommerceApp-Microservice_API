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
    }
}
