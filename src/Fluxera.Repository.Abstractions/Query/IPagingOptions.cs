namespace Fluxera.Repository.Query
{
	using System;
	using System.Linq;
	using JetBrains.Annotations;

	/// <summary>
	///     A contract for paging options that are passed in queries.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	[PublicAPI]
	public interface IPagingOptions<T> where T : class
	{
		/// <summary>
		///     Gets the page number.
		/// </summary>
		internal int PageNumberAmount { get; }

		/// <summary>
		///     Gets the page size.
		/// </summary>
		internal int PageSizeAmount { get; }

		/// <summary>
		///     Gets the total item count.
		/// </summary>
		internal int TotalItemCount { get; }

		/// <summary>
		///     Set the page number.
		/// </summary>
		/// <param name="pageNumberAmount"></param>
		/// <returns></returns>
		IPagingOptions<T> PageNumber(int pageNumberAmount);

		/// <summary>
		///     Sets the page size.
		/// </summary>
		/// <param name="pageSizeAmount"></param>
		/// <returns></returns>
		IPagingOptions<T> PageSize(int pageSizeAmount);

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
