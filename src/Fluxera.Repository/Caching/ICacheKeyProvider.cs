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
		string GetGenerationCacheKey<TAggregateRoot>(
			RepositoryName repositoryName) where TAggregateRoot : AggregateRoot<TAggregateRoot>;

		string GetAddCacheKey<TAggregateRoot>(
			RepositoryName repositoryName, 
			string id) where TAggregateRoot : AggregateRoot<TAggregateRoot>;

		string GetUpdateCacheKey<TAggregateRoot>(
			RepositoryName repositoryName, 
			string id) where TAggregateRoot : AggregateRoot<TAggregateRoot>;

		string GetDeleteCacheKey<TAggregateRoot>(
			RepositoryName repositoryName, 
			string id) where TAggregateRoot : AggregateRoot<TAggregateRoot>;

		string GetGetCacheKey<TAggregateRoot>(
			RepositoryName repositoryName, 
			string id) where TAggregateRoot : AggregateRoot<TAggregateRoot>;

		string GetGetCacheKey<TAggregateRoot, TResult>(
			RepositoryName repositoryName, 
			string id,
			Expression<Func<TAggregateRoot, TResult>> selector) where TAggregateRoot : AggregateRoot<TAggregateRoot>;

		string GetCountCacheKey<TAggregateRoot>(
			RepositoryName repositoryName, 
			in long generation);

		string GetCountCacheKey<TAggregateRoot>(
			RepositoryName repositoryName, 
			in long generation,
			Expression<Func<TAggregateRoot, bool>> predicate) where TAggregateRoot : AggregateRoot<TAggregateRoot>;

		string GetFindOneCacheKey<TAggregateRoot>(
			RepositoryName repositoryName, 
			in long generation,
			Expression<Func<TAggregateRoot, bool>> predicate, 
			IQueryOptions<TAggregateRoot> queryOptions) where TAggregateRoot : AggregateRoot<TAggregateRoot>;

		string GetFindOneCacheKey<TAggregateRoot, TResult>(
			RepositoryName repositoryName, 
			in long generation,
			Expression<Func<TAggregateRoot, bool>> predicate, 
			Expression<Func<TAggregateRoot, TResult>> selector,
			IQueryOptions<TAggregateRoot> queryOptions) where TAggregateRoot : AggregateRoot<TAggregateRoot>;

		string GetFindManyCacheKey<TAggregateRoot>(
			RepositoryName repositoryName, 
			in long generation,
			Expression<Func<TAggregateRoot, bool>> predicate, 
			IQueryOptions<TAggregateRoot> queryOptions) where TAggregateRoot : AggregateRoot<TAggregateRoot>;

		string GetFindManyCacheKey<TAggregateRoot, TResult>(
			RepositoryName repositoryName, 
			in long generation,
			Expression<Func<TAggregateRoot, bool>> predicate, 
			Expression<Func<TAggregateRoot, TResult>> selector,
			IQueryOptions<TAggregateRoot> queryOptions) where TAggregateRoot : AggregateRoot<TAggregateRoot>;

		string GetExistsCacheKey<TAggregateRoot>(
			RepositoryName repositoryName, 
			in long generation,
			string id) 
			where TAggregateRoot : AggregateRoot<TAggregateRoot>;

		string GetExistsCacheKey<TAggregateRoot>(
			RepositoryName repositoryName, 
			in long generation, 
			Expression<Func<TAggregateRoot, bool>> predicate) 
			where TAggregateRoot : AggregateRoot<TAggregateRoot>;
	}
}
