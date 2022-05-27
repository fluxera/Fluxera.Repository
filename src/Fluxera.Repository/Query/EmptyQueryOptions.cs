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
		public bool IsEmpty()
		{
			return true;
		}

		/// <inheritdoc />
		public bool TryGetPagingOptions(out IPagingOptions<T> options)
		{
			options = null;
			return false;
		}

		/// <inheritdoc />
		public bool TryGetSkipTakeOptions(out ISkipTakeOptions<T> options)
		{
			options = null;
			return false;
		}

		/// <inheritdoc />
		public bool TryGetSortingOptions(out ISortingOptions<T> options)
		{
			options = null;
			return false;
		}

		/// <inheritdoc />
		public override string ToString()
		{
			return "QueryOptions<{0}>(Empty)".FormatInvariantWith(typeof(T).Name);
		}
	}
}
