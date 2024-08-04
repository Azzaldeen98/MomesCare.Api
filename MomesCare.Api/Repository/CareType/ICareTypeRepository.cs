using Microsoft.EntityFrameworkCore;
using MomesCare.Api.Repository.Base;
using MomesCare.Api.Entities.Models;

namespace MomesCare.Api.Repository
{
    public interface ICareTypeRepository : IRepository<CareType>
    {
        public DbSet<CareType> DbSet { get; }
        public Task<CareType> UpdateAsync(CareType entity);
        public Task CreateAsync(CareType entity);
        public Task RemoveAsync(CareType entity);

    }
}
