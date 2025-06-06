using Domain.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories
{
    static class BaseSpecificationsEvaluator<T> where T : class
    {




        public static IQueryable<T> GetQuery<T>(IQueryable<T> InputQuery, BaseSpecifications<T> specifications) where T : class
        {

            var query = InputQuery;


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
            return query;

        }


    }
}
