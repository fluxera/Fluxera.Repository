namespace Fluxera.Repository.Query
{
	using System;
	using System.Collections.Generic;
	using System.Linq.Expressions;
	using JetBrains.Annotations;

	[PublicAPI]
	public interface ISortingOptions<T> : IQueryOptions<T> where T : class
	{
		ISortExpression<T> PrimaryExpression { get; }

		IEnumerable<ISortExpression<T>> SecondaryExpressions { get; }

		ISortingOptions<T> ThenBy(Expression<Func<T, object>> sortExpression);

		ISortingOptions<T> ThenByDescending(Expression<Func<T, object>> sortExpression);

		IPagingOptions<T> Paging(int pageNumber, int pageSize);

		IPagingOptions<T> Paging();

		ISkipTakeOptions<T> Skip(int skip);

		ISkipTakeOptions<T> Take(int take);
	}
}
