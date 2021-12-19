namespace Fluxera.Repository.MongoDB
{
	using System.Linq;
	using global::MongoDB.Driver.Linq;

	internal static class QueryableExtensions
	{
		public static IMongoQueryable<T> ToMongoQueryable<T>(this IQueryable<T> queryable)
		{
			return (IMongoQueryable<T>)queryable;
		}
	}
}
