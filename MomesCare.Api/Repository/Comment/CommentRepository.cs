using Microsoft.EntityFrameworkCore;
using MomesCare.Api;
using MomesCare.Api.Entities.Models;
using MomesCare.Api.Helpers;
using MomesCare.Api.Repository.Base;



namespace MomesCare.Api.Repository
{
    public class CommentRepository : Repository<Comment>, ICommentRepository
    {


        private readonly DataContext _db;
        private readonly IUserClaimsHelper userClaimsHelper;

        public DbSet<Comment> DbSet { get => _db.Comments; }
        public DbSet<CommentLike> DbSetLike { get => _db.CommentLikes; }
        public CommentRepository(DataContext db, IUserClaimsHelper userClaimsHelper) : base(db,userClaimsHelper)
        {
            _db = db;
            this.userClaimsHelper = userClaimsHelper;
        }

        public  async Task CreateAsync(Comment entity)
        {

            entity.CreatedAt = Helper.GetCurrentTime();

            await _dbSet.AddAsync(entity);
            await _db.SaveChangesAsync();

        }

        public async Task<Comment> UpdateAsync(Comment entity)
        {
            _db.ChangeTracker.Clear();
            DbSet.Update(entity);
            await _db.SaveChangesAsync();
            return entity;
        }

        public async Task RemoveAsync(Comment entity)
        {
            DbSet.Remove(entity);
            await _db.SaveChangesAsync();
        }


        public async Task LikeAsync(CommentLike entity)
        {


            entity.user = await getCurrentUserAsync();
            entity.CreatedAt = Helper.GetCurrentTime();
            await DbSetLike.AddAsync(entity);
            await _db.SaveChangesAsync();
        }


        public async Task UnLikeAsync(int commentId)
        {
            var entity = await DbSetLike.Include(x => x.comment).Include(x => x.user)
                .FirstOrDefaultAsync(x => x.comment.Id == commentId && x.user.Id == userClaimsHelper.UserId);

            if (entity == null)
                return;

            DbSetLike.Remove(entity);
            await _db.SaveChangesAsync();

        }



    }
}
