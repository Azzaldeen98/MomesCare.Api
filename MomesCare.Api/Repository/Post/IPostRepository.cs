using Microsoft.EntityFrameworkCore;
using MomesCare.Api.Repository.Base;
using MomesCare.Api.Entities.Models;

namespace MomesCare.Api.Repository
{
    public interface IPostRepository : IRepository<Post>
    {
        public DbSet<Post> DbSet { get; }
        public Task<Post> UpdateAsync(Post entity);
        public Task CreateAsync(Post entity);
        public Task RemoveAsync(Post entity);
        public Task LikeAsync(PostLike entity);
        public Task UnLikeAsync(int postId);



    }
}
