namespace Fluxera.Repository.Query
{
	using System;
	using System.Linq.Expressions;
	using JetBrains.Annotations;

	/// <summary>
	///     A helper service to create entry points for building <see cref="IQueryOptions{T}" />.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	[PublicAPI]
	public interface IQueryOptionsBuilder<T> where T : class
	{
		/// <summary>
		///     Creates an entry point for configuring the sorting options.
		/// </summary>
		/// <param name="sortExpression"></param>
		/// <returns></returns>
		ISortingOptions<T> OrderBy<TValue>(Expression<Func<T, TValue>> sortExpression);

		/// <summary>
		///     Creates an entry point for configuring the sorting options.
		/// </summary>
		/// <param name="sortExpression"></param>
		/// <returns></returns>
		ISortingOptions<T> OrderByDescending(Expression<Func<T, object>> sortExpression);

		/// <summary>
		///     Creates an entry point for configuring the skip/take options.
		/// </summary>
		/// <param name="skipAmount"></param>
		/// <returns></returns>
		ISkipTakeOptions<T> Skip(int skipAmount);

		/// <summary>
		///     Creates an entry point for configuring the skip/take options.
		/// </summary>
		/// <param name="takeAmount"></param>
		/// <returns></returns>
		ISkipTakeOptions<T> Take(int takeAmount);

		/// <summary>
		///     Creates an entry point for configuring the skip/take options.
		/// </summary>
		/// <param name="skipAmount"></param>
		/// <param name="takeAmount"></param>
		/// <returns></returns>
		ISkipTakeOptions<T> SkipTake(int skipAmount, int takeAmount);

		/// <summary>
		///     Creates an entry point for configuring the paging options.
		/// </summary>
		/// <param name="pageNumber"></param>
		/// <param name="pageSize"></param>
		/// <returns></returns>
		IPagingOptions<T> Paging(int pageNumber, int pageSize);

		/// <summary>
		///     Creates an entry point for configuring the paging options.
		/// </summary>
		/// <returns></returns>
		IPagingOptions<T> Paging();

		/// <summary>
		///     Builds the query options instance.
		/// </summary>
		/// <returns></returns>
		IQueryOptions<T> Build();
	}
}
