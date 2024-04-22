namespace Fluxera.Repository.Query.Impl
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Linq.Expressions;
	using Fluxera.Guards;
	using JetBrains.Annotations;

	/// <summary>
	///     Extension methods for the <see cref="IQueryOptions{T}" /> type.
	/// </summary>
	[PublicAPI]
	public static class QueryOptionsExtensions
	{
		/// <summary>
		///     Converts the given query options.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <typeparam name="TOther"></typeparam>
		/// <param name="queryOptions"></param>
		/// <param name="expressionMapper"></param>
		/// <returns></returns>
		public static IQueryOptions<TOther> Convert<T, TOther>(this IQueryOptions<T> queryOptions,
			Func<LambdaExpression, Expression<Func<TOther, object>>> expressionMapper)
			where T : class
			where TOther : class
		{
			queryOptions = Guard.Against.Null(queryOptions);
			expressionMapper = Guard.Against.Null(expressionMapper);

			if(queryOptions.IsEmpty)
			{
				return QueryOptionsBuilder.Empty<TOther>();
			}

			QueryOptionsImpl<T> queryOptionsImpl = (QueryOptionsImpl<T>)queryOptions;
			IQueryOptionsBuilder<TOther> queryOptionsBuilder = QueryOptionsBuilder.CreateFor<TOther>();

			if(queryOptionsImpl.SortingOptions is not null)
			{
				ISortingOptions<T> sortingOptions = queryOptionsImpl.SortingOptions;
				ISortExpression<T> primaryExpression = sortingOptions.PrimaryExpression;
				IList<ISortExpression<T>> secondaryExpressions = sortingOptions.SecondaryExpressions;

				ISortingOptions<TOther> mappedSortingOptions = primaryExpression.IsDescending
					? queryOptionsBuilder.OrderByDescending(expressionMapper.Invoke(primaryExpression.LambdaExpression))
					: queryOptionsBuilder.OrderBy(expressionMapper.Invoke(primaryExpression.LambdaExpression));

				if(secondaryExpressions.Any())
				{
					foreach(ISortExpression<T> secondaryExpression in secondaryExpressions)
					{
						mappedSortingOptions = secondaryExpression.IsDescending
							? mappedSortingOptions.ThenByDescending(expressionMapper.Invoke(secondaryExpression.LambdaExpression))
							: mappedSortingOptions.ThenBy(expressionMapper.Invoke(secondaryExpression.LambdaExpression));
					}
				}
			}

			if(queryOptionsImpl.SkipTakeOptions is not null)
			{
				ISkipTakeOptions<T> skipTakeOptions = queryOptionsImpl.SkipTakeOptions;

				if(skipTakeOptions.SkipAmount.HasValue && skipTakeOptions.TakeAmount.HasValue)
				{
					queryOptionsBuilder.SkipTake(skipTakeOptions.SkipAmount.Value, skipTakeOptions.TakeAmount.Value);
				}

				if(skipTakeOptions.SkipAmount.HasValue && !skipTakeOptions.TakeAmount.HasValue)
				{
					queryOptionsBuilder.Skip(skipTakeOptions.SkipAmount.Value);
				}

				if(skipTakeOptions.TakeAmount.HasValue && !skipTakeOptions.SkipAmount.HasValue)
				{
					queryOptionsBuilder.Take(skipTakeOptions.TakeAmount.Value);
				}
			}

			if(queryOptionsImpl.PagingOptions is not null)
			{
				IPagingOptions<T> pagingOptions = queryOptionsImpl.PagingOptions;

				queryOptionsBuilder.Paging(pagingOptions.PageNumberAmount, pagingOptions.PageSizeAmount);
			}

			return queryOptionsBuilder.Build();
		}
	}
}
