using Domain.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories
{
    class ProjectionSpecificationsEvaluator<T, TResult> where T : class
    {
        public static IQueryable<TResult> GetQuery(IQueryable<T> inputQuery, ProjectionSpecifications<T, TResult> specifications)
        {
            var query = inputQuery;


            if (specifications.Criteria != null)
                query = query.Where(specifications.Criteria);




            if (specifications.OrderBy != null)
            {

                query = query.OrderBy(specifications.OrderBy);
            }




            else if (specifications.OrderByDescending != null)
            {

                query = query.OrderByDescending(specifications.OrderByDescending);
            }


            foreach (var item in specifications.IncludeExpressions)
            {

                query = query.Include(item);

            }

            foreach (var item in specifications.ThenIncludes)
            {
                query = item(query);
            }

            //item(query) applies the function to your query(i.e., it modifies the query by adding .Include() and .ThenInclude()).


            if (specifications.isPaginated)
            {

                query = query.Skip(specifications.Skip).Take(specifications.Take);
            }


            return query.Select(specifications.Projection);


        }
    }

}
