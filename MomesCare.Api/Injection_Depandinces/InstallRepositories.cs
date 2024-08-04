
using MomesCare.Api.Repository;

namespace MomesCare.Api.Installs
{
    public static class InstallRepositories
    {
        public static void InstallApiClientRepository(this IServiceCollection serviceCollection)
        {
        
            serviceCollection.AddScoped<IPostRepository, PostRepository>();
            serviceCollection.AddScoped<ICommentRepository, CommentRepository>();
            serviceCollection.AddScoped<IProfileRepository, ProfileRepository>();
            serviceCollection.AddScoped<IUserRepository, UserRepository>();
            serviceCollection.AddScoped<IBabyRepository, BabyRepository>();
            serviceCollection.AddScoped<ICourseRepository, CourseRepository>();
            serviceCollection.AddScoped<ICourseItemRepository, CourseItemRepository>();
            serviceCollection.AddScoped<IBroadcastLiveRepository, BroadcastLiveRepository>();
            serviceCollection.AddScoped<IJoinBroadcastLiveRepository, JoinBroadcastLiveRepository>();
            serviceCollection.AddScoped<IDailyCareTimesRepository, DailyCareTimesRepository>();
            serviceCollection.AddScoped<IAgeGroupRepository, AgeGroupRepository>();
            serviceCollection.AddScoped<ICareTypeRepository, CareTypeRepository>();
            serviceCollection.AddScoped<IDailyBabyCareNotifySentRepository, DailyBabyCareNotifySentRepository>();

        }
    }
}


