using StudentEnrollment.DTOs;
using static StudentEnrollment.Responses.ServiceResponses;

namespace StudentEnrollment.Interface
{
    public interface IStudentService
    {
        Task<UserProfileDTO> GetProfile(string userId);

        Task<UpdateResponse> UpdateProfile(UserProfileDTO userProfileDTO, string userId);
    }
}
