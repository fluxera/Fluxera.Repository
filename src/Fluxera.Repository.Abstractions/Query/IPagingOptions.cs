namespace Fluxera.Repository.Query
{
	using JetBrains.Annotations;

	/// <summary>
	///     A contract for paging options that are passed in queries.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	[PublicAPI]
	public interface IPagingOptions<T> : IQueryOptions<T> where T : class
	{
		/// <summary>
		///     The page size.
		/// </summary>
		int PageSizeAmount { get; }

		/// <summary>
		///     The page number to request.
		/// </summary>
		int PageNumberAmount { get; }

		/// <summary>
		///     The total value to skip to get to the beginning of the page..
		/// </summary>
		int SkipAmount { get; }

		/// <summary>
		///     The amount of items to take (page size).
		/// </summary>
		int TakeAmount { get; }

		/// <summary>
		///     The total amount of items of the query.
		/// </summary>
		int TotalItemCount { get; }

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
	}
}
