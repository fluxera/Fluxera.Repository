namespace Fluxera.Repository.Query
{
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
