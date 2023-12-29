using StudentEnrollment.DTOs;

namespace StudentEnrollment.Responses
{
    public class ServiceResponses
    {
        public record class GeneralResponse(bool Flag, string Message);
        public record class LoginResponse(bool Flag, string Token, string Message);
        public record class UpdateResponse(bool Flag, string Message, UserProfileDTO userProfileDTO);
    }
}