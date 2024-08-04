using Microsoft.EntityFrameworkCore;
using MomesCare.Api;
using MomesCare.Api.Entities.Models;
using MomesCare.Api.Helpers;
using MomesCare.Api.Repository.Base;



namespace MomesCare.Api.Repository
{
    public class DailyCareTimesRepository : Repository<DailyCareTimes>, IDailyCareTimesRepository
    {


        private readonly DataContext _db;
        private readonly IUserClaimsHelper _userClaimsHelper;

        public DbSet<DailyCareTimes> DbSet { get => _db.DailyCareTimes; }

        public DailyCareTimesRepository(DataContext db, IUserClaimsHelper userClaimsHelper) : base(db,userClaimsHelper)
        {
            _db = db;
            this._userClaimsHelper = userClaimsHelper;
        }

        public  async Task CreateAsync(DailyCareTimes entity)
        {


            await _dbSet.AddAsync(entity);
            await _db.SaveChangesAsync();

        }

        public async Task<DailyCareTimes> UpdateAsync(DailyCareTimes entity)
        {
            _db.ChangeTracker.Clear();
            DbSet.Update(entity);
            await _db.SaveChangesAsync();
            return entity;
        }

        public async Task RemoveAsync(DailyCareTimes entity)
        {
            DbSet.Remove(entity);
            await _db.SaveChangesAsync();
        }  
        



    }
}
