namespace Fluxera.Repository.LiteDB
{
	using Fluxera.Entity;
	using Fluxera.Repository.Query;
	using global::LiteDB.Async;

	internal static class LiteRepositoryExtensions
	{
		internal static ILiteQueryableAsync<TAggregateRoot> ApplyOptions<TAggregateRoot, TKey>(
			this ILiteQueryableAsync<TAggregateRoot> queryable, IQueryOptions<TAggregateRoot>? options)
			where TAggregateRoot : AggregateRoot<TAggregateRoot, TKey>
		{
			if(options is null)
			{
				return queryable;
			}

			// TODO
			//if(options.HasPagingOptions)
			//{
			//	options.TryGetPagingOptions(out IPagingOptions<TAggregateRoot>? pagingOptions);

			//	queryable = (ILiteQueryableAsync<TAggregateRoot>)queryable.Skip(pagingOptions.Skip).Limit(pagingOptions.PageSize);
			//}

			//if(options.HasSkipTakeOptions)
			//{
			//	options.TryGetSkipTakeOptions(out ISkipTakeOptions<TAggregateRoot>? skipTakeOptions);

			//	if(skipTakeOptions.Skip.HasValue)
			//	{
			//		queryable = (ILiteQueryableAsync<TAggregateRoot>)queryable.Skip(skipTakeOptions.Skip.Value);
			//	}

			//	if(skipTakeOptions.Take.HasValue)
			//	{
			//		queryable = (ILiteQueryableAsync<TAggregateRoot>)queryable.Limit(skipTakeOptions.Take.Value);
			//	}
			//}

			//if(options.HasOrderByOptions)
			//{
			//	options.TryGetOrderByOptions(out IOrderByOptions<TAggregateRoot>? orderByOptions);
			//	IOrderByExpression<TAggregateRoot>? orderBy = orderByOptions?.OrderByExpression;
			//	if(orderBy != null)
			//	{
			//		ILiteQueryableAsync<TAggregateRoot> orderedQueryable = orderBy.IsDescending
			//			? queryable.OrderByDescending(orderBy.SortExpression)
			//			: queryable.OrderBy(orderBy.SortExpression);

			//		if(orderByOptions.ThenByExpressions != null)
			//		{
			//			foreach(IOrderByExpression<TAggregateRoot> thenBy in orderByOptions.ThenByExpressions)
			//			{
			//				orderedQueryable = thenBy.IsDescending
			//					? orderedQueryable.OrderBy(thenBy.SortExpression)
			//					: orderedQueryable.OrderByDescending(thenBy.SortExpression);
			//			}
			//		}

			//		queryable = orderedQueryable;
			//	}
			//}

			return queryable;
		}
	}
}
