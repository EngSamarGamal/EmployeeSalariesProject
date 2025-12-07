using App.Common.Dtos.Base;

namespace App.Common.Dtos.Notification
{
    public class GetNotificationDto : BaseDto
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? ImageURL { get; set; }
        public int Type { get; set; }
        public string? RecordId { get; set; }
        public bool IsRead { get; set; }
    }
}
