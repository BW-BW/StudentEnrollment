using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using StudentEnrollment.Data;
using StudentEnrollment.DTOs;
using StudentEnrollment.Interface;
using System.Net;
using static StudentEnrollment.Responses.ServiceResponses;

namespace StudentEnrollment.Services
{
    public class StudentService : IStudentService
    {
        private readonly UserManager<UserModel> userManager;
        private readonly AppDbContext _context;

        public StudentService(UserManager<UserModel> userManager, AppDbContext appDbContext) 
        { 
            this.userManager = userManager;
            _context = appDbContext;
        }

        public async Task<UserProfileDTO> GetProfile(string userId)
        {
            var user = await userManager.FindByIdAsync(userId);

            if (user == null)
            {
                // User not found
                return null;
            }

            // Map the user properties to a DTO or create a UserProfileDTO class
            var userProfile = new UserProfileDTO
            {
                UserId = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Address = user.Address,
                Email = user.Email,
                // Add other properties as needed
            };

            return userProfile;
        }

        public async Task<UpdateResponse> UpdateProfile(UserProfileDTO userProfileDTO, string userId)
        {
            var user = await userManager.FindByIdAsync(userId);

            if (user == null)
            {
                // User not found
                return null;
            }

            user.FirstName = userProfileDTO.FirstName;
            user.LastName = userProfileDTO.LastName;
            user.Address = userProfileDTO.Address;
            user.Email = userProfileDTO.Email;

            var updatedProfileTask = GetProfile(userId);
            var updatedProfile = await updatedProfileTask;

            await _context.SaveChangesAsync();

            return new UpdateResponse(true, "Profile Successfully Updated", updatedProfile);

        }

        public async Task<List<CourseModel>> GetAllCourse()
        {
            var courses = await _context.CourseModels.ToListAsync();
            return courses;
        }

        public async Task<GeneralResponse> Enroll(string userId, int courseId)
        {
            var course = await _context.CourseModels.FindAsync(courseId);
            if (course == null)
            {
                return new GeneralResponse(false, "Course not found");
            }

            var checkDuplicate = await _context.EnrollmentModels
                .Where(em => em.CourseId == courseId && em.StudentId == userId)
                .FirstOrDefaultAsync();
            if (checkDuplicate != null)
            {
                return new GeneralResponse(false, "You are already enrolled");
            }

            //if already enrolled

            var enrollment = new EnrollmentModel
            {
                Status = "Pending",
                CourseId = courseId,
                StudentId = userId,
            };
            _context.EnrollmentModels.Add(enrollment);

            await _context.SaveChangesAsync();

            return new GeneralResponse(true, "Successfully Enrolled");
        }

        public async Task<List<StudentEnrollmentDTO>> GetMyEnrollment(string userId)
        {
            var myEnrollment = await _context.EnrollmentModels
                .Include(em => em.CourseModel) // navigation property "CourseModel" is defined
                .Include(em => em.UserModel)   // navigation property "UserModel" is defined
                .Where(em => em.StudentId == userId)
                .ToListAsync();

            var enrollmentList = new List<StudentEnrollmentDTO>();
            foreach (var enrollment in myEnrollment)
            {
                var StudentEnrollmentDTO = new StudentEnrollmentDTO
                {
                    FirstName = enrollment.UserModel.FirstName,
                    LastName = enrollment.UserModel.LastName,
                    CourseId = enrollment.CourseId,
                    CourseName = enrollment.CourseModel.Name,
                    Status = enrollment.Status,
                    Duration = enrollment.CourseModel.Duration,
                    Price = enrollment.CourseModel.Price,
                };

                enrollmentList.Add(StudentEnrollmentDTO);
            }

            return enrollmentList;
        }

        public async Task<GeneralResponse> WithdrawEnrollment(string userId, int courseId)
        {
            var course = await _context.CourseModels.FindAsync(courseId);
            if (course == null)
            {
                return new GeneralResponse(false, "Course not found");
            }

            var checkEnrollment = await _context.EnrollmentModels
                .Where(em => em.CourseId == courseId && em.StudentId == userId && em.Status != "Withdrawn")
                .FirstOrDefaultAsync();
            if (checkEnrollment == null)
            {
                return new GeneralResponse(false, "You are not enrolled to this course");
            }

            checkEnrollment.Status = "Withdrawn";

            await _context.SaveChangesAsync();

            return new GeneralResponse(true, "Successfully Withdrawn");
        }
    }
}
