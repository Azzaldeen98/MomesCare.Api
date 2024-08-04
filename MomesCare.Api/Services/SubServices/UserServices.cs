using AutoMapper;
using Firebase.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using MomesCare.Api.Entities.Models;
using MomesCare.Api.Entities.ViewModel;
using MomesCare.Api.Helpers;
using MomesCare.Api.Helpers.Enums;
using MomesCare.Api.Repository;
using MomesCare.Api.Repository.Base;
using User = MomesCare.Api.Entities.Models.ApplicationUser;

namespace MomesCare.Api.Services.Static
{
    public class UserServices
    {



        private static IUserClaimsHelper _userClaimsHelper;
        private static IUserRepository _repository;
        private static IProfileRepository _profileRepository;
        private static IMapper _mapper;
        private static UserManager<User> _userManager;


     

         public UserServices(
            IUserClaimsHelper userClaimsHelper,
            IUserRepository repository,
            IProfileRepository profileRepository,
            UserManager<User> userManager,
              IMapper mapper)
        {
            _userClaimsHelper = userClaimsHelper;
            _repository = repository;
            _profileRepository = profileRepository;
            _mapper = mapper;
            _userManager = userManager;

        }
   

        public async  Task initializeFCMToken(FCMToken model)
        {
            if(model.userId == null)
                throw new NullReferenceException(nameof(model));

            var user = await _repository.GetAsync(x => x.Id == model.userId, includeProperties: "cloudMessagingToken");
            if (user == null) 
                throw new KeyNotFoundException("not found  user !!");

            
            var fcmEntity =  _mapper.Map<CloudMessagingToken>(model);
            fcmEntity.user = user;
            fcmEntity.UserId = user.Id;

            if (user.cloudMessagingToken == null)
            {
                await _repository.CreateFCMTokenAsync(fcmEntity);
            }
            else
            {
                fcmEntity.id = user.cloudMessagingToken.id;
                await _repository.UpdateFCMTokenAsync(fcmEntity);
            }
            
            
        }


        public async  Task<UserView> getUserViewAsync(string userId)
        {
            if (userId.IsNullOrEmpty())
                throw new NullReferenceException();

            ApplicationUser? user = await _repository.GetAsync(x => x.Id == userId);

            if (user == null) 
                throw new NullReferenceException();

            var role = (await _userManager.GetRolesAsync(user)).FirstOrDefault();
            if (role == null)
                throw new NullReferenceException();


            var profile = await _profileRepository.GetAsync(x=>x.UserId== userId);


            var userView = new UserView
            {
                id = userId,
                displayName = user.FullName,
                email = user.Email??"",
                urlImage="",
                role = (role!=null)? role : UserRoles.User.ToString(),
            };

            if(profile!=null)
                userView.urlImage = profile.Image;

            return userView;


        }

    }
            
}
