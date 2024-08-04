using Microsoft.EntityFrameworkCore;
using MomesCare.Api;
using MomesCare.Api.Entities.Models;
using MomesCare.Api.Helpers;
using MomesCare.Api.Repository.Base;



namespace MomesCare.Api.Repository
{
    public class BroadcastLiveRepository : Repository<BroadcastLive>, IBroadcastLiveRepository
    {


        private readonly DataContext _db;
        private readonly IUserClaimsHelper userClaimsHelper;

        public DbSet<BroadcastLive> DbSet { get => _db.BroadcastLives; }

        public BroadcastLiveRepository(DataContext db, IUserClaimsHelper userClaimsHelper) : base(db,userClaimsHelper)
        {
            _db = db;
            this.userClaimsHelper = userClaimsHelper;
        }

        public  async Task CreateAsync(BroadcastLive entity)
        {

            entity.createdAt = Helper.GetCurrentTime();
            await _dbSet.AddAsync(entity);
            await _db.SaveChangesAsync();

        }

        public async Task<BroadcastLive> UpdateAsync(BroadcastLive entity)
        {
            _db.ChangeTracker.Clear();
            entity.createdAt = Helper.GetCurrentTime();
            DbSet.Update(entity);
            await _db.SaveChangesAsync();
            return entity;
        }

        public async Task RemoveAsync(BroadcastLive entity)
        {
            DbSet.Remove(entity);
            await _db.SaveChangesAsync();
        }


    }
}
