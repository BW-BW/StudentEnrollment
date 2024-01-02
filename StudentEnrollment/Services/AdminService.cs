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
    }
}
