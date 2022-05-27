namespace Fluxera.Repository.Query
{
	using System;
	using System.Linq.Expressions;
	using JetBrains.Annotations;

	/// <summary>
	///     A static helper class to create entry points for building <see cref="IQueryOptions{T}" />.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	[PublicAPI]
	public static class QueryOptions<T> where T : class
	{
		/// <summary>
		///     Creates an empty options instance.
		/// </summary>
		/// <returns></returns>
		public static IQueryOptions<T> Empty()
		{
			return new EmptyQueryOptions<T>();
		}

		/// <summary>
		///     Creates an entry point for configuring the sorting options.
		/// </summary>
		/// <param name="sortExpression"></param>
		/// <returns></returns>
		public static ISortingOptions<T> OrderBy(Expression<Func<T, object>> sortExpression)
		{
			QueryOptionsImpl<T> queryOptions = new QueryOptionsImpl<T>();
			return queryOptions.OrderBy(sortExpression);
		}

		/// <summary>
		///     Creates an entry point for configuring the sorting options.
		/// </summary>
		/// <param name="sortExpression"></param>
		/// <returns></returns>
		public static ISortingOptions<T> OrderByDescending(Expression<Func<T, object>> sortExpression)
		{
			QueryOptionsImpl<T> queryOptions = new QueryOptionsImpl<T>();
			return queryOptions.OrderByDescending(sortExpression);
		}

		/// <summary>
		///     Creates an entry point for configuring the skip/take options.
		/// </summary>
		/// <param name="skipAmount"></param>
		/// <returns></returns>
		public static ISkipTakeOptions<T> Skip(int skipAmount)
		{
			QueryOptionsImpl<T> queryOptions = new QueryOptionsImpl<T>();
			return queryOptions.Skip(skipAmount);
		}

		/// <summary>
		///     Creates an entry point for configuring the skip/take options.
		/// </summary>
		/// <param name="takeAmount"></param>
		/// <returns></returns>
		public static ISkipTakeOptions<T> Take(int takeAmount)
		{
			QueryOptionsImpl<T> queryOptions = new QueryOptionsImpl<T>();
			return queryOptions.Take(takeAmount);
		}

		/// <summary>
		///     Creates an entry point for configuring the skip/take options.
		/// </summary>
		/// <param name="skipAmount"></param>
		/// <param name="takeAmount"></param>
		/// <returns></returns>
		public static ISkipTakeOptions<T> SkipTake(int skipAmount, int takeAmount)
		{
			QueryOptionsImpl<T> queryOptions = new QueryOptionsImpl<T>();
			return queryOptions.SkipTake(skipAmount, takeAmount);
		}

		/// <summary>
		///     Creates an entry point for configuring the paging options.
		/// </summary>
		/// <param name="pageNumber"></param>
		/// <param name="pageSize"></param>
		/// <returns></returns>
		public static IPagingOptions<T> Paging(int pageNumber, int pageSize)
		{
			QueryOptionsImpl<T> queryOptions = new QueryOptionsImpl<T>();
			return queryOptions.Paging(pageNumber, pageSize);
		}

		/// <summary>
		///     Creates an entry point for configuring the paging options.
		/// </summary>
		/// <returns></returns>
		public static IPagingOptions<T> Paging()
		{
			QueryOptionsImpl<T> queryOptions = new QueryOptionsImpl<T>();
			return queryOptions.Paging();
		}
	}
}
