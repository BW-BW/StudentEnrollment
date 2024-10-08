﻿using Microsoft.AspNetCore.Identity;

namespace StudentEnrollment.Data
{
    public class UserModel : IdentityUser
    {
        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public string Address { get; set; } = string.Empty;

        public ICollection<EnrollmentModel> EnrollmentModels { get; set; }
    }
}
