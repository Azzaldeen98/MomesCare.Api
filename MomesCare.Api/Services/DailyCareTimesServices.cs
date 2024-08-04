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
    public class DailyCareTimesServices
    {

        private readonly IUserClaimsHelper _userClaimsHelper;
        private readonly IBabyRepository _babyRepository;
        private readonly IAgeGroupRepository ageGroupRepository;
        private readonly ICareTypeRepository careTypeRepository;
        private readonly IDailyCareTimesRepository _repository;
        private readonly IMapper _mapper;


        private bool hasAuothorize(string userId)
        {
            if (userId != _userClaimsHelper.UserId)
                throw new Exception("no authorize !!");
            return true;
        }

        public DailyCareTimesServices(
            IUserClaimsHelper userClaimsHelper,
            IBabyRepository babyRepository,
            IMapper mapper,
            IDailyCareTimesRepository repository,
            IAgeGroupRepository ageGroupRepository,
            ICareTypeRepository careTypeRepository)
        {

            this._babyRepository = babyRepository;
            this._repository = repository;
            this._mapper = mapper;
            this._userClaimsHelper = userClaimsHelper;
            _repository = repository;
            this.ageGroupRepository = ageGroupRepository;
            this.careTypeRepository = careTypeRepository;
        }


        private DailyCareTimesIndex getDailyCareTimesIndex(DailyCareTimes dailyCareTimes)
        {
            var dailyCareTimesIndex = _mapper.Map<DailyCareTimesIndex>(dailyCareTimes);
            dailyCareTimesIndex.careType = _mapper.Map<CareTypeIndex>(dailyCareTimes.careType);
            dailyCareTimesIndex.ageGroup = _mapper.Map<AgeGroupIndex>(dailyCareTimes.ageGroup);
            return dailyCareTimesIndex;
        }

        public async Task createAsync(DailyCareTimesCreate model)
        {
     
            //var items = await _repository.GetAllAsync(x => x.user.Id==_userClaimsHelper.UserId);

            //if (items != null && items.Any(x=>x.Name.Equals(model.Name, StringComparison.OrdinalIgnoreCase)))
            //    throw new ExistsException("لديك طفل بهذا الاسم يجب ان يكون اسم الطفل فريد !!");

            var dailyCareTimes = _mapper.Map<DailyCareTimes>(model);
            dailyCareTimes.ageGroup = await ageGroupRepository.GetAsync(x=>x.id==model.ageGroupId);
            dailyCareTimes.careType = await careTypeRepository.GetAsync(x=>x.id==model.careTypeId);

            if (dailyCareTimes.ageGroup == null )    
                throw new AgeGroupException("null Reference");
            if (dailyCareTimes.careType == null)
                throw new CareTypeException("null Reference");
       

            await _repository.CreateAsync(dailyCareTimes);

        }

        public async Task updateAsync(DailyCareTimesUpdate model)
        {
            var dailyCareTimes = await _repository.GetAsync(x=>x.id == model.id,includeProperties: "ageGroup,careType");
            if (dailyCareTimes == null)
                throw new  Exception("not found !!");

            //dailyCareTimes = _mapper.Map<DailyCareTimes>(model);

            dailyCareTimes.time = Helper.ConvertToTimeSpan(model.time);
            dailyCareTimes.descript = model.descript;
            dailyCareTimes.state = model.state;
            if (model.ageGroupId != dailyCareTimes.ageGroup.id)
            {
                dailyCareTimes.ageGroup = await ageGroupRepository.GetAsync(x => x.id == model.ageGroupId);
                if (dailyCareTimes.ageGroup == null)
                    throw new AgeGroupException("null Reference");
            }
            if (model.careTypeId != dailyCareTimes.careType.id)
            {
                dailyCareTimes.careType = await careTypeRepository.GetAsync(x => x.id == model.careTypeId);

                if (dailyCareTimes.careType == null)
                    throw new CareTypeException("null Reference");
            }
        

            await _repository.UpdateAsync(dailyCareTimes);
            

        }

        public async Task deleteAsync(int id)
        {
            var DailyCareTimes = await _repository.GetAsync(x => x.id == id);
            if (DailyCareTimes == null)
                throw new Exception("not found !!");

            await _repository.RemoveAsync(DailyCareTimes);

        }

        public async Task<IEnumerable<DailyCareTimes>> getAllByTimeAsync(TimeSpan currentTime)
        {
            var items = (await _repository.GetAllAsync(x=> Helper.CurrentTimeEqualOrMoreThanTime(x.time),includeProperties: "ageGroup,careType"));
            return items;
        }

        public async Task<IEnumerable<DailyCareTimesIndex>> getAllAsync()
        {
            var dailyCareTimes = await _repository.GetAllAsync(includeProperties: "ageGroup,careType");
               
            if (dailyCareTimes == null)
                return new List<DailyCareTimesIndex>();

            var dailyCareTimesIndex = _mapper.Map<List<DailyCareTimesIndex>>(dailyCareTimes);
            int index = 0;
            foreach (var item in dailyCareTimes)
            {
                var itemIndex = dailyCareTimesIndex[index++];

                itemIndex.careType = _mapper.Map<CareTypeIndex>(item.careType);
                itemIndex.ageGroup = _mapper.Map<AgeGroupIndex>(item.ageGroup);
            }

            return dailyCareTimesIndex;

        }

        public async Task<List<DailyCareTimesIndex>> getAllByAgeAsync(double age)
        {

            var dailyCareTimes = await _repository.GetAllAsync(x => age>=x.ageGroup.min && age <= x.ageGroup.max,
               includeProperties: "ageGroup,careType");

            if (dailyCareTimes == null)
                throw new Exception("not found !!");

            var dailyCareTimesIndex = _mapper.Map<List<DailyCareTimesIndex>>(dailyCareTimes);
            int index = 0;
            foreach(var item in dailyCareTimes)
            {
                var itemIndex = dailyCareTimesIndex[index++];

                itemIndex.careType = _mapper.Map<CareTypeIndex>(item.careType);
                itemIndex.ageGroup = _mapper.Map<AgeGroupIndex>(item.ageGroup);
            }

            return dailyCareTimesIndex;

        } 
     
        public async Task<List<DailyCareTimesIndex>> getByCareTypeAndAgeAsync(double countMonths, int? careType_id=0)
        {
          

            var dailyCareTimes = await _repository.GetAllAsync(x => countMonths >= x.ageGroup.min && countMonths <= x.ageGroup.max 
            && ((careType_id == null || careType_id==0) ? true : x.careType.id==careType_id) , includeProperties: "ageGroup,careType");

            if (dailyCareTimes == null)
                throw new Exception("not found !!");

            var dailyCareTimesIndex = new List<DailyCareTimesIndex>();
            foreach (var item in dailyCareTimes)
            {
                var tm = getDailyCareTimesIndex(item);
                if(tm!=null)
                    dailyCareTimesIndex.Add(tm);
            }

            return dailyCareTimesIndex;

        }

        public async Task<DailyCareTimesIndex> getByAgeGroupAsync(AgeGroup ageGroup,CareType? careType)
        {
            var dailyCareTimes = await _repository.GetAsync(x => (ageGroup.min >= x.ageGroup.min && ageGroup.min < x.ageGroup.max
            && ageGroup.max <= x.ageGroup.max ) && (careType!=null? x.careType.id == careType.id: true),includeProperties: "ageGroup,careType");

            if (dailyCareTimes == null)
                throw new Exception("not found !!");

            return getDailyCareTimesIndex(dailyCareTimes);
        }
  
 

        public async Task<List<DailyCareTimesIndex>> getBabyDailyCareTimesAsync(int baby_id,int? careType_id=0)
        {

            var baby = await _babyRepository.GetAsync(x => x.Id== baby_id);

            if (baby == null)
                throw new Exception("not found !!");

           int countMonths = Helper.GetMonthsDifference(baby.BirthDay,Helper.GetCurrentTime());
 
         
           return await getByCareTypeAndAgeAsync(countMonths, careType_id);
     

        }

        public async Task<DailyCareTimesIndex> getOne(int id)
        {
            var dailyCareTimes = await _repository.GetAsync(X => X.id == id, includeProperties: "ageGroup,careType");
            
            if(dailyCareTimes == null)
                throw new Exception("not found !!");

            var dailyCareTimesIndex = _mapper.Map<DailyCareTimesIndex>(dailyCareTimes);

            return dailyCareTimesIndex;
        
        }      
        



    }
}
