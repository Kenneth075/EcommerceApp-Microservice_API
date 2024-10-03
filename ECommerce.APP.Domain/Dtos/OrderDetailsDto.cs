using System;
using System.ComponentModel.DataAnnotations;

namespace ECommerce.APP.Domain.Dtos
{
    public record OrderDetailsDto(
         [Required] Guid OrderId,
         [Required] Guid ProductId,
         [Required] Guid ClientId,
         [Required] string ClientName,
         [Required, EmailAddress] string ClientEmail,
         [Required] string ClientPhoneNumber,
         [Required] string ProductName,
         [Required] int PurchaseQuantity,
         [Required, DataType(DataType.Currency)] decimal UnitPrice,
         [Required, DataType(DataType.Currency)] decimal TotalPrice,
         [Required] DateTime OrderDate
        );
   
}
