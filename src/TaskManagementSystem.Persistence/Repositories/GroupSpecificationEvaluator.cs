using Domain.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories
{
    internal class GroupSpecificationEvaluator<T, Tkey, TResult> where T : class
    {

        public static IQueryable<TResult> GetQuery(IQueryable<T> inputQuery, GroupSpecification<T, Tkey, TResult> specifications)
        {
            var query = inputQuery;

            // 1. Filter first (WHERE clause)
            if (specifications.Criteria != null)
                query = query.Where(specifications.Criteria);

            // 2. Includes (if absolutely needed for filtering)
            foreach (var item in specifications.IncludeExpressions)
                query = query.Include(item);

            if (specifications.OrderBy != null)
                query = query.OrderBy(specifications.OrderBy);

            else if (specifications.OrderByDescending != null)
                query = query.OrderByDescending(specifications.OrderByDescending);



            foreach (var item in specifications.ThenIncludes)
                query = item(query);


            if (specifications.isPaginated)
                query = query.Skip(specifications.Skip).Take(specifications.Take);

            // 3. Group and project
            var groupedQuery = query.GroupBy(specifications.GroupBy)
                                  .Select(specifications.GroupSelector);

            // 4. Apply result ordering (if specified)
            if (specifications.ResultOrderBy != null)
                groupedQuery = groupedQuery.OrderBy(specifications.ResultOrderBy);


            else if (specifications.ResultOrderByDescending != null)
                groupedQuery = groupedQuery.OrderByDescending(specifications.ResultOrderByDescending);




            return groupedQuery;


        }
    }
}
