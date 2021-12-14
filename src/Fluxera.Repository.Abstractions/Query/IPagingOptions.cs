namespace Fluxera.Repository.Query
{
	using JetBrains.Annotations;

	[PublicAPI]
	public interface IPagingOptions<T> where T : class
	{
		public int PageNumber { get; }

		public int PageSize { get; }

		public int Skip { get; }

		public int TotalItemCount { get; set; }
	}
}
