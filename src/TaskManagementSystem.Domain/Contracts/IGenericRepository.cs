using Domain.Entities.TaskModels;
using System.Linq.Expressions;

namespace Domain.Contracts
{
    public interface IGenericRepository<TEntity, TKey> where TEntity : BaseEntity<TKey>
    {


        Task<IEnumerable<TEntity>> GetAllAsync(bool AsNotTracking = true);

        Task<IEnumerable<TEntity>> GetAllWithSpecifications(BaseSpecifications<TEntity> specifications, bool AsNotTracking = true);


        Task<IEnumerable<T>> GetAllProjectedAsync<T>() where T : class;


        Task<IEnumerable<T>> GetAllProjectedAsync<T>(BaseSpecifications<TEntity> specifications) where T : class;



        Task<IEnumerable<TResult>> GetAllWithProjectionSpecifications<TResult>(ProjectionSpecifications<TEntity, TResult> specifications);



        Task<IEnumerable<TResult>> GetAllWithGrouping<TResult, Key>(GroupSpecification<TEntity, Key, TResult> specification);

        Task<TEntity?> GetByIdAsync(TKey id);

        Task<T?> GetWithIdProjectedAsync<T>(TKey id) where T : class;


        Task<TEntity?> GetWithIdSpecifications(BaseSpecifications<TEntity> specifications);

        Task<TResult?> GetWithIdProjectionSpecifications<TResult>(ProjectionSpecifications<TEntity, TResult> specifications);

        Task AddAsync(TEntity entity);


        void Update(TEntity entity);
        void Delete(TEntity entity);


        public Task<TEntity?> FindAsync(Expression<Func<TEntity, bool>> predicate);
        public Task<IEnumerable<TEntity>> FindAllAsync(Expression<Func<TEntity, bool>> predicate);



        Task DeleteRange(Expression<Func<TEntity, bool>> predicate);
        void DeleteRange(IEnumerable<TEntity> items);


        Task<int> CountAsync(BaseSpecifications<TEntity> specifications);

        Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate);

















    }
}
