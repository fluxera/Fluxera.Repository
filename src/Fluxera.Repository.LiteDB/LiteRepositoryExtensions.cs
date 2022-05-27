namespace Fluxera.Repository.LiteDB
{
	using System;
	using Fluxera.Repository.Query;
	using global::LiteDB.Async;

	internal static class LiteRepositoryExtensions
	{
		internal static ILiteQueryableAsync<T> Apply<T>(this ILiteQueryableAsync<T> queryable, IQueryOptions<T> options) where T : class
		{
			if(options is null)
			{
				return queryable;
			}

			if(options.TryGetPagingOptions(out IPagingOptions<T> pagingOptions))
			{
				queryable = (ILiteQueryableAsync<T>)queryable
					.Skip(pagingOptions!.SkipAmount)
					.Limit(pagingOptions.TakeAmount);
			}

			if(options.TryGetSkipTakeOptions(out ISkipTakeOptions<T> skipTakeOptions))
			{
				if(skipTakeOptions!.SkipAmount.HasValue)
				{
					queryable = (ILiteQueryableAsync<T>)queryable.Skip(skipTakeOptions.SkipAmount.Value);
				}

				if(skipTakeOptions.TakeAmount.HasValue)
				{
					queryable = (ILiteQueryableAsync<T>)queryable.Limit(skipTakeOptions.TakeAmount.Value);
				}
			}

			if(options.TryGetSortingOptions(out ISortingOptions<T> orderByOptions))
			{
				ISortExpression<T> primaryExpression = orderByOptions!.PrimaryExpression;

				ILiteQueryableAsync<T> orderedQueryable = primaryExpression.IsDescending
					? queryable.OrderByDescending(primaryExpression.Expression)
					: queryable.OrderBy(primaryExpression.Expression);

				foreach(ISortExpression<T> _ in orderByOptions.SecondaryExpressions)
				{
					throw new NotSupportedException("The ThenBy syntax it currently not supported for LiteDB.");
					//orderedQueryable = expression.IsDescending
					//	? orderedQueryable.OrderByDescending(expression.Expression)
					//	: orderedQueryable.OrderBy(expression.Expression);
				}

				queryable = orderedQueryable;
			}

			return queryable;
		}
	}
}
