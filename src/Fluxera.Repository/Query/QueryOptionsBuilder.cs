namespace Fluxera.Repository.Query
{
	using System;
	using System.Linq.Expressions;
	using JetBrains.Annotations;

	/// <summary>
	///     A helper service to create entry points for building <see cref="IQueryOptions{T}" />.
	/// </summary>
	[PublicAPI]
	public static class QueryOptionsBuilder
	{
		/// <summary>
		///     Creates a builder instance.
		/// </summary>
		/// <returns></returns>
		public static QueryOptionsBuilder<T> CreateFor<T>() where T : class
		{
			return new QueryOptionsBuilder<T>();
		}

		/// <summary>
		///     Creates an empty options instance.
		/// </summary>
		/// <returns></returns>
		public static IQueryOptions<T> Empty<T>()
			where T : class
		{
			return QueryOptions<T>.Empty();
		}
	}

	/// <summary>
	///     A helper service to create entry points for building <see cref="IQueryOptions{T}" />.
	/// </summary>
	[PublicAPI]
	public class QueryOptionsBuilder<T> where T : class
	{
		private QueryOptionsImpl<T> queryOptions;

		internal QueryOptionsBuilder()
		{
			// Note: Intentionally left blank.
		}

		/// <summary>
		///     Creates an entry point for configuring the sorting options.
		/// </summary>
		/// <param name="sortExpression"></param>
		/// <returns></returns>
		public ISortingOptions<T> OrderBy<TValue>(Expression<Func<T, TValue>> sortExpression)
		{
			this.queryOptions ??= new QueryOptionsImpl<T>();
			return this.queryOptions.OrderBy(sortExpression);
		}

		/// <summary>
		///     Creates an entry point for configuring the sorting options.
		/// </summary>
		/// <param name="sortExpression"></param>
		/// <returns></returns>
		public ISortingOptions<T> OrderByDescending(Expression<Func<T, object>> sortExpression)
		{
			this.queryOptions ??= new QueryOptionsImpl<T>();
			return this.queryOptions.OrderByDescending(sortExpression);
		}

		/// <summary>
		///     Creates an entry point for configuring the skip/take options.
		/// </summary>
		/// <param name="skipAmount"></param>
		/// <returns></returns>
		public ISkipTakeOptions<T> Skip(int skipAmount)
		{
			this.queryOptions ??= new QueryOptionsImpl<T>();
			return this.queryOptions.Skip(skipAmount);
		}

		/// <summary>
		///     Creates an entry point for configuring the skip/take options.
		/// </summary>
		/// <param name="takeAmount"></param>
		/// <returns></returns>
		public ISkipTakeOptions<T> Take(int takeAmount)
		{
			this.queryOptions ??= new QueryOptionsImpl<T>();
			return this.queryOptions.Take(takeAmount);
		}

		/// <summary>
		///     Creates an entry point for configuring the skip/take options.
		/// </summary>
		/// <param name="skipAmount"></param>
		/// <param name="takeAmount"></param>
		/// <returns></returns>
		public ISkipTakeOptions<T> SkipTake(int skipAmount, int takeAmount)
		{
			this.queryOptions ??= new QueryOptionsImpl<T>();
			return this.queryOptions.SkipTake(skipAmount, takeAmount);
		}

		/// <summary>
		///     Creates an entry point for configuring the paging options.
		/// </summary>
		/// <param name="pageNumber"></param>
		/// <param name="pageSize"></param>
		/// <returns></returns>
		public IPagingOptions<T> Paging(int pageNumber, int pageSize)
		{
			this.queryOptions ??= new QueryOptionsImpl<T>();
			return this.queryOptions.Paging(pageNumber, pageSize);
		}

		/// <summary>
		///     Creates an entry point for configuring the paging options.
		/// </summary>
		/// <returns></returns>
		public IPagingOptions<T> Paging()
		{
			this.queryOptions ??= new QueryOptionsImpl<T>();
			return this.queryOptions.Paging();
		}

		/// <summary>
		///     Builds the query options instance.
		/// </summary>
		/// <returns></returns>
		public IQueryOptions<T> Build()
		{
			return this.queryOptions;
		}
	}
}
