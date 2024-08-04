using AutoMapper;
using Microsoft.EntityFrameworkCore;

using MomesCare.Api.Repository;
using IdentityModel;
using System.Collections.Generic;
using MomesCare.Api.Helpers;
using MomesCare.Api.Entities.ViewModel;
using MomesCare.Api.Entities.Models;
using MomesCare.Api.Entities.ViewModel.Post;
using MomesCare.Api.Entities.ViewModel.Comment;

using Profile = MomesCare.Api.Entities.Models.Profile;
using MomesCare.Api.Entities.ViewModel.Profile;
using Microsoft.Extensions.Hosting;
using MomesCare.Api.Entities.ViewModel.Baby;
using Microsoft.IdentityModel.Tokens;
using Firebase.Auth;
using Azure.Core;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MomesCare.Api.ApiClient.Entitis;
using static System.Runtime.InteropServices.JavaScript.JSType;
using MomesCare.Api.Entities.ViewModel.Doctor;


namespace MomesCare.Api.Services
{
    public class ProfileServices
    {

        private readonly IUserClaimsHelper _userClaimsHelper;
        private readonly IPostRepository _postRepositoy;
        private readonly ICommentRepository _commentRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserRepository _userRepositoy;
        private readonly IProfileRepository _repository;

        private readonly IMapper _mapper;



        public ProfileServices(
            IUserClaimsHelper userClaimsHelper,
            IProfileRepository repository,
            IPostRepository postRepositoy,
            IUserRepository userRepositoy,
            IMapper mapper,
            ICommentRepository commentRepository,
              UserManager<ApplicationUser> userManager)
        {

            this._repository = repository;
            this._postRepositoy = postRepositoy;
            this._userRepositoy = userRepositoy;
            this._mapper = mapper;
            this._userClaimsHelper = userClaimsHelper;
            _commentRepository = commentRepository;
            this._userManager = userManager;
        }


        private bool hasAuothorize(string userId)
        {
            if (userId != _userClaimsHelper.UserId)
                throw new Exception("no authorize !!");
            return true;
        }


        public async Task<List<PostIndex>> getMyPostAsync()
        {

            var posts = await _postRepositoy.GetQueryable(x => x.user.Id == _userClaimsHelper.UserId)
             .Include(x => x.user)
                 .ThenInclude(x => x.profile)
             .Include(x => x.likes)
                 .ThenInclude(x => x.user)
             .Include(x => x.comments)
                 .ThenInclude(x => x.user)
                     .ThenInclude(x => x.profile)
             .Include(x => x.comments)
                 .ThenInclude(x => x.likes)
                     .ThenInclude(x => x.user)
             .OrderByDescending(x => x.Id)
             .ToListAsync();



            var items = new List<PostIndex>();
            foreach (var post in posts)
            {
                var _post = await Helper.getPostIndexAsync(post, _userClaimsHelper.UserId, _mapper); ;
                items.Add(_post);
            }

            return items;

        }
        public async Task<List<PostIndex>> getMyFavoritePostsAsync()
        {

            var posts = await _postRepositoy.GetQueryable(x => x.likes.Any(Y=>Y.user.Id == _userClaimsHelper.UserId))
             .Include(x => x.user)
                 .ThenInclude(x => x.profile)
             .Include(x => x.likes)
                 .ThenInclude(x => x.user)
             .Include(x => x.comments)
                 .ThenInclude(x => x.user)
                     .ThenInclude(x => x.profile)
             .Include(x => x.comments)
                 .ThenInclude(x => x.likes)
                     .ThenInclude(x => x.user)
             .OrderByDescending(x => x.Id)
             .ToListAsync();



            var items = new List<PostIndex>();
            foreach (var post in posts)
            {
                var _post = await Helper.getPostIndexAsync(post, _userClaimsHelper.UserId, _mapper); ;
                items.Add(_post);
            }

            return items;

        }
    
        public async Task<ProfileIndex> getInfoAsync(string userId= null)
        {
          
         if(userId==null)
             userId=_userClaimsHelper.UserId;

            var user = await _userRepositoy.GetQueryable(x => x.Id == userId)
             .Include(x => x.babies)
             .Include(x => x.profile)
             .Include(x => x.posts)
                //.ThenInclude(x => x.likes)
                //    .ThenInclude(x => x.post)
             .Include(x => x.postLikes)
                .ThenInclude(x => x.post)
             .Include(x => x.comments)
                //.ThenInclude(x => x.likes)
                //    .ThenInclude(x => x.comment)
             .Include(x => x.commentLikes)
                .ThenInclude(x => x.comment)
             .FirstOrDefaultAsync();

            if(user==null)
                throw new Exception("not found !!");

            var profile = user.profile==null ? new ProfileIndex():_mapper.Map<ProfileIndex>(user.profile);
       
             profile.Babies = _mapper.Map<List<BabyIndex>>(user.babies);
            if (profile.Babies.Count>0)
                profile.Babies = profile.Babies.OrderByDescending(x=>x.BirthDay).ToList();


            profile.LikesGiven= user.postLikes.Where(x=>x.user.Id == _userClaimsHelper.UserId).Count();
            profile.LikesGiven+= user.commentLikes.Where(x=>x.user.Id == _userClaimsHelper.UserId).Count();
            // Total likes on Posts
            profile.LikesReceived = await _postRepositoy.GetQueryable(x => x.user.Id == _userClaimsHelper.UserId).SelectMany(x => x.likes).CountAsync();
            // Total likes on comments
            profile.LikesReceived += await _commentRepository.GetQueryable(x => x.user.Id == _userClaimsHelper.UserId).SelectMany(x => x.likes).CountAsync();
            profile.posts = user.posts.Count();
            profile.comments = user.comments.Count();

            user.profile = null;
            user.babies = null;
            user.posts= null;
            user.comments= null; 
            user.postLikes= null;
            user.commentLikes= null;

            profile.DisplayName = user.FullName;
            profile.Email = user.Email;

            return profile;

        }   
        
