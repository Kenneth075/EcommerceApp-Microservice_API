using System.ComponentModel.DataAnnotations;

namespace ECommerce.APP.Domain.Dtos
{
    public record ProductDto(
         Guid Id,
         [Required]string Name,
         [Required, Range(1,int.MaxValue), DataType(DataType.Currency)] decimal Price,
         [Required, Range(1,int.MaxValue)]int Quantity
        );

    public record ProdDto(
         [Required] string Name,
         [Required, Range(1, int.MaxValue), DataType(DataType.Currency)] decimal Price,
         [Required, Range(1, int.MaxValue)] int Quantity
        );

}
