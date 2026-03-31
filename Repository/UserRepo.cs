using Microsoft.EntityFrameworkCore;
using RBAC_API_project.Data;
using RBAC_API_project.Models;

namespace RBAC_API_project.Repository
{
    
    public interface IUserRepo
    {
        Task<bool> UserExistAsync(string Email);
        Task<User> RegisterAsyn(string Name, string Email, string Password);
        Task<User?> GetByEmailAsync(string Email);   
    }
    public class UserRepo:IUserRepo
    {
        private readonly UserDb _userDb;
        public UserRepo(UserDb userDb){
            _userDb = userDb;
        }
        public async Task<bool> UserExistAsync(string Email)
        {
            return await _userDb.Users.AnyAsync(u => u.Email.ToLower() == Email.ToLower());
        }
        public async Task<User> RegisterAsyn(string Name, string Email, string Password)
        {
            /// 1.create new user
            User new_user = new User(Name, Email, Password) ;

            /// 2.save to DB
            await _userDb.Users.AddAsync(new_user);
            await _userDb.SaveChangesAsync();

            /// 3.Return the new user
            return new_user;
        }


        public async Task<User?> GetByEmailAsync(string Email)
        {
            return await _userDb.Users
                .FirstOrDefaultAsync(u => u.Email.ToLower() == Email.ToLower());
        }

    }
}
