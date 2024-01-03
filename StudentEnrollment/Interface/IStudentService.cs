using StudentEnrollment.Data;
using StudentEnrollment.DTOs;
using static StudentEnrollment.Responses.ServiceResponses;

namespace StudentEnrollment.Interface
{
    public interface IStudentService
    {
        Task<UserProfileDTO> GetProfile(string userId);

        Task<UpdateResponse> UpdateProfile(UserProfileDTO userProfileDTO, string userId);

        Task<List<CourseModel>> GetAllCourse();
        Task<GeneralResponse> Enroll(string userId, int courseId);
        Task<List<StudentEnrollmentDTO>> GetMyEnrollment(string userId);
        Task<GeneralResponse> WithdrawEnrollment(string userId, int courseId);
    }
}
