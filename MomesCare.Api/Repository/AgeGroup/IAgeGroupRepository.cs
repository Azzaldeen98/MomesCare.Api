using Microsoft.EntityFrameworkCore;
using MomesCare.Api.Repository.Base;
using MomesCare.Api.Entities.Models;

namespace MomesCare.Api.Repository
{
    public interface IAgeGroupRepository : IRepository<AgeGroup>
    {
        public DbSet<AgeGroup> DbSet { get; }
        public Task<AgeGroup> UpdateAsync(AgeGroup entity);
        public Task CreateAsync(AgeGroup entity);
        public Task RemoveAsync(AgeGroup entity);

    }
}
