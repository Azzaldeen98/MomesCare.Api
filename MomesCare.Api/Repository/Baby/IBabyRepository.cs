using Microsoft.EntityFrameworkCore;
using MomesCare.Api.Repository.Base;
using MomesCare.Api.Entities.Models;

namespace MomesCare.Api.Repository
{
    public interface IBabyRepository : IRepository<Baby>
    {
        public DbSet<Baby> DbSet { get; }
        public Task CreateAsync(Baby entity);
        public Task<Baby> UpdateAsync(Baby entity);
        public Task RemoveAsync(Baby entity);
     

    }
}
