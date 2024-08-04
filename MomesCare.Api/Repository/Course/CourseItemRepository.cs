using Microsoft.EntityFrameworkCore;
using MomesCare.Api;
using MomesCare.Api.Entities.Models;
using MomesCare.Api.Helpers;
using MomesCare.Api.Repository.Base;



namespace MomesCare.Api.Repository
{
    public class CourseItemRepository : Repository<CourseItem>, ICourseItemRepository
    {
    


        private readonly DataContext _db;
        private readonly IUserClaimsHelper userClaimsHelper;

        public DbSet<CourseItem> DbSet { get => _db.CourseItems; }

        public CourseItemRepository(DataContext db, IUserClaimsHelper userClaimsHelper) : base(db, userClaimsHelper)
        {
            _db = db;
            this.userClaimsHelper = userClaimsHelper;
        }

        public async Task CreateAsync(CourseItem entity)
        {
            await _dbSet.AddAsync(entity);
            await _db.SaveChangesAsync();

        }

        public async Task<CourseItem> UpdateAsync(CourseItem entity)
        {
            _db.ChangeTracker.Clear();
            DbSet.Update(entity);
            await _db.SaveChangesAsync();
            return entity;
        }

        public async Task RemoveAsync(CourseItem entity)
        {
            DbSet.Remove(entity);
            await _db.SaveChangesAsync();
        }


    }
}
