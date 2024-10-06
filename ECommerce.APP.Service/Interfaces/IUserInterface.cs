using ECommerce.APP.Domain.Dtos;
using ECommerce.APP.Domain.Entities;
using ECommerce.APP.SharedLibrary.AppResponses;

namespace ECommerce.APP.Service.Interfaces
{
    public interface IUserInterface
    {
        Task<AppResponse> RegisterUserAsync(RegisterAppUserDto user);
        Task<AppResponse> LoginUserAsync(LoginDto login);
        Task<IEnumerable<AppUser>> GetUserAsync();
        Task<AppUserDto> GetAppUserAsync(Guid userId);
        Task<AppResponse> DeleteUserAsync(Guid userId);
    }
}
