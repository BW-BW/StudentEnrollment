using System.ComponentModel.DataAnnotations;

namespace StudentEnrollment.DTOs
{
    public class CourseDTO
    {
        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string Duration { get; set; } = string.Empty;

        [Required]
        public string Price { get; set; } = string.Empty;
    }
}
