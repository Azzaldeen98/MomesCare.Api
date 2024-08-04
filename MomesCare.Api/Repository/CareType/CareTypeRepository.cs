using Microsoft.EntityFrameworkCore;
using MomesCare.Api;
using MomesCare.Api.Entities.Models;
using MomesCare.Api.Helpers;
using MomesCare.Api.Repository.Base;



namespace MomesCare.Api.Repository
{
    public class CareTypeRepository : Repository<CareType>, ICareTypeRepository
    {


        private readonly DataContext _db;
        private readonly IUserClaimsHelper _userClaimsHelper;

        public DbSet<CareType> DbSet { get => _db.CareTypes; }

        public CareTypeRepository(DataContext db,
            IUserClaimsHelper userClaimsHelper) : base(db,userClaimsHelper)
        {
            _db = db;
            this._userClaimsHelper = userClaimsHelper;
        }

        public  async Task CreateAsync(CareType entity)
        {


            await _dbSet.AddAsync(entity);
            await _db.SaveChangesAsync();

        }

        public async Task<CareType> UpdateAsync(CareType entity)
        {
            _db.ChangeTracker.Clear();
            DbSet.Update(entity);
            await _db.SaveChangesAsync();
            return entity;
        }

        public async Task RemoveAsync(CareType entity)
        {
            DbSet.Remove(entity);
            await _db.SaveChangesAsync();
        }  
        



    }
}
