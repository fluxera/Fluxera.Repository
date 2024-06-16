namespace Fluxera.Repository.Queries
{
	using System.Collections.Generic;
	using System.ComponentModel;
	using Fluxera.Queries.Expressions;
	using Fluxera.Queries.Options;
	using Fluxera.Repository.Query;
	using Fluxera.Repository.Query.Impl;
	using JetBrains.Annotations;

	/// <summary>
	///		Extension methods for the <see cref="QueryOptions"/> type.
	/// </summary>
	[PublicAPI]
	public static class QueryOptionsExtensions
	{
		/// <summary>
		///		Builds <see cref="IQueryOptions{T}"/> from the <see cref="QueryOptions"/>.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="queryOptions"></param>
		/// <returns></returns>
		public static IQueryOptions<T> ToQueryOptions<T>(this QueryOptions queryOptions)
			where T : class
		{
			IQueryOptionsBuilder<T> queryOptionsBuilder = QueryOptionsBuilder.CreateFor<T>();
			return queryOptionsBuilder.Apply(queryOptions).Build();
		}

		/// <summary>
		///		Applies the given <see cref="QueryOptions"/> to the <see cref="IQueryOptionsBuilder{T}"/>.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="builder"></param>
		/// <param name="queryOptions"></param>
		/// <returns></returns>
		public static IQueryOptionsBuilder<T> Apply<T>(this IQueryOptionsBuilder<T> builder, QueryOptions queryOptions)
			where T : class
		{
			// 1. Apply the orderby options. 
			builder.Apply(queryOptions.OrderBy);

			// 2. Apply paging.
			builder.Apply(queryOptions.Skip, queryOptions.Top);

			return builder;
		}

		/// <summary>
		///		Applies the given <see cref="OrderByQueryOption"/> to the <see cref="IQueryOptionsBuilder{T}"/>.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="builder"></param>
		/// <param name="orderByQueryOption"></param>
		/// <returns></returns>
		public static IQueryOptionsBuilder<T> Apply<T>(this IQueryOptionsBuilder<T> builder, OrderByQueryOption orderByQueryOption)
			where T : class
		{
			if(orderByQueryOption is not null && orderByQueryOption.Properties.Count > 0)
			{
				IReadOnlyCollection<OrderByExpression<T>> orderByExpressions = orderByQueryOption.ToOrderByExpressions<T>();

				builder.Apply(orderByExpressions);
			}

			return builder;
		}

		/// <summary>
		///		Applies the given <see cref="OrderByQueryOption"/> to the <see cref="IQueryOptionsBuilder{T}"/>.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="builder"></param>
		/// <param name="orderByExpressions"></param>
		/// <returns></returns>
		public static IQueryOptionsBuilder<T> Apply<T>(this IQueryOptionsBuilder<T> builder, IReadOnlyCollection<OrderByExpression<T>> orderByExpressions)
			where T : class
		{
			bool isFirstClause = true;

			ISortingOptions<T> sortingOptions = null;

			foreach(OrderByExpression<T> expression in orderByExpressions)
			{
				if(isFirstClause)
				{
					sortingOptions = expression.Direction switch
					{
						OrderByDirection.Ascending  => builder.OrderBy(expression.SelectorExpression),
						OrderByDirection.Descending => builder.OrderByDescending(expression.SelectorExpression),
						_                           => throw new InvalidEnumArgumentException($"Unsupported order by direction {expression.Direction}.")
					};
				}
				else
				{
					sortingOptions = expression.Direction switch
					{
						OrderByDirection.Ascending  => sortingOptions.ThenBy(expression.SelectorExpression),
						OrderByDirection.Descending => sortingOptions.ThenByDescending(expression.SelectorExpression),
						_                           => throw new InvalidEnumArgumentException($"Unsupported order by direction {expression.Direction}.")
					};
				}

				isFirstClause = false;
			}

			return builder;
		}

		/// <summary>
		///		Applies the given <see cref="OrderByQueryOption"/> to the <see cref="IQueryOptionsBuilder{T}"/>.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="builder"></param>
		/// <param name="skipQueryOption"></param>
		/// <param name="topQueryOption"></param>
		/// <returns></returns>
		public static IQueryOptionsBuilder<T> Apply<T>(this IQueryOptionsBuilder<T> builder, SkipQueryOption skipQueryOption, TopQueryOption topQueryOption)
			where T : class
		{
			ISkipTakeOptions<T> skipTakeOptions = skipQueryOption is not null
				? builder.Skip(skipQueryOption.SkipValue ?? 0)
				: builder.Skip(0);

			if(topQueryOption?.TopValue != null)
			{
				skipTakeOptions.Take(topQueryOption.TopValue.Value);
			}

			return builder;
		}
	}
}
