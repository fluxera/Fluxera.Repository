namespace Fluxera.Repository.InMemory
{
	using System.Linq;
	using Fluxera.Guards;
	using Fluxera.Repository.Query;
	using Fluxera.Repository.Specifications;

	internal static class QueryableExtensions
	{
		public static IQueryable<T> Apply<T>(this IQueryable<T> queryable, ISpecification<T> specification) where T : class
		{
			Guard.Against.Null(queryable, nameof(queryable));
			Guard.Against.Null(specification, nameof(specification));

			return specification.ApplyTo(queryable);
		}

		public static IQueryable<T> Apply<T>(this IQueryable<T> queryable, IQueryOptions<T> queryOptions) where T : class
		{
			Guard.Against.Null(queryable, nameof(queryable));
			Guard.Against.Null(queryOptions, nameof(queryOptions));

			return queryOptions.ApplyTo(queryable);
		}
	}
}
