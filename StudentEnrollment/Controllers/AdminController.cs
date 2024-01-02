using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudentEnrollment.DTOs;
using StudentEnrollment.Interface;
using StudentEnrollment.Services;
using System.Security.Claims;

namespace StudentEnrollment.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;

        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        [HttpPost("admin-register")]
        public async Task<IActionResult> CreateAdmin(UserModelDTO userDTO)
        {
            var response = await _adminService.CreateAdmin(userDTO);
            return Ok(response);
        }

        [HttpGet("admin-get")]
        public async Task<IActionResult> GetAllAdmin()
        {
            var response = await _adminService.GetAllAdmin();
            return Ok(response);
        }

        [HttpGet("admin-get/{id}")]
        public async Task<ActionResult> GetAdmin(string id)
        {
            var response = await _adminService.GetAdmin(id);

            return Ok(response);
        }

        [HttpPost("admin-update")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateAdmin(UserProfileDTO userProfileDTO)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                // User not authenticated
                return Unauthorized();
            }

            var userProfile = await _adminService.UpdateAdmin(userProfileDTO, userId);

            if (userProfile == null)
            {
                // User not found
                return NotFound();
            }

            return Ok(userProfile);
        }

        [HttpDelete("admin-delete/{id}")]
        public async Task<ActionResult> DeleteAdmin(string id)
        {
            var deletedHero = await _adminService.DeleteAdmin(id);
            return Ok(deletedHero);
        }
    }
}
