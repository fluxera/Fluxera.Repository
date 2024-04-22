namespace Fluxera.Repository.Query.Impl
{
	using System.Linq;
	using Fluxera.Utilities.Extensions;

	internal sealed class EmptyQueryOptions<T> : IQueryOptions<T> where T : class
	{
		/// <inheritdoc />
		public bool IsEmpty => true;

		/// <inheritdoc />
		IQueryable<T> IQueryOptions<T>.ApplyTo(IQueryable<T> queryable)
		{
			return queryable;
		}

		/// <inheritdoc />
		public override string ToString()
		{
			return "QueryOptions<{0}>(Empty)".FormatInvariantWith(typeof(T).Name);
		}
	}
}
