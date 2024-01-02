using StudentEnrollment.Data;
using static StudentEnrollment.Responses.ServiceResponses;
using StudentEnrollment.DTOs;

namespace StudentEnrollment.Interface
{
    public interface IAdminService
    {
        Task<GeneralResponse> CreateAdmin(UserModelDTO userModelDTO);
        Task<List<UserProfileDTO>> GetAllAdmin();
        Task<ReadResponse> GetAdmin(string id);
        Task<UpdateResponse> UpdateAdmin(UserProfileDTO userProfileDTO, string userId);
        Task<DeleteResponse> DeleteAdmin(string id);
    }
}
