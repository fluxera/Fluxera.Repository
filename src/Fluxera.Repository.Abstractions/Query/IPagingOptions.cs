namespace Fluxera.Repository.Query
{
	using JetBrains.Annotations;

	[PublicAPI]
	public interface IPagingOptions<T> : IQueryOptions<T> where T : class
	{
		int PageSize { get; }

		int PageNumber { get; }

		int Skip { get; }

		int Take { get; }

		int TotalItemCount { get; }
	}
}
