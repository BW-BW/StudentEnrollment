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

            /*var userProfile = new UserProfileDTO
            {
                FirstName = userProfileDTO.FirstName,
                LastName = userProfileDTO.LastName,
                Address = userProfileDTO.Address,
                Email = userProfileDTO.Email,
            };*/

            var updatedProfileTask = GetProfile(userId);
            var updatedProfile = await updatedProfileTask;

            //var updatedProfile = GetProfile(userId);
            /*{
                FirstName = userProfileDTO.FirstName,
                LastName = userProfileDTO.LastName,
                Address = userProfileDTO.Address,
                Email = userProfileDTO.Email,
            };*/

            await _context.SaveChangesAsync();

            return new UpdateResponse(true, "Profile Successfully Updated", updatedProfile);

        }
    }
}
