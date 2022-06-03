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
		/// <typeparam name="TAggregateRoot"></typeparam>
		/// <typeparam name="TKey"></typeparam>
		/// <param name="repositoryName"></param>
		/// <returns></returns>
		string GetGenerationCacheKey<TAggregateRoot, TKey>(RepositoryName repositoryName)
			where TAggregateRoot : AggregateRoot<TAggregateRoot, TKey>
			where TKey : notnull, IComparable<TKey>, IEquatable<TKey>;

		/// <summary>
		///     Creates a cache key for a single added entity key.
		/// </summary>
		/// <typeparam name="TAggregateRoot"></typeparam>
		/// <typeparam name="TKey"></typeparam>
		/// <param name="repositoryName"></param>
		/// <param name="id"></param>
		/// <returns></returns>
		string GetAddCacheKey<TAggregateRoot, TKey>(RepositoryName repositoryName, TKey id)
			where TAggregateRoot : AggregateRoot<TAggregateRoot, TKey>
			where TKey : notnull, IComparable<TKey>, IEquatable<TKey>;

		/// <summary>
		///     Creates a cache key for a single updated entity key.
		/// </summary>
		/// <typeparam name="TAggregateRoot"></typeparam>
		/// <typeparam name="TKey"></typeparam>
		/// <param name="repositoryName"></param>
		/// <param name="id"></param>
		/// <returns></returns>
		string GetUpdateCacheKey<TAggregateRoot, TKey>(RepositoryName repositoryName, TKey id)
			where TAggregateRoot : AggregateRoot<TAggregateRoot, TKey>
			where TKey : notnull, IComparable<TKey>, IEquatable<TKey>;

		/// <summary>
		///     Creates a cache key for a single removed entity key.
		/// </summary>
		/// <typeparam name="TAggregateRoot"></typeparam>
		/// <typeparam name="TKey"></typeparam>
		/// <param name="repositoryName"></param>
		/// <param name="id"></param>
		/// <returns></returns>
		string GetDeleteCacheKey<TAggregateRoot, TKey>(RepositoryName repositoryName, TKey id)
			where TAggregateRoot : AggregateRoot<TAggregateRoot, TKey>
			where TKey : notnull, IComparable<TKey>, IEquatable<TKey>;

		/// <summary>
		///     Creates a cache key for a single get-by-id operation.
		/// </summary>
		/// <typeparam name="TAggregateRoot"></typeparam>
		/// <typeparam name="TKey"></typeparam>
		/// <param name="repositoryName"></param>
		/// <param name="id"></param>
		/// <returns></returns>
		string GetGetCacheKey<TAggregateRoot, TKey>(RepositoryName repositoryName, TKey id)
			where TAggregateRoot : AggregateRoot<TAggregateRoot, TKey>
			where TKey : notnull, IComparable<TKey>, IEquatable<TKey>;

		/// <summary>
		///     Creates a cache key for a single get-by-id with selector operation.
		/// </summary>
		/// <typeparam name="TAggregateRoot"></typeparam>
		/// <typeparam name="TKey"></typeparam>
		/// <typeparam name="TResult"></typeparam>
		/// <param name="repositoryName"></param>
		/// <param name="id"></param>
		/// <param name="selector"></param>
		/// <returns></returns>
		string GetGetCacheKey<TAggregateRoot, TKey, TResult>(RepositoryName repositoryName, TKey id,
			Expression<Func<TAggregateRoot, TResult>> selector)
			where TAggregateRoot : AggregateRoot<TAggregateRoot, TKey>
			where TKey : notnull, IComparable<TKey>, IEquatable<TKey>;

		/// <summary>
		///     Creates a cache key for a count-all operation.
		/// </summary>
		/// <typeparam name="TAggregateRoot"></typeparam>
		/// <typeparam name="TKey"></typeparam>
		/// <param name="repositoryName"></param>
		/// <param name="generation"></param>
		/// <returns></returns>
		string GetCountCacheKey<TAggregateRoot, TKey>(RepositoryName repositoryName, in long generation)
			where TAggregateRoot : AggregateRoot<TAggregateRoot, TKey>
			where TKey : notnull, IComparable<TKey>, IEquatable<TKey>;

		/// <summary>
		///     Creates a cache key for a count with predicate operation.
		/// </summary>
		/// <typeparam name="TAggregateRoot"></typeparam>
		/// <typeparam name="TKey"></typeparam>
		/// <param name="repositoryName"></param>
		/// <param name="generation"></param>
		/// <param name="predicate"></param>
		/// <returns></returns>
		string GetCountCacheKey<TAggregateRoot, TKey>(RepositoryName repositoryName, in long generation,
			Expression<Func<TAggregateRoot, bool>> predicate)
			where TAggregateRoot : AggregateRoot<TAggregateRoot, TKey>
			where TKey : notnull, IComparable<TKey>, IEquatable<TKey>;

		/// <summary>
		///     Creates a cache key for a sum-all operation.
		/// </summary>
		/// <typeparam name="TAggregateRoot"></typeparam>
		/// <typeparam name="TKey"></typeparam>
		/// <param name="repositoryName"></param>
		/// <param name="generation"></param>
		/// <returns></returns>
		string GetSumCacheKey<TAggregateRoot, TKey>(RepositoryName repositoryName, in long generation)
			where TAggregateRoot : AggregateRoot<TAggregateRoot, TKey>
			where TKey : notnull, IComparable<TKey>, IEquatable<TKey>;

		/// <summary>
		///     Creates a cache key for a sum with predicate operation.
		/// </summary>
		/// <typeparam name="TAggregateRoot"></typeparam>
		/// <typeparam name="TKey"></typeparam>
		/// <param name="repositoryName"></param>
		/// <param name="generation"></param>
		/// <param name="predicate"></param>
		/// <returns></returns>
		string GetSumCacheKey<TAggregateRoot, TKey>(RepositoryName repositoryName, in long generation,
			Expression<Func<TAggregateRoot, bool>> predicate)
			where TAggregateRoot : AggregateRoot<TAggregateRoot, TKey>
			where TKey : notnull, IComparable<TKey>, IEquatable<TKey>;

		/// <summary>
		///     Creates a cache key for a find-one with predicate operation.
		/// </summary>
		/// <typeparam name="TAggregateRoot"></typeparam>
		/// <typeparam name="TKey"></typeparam>
		/// <param name="repositoryName"></param>
		/// <param name="generation"></param>
		/// <param name="predicate"></param>
		/// <param name="queryOptions"></param>
		/// <returns></returns>
		string GetFindOneCacheKey<TAggregateRoot, TKey>(RepositoryName repositoryName, in long generation,
			Expression<Func<TAggregateRoot, bool>> predicate,
			IQueryOptions<TAggregateRoot> queryOptions)
			where TAggregateRoot : AggregateRoot<TAggregateRoot, TKey>
			where TKey : notnull, IComparable<TKey>, IEquatable<TKey>;

		/// <summary>
		///     Creates a cache key for a find-one with predicate and selector operation.
		/// </summary>
		/// <typeparam name="TAggregateRoot"></typeparam>
		/// <typeparam name="TKey"></typeparam>
		/// <typeparam name="TResult"></typeparam>
		/// <param name="repositoryName"></param>
		/// <param name="generation"></param>
		/// <param name="predicate"></param>
		/// <param name="selector"></param>
		/// <param name="queryOptions"></param>
		/// <returns></returns>
		string GetFindOneCacheKey<TAggregateRoot, TKey, TResult>(RepositoryName repositoryName, in long generation,
			Expression<Func<TAggregateRoot, bool>> predicate,
			Expression<Func<TAggregateRoot, TResult>> selector,
			IQueryOptions<TAggregateRoot> queryOptions)
			where TAggregateRoot : AggregateRoot<TAggregateRoot, TKey>
			where TKey : notnull, IComparable<TKey>, IEquatable<TKey>;

		/// <summary>
		///     Creates a cache key for a find-many with predicate operation.
		/// </summary>
		/// <typeparam name="TAggregateRoot"></typeparam>
		/// <typeparam name="TKey"></typeparam>
		/// <param name="repositoryName"></param>
		/// <param name="generation"></param>
		/// <param name="predicate"></param>
		/// <param name="queryOptions"></param>
		/// <returns></returns>
		string GetFindManyCacheKey<TAggregateRoot, TKey>(RepositoryName repositoryName, in long generation,
			Expression<Func<TAggregateRoot, bool>> predicate,
			IQueryOptions<TAggregateRoot> queryOptions)
			where TAggregateRoot : AggregateRoot<TAggregateRoot, TKey>
			where TKey : notnull, IComparable<TKey>, IEquatable<TKey>;

		/// <summary>
		///     Creates a cache key for a find-many with predicate and selector operation.
		/// </summary>
		/// <typeparam name="TAggregateRoot"></typeparam>
		/// <typeparam name="TKey"></typeparam>
		/// <typeparam name="TResult"></typeparam>
		/// <param name="repositoryName"></param>
		/// <param name="generation"></param>
		/// <param name="predicate"></param>
		/// <param name="selector"></param>
		/// <param name="queryOptions"></param>
		/// <returns></returns>
		string GetFindManyCacheKey<TAggregateRoot, TKey, TResult>(RepositoryName repositoryName, in long generation,
			Expression<Func<TAggregateRoot, bool>> predicate,
			Expression<Func<TAggregateRoot, TResult>> selector,
			IQueryOptions<TAggregateRoot> queryOptions)
			where TAggregateRoot : AggregateRoot<TAggregateRoot, TKey>
			where TKey : notnull, IComparable<TKey>, IEquatable<TKey>;

		/// <summary>
		///     Creates a cache key for an exists with ID operation.
		/// </summary>
		/// <typeparam name="TAggregateRoot"></typeparam>
		/// <typeparam name="TKey"></typeparam>
		/// <param name="repositoryName"></param>
		/// <param name="generation"></param>
		/// <param name="id"></param>
		/// <returns></returns>
		string GetExistsCacheKey<TAggregateRoot, TKey>(RepositoryName repositoryName, in long generation, TKey id)
			where TAggregateRoot : AggregateRoot<TAggregateRoot, TKey>
			where TKey : notnull, IComparable<TKey>, IEquatable<TKey>;

		/// <summary>
		///     Creates a cache key for an exists with predicate operation.
		/// </summary>
		/// <typeparam name="TAggregateRoot"></typeparam>
		/// <typeparam name="TKey"></typeparam>
		/// <param name="repositoryName"></param>
		/// <param name="generation"></param>
		/// <param name="predicate"></param>
		/// <returns></returns>
		string GetExistsCacheKey<TAggregateRoot, TKey>(RepositoryName repositoryName, in long generation,
			Expression<Func<TAggregateRoot, bool>> predicate)
			where TAggregateRoot : AggregateRoot<TAggregateRoot, TKey>
			where TKey : notnull, IComparable<TKey>, IEquatable<TKey>;
	}
}
