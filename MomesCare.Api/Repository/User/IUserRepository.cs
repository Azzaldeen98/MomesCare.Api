using Microsoft.EntityFrameworkCore;
using MomesCare.Api.Repository.Base;
using MomesCare.Api.Entities.Models;

namespace MomesCare.Api.Repository
{
    public interface IUserRepository : IRepository<ApplicationUser>
    {
        public DbSet<ApplicationUser> DbSet { get; }
        public Task<ApplicationUser> UpdateAsync(ApplicationUser entity);
        public Task RemoveAsync(ApplicationUser entity);


        public Task<IEnumerable<CloudMessagingToken>> getAllFCMTokensAsync();
        public Task CreateFCMTokenAsync(CloudMessagingToken entity);
        public Task UpdateFCMTokenAsync(CloudMessagingToken entity);
        public Task RemoveFCMTokenAsync(CloudMessagingToken entity);


    }
}
