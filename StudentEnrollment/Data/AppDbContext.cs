using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace StudentEnrollment.Data
{
    public class AppDbContext : IdentityDbContext<UserModel>

    {
        public DbSet<CourseModel> CourseModels { get; set; }
        public DbSet<EnrollmentModel> EnrollmentModels { get; set; }

        public AppDbContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {


            modelBuilder.Entity<EnrollmentModel>()
                .HasKey(cs => new { cs.CourseId, cs.StudentId });

            modelBuilder.Entity<EnrollmentModel>()
                .HasOne(cs => cs.CourseModel)
                .WithMany(cm => cm.EnrollmentModels)
                .HasForeignKey(cs => cs.CourseId);

            modelBuilder.Entity<EnrollmentModel>()
                .HasOne(cs => cs.UserModel)
                .WithMany(s => s.EnrollmentModels)
                .HasForeignKey(cs => cs.StudentId);

            base.OnModelCreating(modelBuilder);
        }
    }
}
