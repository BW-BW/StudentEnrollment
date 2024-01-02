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
    public class StudentController : ControllerBase
    {
        private readonly IStudentService _studentService;

        public StudentController(IStudentService studentService)
        {
            _studentService = studentService;
        }

        [HttpGet("profile")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> GetProfile()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                // User not authenticated
                return Unauthorized();
            }

            var userProfile = await _studentService.GetProfile(userId);

            if (userProfile == null)
            {
                // User not found
                return NotFound();
            }

            return Ok(userProfile);
        }

        [HttpPost("update-profile")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> UpdateProfile(UserProfileDTO userProfileDTO)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                // User not authenticated
                return Unauthorized();
            }

            var userProfile = await _studentService.UpdateProfile(userProfileDTO, userId);

            if (userProfile == null)
            {
                // User not found
                return NotFound();
            }

            return Ok(userProfile);
        }

        [HttpGet("course-get")]
        public async Task<IActionResult> GetAllCourse()
        {
            var response = await _studentService.GetAllCourse();
            return Ok(response);
        }

        [HttpPost("enroll")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> Enroll(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                // User not authenticated
                return Unauthorized();
            }

            var enroll = await _studentService.Enroll(userId, id);

            return Ok(enroll);
        }

        [HttpGet("myenrollment-get")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> GetMyEnrollment()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                // User not authenticated
                return Unauthorized();
            }
            var response = await _studentService.GetMyEnrollment(userId);
            return Ok(response);
        }

        [HttpPost("withdraw")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> WithdrawEnrollment(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                // User not authenticated
                return Unauthorized();
            }

            var enroll = await _studentService.WithdrawEnrollment(userId, id);

            return Ok(enroll);
        }
    }
}
