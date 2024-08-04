using AutoMapper;
using Microsoft.EntityFrameworkCore;

using MomesCare.Api.Repository;
using IdentityModel;
using System.Collections.Generic;
using MomesCare.Api.Helpers;
using MomesCare.Api.Entities.ViewModel;
using MomesCare.Api.Entities.Models;
using MomesCare.Api.Entities.ViewModel.Baby;
using MomesCare.Api.Entities.ViewModel.Comment;
using MomesCare.Api.Entities.ViewModel.Post;
using MomesCare.Api.Exceptions;

namespace MomesCare.Api.Services
{
    public class BabyServices
    {

        private readonly IUserClaimsHelper _userClaimsHelper;
        private readonly IBabyRepository _repository;
        private readonly IMapper _mapper;


        private bool hasAuothorize(string userId)
        {
            if (userId != _userClaimsHelper.UserId)
                throw new Exception("no authorize !!");
            return true;
        }

        public BabyServices(
            IUserClaimsHelper userClaimsHelper,
            IBabyRepository repository,
            IMapper mapper) 
        {

            this._repository = repository;
            this._mapper = mapper;
            this._userClaimsHelper = userClaimsHelper;  

        }

        public async Task getInfoAsync()
        {
            //var Baby = _mapper.Map<Baby>(model);
   

           

        }


        public async Task createAsync(BabyCreate model)
        {
     
            var babies = await _repository.GetAllAsync(x => x.user.Id==_userClaimsHelper.UserId);

            if (babies != null && babies.Any(x=>x.Name.Equals(model.Name, StringComparison.OrdinalIgnoreCase)))
                throw new ExistsException("لديك طفل بهذا الاسم يجب ان يكون اسم الطفل فريد !!");

            var baby = _mapper.Map<Baby>(model);
            baby.user = await _repository.getUserAsync(_userClaimsHelper.UserId);

            hasAuothorize(baby.user.Id ?? "");

            await _repository.CreateAsync(baby);

        }

        public async Task<BabyIndex> updateAsync(BabyUpdate model)
        {
            var baby = await _repository.GetAsync(x=>x.Id== model.Id,includeProperties:"user");
            if (baby == null)
                throw new  Exception("not found !!");


            hasAuothorize(baby.user.Id);
            baby = _mapper.Map<Baby>(model);

            var item = await _repository.UpdateAsync(baby);
            var res = _mapper.Map<BabyIndex>(item);
    
            return res;

        }   

    

        public async Task<IEnumerable<BabyIndex>> getAllAsync()
        {
            var babies = (await _repository.GetAllAsync(x=>x.user.Id==_userClaimsHelper.UserId, includeProperties: "user"))
                .OrderByDescending(x => x.Id);
            if (babies == null)
                return new List<BabyIndex>();

            var items = _mapper.Map<List<BabyIndex>>(babies);
    
            return items;

            

        }      

        
        public async Task<BabyIndex> getOne(int id)
        {
            var Baby = await _repository.GetAsync(X => X.Id == id, includeProperties: "user");
            
            if(Baby==null)
                throw new Exception("not found !!");

            var BabyIndex = _mapper.Map<BabyIndex>(Baby);

            return BabyIndex;
        
        }      
        
        public async Task deleteAsync(int id)
        {
            var baby = await _repository.GetAsync(x => x.Id == id && x.user.Id == _userClaimsHelper.UserId, includeProperties:"user");
            if (baby == null)
                throw new  Exception("not found !!");

            hasAuothorize(baby.user.Id);

            await _repository.RemoveAsync(baby);
            
        }


    }
}
