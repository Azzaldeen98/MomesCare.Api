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

namespace MomesCare.Api.Services
{
    public class CareTypeServices
    {

        private readonly IUserClaimsHelper _userClaimsHelper;
        private readonly ICareTypeRepository _repository;
        private readonly IMapper _mapper;


        public CareTypeServices(
            IUserClaimsHelper userClaimsHelper,
            IMapper mapper,
            ICareTypeRepository repository){

            this._mapper = mapper;
            this._userClaimsHelper = userClaimsHelper;
            _repository = repository;

        }


        public async Task createAsync(CareTypeCreate model)
        {

            var dailyCareTimes = _mapper.Map<CareType>(model);
            await _repository.CreateAsync(dailyCareTimes);

        }

        public async Task updateAsync(CareTypeUpdate model)
        {
            var careType = await _repository.GetAsync(x=>x.id == model.id);
            if (careType == null)
                throw new  Exception("not found !!");


            careType.state = model.state;
            careType.name = model.name;
   
            await _repository.UpdateAsync(careType);
            

        }


        public async Task<IEnumerable<CareTypeIndex>> getAllAsync()
        {
            var items = await _repository.GetAllAsync();
            if (items == null)
                return new List<CareTypeIndex>();

            return _mapper.Map<List<CareTypeIndex>>(items);

        }

        public async Task<IEnumerable<CareTypeIndex>> getAllWithItemsAsync()
        {
            var dailyCareTimes = (await _repository.GetAllAsync(includeProperties:"dailyCareTimes"));
           
            if (dailyCareTimes == null)
                return new List<CareTypeIndex>();

            var dailyCareTimesIndex = _mapper.Map<List<CareTypeIndex>>(dailyCareTimes);
            int index = 0;
            foreach (var item in dailyCareTimesIndex)
            {
                var careType = dailyCareTimes[index++];
                item.dailyCareTimes = _mapper.Map<List<DailyCareTimesIndex>>(careType.dailyCareTimes);

            }
            return dailyCareTimesIndex;
        }


     
  
        public async Task<CareTypeIndex> getOne(int id)
        {
            var dailyCareTime = await _repository.GetAsync(X => X.id == id, includeProperties: "dailyCareTimes");
            
            if(dailyCareTime == null)
                throw new Exception("not found !!");

            var dailyCareTimesIndex = _mapper.Map<CareTypeIndex>(dailyCareTime);
            dailyCareTimesIndex.dailyCareTimes = _mapper.Map<List<DailyCareTimesIndex>>(dailyCareTime.dailyCareTimes);

            return dailyCareTimesIndex;
        
        }      
        
        public async Task deleteAsync(int id)
        {
            var DailyCareTimes = await _repository.GetAsync(x => x.id == id);
            if (DailyCareTimes == null)
                throw new  Exception("not found !!");

            await _repository.RemoveAsync(DailyCareTimes);
            
        }


    }
}
