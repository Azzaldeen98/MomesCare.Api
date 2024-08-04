using MomesCare.Api.ApiClient;
using MomesCare.Api.Services;
using MomesCare.Api.Services.BackgroundServices.NewFolder;
using MomesCare.Api.Services.Static;
using MomesCare.Api.Services.SubServices;

namespace MomesCare.Api.Injection_Depandinces
{
    public static class InstallServices
    {
        public static void InstallApiClientServices(this IServiceCollection serviceCollection)
        {



            /// SubServices
            serviceCollection.AddSingleton<ServiceMessages>();
            serviceCollection.AddSingleton<INotificationService, NotificationService>();

            /// BackGround Services
            serviceCollection.AddScoped<DailyBabyCareNotifySentService>();
            serviceCollection.AddScoped<DailyCareTimesBackgroundServices>();

            /// Firebase Services
            serviceCollection.AddSingleton<ApiClient.FirebaseAdmin>();
            serviceCollection.AddSingleton<IFirebaseClientFactory, FirebaseClientFactory>();

            /// Controller Services
            serviceCollection.AddScoped<PostServices>();
            serviceCollection.AddScoped<ProfileServices>();
            serviceCollection.AddScoped<CommentServices>();
            serviceCollection.AddScoped<BabyServices>();
            serviceCollection.AddScoped<CourseServices>();
            serviceCollection.AddScoped<BroadcastLiveServices>();
            serviceCollection.AddScoped<UserServices>();
            serviceCollection.AddScoped<DailyCareTimesServices>();
            serviceCollection.AddScoped<AgeGroupsServices>();
            serviceCollection.AddScoped<CareTypeServices>();

           
            

        }
    }
}


