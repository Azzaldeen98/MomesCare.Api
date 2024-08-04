using AutoMapper;
using Microsoft.EntityFrameworkCore;

using MomesCare.Api.Repository;
using IdentityModel;
using System.Collections.Generic;
using MomesCare.Api.Helpers;
using MomesCare.Api.Entities.ViewModel;
using MomesCare.Api.Entities.Models;
using MomesCare.Api.Entities.ViewModel.AgeGroup;
using MomesCare.Api.Entities.ViewModel.Comment;
using MomesCare.Api.Entities.ViewModel.Post;
using MomesCare.Api.Exceptions;
using MomesCare.Api.Entities.ViewModel.CareType;
using MomesCare.Api.Entities.ViewModel.Courses;
using MomesCare.Api.Entities.ViewModel.AgeGroup;
using System.Text.RegularExpressions;
using System.Linq.Expressions;
using MomesCare.Api.Entities.ViewModel.DailyCareTimes;

namespace MomesCare.Api.Services
{
    public class AgeGroupsServices
    {

        private readonly IUserClaimsHelper _userClaimsHelper;
        private readonly IAgeGroupRepository _repository;
        private readonly IBabyRepository _babyRepository;

        private readonly IMapper _mapper;


      

        public AgeGroupsServices(
            IUserClaimsHelper userClaimsHelper,
            IMapper mapper,
            IAgeGroupRepository ageGroupRepository,
            IBabyRepository babyRepository)
        {

             _mapper = mapper;
            _userClaimsHelper = userClaimsHelper;
            _repository = ageGroupRepository;
            _babyRepository = babyRepository;
        }




        public async Task createAsync(AgeGroupCreate model)
        {
     
            var AgeGroup = _mapper.Map<AgeGroup>(model);

            await _repository.CreateAsync(AgeGroup);

        }

        public async Task updateAsync(AgeGroupUpdate model)
        {
            var ageGroup = await _repository.GetAsync(x=>x.id == model.id);
            if (ageGroup == null)
                throw new  Exception("not found !!");

            ageGroup.timePeriodScale = model.timePeriodScale;
            ageGroup.min =model.min;
            ageGroup.max =model.max;
            
            await _repository.UpdateAsync(ageGroup);
            

        }

        public async Task deleteAsync(int id)
        {
            var AgeGroup = await _repository.GetAsync(x => x.id == id);
            if (AgeGroup == null)
                throw new Exception("not found !!");

            await _repository.RemoveAsync(AgeGroup);

        }



        public async Task<IEnumerable<AgeGroupIndex>> getAllAsync()
        {
            var items = await _repository.GetAllAsync();
            if (items == null)
                return new List<AgeGroupIndex>();

            return _mapper.Map<List<AgeGroupIndex>>(items);

        }    

        public async Task<IEnumerable<AgeGroupIndex>> getAllWithItemsAsync()
        {
            var items = await _repository.GetAllAsync(includeProperties: "dailyCareTimes");
            if (items == null)
                return new List<AgeGroupIndex>();

            var ageGroupIndex = _mapper.Map<List<AgeGroupIndex>>(items);
            int index = 0;
            foreach (var item in ageGroupIndex)
            {
                var ageGroup = items[index++];
                item.dailyCareTimes = _mapper.Map<List<DailyCareTimesIndex>>(ageGroup.dailyCareTimes);
               
            }

            return ageGroupIndex;

        }

        public async Task<List<AgeGroupIndex>> getByAgeAsync(double age)
        {

            var ageGroup = await _repository.GetAllAsync(x => age>=x.min && age <= x.max, includeProperties: "dailyCareTimes");

            if (ageGroup == null)
                throw new Exception("not found !!");

            var ageGroupIndex = _mapper.Map<List<AgeGroupIndex>>(ageGroup);
            int index = 0;
            foreach (var itemIndex in ageGroupIndex)
            {
                var item = ageGroup[index++];
                itemIndex.dailyCareTimes = _mapper.Map<List<DailyCareTimesIndex>>(item.dailyCareTimes);

            }

            return ageGroupIndex;


        } 
    
        public async Task<AgeGroupIndex> getOne(int id)
        {
            var ageGroup = await _repository.GetAsync(X => X.id == id, includeProperties: "dailyCareTimes");
            
            if(ageGroup == null)
                throw new Exception("not found !!");

            var ageGroupIndex = _mapper.Map<AgeGroupIndex>(ageGroup);
            ageGroupIndex.dailyCareTimes = _mapper.Map<List<DailyCareTimesIndex>>(ageGroup.dailyCareTimes);


            return ageGroupIndex;

        
        
        }      
        
        public async Task<AgeGroupIndex> getBabyAgeGroupAsync(int babyId)
        {
            var baby = await _babyRepository.GetAsync(x => x.Id == babyId);

            if (baby == null)
                throw new Exception("not found !!");

            int countMonths = Helper.GetMonthsDifference(baby.BirthDay, DateTime.UtcNow);

            var ageGroups = await _repository.GetAllAsync(x => countMonths >= x.min && countMonths <= x.max);

            if (ageGroups == null)
                return null!;

            var ageGroup = ageGroups.FirstOrDefault();

            return _mapper.Map<AgeGroupIndex>(ageGroup) ?? null!;
        }


    }
}
