using System.ComponentModel.DataAnnotations;

namespace ECommerce.APP.Domain.Dtos
{
    public record AppUserDto(
         Guid Id,
         [Required] string Name,
         [Required] string PhoneNumber,
         [Required] string Address,
         [Required, EmailAddress] string Email,
         [Required] string Password,
         [Required] string Role
        );

    public record RegisterAppUserDto(
         [Required] string Name,
         [Required] string PhoneNumber,
         [Required] string Address,
         [Required, EmailAddress] string Email,
         [Required] string Password,
         [Required] string Role
        );

}
