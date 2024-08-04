using Microsoft.EntityFrameworkCore;
using MomesCare.Api;
using MomesCare.Api.Entities.Models;
using MomesCare.Api.Helpers;
using MomesCare.Api.Repository.Base;



namespace MomesCare.Api.Repository
{
    public class JoinBroadcastLiveRepository : Repository<JoinBroadcastLive>, IJoinBroadcastLiveRepository
    {


        private readonly DataContext _db;
        private readonly IUserClaimsHelper userClaimsHelper;

        public DbSet<JoinBroadcastLive> DbSet { get => _db.JoinBroadcastLives; }

        public JoinBroadcastLiveRepository(DataContext db, IUserClaimsHelper userClaimsHelper) : base(db,userClaimsHelper)
        {
            _db = db;
            this.userClaimsHelper = userClaimsHelper;
        }

        public  async Task CreateAsync(JoinBroadcastLive entity)
        {

            entity.createdAt = Helper.GetCurrentTime();
            await _dbSet.AddAsync(entity);
            await _db.SaveChangesAsync();

        }

        public async Task<JoinBroadcastLive> UpdateAsync(JoinBroadcastLive entity)
        {
            _db.ChangeTracker.Clear();
            DbSet.Update(entity);
            await _db.SaveChangesAsync();
            return entity;
        }

        public async Task RemoveAsync(JoinBroadcastLive entity)
        {
            DbSet.Remove(entity);
            await _db.SaveChangesAsync();
        }


    }
}
