using Microsoft.EntityFrameworkCore;
using MomesCare.Api.Repository.Base;
using MomesCare.Api.Entities.Models;

namespace MomesCare.Api.Repository
{
    public interface IProfileRepository : IRepository<Profile>
    {
        public DbSet<Profile> DbSet { get; }
        public Task<Profile> UpdateAsync(Profile entity);
        public Task CreateAsync(Profile entity);
        public Task RemoveAsync(Profile entity);  
        public Task<Doctor> UpdateAsync(Doctor entity);
        public Task CreateAsync(Doctor entity);
        public Task RemoveAsync(Doctor entity);
     

    }
}
