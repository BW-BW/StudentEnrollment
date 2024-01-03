using StudentEnrollment.DTOs;
using static StudentEnrollment.Responses.ServiceResponses;

namespace StudentEnrollment.Interface
{
    public interface IAccountService
    {
        Task<GeneralResponse> CreateAccount(UserModelDTO userModelDTO);

        Task<LoginResponse> LoginAccount(LoginDTO loginDTO);

        Task<GeneralResponse> ChangePassword(string email, string oldPassword, string newPassword);

        Task<UserProfileDTO> GetProfile(string userId);
    }
}
