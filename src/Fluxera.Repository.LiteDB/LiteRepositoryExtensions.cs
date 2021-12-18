namespace Fluxera.Repository.LiteDB
{
	using Fluxera.Repository.Query;
	using global::LiteDB.Async;

	internal static class LiteRepositoryExtensions
	{
		internal static ILiteQueryableAsync<T> Apply<T>(this ILiteQueryableAsync<T> queryable, IQueryOptions<T>? options) where T : class
		{
			if(options is null)
			{
				return queryable;
			}

			if(options.TryGetPagingOptions(out IPagingOptions<T>? pagingOptions))
			{
				queryable = (ILiteQueryableAsync<T>)queryable
					.Skip(pagingOptions.Skip)
					.Limit(pagingOptions.Take);
			}

			if(options.TryGetSkipTakeOptions(out ISkipTakeOptions<T>? skipTakeOptions))
			{
				if(skipTakeOptions.SkipNumber.HasValue)
				{
					queryable = (ILiteQueryableAsync<T>)queryable.Skip(skipTakeOptions.SkipNumber.Value);
				}

				if(skipTakeOptions.TakeNumber.HasValue)
				{
					queryable = (ILiteQueryableAsync<T>)queryable.Limit(skipTakeOptions.TakeNumber.Value);
				}
			}

			if(options.TryGetSortingOptions(out ISortingOptions<T>? orderByOptions))
			{
				ISortExpression<T> primaryExpression = orderByOptions.PrimaryExpression;

				ILiteQueryableAsync<T> orderedQueryable = primaryExpression.IsDescending
					? queryable.OrderByDescending(primaryExpression.Expression)
					: queryable.OrderBy(primaryExpression.Expression);

				foreach(ISortExpression<T> expression in orderByOptions.SecondaryExpressions)
				{
					orderedQueryable = expression.IsDescending
						? orderedQueryable.OrderByDescending(expression.Expression)
						: orderedQueryable.OrderBy(expression.Expression);
				}

				queryable = orderedQueryable;
			}

			return queryable;
		}
	}
}
