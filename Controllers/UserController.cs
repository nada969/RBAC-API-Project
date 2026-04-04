using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using RBAC_API_project.Data;
using RBAC_API_project.Models;
using RBAC_API_project.Services;

namespace RBAC_API_project.Controllers
{
    //[Authorize(AuthenticationSchemes ="Bearer")]
    [Route("api/[controller]")]
    [ApiController]
    public record RegisterRequest(string Name, string Email, string Password);
    public record LoginRequest(string Email, string Password);

   
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            if (await _userService.UserExistAsync(request.Email))
            {
                return BadRequest("Email is already here");
            }
            User new_user = await _userService.RegisterAsyn(request.Name, request.Email, request.Password);
            return Ok(new { new_user.Id, new_user.Name, new_user.Email });

        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            if (string.IsNullOrWhiteSpace(loginRequest.Email) || string.IsNullOrWhiteSpace(loginRequest.Password))
                return BadRequest("Email and password are required");

            User? user = await _userService.LoginAsync(loginRequest.Email, loginRequest.Password);
            if (user == null)
                return BadRequest("Invalid email or password");

            return Ok(new { user.Id, user.Name, user.Email });
        }
        [HttpGet("test")]
        public async Task<IActionResult> Test()
        {
            var user = await _userService.LoginAsync("momo@gmail.com", "str533");
            return Ok(user == null ? "NOT FOUND" : "FOUND: " + user.Name);
        }
       
    }
}




//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Http.HttpResults;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Identity.Data;
//using Microsoft.AspNetCore.Mvc;
//using RBAC_API_project.Data;
//using RBAC_API_project.DTO;
//using RBAC_API_project.Models;
//using RBAC_API_project.Services;

//namespace RBAC_API_project.Controllers
//{

//    [Route("api/[controller]")]
//    [ApiController]
//    public class UserController : ControllerBase
//    {
//        private readonly IUserService _userService;

//        public UserController(IUserService userService)
//        {
//            _userService = userService;
//        }
//        public static User user = new();

//        [HttpPost("register")]
//        public async Task<IActionResult> Register(UserDTO request)
//        {
//            var hashPassword = new PasswordHasher<User>()
//                .HashPassword(user, request.Password);
//            user.Name = request.Name;
//            user.Password = hashPassword;
//            user.Email = request.Email;
//            User new_user = await _userService.RegisterAsyn(user.Name,user.Email,user.Password);
//            return Ok(user);
//        }

//        [HttpPost("login")]
//        public ActionResult<string> Login(UserDTO request)
//        {
//            if(user.Name != request.Name)
//            {
//                return BadRequest("Name Wrong");
//            }
//            if(new PasswordHasher<User>().VerifyHashedPassword(user,user.Password,request.Password) == PasswordVerificationResult.Failed)
//            {
//                return BadRequest("Password Wrong");
//            }
//            string Token = "Success";
//            return Ok(new { user.Id, user.Name, user.Email });
//        }

//    }


//}
