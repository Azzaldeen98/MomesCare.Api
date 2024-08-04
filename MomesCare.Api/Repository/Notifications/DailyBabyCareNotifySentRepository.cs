﻿using Microsoft.EntityFrameworkCore;
using MomesCare.Api;
using MomesCare.Api.Entities.Models;
using MomesCare.Api.Helpers;
using MomesCare.Api.Repository.Base;



namespace MomesCare.Api.Repository
{
    public class DailyBabyCareNotifySentRepository : Repository<BabyHealthCareNotificationsSent>, IDailyBabyCareNotifySentRepository
    {


        private readonly DataContext _db;
        private readonly IUserClaimsHelper _userClaimsHelper;

        public DbSet<BabyHealthCareNotificationsSent> DbSet { get => _db.DailyBabyCareNotificationsSents; }

        public DailyBabyCareNotifySentRepository(DataContext db, IUserClaimsHelper userClaimsHelper) : base(db,userClaimsHelper)
        {
            _db = db;
            this._userClaimsHelper = userClaimsHelper;
        }

        public  async Task CreateAsync(BabyHealthCareNotificationsSent entity)
        {


            await _dbSet.AddAsync(entity);
            await _db.SaveChangesAsync();

        }

        public async Task<BabyHealthCareNotificationsSent> UpdateAsync(BabyHealthCareNotificationsSent entity)
        {
            _db.ChangeTracker.Clear();
            DbSet.Update(entity);
            await _db.SaveChangesAsync();
            return entity;
        }

        public async Task RemoveAsync(BabyHealthCareNotificationsSent entity)
        {
            DbSet.Remove(entity);
            await _db.SaveChangesAsync();
        }  
        



    }
}
