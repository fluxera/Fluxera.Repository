namespace Fluxera.Repository.Query
{
	using System;
	using System.Linq;
	using System.Linq.Expressions;
	using JetBrains.Annotations;

	/// <summary>
	///     A contract for configuring the include options of a query.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	[PublicAPI]
	public interface IIncludeOptions<T> where T : class
	{
		/// <summary>
		///     Configures an include expression.
		/// </summary>
		/// <param name="includeExpression"></param>
		/// <returns></returns>
		IIncludeOptions<T> Include(Expression<Func<T, object>> includeExpression);

		/// <summary>
		///     Configures the primary sort expression.
		/// </summary>
		/// <param name="sortExpression"></param>
		/// <returns></returns>
		ISortingOptions<T> OrderBy(Expression<Func<T, object>> sortExpression);

		/// <summary>
		///     Configures the primary sort expression (descending).
		/// </summary>
		/// <param name="sortExpression"></param>
		/// <returns></returns>
		ISortingOptions<T> OrderByDescending(Expression<Func<T, object>> sortExpression);

		/// <summary>
		///     Configures an optional skip amount.
		/// </summary>
		/// <param name="skip"></param>
		/// <returns></returns>
		ISkipTakeOptions<T> Skip(int skip);

		/// <summary>
		///     Configures an optional take amount.
		/// </summary>
		/// <param name="take"></param>
		/// <returns></returns>
		ISkipTakeOptions<T> Take(int take);

		/// <summary>
		///     Configures an optional skip/take amount.
		/// </summary>
		/// <param name="skip"></param>
		/// <param name="take"></param>
		/// <returns></returns>
		ISkipTakeOptions<T> SkipTake(int skip, int take);

		/// <summary>
		///     Configures the optional paging.
		/// </summary>
		/// <param name="pageNumber">The page number.</param>
		/// <param name="pageSize">The page size.</param>
		/// <returns></returns>
		IPagingOptions<T> Paging(int pageNumber, int pageSize);

		/// <summary>
		///     Configures the optional paging.
		/// </summary>
		/// <returns></returns>
		IPagingOptions<T> Paging();

		/// <summary>
		///     Builds a query options instance from this options.
		/// </summary>
		/// <returns></returns>
		IQueryOptions<T> Build();

		/// <summary>
		///     Applies the query options to the given <see cref="IQueryable" />.
		/// </summary>
		/// <param name="queryable"></param>
		/// <returns></returns>
		internal IQueryable<T> ApplyTo(IQueryable<T> queryable);
	}
}
