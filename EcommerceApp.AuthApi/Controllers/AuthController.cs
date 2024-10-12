using ECommerce.APP.Domain.Dtos;
using ECommerce.APP.Domain.Entities;
using ECommerce.APP.Service.Interfaces;
using ECommerce.APP.SharedLibrary.AppResponses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceApp.AuthApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class AuthController : ControllerBase
    {
        private readonly IUserInterface userInterface;

        public AuthController(IUserInterface userInterface)
        {
            this.userInterface = userInterface;
        }

        [HttpGet("GetUsers")]
        [Authorize(Roles ="Admin")]
        public async Task<ActionResult<AppUser>> GetAllUserAsync()
        {
            var user = await userInterface.GetUserAsync();
            if(user == null)
                return NotFound();
            return Ok(user);
        }

        [HttpGet("{id:guid}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<AppUserDto>> GetAppUserAsync(Guid id)
        {
            var result = await userInterface.GetAppUserAsync(id);
            if (result == null)
                return NotFound();
            return Ok(result);
        }

        [HttpPost("Register")]
        public async Task<ActionResult<AppResponse>> RegisterAsync(RegisterAppUserDto appUser)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await userInterface.RegisterUserAsync(appUser);
            if(result.Flag == false)
                return BadRequest(result);
            return Ok(result);
        }

        [HttpPost("Login")]
        public async Task<ActionResult<AppResponse>> LoginAsync(LoginDto loginDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await userInterface.LoginUserAsync(loginDto);
            if(result.Flag == false)
                return BadRequest(result);
            return Ok(result);
        }

        [HttpDelete("User/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<AppResponse>> DeleteUserAsync(Guid id)
        {
            var user = await userInterface.DeleteUserAsync(id);
            if (user.Flag == false)
                return NotFound();
            return Ok(user);
        }
    }
}
