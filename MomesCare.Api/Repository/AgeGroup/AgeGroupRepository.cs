using Microsoft.EntityFrameworkCore;
using MomesCare.Api;
using MomesCare.Api.Entities.Models;
using MomesCare.Api.Helpers;
using MomesCare.Api.Repository.Base;



namespace MomesCare.Api.Repository
{
    public class AgeGroupRepository : Repository<AgeGroup>, IAgeGroupRepository
    {


        private readonly DataContext _db;
        private readonly IUserClaimsHelper _userClaimsHelper;

        public DbSet<AgeGroup> DbSet { get => _db.AgeGroups; }

        public AgeGroupRepository(DataContext db,
            IUserClaimsHelper userClaimsHelper) : base(db,userClaimsHelper)
        {
            _db = db;
            this._userClaimsHelper = userClaimsHelper;
        }

        public  async Task CreateAsync(AgeGroup entity)
        {


            await _dbSet.AddAsync(entity);
            await _db.SaveChangesAsync();

        }

        public async Task<AgeGroup> UpdateAsync(AgeGroup entity)
        {
            _db.ChangeTracker.Clear();
            DbSet.Update(entity);
            await _db.SaveChangesAsync();
            return entity;
        }

        public async Task RemoveAsync(AgeGroup entity)
        {
            DbSet.Remove(entity);
            await _db.SaveChangesAsync();
        }  
        



    }
}
