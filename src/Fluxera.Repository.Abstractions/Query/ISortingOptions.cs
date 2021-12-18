namespace Fluxera.Repository.Query
{
	using System;
	using System.Linq.Expressions;

	public interface ISortingOptions<T> : IQueryOptions<T> where T : class
	{
		ISortingOptions<T> ThenBy(Expression<Func<T, object>> sortExpression);

		ISortingOptions<T> ThenByDescending(Expression<Func<T, object>> sortExpression);

		IPagingOptions<T> Paging(int pageNumber, int pageSize);

		IPagingOptions<T> Paging();

		ISkipTakeOptions<T> Skip(int skip);

		ISkipTakeOptions<T> Take(int take);
	}
}
