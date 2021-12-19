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
		int PageSize { get; }

		/// <summary>
		///     The page number to request.
		/// </summary>
		int PageNumber { get; }

		/// <summary>
		///     The total value to skip to get to the beginning of the page..
		/// </summary>
		int Skip { get; }

		/// <summary>
		///     The amount of items to take (page size).
		/// </summary>
		int Take { get; }

		/// <summary>
		///     The total amount of items of the query.
		///     TODO: At the moment this only works with LINQ, but not very nice.
		/// </summary>
		int TotalItemCount { get; }
	}
}
