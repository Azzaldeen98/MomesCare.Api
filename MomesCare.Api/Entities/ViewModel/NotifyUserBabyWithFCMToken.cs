

using MomesCare.Api.Entities.ViewModel.DailyCareTimes;

namespace MomesCare.Api.Entities.ViewModel
{
    public class NotifyUserBabyWithFCMToken
    {
        public string fcmToken { get; set; }
        public MomesCare.Api.Entities.Models.Baby baby { get; set; }
        public string userId { get; set; }
 
    }


}
