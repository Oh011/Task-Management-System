using System.Linq.Expressions;

namespace Domain.Contracts
{
    public abstract class BaseSpecifications<T> where T : class
    {


        public Expression<Func<T, bool>>? Criteria { get; }






        public List<Expression<Func<T, object>>> IncludeExpressions { get; } = new();


        public List<Func<IQueryable<T>, IQueryable<T>>> ThenIncludes { get; } = new();






        // 3. Order By Ascending

        public Expression<Func<T, object>>? OrderBy { get; private set; }


        // 3. Order By Descending


        public Expression<Func<T, object>>? OrderByDescending { get; private set; }



        public int Take { get; private set; }

        public int Skip { get; private set; }


        public bool isPaginated { get; set; }


        public BaseSpecifications(Expression<Func<T, bool>>? Criteria)
        {

            this.Criteria = Criteria;
        }




        protected void AddInclude(Expression<Func<T, object>> Expression)
          => IncludeExpressions.Add(Expression);

        protected void AddThenInclude(Func<IQueryable<T>, IQueryable<T>> includeExpression)
    => ThenIncludes.Add(includeExpression);






        protected void SetOrderBy(Expression<Func<T, object>> Expression) => OrderBy = Expression;



        protected void SetOrderByDescending(Expression<Func<T, object>> Expression) => OrderByDescending = Expression;

        protected void ApplyPagination(int pageIndex, int pageSize)
        {

            isPaginated = true;
            Take = pageSize;

            Skip = (pageIndex - 1) * pageSize;
        }


    }




}
