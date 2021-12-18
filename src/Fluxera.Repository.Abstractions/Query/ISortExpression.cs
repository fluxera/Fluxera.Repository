namespace Fluxera.Repository.Query
{
	using System;
	using System.Linq;
	using System.Linq.Expressions;

	public interface ISortExpression<T> where T : class
	{
		Expression<Func<T, object>> Expression { get; }

		bool IsDescending { get; }

		IOrderedQueryable<T> ApplyTo(IQueryable<T> queryable);

		IQueryable<T> ApplyTo(IOrderedQueryable<T> queryable);
	}
}
