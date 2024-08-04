using Microsoft.EntityFrameworkCore;
using MomesCare.Api.Repository.Base;
using MomesCare.Api.Entities.Models;

namespace MomesCare.Api.Repository
{
    public interface IDailyCareTimesRepository : IRepository<DailyCareTimes>
    {
        public DbSet<DailyCareTimes> DbSet { get; }
        public Task<DailyCareTimes> UpdateAsync(DailyCareTimes entity);
        public Task CreateAsync(DailyCareTimes entity);
        public Task RemoveAsync(DailyCareTimes entity);

    }
}
