using AutoMapper;
using Microsoft.EntityFrameworkCore;

using MomesCare.Api.Repository;
using IdentityModel;
using System.Collections.Generic;
using MomesCare.Api.Helpers;
using MomesCare.Api.Entities.ViewModel;
using MomesCare.Api.Entities.Models;
using MomesCare.Api.Entities.ViewModel.Comment;
using MomesCare.Api.Entities.ViewModel.Comment;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;

namespace MomesCare.Api.Services
{
    public class CommentServices
    {

        private readonly IUserClaimsHelper _userClaimsHelper;
        private readonly ICommentRepository _repository;
        private readonly IPostRepository _postRepository;
        private readonly IMapper _mapper;

        public CommentServices(
                                IPostRepository postRepository,
                                ICommentRepository repository,
                                IUserClaimsHelper userClaimsHelper,
                                IMapper mapper)
        {
            _postRepository = postRepository;
            _repository = repository;
            _userClaimsHelper = userClaimsHelper;
            _mapper = mapper;
        }




        private bool hasAuothorize(string userId)
        {
            if (userId != _userClaimsHelper.UserId)
                throw new Exception("no authorize !!");
            return true;
        }

        private  CommentIndex getCommentIndex(Comment comit)
        {
            var com = _mapper.Map<CommentIndex>(comit);
            com.author = Author.fromUser(comit.user);
            com.likes = (comit == null || comit.likes.IsNullOrEmpty()) ? 0 : comit.likes.Count();
            com.userLiked = comit.likes == null ? false : comit.likes.Any(x => x.user.Id == _userClaimsHelper.UserId);
            return com;
        }


        public async Task<bool> likeUnlikeAsync(int id)
        {
            var comment = await _repository.GetQueryable(x => x.Id == id)
                .Include(x => x.likes)
                    .ThenInclude(x => x.user)
                .FirstOrDefaultAsync();

            if (comment == null) throw new Exception("not found !!");


            bool userLinked = comment.likes.Any(x => x.user.Id == _userClaimsHelper.UserId);

            if (userLinked)
                await _repository.UnLikeAsync(id);
            else
                await _repository.LikeAsync(new CommentLike { comment = comment });


            return !userLinked;

        
        }

        public async Task createAsync(CommentCreate model)
        {

            var post = await _postRepository.GetAsync(x => x.Id == model.postId);
            if (post == null)
                throw new Exception("not found !!");

            var comment = _mapper.Map<Comment>(model);
            comment.user =await _repository.getCurrentUserAsync();
            comment.post = post;

            await _repository.CreateAsync(comment);

        }   
        
        public async Task<CommentIndex> updateAsync(CommentUpdate model)
        {
            var comment = await _repository.GetAsync(x=>x.Id== model.Id,includeProperties:"user");
            if (comment == null)
                throw new  Exception("not found !!");

            hasAuothorize(comment.user.Id);

            comment.Contant = model.Contant;
      

            var item = await _repository.UpdateAsync(comment);
            var res = _mapper.Map<CommentIndex>(item);
    
            return res;

        }   

        public async Task<IEnumerable<CommentIndex>> getAllCommentsAsync(int postId)
        {
            var query = await _repository.GetQueryable(x=>x.post.Id== postId, includeProperties:"post")
                .Include(x=>x.user)
                    .ThenInclude(x=>x.profile)
                .Include(x => x.likes)
                    .ThenInclude(x => x.user)
                .OrderByDescending(x=>x.Id)
                .ToListAsync();

            var items = new  List<CommentIndex>();
            foreach (var comment in query)
            {
                comment.post = null;
                var _Comment = getCommentIndex(comment);
                items.Add(_Comment);
            }
                  
            return items;

        }      

        
        public async Task<CommentIndex> getOne(int id)
        {

            var Comment =  await _repository.GetQueryable(x=>x.Id==id)
                .Include(x => x.user)
                    .ThenInclude(x => x.profile)
                .Include(x => x.likes)
                    .ThenInclude(x => x.user)
                .OrderByDescending(x => x.Id)
                .FirstOrDefaultAsync();

          
            if (Comment==null)
                throw new Exception("not found !!");

            var commentIndex = getCommentIndex(Comment);


            return commentIndex;
        
        }      
        
        public async Task deleteAsync(int id)
        {
            var Comment = await _repository.GetAsync(x => x.Id == id,includeProperties:"user");
            if (Comment == null)
                throw new  Exception("not found !!");

            hasAuothorize(Comment.user.Id);

            await _repository.RemoveAsync(Comment);
            
        }


    }
}
