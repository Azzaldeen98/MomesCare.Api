using Microsoft.EntityFrameworkCore;
using MomesCare.Api.Repository.Base;
using MomesCare.Api.Entities.Models;

namespace MomesCare.Api.Repository
{
    public interface ICourseRepository : IRepository<Course>
    {
        public DbSet<Course> DbSet { get; }
        public Task CreateAsync(Course entity);
        public Task<Course> UpdateAsync(Course entity);
        public Task RemoveAsync(Course entity);
     

    }
}
