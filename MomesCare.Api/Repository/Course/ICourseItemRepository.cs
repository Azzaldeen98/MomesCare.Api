using Microsoft.EntityFrameworkCore;
using MomesCare.Api.Repository.Base;
using MomesCare.Api.Entities.Models;

namespace MomesCare.Api.Repository
{
    public interface ICourseItemRepository : IRepository<CourseItem>
    {
        public DbSet<CourseItem> DbSet { get; }
        public Task CreateAsync(CourseItem entity);
        public Task<CourseItem> UpdateAsync(CourseItem entity);
        public Task RemoveAsync(CourseItem entity);
     

    }
}
