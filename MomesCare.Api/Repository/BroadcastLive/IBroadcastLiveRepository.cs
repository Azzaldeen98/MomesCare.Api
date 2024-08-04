using Microsoft.EntityFrameworkCore;
using MomesCare.Api.Repository.Base;
using MomesCare.Api.Entities.Models;

namespace MomesCare.Api.Repository
{
    public interface IBroadcastLiveRepository : IRepository<BroadcastLive>
    {
        public DbSet<BroadcastLive> DbSet { get; }
        public Task<BroadcastLive> UpdateAsync(BroadcastLive entity);
        public Task CreateAsync(BroadcastLive entity);
        public Task RemoveAsync(BroadcastLive entity);
     

    }
}
