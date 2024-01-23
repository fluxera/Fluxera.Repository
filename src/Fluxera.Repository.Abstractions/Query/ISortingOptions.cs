namespace Fluxera.Repository.Query
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Linq.Expressions;
	using JetBrains.Annotations;

	/// <summary>
	///     A contract for configuring the sorting options of a query.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	[PublicAPI]
	public interface ISortingOptions<T> where T : class
	{
		/// <summary>
		///     Gets the primary sort expression.
		/// </summary>
		internal ISortExpression<T> PrimaryExpression { get; }

		/// <summary>
		///     Gets the secondary sort expressions.
		/// </summary>
		internal IList<ISortExpression<T>> SecondaryExpressions { get; }

		/// <summary>
		///     Configures a secondary sort expression.
		/// </summary>
		/// <param name="sortExpression"></param>
		/// <returns></returns>
		ISortingOptions<T> ThenBy<TValue>(Expression<Func<T, TValue>> sortExpression);

		/// <summary>
		///     Configures a secondary sort expression (descending).
		/// </summary>
		/// <param name="sortExpression"></param>
		/// <returns></returns>
		ISortingOptions<T> ThenByDescending<TValue>(Expression<Func<T, TValue>> sortExpression);

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
		///     Builds a query options instance from these options.
		/// </summary>
		/// <param name="applyFunc">A function which can be used to apply additional configuration to the queryable.</param>
		/// <returns></returns>
		IQueryOptions<T> Build(Func<IQueryable<T>, IQueryable<T>> applyFunc = null);

		/// <summary>
		///     Applies the query options to the given <see cref="IQueryable" />.
		/// </summary>
		/// <param name="queryable"></param>
		/// <returns></returns>
		internal IQueryable<T> ApplyTo(IQueryable<T> queryable);
	}
}
