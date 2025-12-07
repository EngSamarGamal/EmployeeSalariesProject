namespace App.Domain.Models.Identity
{
    using Microsoft.AspNetCore.Identity;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;

    public class ApplicationUser : IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }

        [NotMapped]
        public string? FullName => $"{FirstName} {LastName}";
        public string? ProfilePicture { get; set; }

        public List<RefreshToken>? RefreshTokens { get; set; }

        public bool IsActive { get; set; } = true;
        public bool IsDeleted { get; set; }
        public bool IsVerified { get; set; } = false;

        public int AccountType { get; set; } // 1 = SuperAdmin, 2 = Admin, 3 = Guest

        public string? CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public bool IsOwner { get; set; } = false;

        public void SetUserIsDeleted()
        {
            IsDeleted = true;
            UpdatedOn = DateTime.UtcNow;
        }

        public static ApplicationUser Instance() => new ApplicationUser();
    }
}