        public async Task createAsync(ProfileCreate model)
        {
            //var data = await _repository.GetAsync(x=>x.UserId ==_userClaimsHelper.UserId,includeProperties: "user");

            if (model==null || model.Image == null)
                 throw new Exception("not null !!");


            var profile = _mapper.Map<Profile>(model);
            profile.user = await _repository.getCurrentUserAsync();
            profile.UserId = profile.user.Id;
            

            await _repository.CreateAsync(profile);
        }

        public async Task<ProfileIndex> updateAsync(ProfileUpdate model)
        {
            var profile = await _repository.GetAsync(x=>x.UserId ==_userClaimsHelper.UserId,includeProperties: "user");

            if (model.Image == null)
                throw new Exception("not null !!");

            var newProfile = _mapper.Map<Profile>(model);
             newProfile.UserId = profile.UserId;
             newProfile.user = profile.user;


            var item = await _repository.UpdateAsync(newProfile);
            return  _mapper.Map<ProfileIndex>(item);
        }
   
        public async Task updateDoctorAsync(DoctorUpdate model)
        {
            var user = await _userRepositoy.GetAsync(x => x.doctor.Id == model.Id, includeProperties: "doctor");

            if (user == null)
                throw new Exception("not null !!");

            Doctor newDoctor= _mapper.Map<Doctor>(model);  //= (user.doctor == null) ? new Doctor() : _mapper.Map<Doctor>(model);
            newDoctor.UserId = user.Id;
            newDoctor.user = user;


            if (user.doctor == null)
            {
                newDoctor.Status = true;

               await _repository.CreateAsync(newDoctor);
             
            }
            else
            {
                newDoctor.Status = user.doctor.Status;
                await _repository.UpdateAsync(newDoctor);
            }

        }


        public async Task updateImageAsync(UpdateImage model)
        {
            var profile = await _repository.GetAsync(x=>x.UserId ==_userClaimsHelper.UserId,includeProperties: "user");

            if (model.UrlImage == null)
                throw new Exception("not null !!");

            Profile newProfile;

            if (profile != null)
            {
                profile.Image = model.UrlImage;
                newProfile = await _repository.UpdateAsync(profile);
            }
            else
            {
                newProfile = new Profile
                {
                    UserId = _userClaimsHelper.UserId,
                    Image = model.UrlImage,
                    user = await _repository.getCurrentUserAsync(),
                };

                await _repository.CreateAsync(newProfile);
            
            }
    
        }

        public async Task<BaseResponse> changePasswordAsync(ChangePassword model)
        {
            try
            {

                //string currentPassword, string newPassword
                BaseResponse baseResponse = new BaseResponse();
               

                if(model!=null)
                {

                    var user = await _userManager.FindByIdAsync(_userClaimsHelper.UserId);

                    if (user != null)
                    {

                        if (await _userManager.CheckPasswordAsync(user, model.currentPassword))
                        {
                            var response = await _userManager.ChangePasswordAsync(user, model.currentPassword, model.newPassword);

                            if (response == null || (response.Errors != null && response.Errors.Count() > 0))
                            {
                                baseResponse.ErrorsMessage= new List<string>();
                                foreach (var er in response.Errors)
                                    baseResponse.ErrorsMessage.Add(er.Description);

                            }
                            else if (response.Succeeded)
                            {
                                baseResponse.Result = response.Succeeded;
                            }
                               
                        }
                        else
                             baseResponse.ErrorsMessage.Add("Invalid password !!");

                    }
                    else
                    baseResponse.ErrorsMessage.Add("Invalid username !!");
                }
                else
                  baseResponse.ErrorsMessage.Add(" data is null!!");
             
                
                return baseResponse;

            }
            catch (Exception ex)
            {
                throw new  Exception(ex.Message);
            }

          
        }
        public async Task updateEmailAsync(UpdateEmail email)
        {
            try
            {
             
                    if (email!=null && !email.newEmail.IsNullOrEmpty())
                    {

                    var user =  await _userRepositoy.GetAsync(x => x.Id == _userClaimsHelper.UserId);

                        if (user != null)
                        {
                            
                            user.Email = email.newEmail;
                            user.UserName = email.newEmail;
                            user.EmailConfirmed = true;
                            user.NormalizedEmail = email.newEmail;

                            var response = await _userRepositoy.UpdateAsync(user);


                        }
                        else

                            throw new Exception("Invalid email !! ");

                    }
                    else
                        
                        throw new Exception("Invalid email !! ");

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }
        public async Task updateNameAsync(UserNameUpdate model)
        {
            try
            {

                if (model!=null && !model.name.IsNullOrEmpty())
                {
                    var user = await _userRepositoy.GetAsync(x => x.Id == _userClaimsHelper.UserId);

                    if (user != null)
                    {
                        (user.FirstName, user.LastName) =Helper.SplitName(model.name);
                        var response = await _userRepositoy.UpdateAsync(user);

                    }
                    else

                        throw new Exception("Invalid user!!");
                }
                else

                    throw new Exception("value is  empty !! ");

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }
        public async Task deleteAsync()
        {
            var profile = await _repository.GetAsync(x => x.user.Id == _userClaimsHelper.UserId,includeProperties:"user");
            if (profile == null)
                throw new  Exception("not found !!");

            hasAuothorize(profile.user.Id);

            await _repository.RemoveAsync(profile);
            
        }


    }
}
