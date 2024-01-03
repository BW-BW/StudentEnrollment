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
        public async Task<IActionResult> ChangePassword(UserModelDTO userModelDTO, string newPassword)
        {
            var response = await _accountService.ChangePassword(userModelDTO, newPassword);
            return Ok(response);
        }

        [HttpGet("profile")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetProfile()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                // User not authenticated
                return Unauthorized();
            }

            var userProfile = await _accountService.GetProfile(userId);

            if (userProfile == null)
            {
                // User not found
                return NotFound();
            }

            return Ok(userProfile);
        }
    }
}
