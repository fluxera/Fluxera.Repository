namespace Fluxera.Repository.Caching
{
	using System;
	using System.Linq.Expressions;
	using Fluxera.Entity;
	using Fluxera.Repository.Query;
	using JetBrains.Annotations;

	/// <summary>
	///     A cache key provider contract.
	/// </summary>
	[PublicAPI]
	public interface ICacheKeyProvider
	{
		/// <summary>
		///     Creates a key for the repository global cache counter. This value
		///     will be stored in the cache alongside every other cached data.
		/// </summary>
		/// <typeparam name="TEntity"></typeparam>
		/// <typeparam name="TKey"></typeparam>
		/// <param name="repositoryName"></param>
		/// <returns></returns>
		string GetGenerationCacheKey<TEntity, TKey>(RepositoryName repositoryName)
			where TEntity : Entity<TEntity, TKey>
			where TKey : notnull, IComparable<TKey>, IEquatable<TKey>;

		/// <summary>
		///     Creates a cache key for a single added entity key.
		/// </summary>
		/// <typeparam name="TEntity"></typeparam>
		/// <typeparam name="TKey"></typeparam>
		/// <param name="repositoryName"></param>
		/// <param name="id"></param>
		/// <returns></returns>
		string GetAddCacheKey<TEntity, TKey>(RepositoryName repositoryName, TKey id)
			where TEntity : Entity<TEntity, TKey>
			where TKey : notnull, IComparable<TKey>, IEquatable<TKey>;

		/// <summary>
		///     Creates a cache key for a single updated entity key.
		/// </summary>
		/// <typeparam name="TEntity"></typeparam>
		/// <typeparam name="TKey"></typeparam>
		/// <param name="repositoryName"></param>
		/// <param name="id"></param>
		/// <returns></returns>
		string GetUpdateCacheKey<TEntity, TKey>(RepositoryName repositoryName, TKey id)
			where TEntity : Entity<TEntity, TKey>
			where TKey : notnull, IComparable<TKey>, IEquatable<TKey>;

		/// <summary>
		///     Creates a cache key for a single removed entity key.
		/// </summary>
		/// <typeparam name="TEntity"></typeparam>
		/// <typeparam name="TKey"></typeparam>
		/// <param name="repositoryName"></param>
		/// <param name="id"></param>
		/// <returns></returns>
		string GetDeleteCacheKey<TEntity, TKey>(RepositoryName repositoryName, TKey id)
			where TEntity : Entity<TEntity, TKey>
			where TKey : notnull, IComparable<TKey>, IEquatable<TKey>;

		/// <summary>
		///     Creates a cache key for a single get-by-id operation.
		/// </summary>
		/// <typeparam name="TEntity"></typeparam>
		/// <typeparam name="TKey"></typeparam>
		/// <param name="repositoryName"></param>
		/// <param name="id"></param>
		/// <returns></returns>
		string GetGetCacheKey<TEntity, TKey>(RepositoryName repositoryName, TKey id)
			where TEntity : Entity<TEntity, TKey>
			where TKey : notnull, IComparable<TKey>, IEquatable<TKey>;

		/// <summary>
		///     Creates a cache key for a single get-by-id with selector operation.
		/// </summary>
		/// <typeparam name="TEntity"></typeparam>
		/// <typeparam name="TKey"></typeparam>
		/// <typeparam name="TResult"></typeparam>
		/// <param name="repositoryName"></param>
		/// <param name="id"></param>
		/// <param name="selector"></param>
		/// <returns></returns>
		string GetGetCacheKey<TEntity, TKey, TResult>(RepositoryName repositoryName, TKey id,
			Expression<Func<TEntity, TResult>> selector)
			where TEntity : Entity<TEntity, TKey>
			where TKey : notnull, IComparable<TKey>, IEquatable<TKey>;

		/// <summary>
		///     Creates a cache key for a count-all operation.
		/// </summary>
		/// <typeparam name="TEntity"></typeparam>
		/// <typeparam name="TKey"></typeparam>
		/// <param name="repositoryName"></param>
		/// <param name="generation"></param>
		/// <returns></returns>
		string GetCountCacheKey<TEntity, TKey>(RepositoryName repositoryName, in long generation)
			where TEntity : Entity<TEntity, TKey>
			where TKey : notnull, IComparable<TKey>, IEquatable<TKey>;

		/// <summary>
		///     Creates a cache key for a count with predicate operation.
		/// </summary>
		/// <typeparam name="TEntity"></typeparam>
		/// <typeparam name="TKey"></typeparam>
		/// <param name="repositoryName"></param>
		/// <param name="generation"></param>
		/// <param name="predicate"></param>
		/// <returns></returns>
		string GetCountCacheKey<TEntity, TKey>(RepositoryName repositoryName, in long generation,
			Expression<Func<TEntity, bool>> predicate)
			where TEntity : Entity<TEntity, TKey>
			where TKey : notnull, IComparable<TKey>, IEquatable<TKey>;

		/// <summary>
		///     Creates a cache key for a sum-all operation.
		/// </summary>
		/// <typeparam name="TEntity"></typeparam>
		/// <typeparam name="TKey"></typeparam>
		/// <param name="repositoryName"></param>
		/// <param name="generation"></param>
		/// <returns></returns>
		string GetSumCacheKey<TEntity, TKey>(RepositoryName repositoryName, in long generation)
			where TEntity : Entity<TEntity, TKey>
			where TKey : notnull, IComparable<TKey>, IEquatable<TKey>;

		/// <summary>
		///     Creates a cache key for a average with predicate operation.
		/// </summary>
		/// <typeparam name="TEntity"></typeparam>
		/// <typeparam name="TKey"></typeparam>
		/// <param name="repositoryName"></param>
		/// <param name="generation"></param>
		/// <param name="predicate"></param>
		/// <returns></returns>
		string GetAverageCacheKey<TEntity, TKey>(RepositoryName repositoryName, in long generation,
			Expression<Func<TEntity, bool>> predicate)
			where TEntity : Entity<TEntity, TKey>
			where TKey : notnull, IComparable<TKey>, IEquatable<TKey>;

		/// <summary>
		///     Creates a cache key for a average-all operation.
		/// </summary>
		/// <typeparam name="TEntity"></typeparam>
		/// <typeparam name="TKey"></typeparam>
		/// <param name="repositoryName"></param>
		/// <param name="generation"></param>
		/// <returns></returns>
		string GetAverageCacheKey<TEntity, TKey>(RepositoryName repositoryName, in long generation)
			where TEntity : Entity<TEntity, TKey>
			where TKey : notnull, IComparable<TKey>, IEquatable<TKey>;

		/// <summary>
		///     Creates a cache key for a sum with predicate operation.
		/// </summary>
		/// <typeparam name="TEntity"></typeparam>
		/// <typeparam name="TKey"></typeparam>
		/// <param name="repositoryName"></param>
		/// <param name="generation"></param>
		/// <param name="predicate"></param>
		/// <returns></returns>
		string GetSumCacheKey<TEntity, TKey>(RepositoryName repositoryName, in long generation,
			Expression<Func<TEntity, bool>> predicate)
			where TEntity : Entity<TEntity, TKey>
			where TKey : notnull, IComparable<TKey>, IEquatable<TKey>;

		/// <summary>
		///     Creates a cache key for a find-one with predicate operation.
		/// </summary>
		/// <typeparam name="TEntity"></typeparam>
		/// <typeparam name="TKey"></typeparam>
		/// <param name="repositoryName"></param>
		/// <param name="generation"></param>
		/// <param name="predicate"></param>
		/// <param name="queryOptions"></param>
		/// <returns></returns>
		string GetFindOneCacheKey<TEntity, TKey>(RepositoryName repositoryName, in long generation,
			Expression<Func<TEntity, bool>> predicate,
			IQueryOptions<TEntity> queryOptions)
			where TEntity : Entity<TEntity, TKey>
			where TKey : notnull, IComparable<TKey>, IEquatable<TKey>;

		/// <summary>
		///     Creates a cache key for a find-one with predicate and selector operation.
		/// </summary>
		/// <typeparam name="TEntity"></typeparam>
		/// <typeparam name="TKey"></typeparam>
		/// <typeparam name="TResult"></typeparam>
		/// <param name="repositoryName"></param>
		/// <param name="generation"></param>
		/// <param name="predicate"></param>
		/// <param name="selector"></param>
		/// <param name="queryOptions"></param>
		/// <returns></returns>
		string GetFindOneCacheKey<TEntity, TKey, TResult>(RepositoryName repositoryName, in long generation,
			Expression<Func<TEntity, bool>> predicate,
			Expression<Func<TEntity, TResult>> selector,
			IQueryOptions<TEntity> queryOptions)
			where TEntity : Entity<TEntity, TKey>
			where TKey : notnull, IComparable<TKey>, IEquatable<TKey>;

		/// <summary>
		///     Creates a cache key for a find-many with predicate operation.
		/// </summary>
		/// <typeparam name="TEntity"></typeparam>
		/// <typeparam name="TKey"></typeparam>
		/// <param name="repositoryName"></param>
		/// <param name="generation"></param>
		/// <param name="predicate"></param>
		/// <param name="queryOptions"></param>
		/// <returns></returns>
		string GetFindManyCacheKey<TEntity, TKey>(RepositoryName repositoryName, in long generation,
			Expression<Func<TEntity, bool>> predicate,
			IQueryOptions<TEntity> queryOptions)
			where TEntity : Entity<TEntity, TKey>
			where TKey : notnull, IComparable<TKey>, IEquatable<TKey>;

		/// <summary>
		///     Creates a cache key for a find-many with predicate and selector operation.
		/// </summary>
		/// <typeparam name="TEntity"></typeparam>
		/// <typeparam name="TKey"></typeparam>
		/// <typeparam name="TResult"></typeparam>
		/// <param name="repositoryName"></param>
		/// <param name="generation"></param>
		/// <param name="predicate"></param>
		/// <param name="selector"></param>
		/// <param name="queryOptions"></param>
		/// <returns></returns>
		string GetFindManyCacheKey<TEntity, TKey, TResult>(RepositoryName repositoryName, in long generation,
			Expression<Func<TEntity, bool>> predicate,
			Expression<Func<TEntity, TResult>> selector,
			IQueryOptions<TEntity> queryOptions)
			where TEntity : Entity<TEntity, TKey>
			where TKey : notnull, IComparable<TKey>, IEquatable<TKey>;

		/// <summary>
		///     Creates a cache key for an exists with ID operation.
		/// </summary>
		/// <typeparam name="TEntity"></typeparam>
		/// <typeparam name="TKey"></typeparam>
		/// <param name="repositoryName"></param>
		/// <param name="generation"></param>
		/// <param name="id"></param>
		/// <returns></returns>
		string GetExistsCacheKey<TEntity, TKey>(RepositoryName repositoryName, in long generation, TKey id)
			where TEntity : Entity<TEntity, TKey>
			where TKey : notnull, IComparable<TKey>, IEquatable<TKey>;

		/// <summary>
		///     Creates a cache key for an exists with predicate operation.
		/// </summary>
		/// <typeparam name="TEntity"></typeparam>
		/// <typeparam name="TKey"></typeparam>
		/// <param name="repositoryName"></param>
		/// <param name="generation"></param>
		/// <param name="predicate"></param>
		/// <returns></returns>
		string GetExistsCacheKey<TEntity, TKey>(RepositoryName repositoryName, in long generation,
			Expression<Func<TEntity, bool>> predicate)
			where TEntity : Entity<TEntity, TKey>
			where TKey : notnull, IComparable<TKey>, IEquatable<TKey>;
	}
}
