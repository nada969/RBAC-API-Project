using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using RBAC_API_project.Data;
using RBAC_API_project.DTO;
using RBAC_API_project.Models;
using RBAC_API_project.Repository;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace RBAC_API_project.Services
{
    public interface IUserService
    {
        Task<bool> UserExistAsync(string Email);
        Task<User?> LoginAsync(string Email, string Password);
        Task<User> RegisterAsyn(string Name, string Email, string Password);

    }
    public class UserService : IUserService
    {
        private readonly IUserRepo _userRepo;
        private readonly UserManager<User> _userManager;
        private readonly JWT _jwt;
        public UserService(IUserRepo userRepo, IOptions<JWT> jwt)
        {
            _userRepo = userRepo;
            _jwt = jwt.Value;
        }

        public async Task<bool> UserExistAsync(string Email)
        {
            return await _userRepo.UserExistAsync(Email);
        }
        public async Task<User?> LoginAsync(string Email, string Password)
        {

            User? user = await _userRepo.GetByEmailAsync(Email);
            if (user == null)
                return null;

            bool IsValid = BCrypt.Net.BCrypt.Verify(Password, user.PasswordHash);
            return IsValid ? user : null;
        }
        public async Task<User> RegisterAsyn(string Name, string Email, string Password)
        {
            
            User new_user = await _userRepo.RegisterAsyn(Name, Email, Password);
            var jwtSecurityToken = await CreateJwtToken(new_user);

            return new User
            {
                Email = new_user.Email,
                ExpiresOn = jwtSecurityToken.ValidTo,
                IsAuthenticated = true,
                Roles = new List<string> { "User" },
                Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
                Name = new_user.Name
            };
        }

        private async Task<JwtSecurityToken> CreateJwtToken(User user)
        {

            var userClaims = await _userManager.GetClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);
            var roleClaims = new List<Claim>();

            foreach (var role in roles)
                roleClaims.Add(new Claim("roles", role));

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Name),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("uid", user.Id)
            }
            .Union(userClaims)
            .Union(roleClaims);

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.key));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _jwt.Issuer,
                audience: _jwt.Audience,
                claims: claims,
                expires: DateTime.Now.AddDays(_jwt.DurationInDays),
                signingCredentials: signingCredentials);

            return jwtSecurityToken;
        } 
    }
    
}
