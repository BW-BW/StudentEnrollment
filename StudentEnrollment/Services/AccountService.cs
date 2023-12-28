using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using StudentEnrollment.Data;
using StudentEnrollment.DTOs;
using StudentEnrollment.Interface;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using static StudentEnrollment.Responses.ServiceResponses;

namespace StudentEnrollment.Services
{
    public class AccountService(
        UserManager<UserModel> userManager,
        RoleManager<IdentityRole> roleManager,
        IConfiguration config) : IAccountService
    {

        public async Task<GeneralResponse> CreateAccount(UserModelDTO userModelDTO)
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
            var user = await userManager.FindByEmailAsync(newUser.Email);
            if (user is not null) 
                return new GeneralResponse(false, "User registered already");

            var createUser = await userManager.CreateAsync(newUser!, userModelDTO.Password);
            if (!createUser.Succeeded) 
                return new GeneralResponse(false, "Error occured.. please try again");

            //Assign Default Role : Admin to first registrar; rest is user
            var checkAdmin = await roleManager.FindByNameAsync("Admin");
            if (checkAdmin is null)
            {
                await roleManager.CreateAsync(new IdentityRole() { Name = "Admin" });
                await userManager.AddToRoleAsync(newUser, "Admin");
                return new GeneralResponse(true, "Account Created");
            }
            else
            {
                var checkUser = await roleManager.FindByNameAsync("User");
                if (checkUser is null)
                    await roleManager.CreateAsync(new IdentityRole() { Name = "User" });

                await userManager.AddToRoleAsync(newUser, "User");
                return new GeneralResponse(true, "Account Created");
            }
        }

        public async Task<LoginResponse> LoginAccount(UserModelDTO loginDTO)
        {
            if (loginDTO == null)
                return new LoginResponse(false, null!, "Login container is empty");

            var getUser = await userManager.FindByEmailAsync(loginDTO.Email);
            if (getUser is null)
                return new LoginResponse(false, null!, "User not found");

            bool checkUserPasswords = await userManager.CheckPasswordAsync(getUser, loginDTO.Password);
            if (!checkUserPasswords)
                return new LoginResponse(false, null!, "Invalid email/password");

            var getUserRole = await userManager.GetRolesAsync(getUser);
            var userSession = new UserSession(getUser.Id, getUser.FirstName, getUser.Email, getUserRole.First());
            string token = GenerateToken(userSession);
            
            return new LoginResponse(true, token!, "Login completed");
        }

        private string GenerateToken(UserSession user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]!));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var userClaims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role)
            };
            var token = new JwtSecurityToken(
                issuer: config["Jwt:Issuer"],
                audience: config["Jwt:Audience"],
                claims: userClaims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: credentials
                );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<GeneralResponse> ChangePassword(UserModelDTO userModelDTO, string newPassword)
        {
            if (userModelDTO == null || string.IsNullOrWhiteSpace(newPassword))
            {
                return new GeneralResponse(false, "Invalid input parameters");
            }

            var user = await userManager.FindByEmailAsync(userModelDTO.Email);

            if (user == null)
            {
                return new GeneralResponse(false, "User not found");
            }

            // Use userManager.ChangePasswordAsync to change the password
            var changePasswordResult = await userManager.ChangePasswordAsync(user, userModelDTO.Password, newPassword);

            if (!changePasswordResult.Succeeded)
            {
                // Password change failed, handle the errors appropriately
                var errorMessage = string.Join(", ", changePasswordResult.Errors.Select(error => error.Description));
                return new GeneralResponse(false, errorMessage);
            }

            // Password change successful
            return new GeneralResponse(true, "Password changed successfully");
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
    }
}
