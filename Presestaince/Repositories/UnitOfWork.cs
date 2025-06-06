using AutoMapper;
using Domain.Contracts;
using Domain.Entities.TaskModels;
using Persistence.Data;
using System.Collections.Concurrent;

namespace Persistence.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {


        private readonly ApplicationDbContext _context;

        private readonly IMapper _mapper;


        private ConcurrentDictionary<string, object> Repositories;


        public UnitOfWork(ApplicationDbContext context, IMapper mapper)
        {

            _context = context;

            _mapper = mapper;

            Repositories = new ConcurrentDictionary<string, object>();
        }


        public async Task<int> SaveChanges()
        {



            return await _context.SaveChangesAsync();
        }

        public IGenericRepository<TEntity, TKey> GetRepository<TEntity, TKey>() where TEntity : BaseEntity<TKey>
        {


            return (IGenericRepository<TEntity, TKey>)Repositories.GetOrAdd(typeof(TEntity).Name, (_) => new GenericRepository<TEntity, TKey>(_context, _mapper));
        }

        public void Dispose()
        {


            _context.Dispose();

        }
    }
}
