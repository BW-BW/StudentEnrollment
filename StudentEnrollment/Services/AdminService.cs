using Microsoft.AspNetCore.Identity;
using static StudentEnrollment.Responses.ServiceResponses;
using StudentEnrollment.Data;
using StudentEnrollment.DTOs;
using StudentEnrollment.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace StudentEnrollment.Services
{
    public class AdminService : IAdminService
    {
        private readonly UserManager<UserModel> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly AppDbContext _context;

        public AdminService(UserManager<UserModel> userManager, AppDbContext appDbContext, RoleManager<IdentityRole> roleManager)
        {
            this._userManager = userManager;
            _context = appDbContext;
            this._roleManager = roleManager;
        }

        public async Task<GeneralResponse> CreateAdmin(UserModelDTO userModelDTO)
        {
            if (userModelDTO is null)
                return new GeneralResponse(false, "Model is empty");
            var newUser = new UserModel
            {
                FirstName = userModelDTO.FirstName,
                LastName = userModelDTO.LastName,
                Address = userModelDTO.Address,
                Email = userModelDTO.Email,
                PasswordHash = userModelDTO.Password,
                UserName = userModelDTO.Email
            };
            var user = await _userManager.FindByEmailAsync(newUser.Email);
            if (user is not null)
                return new GeneralResponse(false, "User registered already");

            var createUser = await _userManager.CreateAsync(newUser!, userModelDTO.Password);
            if (!createUser.Succeeded)
                return new GeneralResponse(false, "Error occured.. please try again");

            var checkAdmin = await _roleManager.FindByNameAsync("Admin");
            if (checkAdmin is null)
            {
                await _roleManager.CreateAsync(new IdentityRole() { Name = "Admin" });
            }

            await _userManager.AddToRoleAsync(newUser, "Admin");
            return new GeneralResponse(true, "Account Created");
        }

        public async Task<List<UserProfileDTO>> GetAllAdmin()
        {
            var adminRole = await _roleManager.FindByNameAsync("Admin");

            var users = await _userManager.GetUsersInRoleAsync(adminRole.Name);

            // Create a list of UserDto instances by mapping each user
            var userDtos = new List<UserProfileDTO>();
            foreach (var user in users)
            {
                var userDto = new UserProfileDTO
                {
                    UserId = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Address = user.Address,
                    Email = user.Email
                };

                userDtos.Add(userDto);
            }

            return userDtos;
        }

        public async Task<ReadResponse> GetAdmin(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            var empty = new UserProfileDTO { };

            if (user == null)
                return new ReadResponse(false, "User not found", empty);

            bool isInAdminRole = await _userManager.IsInRoleAsync(user, "Admin");
            if (!isInAdminRole)
                return new ReadResponse(false, "User is not an admin", empty);

            var userDto = new UserProfileDTO
            {
                UserId = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Address = user.Address,
                Email = user.Email
            };

            return new ReadResponse(true, "User found", userDto); ;
        }

        public async Task<UpdateResponse> UpdateAdmin(UserProfileDTO userProfileDTO, string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                var empty = new UserProfileDTO { };

                return new UpdateResponse(false, "User not found", empty);
            }

            user.FirstName = userProfileDTO.FirstName;
            user.LastName = userProfileDTO.LastName;
            user.Address = userProfileDTO.Address;
            user.Email = userProfileDTO.Email;

            var updatedProfileTask = GetAdmin(userId);
            var updatedProfile = await updatedProfileTask;
            var response = updatedProfile.userProfileDTO;

            await _context.SaveChangesAsync();

            return new UpdateResponse(true, "Profile Successfully Updated", response);

        }

        public async Task<DeleteResponse> DeleteAdmin(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            var empty = new UserProfileDTO { };

            if (user == null)
                return new DeleteResponse(false, "User not found", empty);

            bool isInAdminRole = await _userManager.IsInRoleAsync(user, "Admin");
            if (!isInAdminRole)
                return new DeleteResponse(false, "User is not an admin", empty);

            var userDto = new UserProfileDTO
            {
                UserId = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Address = user.Address,
                Email = user.Email
            };

            await _userManager.DeleteAsync(user);

            return new DeleteResponse(true, "This user below is now deleted", userDto); ;
        }

        public async Task<GeneralResponse> AddCourse(CourseDTO courseDTO)
        {
            var course = new CourseModel
            {
                Name = courseDTO.Name,
                Duration = courseDTO.Duration,
                Price = courseDTO.Price,
            };
            _context.CourseModels.Add(course);

            await _context.SaveChangesAsync();

            return new GeneralResponse(true, "Course Created");
        }

        public async Task<List<CourseDTO>> GetAllCourse()
        {
            var courses = await _context.CourseModels.ToListAsync();

            var courseDTOs = new List<CourseDTO>();
            foreach (var course in courses)
            {
                var courseDTO = new CourseDTO
                {
                    Name = course.Name,
                    Duration = course.Duration,
                    Price = course.Price,
                };

                courseDTOs.Add(courseDTO);
            }

            return courseDTOs;
        }

        public async Task<CourseResponse> GetCourse(int id)
        {
            var course = await _context.CourseModels.FindAsync(id);

            if (course == null)
            {
                var empty = new CourseDTO { };

                return new CourseResponse(false, "Course not found", empty);
            }

            var courseDTO = new CourseDTO
            {
                Name = course.Name,
                Duration = course.Duration,
                Price = course.Price,
            };

            return new CourseResponse(true, "Course found", courseDTO);
        }

        public async Task<CourseResponse> UpdateCourse(CourseDTO courseDTO, int id)
        {
            var updatecourse = await _context.CourseModels.FindAsync(id);

            if (updatecourse == null)
            {
                var empty = new CourseDTO { };

                return new CourseResponse(false, "Course not found", empty);
            }

            updatecourse.Name = courseDTO.Name;
            updatecourse.Duration = courseDTO.Duration;
            updatecourse.Price = courseDTO.Price;

            await _context.SaveChangesAsync();

            var getTask = GetCourse(id);
            var response = await getTask;
            var updatedDTO = response.courseDTO;

            return new CourseResponse(true, "Course updated", updatedDTO);
        }

        public async Task<CourseResponse> DeleteCourse(int id)
        {
            var deletecourse = await _context.CourseModels.FindAsync(id);
            if (deletecourse == null)
            {
                var empty = new CourseDTO { };

                return new CourseResponse(false, "Course not found", empty);
            }

            _context.CourseModels.Remove(deletecourse);
            await _context.SaveChangesAsync();

            var courseDTO = new CourseDTO
            {
                Name = deletecourse.Name,
                Duration = deletecourse.Duration,
                Price = deletecourse.Price,
            };

            return new CourseResponse(true, "Course not found", courseDTO); ;
        }

        public async Task<List<StudentEnrollmentDTO>> GetAllEnrollment(bool pending)
        {
            var myEnrollment = await _context.EnrollmentModels
                .Include(em => em.CourseModel) // navigation property "CourseModel" is defined
                .Include(em => em.UserModel)   // navigation property "UserModel" is defined
                .ToListAsync();

            var myEnrollmentPending = await _context.EnrollmentModels
                .Include(em => em.CourseModel) // navigation property "CourseModel" is defined
                .Include(em => em.UserModel)   // navigation property "UserModel" is defined
                .Where(em => em.Status == "Pending")
                .ToListAsync();

            var enrollmentList = new List<StudentEnrollmentDTO>();

            if (pending)
            {
                foreach (var enrollment in myEnrollmentPending)
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

        public async Task<GeneralResponse> ApproveRejectEnrollment(int enrollmentId, bool decision)
        {
            var theEnrollment = await _context.EnrollmentModels.FindAsync(enrollmentId);
            if (theEnrollment == null)
            {
                return new GeneralResponse(false, "Enrollment Not Found");
            }

            if (theEnrollment.Status != "Pending")
            {
                return new GeneralResponse(false, "Cannot Approve or Reject this one");
            }

            //check if the enrollment is pending
            /*var checkEnrollment = await _context.EnrollmentModels
                .Where(em => em.Status == "Pending" && em.CourseId == courseId && em.StudentId == userId)
                .FirstOrDefaultAsync();
            if (checkEnrollment == null)
            {
                return new GeneralResponse(false, "Cannot Approve or Reject this one");
            }*/

            if (decision == true)
            {
                theEnrollment.Status = "Active";

                await _context.SaveChangesAsync();
                return new GeneralResponse(true, "Successfully Approved");
            } else
            {
                theEnrollment.Status = "Rejected";

                await _context.SaveChangesAsync();
                return new GeneralResponse(true, "Successfully Rejected");
            }
        }
    }
}
