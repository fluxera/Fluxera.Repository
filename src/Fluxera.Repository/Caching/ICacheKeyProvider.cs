namespace Fluxera.Repository.Caching
{
	using System;
	using System.Linq.Expressions;
	using Fluxera.Entity;
	using Fluxera.Repository.Query;
	using JetBrains.Annotations;

	[PublicAPI]
	public interface ICacheKeyProvider
	{
		string GetGenerationCacheKey<TAggregateRoot, TKey>(
			RepositoryName repositoryName) where TAggregateRoot : AggregateRoot<TAggregateRoot, TKey>;

		string GetAddCacheKey<TAggregateRoot, TKey>(
			RepositoryName repositoryName,
			TKey id) where TAggregateRoot : AggregateRoot<TAggregateRoot, TKey>;

		string GetUpdateCacheKey<TAggregateRoot, TKey>(
			RepositoryName repositoryName,
			TKey id) where TAggregateRoot : AggregateRoot<TAggregateRoot, TKey>;

		string GetDeleteCacheKey<TAggregateRoot, TKey>(
			RepositoryName repositoryName,
			TKey id) where TAggregateRoot : AggregateRoot<TAggregateRoot, TKey>;

		string GetGetCacheKey<TAggregateRoot, TKey>(
			RepositoryName repositoryName,
			TKey id) where TAggregateRoot : AggregateRoot<TAggregateRoot, TKey>;

		string GetGetCacheKey<TAggregateRoot, TKey, TResult>(
			RepositoryName repositoryName,
			TKey id,
			Expression<Func<TAggregateRoot, TResult>> selector) where TAggregateRoot : AggregateRoot<TAggregateRoot, TKey>;

		string GetCountCacheKey<TAggregateRoot, TKey>(
			RepositoryName repositoryName,
			in long generation) where TAggregateRoot : AggregateRoot<TAggregateRoot, TKey>;

		string GetCountCacheKey<TAggregateRoot, TKey>(
			RepositoryName repositoryName,
			in long generation,
			Expression<Func<TAggregateRoot, bool>> predicate) where TAggregateRoot : AggregateRoot<TAggregateRoot, TKey>;

		string GetFindOneCacheKey<TAggregateRoot, TKey>(
			RepositoryName repositoryName,
			in long generation,
			Expression<Func<TAggregateRoot, bool>> predicate,
			IQueryOptions<TAggregateRoot> queryOptions) where TAggregateRoot : AggregateRoot<TAggregateRoot, TKey>;

		string GetFindOneCacheKey<TAggregateRoot, TKey, TResult>(
			RepositoryName repositoryName,
			in long generation,
			Expression<Func<TAggregateRoot, bool>> predicate,
			Expression<Func<TAggregateRoot, TResult>> selector,
			IQueryOptions<TAggregateRoot> queryOptions) where TAggregateRoot : AggregateRoot<TAggregateRoot, TKey>;

		string GetFindManyCacheKey<TAggregateRoot, TKey>(
			RepositoryName repositoryName,
			in long generation,
			Expression<Func<TAggregateRoot, bool>> predicate,
			IQueryOptions<TAggregateRoot> queryOptions) where TAggregateRoot : AggregateRoot<TAggregateRoot, TKey>;

		string GetFindManyCacheKey<TAggregateRoot, TKey, TResult>(
			RepositoryName repositoryName,
			in long generation,
			Expression<Func<TAggregateRoot, bool>> predicate,
			Expression<Func<TAggregateRoot, TResult>> selector,
			IQueryOptions<TAggregateRoot> queryOptions) where TAggregateRoot : AggregateRoot<TAggregateRoot, TKey>;

		string GetExistsCacheKey<TAggregateRoot, TKey>(
			RepositoryName repositoryName,
			in long generation,
			TKey id)
			where TAggregateRoot : AggregateRoot<TAggregateRoot, TKey>;

		string GetExistsCacheKey<TAggregateRoot, TKey>(
			RepositoryName repositoryName,
			in long generation,
			Expression<Func<TAggregateRoot, bool>> predicate)
			where TAggregateRoot : AggregateRoot<TAggregateRoot, TKey>;
	}
}
