namespace Fluxera.Repository.Query
{
	using System.Linq;
	using Fluxera.Utilities.Extensions;

	internal sealed class EmptyQueryOptions<T> : IQueryOptions<T> where T : class
	{
		/// <inheritdoc />
		IQueryable<T> IQueryOptions<T>.ApplyTo(IQueryable<T> queryable)
		{
			return queryable;
		}

		/// <inheritdoc />
		bool IQueryOptions<T>.IsEmpty()
		{
			return true;
		}

		/// <inheritdoc />
		bool IQueryOptions<T>.TryGetPagingOptions(out IPagingOptions<T> options)
		{
			options = null;
			return false;
		}

		/// <inheritdoc />
		bool IQueryOptions<T>.TryGetSkipTakeOptions(out ISkipTakeOptions<T> options)
		{
			options = null;
			return false;
		}

		/// <inheritdoc />
		bool IQueryOptions<T>.TryGetSortingOptions(out ISortingOptions<T> options)
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
