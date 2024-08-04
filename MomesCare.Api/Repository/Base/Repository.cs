
using Microsoft.EntityFrameworkCore;
using MomesCare.Api.Entities.Models;
using MomesCare.Api.Entities.ViewModel;
using MomesCare.Api.Helpers;
using System.Linq.Expressions;

namespace MomesCare.Api.Repository.Base
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly DataContext _db;
        private readonly IUserClaimsHelper _userClaimsHelper;
        internal DbSet<T> _dbSet;

        public Repository(DataContext db,IUserClaimsHelper userClaimsHelper)
        {
            _db = db;
            this._userClaimsHelper = userClaimsHelper;
            _dbSet = _db.Set<T>();
        }

        public virtual async Task CreateAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            await SaveAsync();
        }

        public async Task<T> GetAsync(Expression<Func<T, bool>> filter = null, bool tracked = true, string? includeProperties = null)
        {
            IQueryable<T> query = _dbSet;
            if (!tracked)
            {
                query = query.AsNoTracking();
            }
            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (includeProperties != null)
            {
                foreach (var includeProp in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProp);
                }
            }
            return await query.FirstOrDefaultAsync();
        }

        public async Task<List<T>> GetAllAsync(Expression<Func<T, bool>> filter = null, string includeProperties = null,
            int skip = 0, int take = 1)
        {
            IQueryable<T> query = _dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }
            if (skip > 0)
            {
                query = query.Skip(skip).Take(take);
            }

            if (includeProperties != null)
            {
                foreach (var includeProp in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProp);
                }
            }
            return await query.ToListAsync();
        }

        public IQueryable<T> GetQueryable(Expression<Func<T, bool>>? filter = null, string? includeProperties = null,
        int skip = 0, int take = 1)
        {
            IQueryable<T> query = _dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }
            if (skip > 0)
            {
                query = query.Skip(skip).Take(take);
            }

            if (includeProperties != null)
            {
                foreach (var includeProp in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProp);
                }
            }
            return query;
        }


        public IQueryable<R> GetAllIncludes<R>(IQueryable<R> query, string? includeProperties = null,
        int skip = 0, int take = 1)
        {
      
            if (skip > 0)
            {
                query = query.Skip(skip).Take(take);
            }

            if (includeProperties != null)
            {
                foreach (var includeProp in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    //query = query.Include(includeProp);
                }
            }
            return query;
        }

        public async Task RemoveAsync(T entity)
        {
            _dbSet.Remove(entity);
            await SaveAsync();
        }

        public async Task<int> SaveAsync()
        {
            return await _db.SaveChangesAsync();
        }

        public async Task RemoveRange(List<T> entities)
        {
            _dbSet.RemoveRange(entities);
            await SaveAsync();
        }


        public async Task<ApplicationUser> getCurrentUserAsync()
        {
           
             var user= await _db.Users.FindAsync(_userClaimsHelper.UserId);
            return user!;
        }


        public async Task<ApplicationUser> getUserAsync(string userId)
        {
          return await _db.Users.FindAsync(userId);
            
      
        }


   

    }
}
