using Microsoft.EntityFrameworkCore;
using MomesCare.Api;
using MomesCare.Api.Entities.Models;
using MomesCare.Api.Helpers;
using MomesCare.Api.Repository.Base;



namespace MomesCare.Api.Repository
{
    public class BabyRepository : Repository<Baby>, IBabyRepository
    {
    


        private readonly DataContext _db;
        private readonly IUserClaimsHelper userClaimsHelper;

        public DbSet<Baby> DbSet { get => _db.Babies; }

        public BabyRepository(DataContext db, IUserClaimsHelper userClaimsHelper) : base(db, userClaimsHelper)
        {
            _db = db;
            this.userClaimsHelper = userClaimsHelper;
        }

        public async Task CreateAsync(Baby entity)
        {

            entity.CreatedAt = Helper.GetCurrentTime();

            await _dbSet.AddAsync(entity);
            await _db.SaveChangesAsync();

        }

        public async Task<Baby> UpdateAsync(Baby entity)
        {
            _db.ChangeTracker.Clear();
            DbSet.Update(entity);
            await _db.SaveChangesAsync();
            return entity;
        }

        public async Task RemoveAsync(Baby entity)
        {
            DbSet.Remove(entity);
            await _db.SaveChangesAsync();
        }


    }
}
