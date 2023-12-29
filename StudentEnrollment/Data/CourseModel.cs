namespace StudentEnrollment.Data
{
    public class CourseModel
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Duration { get; set; }
        public required string Price { get; set; }

        public ICollection<EnrollmentModel> EnrollmentModels { get; set; }
    }
}
