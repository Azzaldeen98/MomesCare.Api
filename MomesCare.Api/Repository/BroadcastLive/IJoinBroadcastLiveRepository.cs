using Microsoft.EntityFrameworkCore;
using MomesCare.Api.Repository.Base;
using MomesCare.Api.Entities.Models;

namespace MomesCare.Api.Repository
{
    public interface IJoinBroadcastLiveRepository : IRepository<JoinBroadcastLive>
    {
        public DbSet<JoinBroadcastLive> DbSet { get; }
        public Task<JoinBroadcastLive> UpdateAsync(JoinBroadcastLive entity);
        public Task CreateAsync(JoinBroadcastLive entity);
        public Task RemoveAsync(JoinBroadcastLive entity);


    }
}
