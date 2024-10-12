using ECommerce.APP.Domain.Dtos;
using ECommerce.APP.Domain.Entities;
using ECommerce.APP.Service.Interfaces;
using ECommerce.APP.SharedLibrary.AppResponses;
using ECommerce.APP.SharedLibrary.Logs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ECommerce.APP.Data.Repositories
{
    public class AppUserRepository : IUserInterface
    {
        private readonly AppDbContext dbContext;
        private readonly IConfiguration configuration;

        public AppUserRepository(AppDbContext dbContext, IConfiguration configuration)
        {
            this.dbContext = dbContext;
            this.configuration = configuration;
        }

        public async Task<AppUserDto> GetAppUserAsync(Guid userId)
        {
            try
            {
                var user = await dbContext.Users.FindAsync(userId);
                if (user == null)
                    return null!;
                return new AppUserDto(
                     user.ClientId,
                     user.Name,
                     user.PhoneNumber!,
                     user.Address!,
                     user.Email!,
                     user.Password!,
                     user.Role!
                    );
            }
            catch (Exception ex)
            {
                LogExceptions.LogExcep(ex);
                throw new Exception("Error, unable to retreieve user details");
            }
        }

        public async Task<AppResponse> LoginUserAsync(LoginDto login)
        {
            try
            {
                var getUser = await GetUserByEmail(login.Email);
                if (getUser == null)
                    return new AppResponse(false, "Invalid credentials");

                var verifyPassword = BCrypt.Net.BCrypt.Verify(login.Password, getUser.Password);
                if (!verifyPassword) return new AppResponse(false, "Invalid password");

                string token = GenerateToken(getUser);

                return new AppResponse(true, token);
            }
            catch (Exception ex)
            {
                LogExceptions.LogExcep(ex);
                return new AppResponse(false,"Login failed, an error occur");
            }

        }

        private string GenerateToken(AppUser getUser)
        {
            var key = Encoding.UTF8.GetBytes(configuration.GetSection("Authentication:Key").Value!);
            var securityKey = new SymmetricSecurityKey(key);
            var credential = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, getUser.Name),
                new Claim(ClaimTypes.Email, getUser.Email!),
            };

            if (!string.IsNullOrEmpty(getUser.Role) && !Equals("string", getUser.Role))
                claims.Add(new(ClaimTypes.Role, getUser.Role!));

            //if (!string.IsNullOrEmpty(appUser.Role) && appUser.Role != "string")
            //    cliams.Add(new(ClaimTypes.Role, appUser.Role!));

            var jwtToken = new JwtSecurityToken(
                issuer: configuration["Authentication:Issuer"],
                audience: configuration["Authentication:Audience"],
                claims:claims,
                expires: DateTime.UtcNow.AddMinutes(15),
                signingCredentials: credential
                );

            return new JwtSecurityTokenHandler().WriteToken(jwtToken);
        }

        public async Task<AppResponse> RegisterUserAsync(RegisterAppUserDto user)
        {
            try
            {
                var userEmail = await GetUserByEmail(user.Email);
                if (userEmail != null)
                    return new AppResponse(false, "User already exist");

                var registerUser = await dbContext.Users.AddAsync(new AppUser
                {
                    Name = user.Name,
                    Password = BCrypt.Net.BCrypt.HashPassword(user.Password),
                    PhoneNumber = user.PhoneNumber,
                    Address = user.Address,
                    Email = user.Email,
                    Role = user.Role
                });
                var result = await dbContext.SaveChangesAsync();
                if (result >= 1)
                    return new AppResponse(true, "User registered successfully");

                return new AppResponse(false, "Invalid data provided");

            }
            catch (Exception ex)
            {
                LogExceptions.LogExcep(ex);
                return new AppResponse(false, "An error occur while registing user");
            }
              
                
        }

        private async Task<AppUser> GetUserByEmail(string email)
        {
            var user = await dbContext.Users.FirstOrDefaultAsync(x => x.Email == email);
            if (user == null) return null!;
            return user;
        }

        public async Task<IEnumerable<AppUser>> GetUserAsync()
        {
            try
            {
                var user = await dbContext.Users.AsNoTracking().ToListAsync();
                if(user == null) return null!;
                return user;
            }
            catch (Exception ex)
            {
                LogExceptions.LogExcep(ex);
                throw new Exception("Fail to retrieve user");
            }
        }


        public async Task<AppResponse> DeleteUserAsync(Guid userId)
        {
            try
            {
                var user = await dbContext.Users.FindAsync(userId);
                if (user == null) return null!;
                dbContext.Users.Remove(user);
                await dbContext.SaveChangesAsync();
                return new AppResponse(true, $"{user.Name} deleted successfully");
            }
            catch (Exception ex)
            {
                LogExceptions.LogExcep(ex);
                throw new Exception("Fail to delete user");
            }
        }
    }
}
