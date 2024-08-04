using Firebase.Auth;
using Firebase.Database.Query;
using FirebaseAdmin;
using FirebaseAdmin.Auth;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;
using MomesCare.Api.ApiClient;
using MomesCare.Api.Helpers;
using MomesCare.Api.Helpers.Enums;
using MomesCare.Api.ApiClient.Entitis;
using Notification = MomesCare.Api.ApiClient.Entitis.Notification;

namespace MomesCare.Api.Services.SubServices
{
    public class ServiceMessages
    {


        public ServiceMessages() { }


        public async Task<string> sendMessageAsync(string title, string body, string topic, string token, string tag = "")
        {
                var message = new Message()
            {

                Notification = new FirebaseAdmin.Messaging.Notification
                {
                    Title = title,
                    Body = body,
                },
                Data = new Dictionary<string, string>
                {
                    ["topic"] = topic,
                    ["tag"] = tag,
                },
                Token = token,
                
            };

            var messaging = FirebaseMessaging.DefaultInstance;
            var result = await messaging.SendAsync(message);
            if (!string.IsNullOrEmpty(result))
            {
                return "Message sent successfully!";
            }
            else
            {
                return "Error sending the message.";
            }

        }


        public async Task StoreNotificationAsync(IFirebaseClientFactory firebaseClientFactory, Notification notification)
        {
            notification.date = Helper.GetCurrentDateAsString();
            notification.status = (int)NotificationStatus.NEW;

            Console.WriteLine(notification);

            //var firebaseClient = firebaseClientFactory.GetClient();
            //var notificationsRef = firebaseClient.Child(DatabaseNodes.Notifictions);

            //try
            //{
            //    var pushResponse = await notificationsRef.PostAsync<Models.Notification>(notification);
            //    Console.WriteLine("Data added successfully. Key: " + pushResponse.Key);
            //}
            //catch (Exception error)
            //{
            //    Console.Error.WriteLine("Error adding data: " + error);
            //}
        }


    }
}
