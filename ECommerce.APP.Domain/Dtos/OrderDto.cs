﻿using System.ComponentModel.DataAnnotations;

namespace ECommerce.APP.Domain.Dtos
{
    public record OrderDto(
        Guid Id,
        Guid ProductId,
        Guid ClientId,
        [Required, Range(1,int.MaxValue)] int purchasedQuantity
        );


    public record CreateOrderDto(
        Guid ProductId,
        Guid ClientId,
        [Required, Range(1, int.MaxValue)] int purchasedQuantity
        );

}
