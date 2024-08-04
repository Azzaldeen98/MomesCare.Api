using Microsoft.EntityFrameworkCore;
using MomesCare.Api;
using MomesCare.Api.Entities.Models;
using MomesCare.Api.Helpers;
using MomesCare.Api.Repository.Base;



namespace MomesCare.Api.Repository
{
    public class ProfileRepository : Repository<Profile>, IProfileRepository
    {


        private readonly DataContext _db;
        private readonly IUserClaimsHelper userClaimsHelper;

        public DbSet<Profile> DbSet { get => _db.Profile; }
        public DbSet<Doctor> DbSetDoctor { get => _db.Doctors; }

        public ProfileRepository(DataContext db, IUserClaimsHelper userClaimsHelper) : base(db,userClaimsHelper)
        {
            _db = db;
            this.userClaimsHelper = userClaimsHelper;
        }

        public  async Task CreateAsync(Profile entity)
        {

            await _dbSet.AddAsync(entity);
            await _db.SaveChangesAsync();

        }    
        

        
        public  async Task CreateAsync(Doctor entity)
        {

            await DbSetDoctor.AddAsync(entity);
            await _db.SaveChangesAsync();

        }

        public async Task<Doctor> UpdateAsync(Doctor entity)
        {

            _db.ChangeTracker.Clear();
            DbSetDoctor.Update(entity);
            await _db.SaveChangesAsync();
            return entity;

        }

        public async Task<Profile> UpdateAsync(Profile entity)
        {
            _db.ChangeTracker.Clear();
            DbSet.Update(entity);
            await _db.SaveChangesAsync();
            return entity;
        }

        public async Task RemoveAsync(Profile entity)
        {
            DbSet.Remove(entity);
            await _db.SaveChangesAsync();
        }  
        
        public async Task RemoveAsync(Doctor entity)
        {
            DbSetDoctor.Remove(entity);
            await _db.SaveChangesAsync();
        }


    }
}
