using StudentEnrollment.DTOs;
using static StudentEnrollment.Responses.ServiceResponses;

namespace StudentEnrollment.Interface
{
    public interface IAccountService
    {
        Task<GeneralResponse> CreateAccount(UserModelDTO userModelDTO);

        Task<LoginResponse> LoginAccount(UserModelDTO userModelDTO);
    }
}
