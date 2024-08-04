using Microsoft.EntityFrameworkCore;
using MomesCare.Api;
using MomesCare.Api.Entities.Models;
using MomesCare.Api.Helpers;
using MomesCare.Api.Repository.Base;



namespace MomesCare.Api.Repository
{
    public class CourseRepository : Repository<Course>, ICourseRepository
    {
    


        private readonly DataContext _db;
        private readonly IUserClaimsHelper userClaimsHelper;

        public DbSet<Course> DbSet { get => _db.Courses; }

        public CourseRepository(DataContext db, IUserClaimsHelper userClaimsHelper) : base(db, userClaimsHelper)
        {
            _db = db;
            this.userClaimsHelper = userClaimsHelper;
        }

        public async Task CreateAsync(Course entity)
        {

            entity.createdAt = Helper.GetCurrentTime();

            await _dbSet.AddAsync(entity);
            await _db.SaveChangesAsync();

        }

        public async Task<Course> UpdateAsync(Course entity)
        {
            _db.ChangeTracker.Clear();
            DbSet.Update(entity);
            await _db.SaveChangesAsync();
            return entity;
        }

        public async Task RemoveAsync(Course entity)
        {
            DbSet.Remove(entity);
            await _db.SaveChangesAsync();
        }


    }
}
