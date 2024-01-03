using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudentEnrollment.DTOs;
using StudentEnrollment.Interface;
using StudentEnrollment.Services;
using System.Security.Claims;
using static StudentEnrollment.Responses.ServiceResponses;

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

        [HttpPut("admin-update")]
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
            var deletedAdmin = await _adminService.DeleteAdmin(id);
            return Ok(deletedAdmin);
        }

        [HttpPost("course-add")]
        public async Task<IActionResult> AddCourse(CourseDTO courseDTO)
        {
            var response = await _adminService.AddCourse(courseDTO);
            return Ok(response);
        }

        [HttpGet("course-get")]
        public async Task<IActionResult> GetAllCourse()
        {
            var response = await _adminService.GetAllCourse();
            return Ok(response);
        }

        [HttpGet("course-get/{id}")]
        public async Task<ActionResult> GetCourse(int id)
        {
            var response = await _adminService.GetCourse(id);

            return Ok(response);
        }

        [HttpPut("course-update")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateCourse(CourseDTO courseDTO, int id)
        {
            var updatedCourse = await _adminService.UpdateCourse(courseDTO, id);
            if (updatedCourse == null)
                return NotFound("Hero not found");

            return Ok(updatedCourse);
        }

        [HttpDelete("course-delete/{id}")]
        public async Task<ActionResult> DeleteCourse(int id)
        {
            var deletedCourse = await _adminService.DeleteCourse(id);
            return Ok(deletedCourse);
        }

        [HttpGet("enrollment-get")]
        public async Task<ActionResult> GetAllEnrollment(bool pending)
        {
            var allEnrollment = await _adminService.GetAllEnrollment(pending);
            return Ok(allEnrollment);
        }

        [HttpPost("enrollment-approve-reject")]
        public async Task<ActionResult> ApproveRejectEnrollment(int enrollmentId, bool decision)
        {
            var result = await _adminService.ApproveRejectEnrollment(enrollmentId, decision);
            return Ok(result);
        }
    }
}
