using Microsoft.EntityFrameworkCore;
using MomesCare.Api.Repository.Base;
using MomesCare.Api.Entities.Models;

namespace MomesCare.Api.Repository
{
    public interface ICommentRepository : IRepository<Comment>
    {
        public DbSet<Comment> DbSet { get; }
        public Task<Comment> UpdateAsync(Comment entity);
        public Task CreateAsync(Comment entity);
        public Task RemoveAsync(Comment entity);
        public Task LikeAsync(CommentLike entity);
        public Task UnLikeAsync(int commentId);
    }
}
