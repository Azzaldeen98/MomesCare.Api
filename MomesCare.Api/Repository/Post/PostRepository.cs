using Microsoft.EntityFrameworkCore;
using MomesCare.Api;
using MomesCare.Api.Entities.Models;
using MomesCare.Api.Helpers;
using MomesCare.Api.Repository.Base;



namespace MomesCare.Api.Repository
{
    public class PostRepository : Repository<Post>, IPostRepository
    {


        private readonly DataContext _db;
        private readonly IUserClaimsHelper _userClaimsHelper;

        public DbSet<Post> DbSet { get => _db.Posts; }
        public DbSet<PostLike> DbSetLike { get => _db.PostLikes; }

        public PostRepository(DataContext db, IUserClaimsHelper userClaimsHelper) : base(db,userClaimsHelper)
        {
            _db = db;
            this._userClaimsHelper = userClaimsHelper;
        }

        public  async Task CreateAsync(Post entity)
        {

            entity.PublishedAt = Helper.GetCurrentTime();

            await _dbSet.AddAsync(entity);
            await _db.SaveChangesAsync();

        }

        public async Task<Post> UpdateAsync(Post entity)
        {
            _db.ChangeTracker.Clear();
            DbSet.Update(entity);
            await _db.SaveChangesAsync();
            return entity;
        }

        public async Task RemoveAsync(Post entity)
        {
            DbSet.Remove(entity);
            await _db.SaveChangesAsync();
        }  
        
        public async Task LikeAsync(PostLike entity)
        {

            entity.user = await getCurrentUserAsync();
            entity.CreatedAt = Helper.GetCurrentTime();
            await DbSetLike.AddAsync(entity);
            await _db.SaveChangesAsync();
        }
        public async Task UnLikeAsync(int postId)
        {
            var entity= await DbSetLike.Include(x=>x.post).Include(x=>x.user)
                .FirstOrDefaultAsync(x => x.post.Id == postId && x.user.Id== _userClaimsHelper.UserId);

            if (entity == null)
                return;

            DbSetLike.Remove(entity);
            await _db.SaveChangesAsync();

        
        }


    }
}
