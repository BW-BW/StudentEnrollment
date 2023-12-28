using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace StudentEnrollment.Data
{
    public class AppDbContext : IdentityDbContext<UserModel>

    {
        public AppDbContext(DbContextOptions options) : base(options)
        {

        }
    }
}
