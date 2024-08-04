using Microsoft.EntityFrameworkCore;
using MomesCare.Api;
using MomesCare.Api.Entities.Models;
using MomesCare.Api.Helpers;
using MomesCare.Api.Repository.Base;

namespace MomesCare.Api.Repository
{
    public class UserRepository : Repository<ApplicationUser>, IUserRepository
    {


        private readonly DataContext _db;
        private readonly IUserClaimsHelper userClaimsHelper;

        public DbSet<ApplicationUser> DbSet { get => _db.Users; }
        public DbSet<CloudMessagingToken> DbSetFCM { get => _db.CloudMessagingTokens; }

        public UserRepository(DataContext db, IUserClaimsHelper userClaimsHelper) : base(db,userClaimsHelper)
        {
            _db = db;
            this.userClaimsHelper = userClaimsHelper;
        }

   
        public async Task<ApplicationUser> UpdateAsync(ApplicationUser entity)
        {
            _db.ChangeTracker.Clear();
            DbSet.Update(entity);
            await _db.SaveChangesAsync();
            return entity;
        }

        public async Task RemoveAsync(ApplicationUser entity)
        {
            DbSet.Remove(entity);
            await _db.SaveChangesAsync();
        }
        public async Task CreateFCMTokenAsync(CloudMessagingToken entity)
        {
            await DbSetFCM.AddAsync(entity);
            await _db.SaveChangesAsync();
        }

        public async Task UpdateFCMTokenAsync(CloudMessagingToken entity)
        {
            _db.ChangeTracker.Clear();
            DbSetFCM.Update(entity);
            await _db.SaveChangesAsync();

        }

        public async Task RemoveFCMTokenAsync(CloudMessagingToken entity)
        {
             DbSetFCM.Remove(entity);
            await _db.SaveChangesAsync();
        }
     
         public async Task<IEnumerable<CloudMessagingToken>> getAllFCMTokensAsync()
        {

            IQueryable<CloudMessagingToken> query = DbSetFCM.Include(x => x.user);
        
            return await query.ToListAsync();
        }

     
    }
}
