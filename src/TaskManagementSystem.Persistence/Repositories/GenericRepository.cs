using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Contracts;
using Domain.Entities.TaskModels;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;
using System.Linq.Expressions;
using IConfigurationProvider = AutoMapper.IConfigurationProvider;


namespace Persistence.Repositories
{
    public class GenericRepository<TEntity, TKey> : IGenericRepository<TEntity, TKey> where TEntity : BaseEntity<TKey>
    {

        private readonly ApplicationDbContext _context;
        private readonly IConfigurationProvider _mapperConfig;


        public GenericRepository(ApplicationDbContext context, IMapper mapper)
        {


            this._context = context;
            _mapperConfig = mapper.ConfigurationProvider;
        }
        public async Task<IEnumerable<TEntity>> GetAllAsync(bool AsNotTracking = true)
        {

            if (AsNotTracking)
            {
                return await _context.Set<TEntity>().AsNoTracking().ToListAsync();

            }

            return await _context.Set<TEntity>().ToListAsync();

        }



        public async Task<IEnumerable<TEntity>> GetAllWithSpecifications(BaseSpecifications<TEntity> specifications, bool AsNotTracking = true)
        {
            if (AsNotTracking)
            {

                return await ApplySpecifications(specifications).AsNoTracking().ToListAsync();
            }


            return await ApplySpecifications(specifications).ToListAsync();
        }



        public async Task<IEnumerable<T>> GetAllProjectedAsync<T>() where T : class
        {


            return await _context.Set<TEntity>().ProjectTo<T>(_mapperConfig).ToListAsync();

        }


        public async Task<T?> GetWithIdProjectedAsync<T>(TKey id) where T : class
        {


            return await _context.Set<TEntity>().ProjectTo<T>(_mapperConfig).FirstOrDefaultAsync();

        }


        public async Task<IEnumerable<T>> GetAllProjectedAsync<T>(BaseSpecifications<TEntity> specifications) where T : class
        {

            var result = ApplySpecifications(specifications);
            return await result.ProjectTo<T>(_mapperConfig).ToListAsync();

        }


        public async Task AddAsync(TEntity entity)
        {
            await _context.Set<TEntity>().AddAsync(entity);
        }

        public void Delete(TEntity entity)
        {


            _context.Set<TEntity>().Remove(entity);
        }


        public async Task<TEntity?> GetByIdAsync(TKey id)
        {


            return await _context.Set<TEntity>().FindAsync(id);
        }

        public void Update(TEntity entity)
        {


            _context.Set<TEntity>().Update(entity);
        }





        public async Task<IEnumerable<TResult>> GetAllWithProjectionSpecifications<TResult>(ProjectionSpecifications<TEntity, TResult> specifications)
        {



            return await ApplyProjectionSpecifications(specifications).ToListAsync();
        }

        public async Task<TEntity?> GetWithIdSpecifications(BaseSpecifications<TEntity> specifications)
        {


            return await ApplySpecifications(specifications).FirstOrDefaultAsync();
        }



        public async Task<TResult?> GetWithIdProjectionSpecifications<TResult>(ProjectionSpecifications<TEntity, TResult> specifications)
        {
            return await ApplyProjectionSpecifications(specifications).FirstOrDefaultAsync();
        }

        public IQueryable<TEntity> ApplySpecifications(BaseSpecifications<TEntity> specifications)
        {


            return BaseSpecificationsEvaluator<TEntity>.GetQuery(_context.Set<TEntity>(), specifications);
        }


        public IQueryable<TResult> ApplyProjectionSpecifications<TResult>(ProjectionSpecifications<TEntity, TResult> specifications)
        {


            return ProjectionSpecificationsEvaluator<TEntity, TResult>.GetQuery(_context.Set<TEntity>(), specifications);
        }

        public async Task<TEntity?> FindAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _context.Set<TEntity>().FirstOrDefaultAsync(predicate);
        }

        public async Task<IEnumerable<TEntity>> FindAllAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _context.Set<TEntity>().Where(predicate).ToListAsync();

        }


        public async Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate)
        {

            return await _context.Set<TEntity>().AnyAsync(predicate);
        }

        public async Task<int> CountAsync(BaseSpecifications<TEntity> specifications)
        {

            var query = _context.Set<TEntity>().AsQueryable();

            if (specifications.Criteria != null)
                query = query.Where(specifications.Criteria);


            return await query.CountAsync();
        }

        public async Task DeleteRange(Expression<Func<TEntity, bool>> predicate)
        {


            var entities = await _context.Set<TEntity>().Where(predicate).ToListAsync();
            _context.Set<TEntity>().RemoveRange(entities);
        }

        public void DeleteRange(IEnumerable<TEntity> items)
        {

            _context.Set<TEntity>().RemoveRange(items);
        }


        private IQueryable<TResult> ApplyGroupingSpecifications<TResult, Key>(IQueryable<TEntity> query, GroupSpecification<TEntity, Key, TResult> specification)
        {

            return GroupSpecificationEvaluator<TEntity, Key, TResult>.GetQuery(query, specification);
        }

        public async Task<IEnumerable<TResult>> GetAllWithGrouping<TResult, Key>(GroupSpecification<TEntity, Key, TResult> specification)
        {

            return await ApplyGroupingSpecifications(_context.Set<TEntity>(), specification).ToListAsync();
        }
    }
}
