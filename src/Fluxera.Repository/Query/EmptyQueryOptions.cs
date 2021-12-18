namespace Fluxera.Repository.Query
{
	using System.Linq;
	using Fluxera.Utilities.Extensions;

	internal sealed class EmptyQueryOptions<T> : IQueryOptions<T> where T : class
	{
		/// <inheritdoc />
		public IQueryable<T> ApplyTo(IQueryable<T> queryable)
		{
			return queryable;
		}

		/// <inheritdoc />
		public bool TryGetPagingOptions(out IPagingOptions<T>? pagingOptions)
		{
			pagingOptions = null;
			return false;
		}

		/// <inheritdoc />
		public bool TryGetSkipTakeOptions(out ISkipTakeOptions<T>? skipTakeOptions)
		{
			skipTakeOptions = null;
			return false;
		}

		/// <inheritdoc />
		public bool TryGetSortingOptions(out ISortingOptions<T>? sortingOptions)
		{
			sortingOptions = null;
			return false;
		}

		/// <inheritdoc />
		public override string ToString()
		{
			return "QueryOptions<{0}>(Empty)".FormatInvariantWith(typeof(T).Name);
		}
	}
}
