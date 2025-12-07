using System;

namespace App.Domain.Models.Base
{
        public class BaseEntity : IBaseEntity
        {
            public Guid Id { get; set; } = Guid.NewGuid();

            public string? CreatedBy { get; set; }
            public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
            public string? UpdatedBy { get; set; }
            public DateTime? UpdatedOn { get; set; }

            public bool IsDeleted { get; set; } = false;

            public void SoftDelete(string? updatedBy = "System")
            {
                IsDeleted = true;
                UpdatedOn = DateTime.UtcNow;
                UpdatedBy = updatedBy;
            }

            public void Restore(string? updatedBy = "System")
            {
                IsDeleted = false;
                UpdatedOn = DateTime.UtcNow;
                UpdatedBy = updatedBy;
            }
        }
    }