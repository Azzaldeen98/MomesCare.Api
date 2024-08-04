namespace MomesCare.Api.Services.SubServices
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    public interface INotificationService
    {
        Task SendNotificationsAsync(Action callBack);
    }

    public class NotificationService : INotificationService
    {
        Action callBack;

        public async Task SendNotificationsAsync(Action callBack)
        {
            this.callBack = callBack;

            await Task.Run(() =>{

                Thread.Sleep(1000);
                this.callBack?.Invoke();

                Console.WriteLine("Notifications sent to all users.");
            });
        }
    }

}
