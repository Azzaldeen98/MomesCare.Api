using AutoMapper;
using Microsoft.EntityFrameworkCore;

using MomesCare.Api.Repository;
using IdentityModel;
using System.Collections.Generic;
using MomesCare.Api.Helpers;
using MomesCare.Api.Entities.ViewModel;
using MomesCare.Api.Entities.Models;
using MomesCare.Api.Entities.ViewModel.DailyCareTimes;
using MomesCare.Api.Entities.ViewModel.Comment;
using MomesCare.Api.Entities.ViewModel.Post;
using MomesCare.Api.Exceptions;
using MomesCare.Api.Entities.ViewModel.CareType;
using MomesCare.Api.Entities.ViewModel.Courses;
using MomesCare.Api.Entities.ViewModel.AgeGroup;
using System.Text.RegularExpressions;
using System.Linq.Expressions;
using MomesCare.Api.Services.SubServices;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MomesCare.Api.Services
{
    public class DailyCareTimesBackgroundServices
    {



        private readonly DataContext _db;
        private readonly ILogger<BackgroundWorkerService> logger;

        private DbSet<DailyCareTimes> _dbDailyCareTimesSet { get => _db.DailyCareTimes; }
        private DbSet<Baby> _dbBabySet { get => _db.Babies; }


        public DailyCareTimesBackgroundServices(
                                DataContext dbContext,
                                ILogger<BackgroundWorkerService> logger,
                                ServiceMessages serviceMessages)
        {
            this._db = dbContext;
            this.logger = logger;
        }


        public async Task<IEnumerable<DailyCareTimes>> getAllByTimeAsync(TimeSpan currentTime)
        {
            try
            {
              
                var items = await _dbDailyCareTimesSet.Where(x => TimeSpan.Compare(x.time, currentTime)<=0) 
                    .Include(x => x.careType)
                    .Include(x => x.ageGroup)
                    .ToListAsync();
                return items;
            }
            catch (Exception ex)
            {
                return null;
                throw;
            }

            
        }

        public async Task<IEnumerable<NotifyUserBabyWithFCMToken>> getUserFCMTokensByAgeGroupAsync(AgeGroup ageGroup)
        {

            //Func<DateTime,int> getAgeInMonths = (DateTime date) => Helper.GetMonthsDifference(date, );
            var currentTime = Helper.GetCurrentTime();


            var babies = await _dbBabySet
                .Include(x => x.user)
                .ThenInclude(x => x.cloudMessagingToken)
                .ToListAsync();

            var filteredBabies = babies
                .Where(x => Helper.GetMonthsDifference(x.BirthDay, currentTime) >= ageGroup.min
                            && Helper.GetMonthsDifference(x.BirthDay, currentTime) <= ageGroup.max)
                .ToList();


            //var babies = await _dbBabySet.Where(x=> Helper.GetMonthsDifference(x.BirthDay,currentTime) >= ageGroup.min 
            //&& Helper.GetMonthsDifference(x.BirthDay, currentTime) <= ageGroup.max)
            //    .Include(x=>x.user)
            //    .ThenInclude(x=>x.cloudMessagingToken)
            //    .ToListAsync();

            if (filteredBabies == null) 
                return null!;

            var userBabyWithFCMTokens = new List<NotifyUserBabyWithFCMToken>();

            foreach(var baby in babies)
            {
                userBabyWithFCMTokens.Add(new NotifyUserBabyWithFCMToken {
                    fcmToken=baby.user.cloudMessagingToken.Token,
                    baby=baby,
                    userId=baby.user.Id
                });
            }

            return userBabyWithFCMTokens;
           
        }
        


    }
}
