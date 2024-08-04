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
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Google.Api.Gax;

namespace MomesCare.Api.Services
{
    public class PostServices
    {

        private readonly IUserClaimsHelper _userClaimsHelper;
        private readonly IPostRepository _repository;
        private readonly IMapper _mapper;


        public PostServices(
            IUserClaimsHelper  userClaimsHelper, 
            IPostRepository repository, 
            IMapper mapper)
        {

            this._repository = repository;
            this._mapper = mapper;
            this._userClaimsHelper = userClaimsHelper;

        }

        private bool hasAuothorize(string userId)
        {
            if (userId != _userClaimsHelper.UserId)
                throw new Exception("no authorize !!");
            return true;
        }

 


        

        public async Task createAsync(PostCreate model)
        {
            var post = _mapper.Map<Post>(model);
            post.user =await _repository.getCurrentUserAsync();

            hasAuothorize(post.user.Id??"");

            await _repository.CreateAsync(post);

        }   
        
        public async Task<bool> likeUnlikeAsync(int postId)
        {
            var post = await _repository.GetQueryable(x => x.Id == postId)
                .Include(x=>x.likes)
                    .ThenInclude(x=>x.user)
                .FirstOrDefaultAsync();

            if(post==null) throw new Exception("not found !!");

            bool userLinked = post.likes.Any(x => x.user.Id == _userClaimsHelper.UserId);

            if (userLinked)
                await _repository.UnLikeAsync(postId);
            else
                await _repository.LikeAsync(new PostLike { post = post });


            return !userLinked;


        }   
        
        public async Task<PostIndex> updateAsync(PostUpdate model)
        {
            var post = await _repository.GetAsync(x=>x.Id== model.Id,includeProperties:"user");
            if (post == null)
                throw new  Exception("not found !!");

            hasAuothorize(post.user.Id);

            post.Title = model.Title;
            post.Body = model.Body;

            var item = await _repository.UpdateAsync(post);
            var res = _mapper.Map<PostIndex>(item);
    
            return res;

        }   

    



        public async Task<IEnumerable<PostIndex>> getAllAsync()
        {
            var query = await _repository.GetQueryable()
                .Include(x=>x.user)
                    .ThenInclude(x=>x.profile)
                .Include(x => x.likes)
                    .ThenInclude(x => x.user)
                .Include(x => x.comments)
                    .ThenInclude(x => x.user)
                        .ThenInclude(x => x.profile)
                .Include(x => x.comments)
                    .ThenInclude(x => x.likes)
                        .ThenInclude(x => x.user)
                .OrderByDescending(x=>x.Id)
                .ToListAsync();

            var items = new  List<PostIndex>();
            foreach (var post in query)
            {
                var _post =await Helper.getPostIndexAsync(post,_userClaimsHelper.UserId,_mapper);
                items.Add(_post);
            }
                  
            return items;

        }      

        
        public async Task<PostIndex> getOne(int id)
        {

            var post =  await _repository.GetQueryable(x=>x.Id==id)
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
                .FirstOrDefaultAsync();

           

            //var post = await _repository.GetQueryable(X => X.Id == id, includeProperties: "comments,likes")
            //    .Include(x => x.user)
            //    .ThenInclude(x => x.profile);

            if (post==null)
                throw new Exception("not found !!");

            var postIndex = await Helper.getPostIndexAsync(post, _userClaimsHelper.UserId, _mapper);
            //var postIndex = _mapper.Map<PostIndex>(post);

            //postIndex.likes= post.likes.Count();
            //postIndex.comments= _mapper.Map<List<CommentIndex>>(post.comments) ?? Array.Empty<CommentIndex>().ToList();
    

            return postIndex;
        
        }      
        
        public async Task deleteAsync(int id)
        {
            var post = await _repository.GetAsync(x => x.Id == id,includeProperties:"user");
            if (post == null)
                throw new  Exception("not found !!");

            hasAuothorize(post.user.Id);

            await _repository.RemoveAsync(post);
            
        }


    }
}
