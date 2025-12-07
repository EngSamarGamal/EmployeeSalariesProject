using System;

namespace App.Domain.Models.Base
{
    public interface IBaseEntity
    {
        public Guid Id { get; set; }

        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }

        public bool IsDeleted { get; set; }
    }
}
