using System.Linq.Expressions;

namespace Domain.Contracts
{
    public abstract class ProjectionSpecifications<T, TResult> : BaseSpecifications<T> where T : class
    {
        public ProjectionSpecifications(Expression<Func<T, bool>>? Criteria) : base(Criteria)
        {
        }

        public Expression<Func<T, TResult>> Projection { get; private set; }



        public Expression<Func<TResult, object>>? ResultOrderBy { get; private set; }
        public Expression<Func<TResult, object>>? ResultOrderByDescending { get; private set; }

        protected void AddProjection(Expression<Func<T, TResult>> projection)
        {
            Projection = projection;
        }


        protected void SetResultOrderBy(Expression<Func<TResult, object>> expression)
 => ResultOrderBy = expression;

        protected void SetResultOrderByDescending(Expression<Func<TResult, object>> expression)
            => ResultOrderByDescending = expression;


    }
}
