using static StudentEnrollment.Responses.ServiceResponses;
using StudentEnrollment.DTOs;

namespace StudentEnrollment.Interface
{
    public interface IAdminService
    {
        Task<GeneralResponse> CreateAdminAsync(UserModelDTO userModelDTO);
        Task<List<UserProfileDTO>> GetAllAdmin();
        Task<ReadResponse> GetAdmin(string id);
        Task<UpdateResponse> UpdateAdmin(UserProfileDTO userProfileDTO, string userId);
        Task<DeleteResponse> DeleteAdmin(string id);

        Task<GeneralResponse> AddCourse(CourseDTO courseDTO);
        Task<List<CourseDTO>> GetAllCourse();
        Task<CourseResponse> GetCourse(int id);
        Task<CourseResponse> UpdateCourse(CourseDTO courseDTO, int id);
        Task<CourseResponse> DeleteCourse(int id);
        Task<List<StudentEnrollmentDTO>> GetAllEnrollment(bool pending);
        Task<GeneralResponse> ApproveRejectEnrollment(int enrollmentId, bool decision);
    }
}
