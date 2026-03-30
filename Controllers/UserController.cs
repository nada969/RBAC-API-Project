using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using RBAC_API_project.Models;
using RBAC_API_project.Services;

namespace RBAC_API_project.Controllers
{
    public record LoginRequest(string Email, string Password);

    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register( string Name, string Email, string Password)
        {
            if(await _userService.UserExistAsync(Email))
            {
                return BadRequest("Email is already here");
            }
            User new_user = await _userService.RegisterAsyn(Name, Email, Password);
            return Ok(new { new_user.Id, new_user.Name, new_user.Email });

        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            User? user = await _userService.LoginAsync(loginRequest.Email,loginRequest.Password);
            if (user == null) return Unauthorized("Invalid Email or Password");
            //var Token = 
            return Ok(user);
        }
    }
}
