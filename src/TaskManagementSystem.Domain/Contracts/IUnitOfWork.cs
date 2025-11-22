using Domain.Entities.TaskModels;

namespace Domain.Contracts
{
    public interface IUnitOfWork : IDisposable
    {


        public Task<int> SaveChanges();





        public IGenericRepository<TEntity, TKey> GetRepository<TEntity, TKey>() where TEntity : BaseEntity<TKey>;
    }
}
