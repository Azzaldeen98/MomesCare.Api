using Microsoft.EntityFrameworkCore;
using MomesCare.Api.Repository.Base;
using MomesCare.Api.Entities.Models;

namespace MomesCare.Api.Repository
{
    public interface IDailyBabyCareNotifySentRepository : IRepository<BabyHealthCareNotificationsSent>
    {
        public DbSet<BabyHealthCareNotificationsSent> DbSet { get; }
        public Task<BabyHealthCareNotificationsSent> UpdateAsync(BabyHealthCareNotificationsSent entity);
        public Task CreateAsync(BabyHealthCareNotificationsSent entity);
        public Task RemoveAsync(BabyHealthCareNotificationsSent entity);

    }
}
