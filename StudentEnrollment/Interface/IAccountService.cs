using StudentEnrollment.DTOs;
using static StudentEnrollment.Responses.ServiceResponses;

namespace StudentEnrollment.Interface
{
    public interface IAccountService
    {
        Task<GeneralResponse> CreateAccount(UserModelDTO userModelDTO);

        Task<LoginResponse> LoginAccount(LoginDTO loginDTO);

        Task<GeneralResponse> ChangePassword(UserModelDTO userModelDTO, string newPassword);

        Task<UserProfileDTO> GetProfile(string userId);
    }
}
