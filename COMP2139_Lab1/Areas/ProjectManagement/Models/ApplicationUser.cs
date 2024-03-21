﻿using Microsoft.AspNetCore.Identity;

namespace COMP2139_Lab1.Areas.ProjectManagement.Models
{
    public class ApplicationUser: IdentityUser
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public int usernameChangeLimit { get; set; } = 10;
        public byte[]? ProfilePicture { get; set; }
    }
}
