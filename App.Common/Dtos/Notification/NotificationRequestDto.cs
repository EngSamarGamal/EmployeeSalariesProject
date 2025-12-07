using App.Common.Dtos.Base;
using Microsoft.AspNetCore.Http;

namespace App.Common.Dtos.Notification
{
    public class NotificationRequestDto : BaseDto
    {
        public string UserId { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? TitleAr { get; set; }
        public string? DescriptionAr { get; set; }
        public IFormFile? ImageURL { get; set; }
        public string Type { get; set; }
        public string? RecordId { get; set; }
        public bool IsRead { get; set; }
    }
}
