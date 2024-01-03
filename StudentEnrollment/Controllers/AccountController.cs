using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudentEnrollment.DTOs;
using StudentEnrollment.Interface;
using System.Security.Claims;

namespace StudentEnrollment.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }


        [HttpPost("register")]
        public async Task<IActionResult> Register(UserModelDTO userDTO)
        {
            var response = await _accountService.CreateAccount(userDTO);
            return Ok(response);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDTO loginDTO)
        {
            var response = await _accountService.LoginAccount(loginDTO);
            return Ok(response);
        }

        [HttpPost("change-password")]
        [Authorize]
        public async Task<IActionResult> ChangePassword(string oldPassword, string newPassword)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                // User not authenticated
                return Unauthorized();
            }

            var userProfile = await _accountService.GetProfile(userId);


            var response = await _accountService.ChangePassword(userProfile.Email, oldPassword, newPassword);
            return Ok(response);
        }
    }
}
