using Microsoft.Data.SqlClient;
using MomesCare.Api.Entities.Models;
using System.Linq.Expressions;

namespace MomesCare.Api.Repository.Base
{
    public interface IRepository<T> where T : class
    {
        Task<List<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null, string? includeProperties = null,
            int pageSize = 0, int pageNumber = 1);
        IQueryable<T> GetQueryable(Expression<Func<T, bool>>? filter = null, string? includeProperties = null,
      int skip = 0, int take = 1);

        Task<T> GetAsync(Expression<Func<T, bool>> filter = null, bool tracked = true, string? includeProperties = null);
        IQueryable<R> GetAllIncludes<R>(IQueryable<R> query, string? includeProperties = null,
    int skip = 0, int take = 1);

        Task CreateAsync(T entity);
        Task RemoveAsync(T entity);
        Task RemoveRange(List<T> entities);
        Task<int> SaveAsync();

        public  Task<ApplicationUser> getCurrentUserAsync();
        public Task<ApplicationUser> getUserAsync(string userId);

    }
}
