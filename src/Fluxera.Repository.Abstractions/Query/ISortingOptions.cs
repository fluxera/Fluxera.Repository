namespace Fluxera.Repository.Query
{
	using System;
	using System.Collections.Generic;
	using System.Linq.Expressions;
	using JetBrains.Annotations;

	/// <summary>
	///     A contract for configuring the sorting options of a query.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	[PublicAPI]
	public interface ISortingOptions<T> : IQueryOptions<T> where T : class
	{
		/// <summary>
		///     Gets the primary sort expression (OderBy/OrderByDescending).
		/// </summary>
		ISortExpression<T> PrimaryExpression { get; }

		/// <summary>
		///     Gets the optional secondary sort expressions (ThenBy/ThenByDescending).
		/// </summary>
		IEnumerable<ISortExpression<T>> SecondaryExpressions { get; }

		/// <summary>
		///     Configures a secondary sort expression.
		/// </summary>
		/// <param name="sortExpression"></param>
		/// <returns></returns>
		ISortingOptions<T> ThenBy(Expression<Func<T, object>> sortExpression);

		/// <summary>
		///     Configures a secondary sort expression (descending).
		/// </summary>
		/// <param name="sortExpression"></param>
		/// <returns></returns>
		ISortingOptions<T> ThenByDescending(Expression<Func<T, object>> sortExpression);

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
	}
}
