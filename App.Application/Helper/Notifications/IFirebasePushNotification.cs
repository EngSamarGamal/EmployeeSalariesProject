using System.Collections.Generic;
using System.Threading.Tasks;

namespace App.Application.Helper.Notifications
{
    public interface IFirebasePushNotification
    {
        Task PushNotification(string body,
                              string name,
                              string title,
                              List<string> deviceIds,
                              string singleDevice,
                              string imageUrl,
                              Dictionary<string, string> data,
                              bool broadcast = false,
                              bool isSingleNotification = false,
                              bool isSilentNotification = false);
    }
}
