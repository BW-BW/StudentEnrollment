namespace StudentEnrollment.Data
{
    public class EnrollmentModel
    {
        public int Id { get; set; }
        public required string Status { get; set; }

        public int CourseId { get; set; }
        public CourseModel CourseModel { get; set; }

        public string StudentId { get; set; }
        public UserModel UserModel { get; set; }
    }
}
