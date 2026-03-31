using RBAC_API_project.Models;
using RBAC_API_project.Repository;

namespace RBAC_API_project.Services
{
    public interface IUserService
    {
        Task<bool> UserExistAsync(string Email);
        Task<User?> LoginAsync(string Email, string Password);
        Task<User> RegisterAsyn(string Name, string Email, string Password);
    }
    public class UserService: IUserService
    {
        private readonly IUserRepo _userRepo;
        public UserService(IUserRepo userRepo)
        {
            _userRepo = userRepo;
        }
        public async Task<bool> UserExistAsync(string Email)
        {
            return await _userRepo.UserExistAsync(Email);
        }
        public async Task<User?> LoginAsync(string Email, string Password)
        {
            if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password))
                return null;

            User? user = await _userRepo.GetByEmailAsync(Email);
            if (user == null)
                return null;

            bool IsValid = BCrypt.Net.BCrypt.Verify(Password, user.Password);
            return IsValid ? user : null;
        }
        public async Task<User> RegisterAsyn(string Name, string Email, string Password)
        {
            return await _userRepo.RegisterAsyn(Name, Email, Password);

        }
    }
}
