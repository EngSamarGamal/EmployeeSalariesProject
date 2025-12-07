using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FirebaseAdmin.Messaging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace App.Application.Helper.Notifications
{
    public class FirebasePushNotification : IFirebasePushNotification
    {
        private readonly IConfiguration _Config;
        private readonly ILogger<FirebasePushNotification> _Logger;


        public FirebasePushNotification(IConfiguration configuration, ILogger<FirebasePushNotification> logger)
        {
            _Config = configuration;
            _Logger = logger;
        }



        public async Task PushNotification(string body,
                                           string name,
                                           string title,
                                           List<string> deviceIds,
                                           string singleDevice,
                                           string imageUrl,
                                           Dictionary<string, string> data,
                                           bool broadcast = false,
                                           bool isSingleNotification = false,
                                           bool isSilentNotification = false)
        {
            try
            {
                if (broadcast)
                {
                    await SendBroadcastNotification(data, title, body);
                    return;
                }

                if (isSilentNotification)
                {
                    await SendSilentNotification(deviceIds, data);
                    return;
                }

                if (isSingleNotification)
                {
                    await SendSingleNotification(singleDevice, data, title, body);
                    return;
                }
                await SendNotification(deviceIds, data, title, body);
            }

            catch (FirebaseMessagingException fireBaseEx)
            {
                _Logger.LogError(fireBaseEx, "Error in Sending Notifications");
            }

            catch (Exception ex)
            {
                _Logger.LogError(ex, "Error in Sending Notifications");
            }

        }
        private async Task SendNotification(List<string> deviceIds,
                                            Dictionary<string, string> data,
                                            string title,
                                            string body)
        {
            if (deviceIds == null || !deviceIds.Any(x => !string.IsNullOrWhiteSpace(x)))
            {
                return;
            }

            deviceIds = deviceIds.Where(x => !string.IsNullOrWhiteSpace(x)).ToList();


            var message = new MulticastMessage()
            {
                Tokens = deviceIds,
                Notification = new Notification
                {
                    Title = title,
                    Body = body
                },
                Data = data,
                Android = new AndroidConfig
                {
                    Notification = new AndroidNotification
                    {
                        ChannelId = "1",
                        ClickAction = "FLUTTER_NOTIFICATION_CLICK",
                        DefaultSound = true,
                        Priority = NotificationPriority.HIGH,
                        EventTimestamp = DateTime.UtcNow
                    }
                }
            };

            var result = await FirebaseMessaging.DefaultInstance.SendEachForMulticastAsync(message);
            _Logger.LogInformation($"Firebase Sucess Count:{result.SuccessCount}, Firebase Failed Count {result.FailureCount} ");
        }

        private async Task SendSingleNotification(string deviceId,
                                                  Dictionary<string, string> data,
                                                  string title,
                                                  string body)
        {
            if (string.IsNullOrWhiteSpace(deviceId))
                return;


            var message = new Message()
            {
                Token = deviceId,
                Notification = new Notification
                {
                    Title = title,
                    Body = body
                },
                Data = data,
                Android = new AndroidConfig
                {
                    Notification = new AndroidNotification
                    {
                        ChannelId = "1",
                        ClickAction = "FLUTTER_NOTIFICATION_CLICK",
                        DefaultSound = true,
                        Priority = NotificationPriority.HIGH,
                        EventTimestamp = DateTime.UtcNow
                    }
                }
            };

            var result = await FirebaseMessaging.DefaultInstance.SendAsync(message);
            _Logger.LogInformation($"Firebase Failed Count {result} ");
        }
        private async Task SendBroadcastNotification(Dictionary<string, string> data, string title, string body)
        {
            var topic = _Config.GetValue<string>("firebaseTopic");
            var broadCastMessage = new Message()
            {
                Topic = topic,
                Notification = new Notification
                {
                    Title = title,
                    Body = body
                },
                Data = data,
                Android = new AndroidConfig
                {
                    Notification = new AndroidNotification
                    {
                        ChannelId = "1",
                        ClickAction = "FLUTTER_NOTIFICATION_CLICK",
                        DefaultSound = true,
                        Priority = NotificationPriority.HIGH,
                        EventTimestamp = DateTime.UtcNow
                    }
                }
            };
            var braodCastResult = await FirebaseMessaging.DefaultInstance.SendAsync(broadCastMessage);
            _Logger.LogInformation($"Firebase Result is {braodCastResult}");
        }
        private async Task SendSilentNotification(List<string> deviceIds, Dictionary<string, string> data)
        {
            if (deviceIds == null || !deviceIds.Any(x => !string.IsNullOrWhiteSpace(x)))
            {
                return;
            }

            deviceIds = deviceIds.Where(x => !string.IsNullOrWhiteSpace(x)).ToList();


            var silentMessage = new MulticastMessage()
            {
                Tokens = deviceIds,
                Data = data,
                Android = new AndroidConfig
                {
                    Notification = new AndroidNotification
                    {
                        ChannelId = "1",
                        ClickAction = "FLUTTER_NOTIFICATION_CLICK",
                        DefaultSound = true,
                        Priority = NotificationPriority.HIGH,
                        EventTimestamp = DateTime.UtcNow
                    }
                }
            };

            var silentResult = await FirebaseMessaging.DefaultInstance.SendEachForMulticastAsync(silentMessage);
            _Logger.LogInformation($"Firebase Sucess Count:{silentResult.SuccessCount} ,Firebase Failed Count {silentResult.FailureCount} ");
        }
    }
}
