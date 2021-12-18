namespace Fluxera.Repository.Query
{
	public interface IPagingOptions<T> : IQueryOptions<T> where T : class
	{
		int PageSize { get; }

		int PageNumber { get; }

		int Skip { get; }

		int Take { get; }

		int TotalItemCount { get; }
	}
}
