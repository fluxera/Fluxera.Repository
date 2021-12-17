namespace Fluxera.Repository.Caching
{
	using System;
	using System.Linq.Expressions;
	using Fluxera.Entity;
	using Fluxera.Linq.Expressions;
	using Fluxera.Repository.Query;
	using Fluxera.Utilities;
	using JetBrains.Annotations;

	[PublicAPI]
	public class DefaultCacheKeyProvider : ICacheKeyProvider
	{
		/// <inheritdoc />
		public string GetGenerationCacheKey<TAggregateRoot, TKey>(RepositoryName repositoryName)
			where TAggregateRoot : AggregateRoot<TAggregateRoot, TKey>
		{
			// Repositories/{GlobalCounter}/Books/Acme.Books.Domain.Model.Book/Generation
			string cacheKey = $"{this.GetCachePrefix(repositoryName, typeof(TAggregateRoot))}/Generation";
			return cacheKey;
		}

		/// <inheritdoc />
		public string GetCountCacheKey<TAggregateRoot, TKey>(RepositoryName repositoryName, in long generation)
			where TAggregateRoot : AggregateRoot<TAggregateRoot, TKey>
		{
			// Repositories/Books/Acme.Books.Domain.Model.Book/{Generation}/Count
			string cacheKey = $"{this.GetCachePrefix(repositoryName, typeof(TAggregateRoot))}/{generation}/Count";

			return cacheKey;
		}

		/// <inheritdoc />
		public string GetCountCacheKey<TAggregateRoot, TKey>(RepositoryName repositoryName, in long generation,
			Expression<Func<TAggregateRoot, bool>> predicate)
			where TAggregateRoot : AggregateRoot<TAggregateRoot, TKey>
		{
			// Repositories/Books/Acme.Books.Domain.Model.Book/{Generation}/Count/{Predicate}
			string cacheKey = $"{this.GetCachePrefix(repositoryName, typeof(TAggregateRoot))}/{generation}/Count";
			cacheKey = $"{cacheKey}/{predicate.ToExpressionString()}";

			return cacheKey;
		}

		/// <inheritdoc />
		public string GetAddCacheKey<TAggregateRoot, TKey>(RepositoryName repositoryName, TKey id)
			where TAggregateRoot : AggregateRoot<TAggregateRoot, TKey>
		{
			// Repositories/Books/Acme.Books.Domain.Model.Book/{ID}
			return this.GetWriteThroughCacheKey<TAggregateRoot, TKey>(repositoryName, id);
		}

		/// <inheritdoc />
		public string GetUpdateCacheKey<TAggregateRoot, TKey>(RepositoryName repositoryName, TKey id)
			where TAggregateRoot : AggregateRoot<TAggregateRoot, TKey>
		{
			// Repositories/Books/Acme.Books.Domain.Model.Book/{ID}
			return this.GetWriteThroughCacheKey<TAggregateRoot, TKey>(repositoryName, id);
		}

		/// <inheritdoc />
		public string GetDeleteCacheKey<TAggregateRoot, TKey>(RepositoryName repositoryName, TKey id)
			where TAggregateRoot : AggregateRoot<TAggregateRoot, TKey>
		{
			// Repositories/Books/Acme.Books.Domain.Model.Book/{ID}
			return this.GetWriteThroughCacheKey<TAggregateRoot, TKey>(repositoryName, id);
		}

		/// <inheritdoc />
		public string GetGetCacheKey<TAggregateRoot, TKey>(RepositoryName repositoryName, TKey id)
			where TAggregateRoot : AggregateRoot<TAggregateRoot, TKey>
		{
			// Repositories/Books/Acme.Books.Domain.Model.Book/{ID}
			return this.GetWriteThroughCacheKey<TAggregateRoot, TKey>(repositoryName, id);
		}

		public string GetGetCacheKey<TAggregateRoot, TKey, TResult>(RepositoryName repositoryName, TKey id,
			Expression<Func<TAggregateRoot, TResult>> selector)
			where TAggregateRoot : AggregateRoot<TAggregateRoot, TKey>
		{
			// Repositories/Books/Acme.Books.Domain.Model.Book/{ID}/{Selector}
			string cacheKey = this.GetWriteThroughCacheKey<TAggregateRoot, TKey>(repositoryName, id);
			cacheKey = $"{cacheKey}/{selector.ToExpressionString()}";

			return cacheKey;
		}

		/// <inheritdoc />
		public string GetFindOneCacheKey<TAggregateRoot, TKey>(RepositoryName repositoryName, in long generation,
			Expression<Func<TAggregateRoot, bool>> predicate, IQueryOptions<TAggregateRoot> queryOptions)
			where TAggregateRoot : AggregateRoot<TAggregateRoot, TKey>
		{
			// Repositories/Books/Acme.Books.Domain.Model.Book/{Generation}/FindOne/{Predicate}
			string cacheKey = $"{this.GetCachePrefix(repositoryName, typeof(TAggregateRoot))}/{generation}/FindOne";
			cacheKey = $"{cacheKey}/{predicate.ToExpressionString()}";

			// Repositories/Books/Acme.Books.Domain.Model.Book/{Generation}/FindOne/{Predicate}/{QueryOptions}
			if(!queryOptions.IsEmpty)
			{
				cacheKey = $"{cacheKey}/{queryOptions}";
			}

			return cacheKey;
		}

		public string GetFindOneCacheKey<TAggregateRoot, TKey, TResult>(RepositoryName repositoryName, in long generation,
			Expression<Func<TAggregateRoot, bool>> predicate, Expression<Func<TAggregateRoot, TResult>> selector,
			IQueryOptions<TAggregateRoot> queryOptions) where TAggregateRoot : AggregateRoot<TAggregateRoot, TKey>
		{
			// Repositories/Books/Acme.Books.Domain.Model.Book/{Generation}/FindOne/{Predicate}/{Selector}
			string cacheKey = $"{this.GetCachePrefix(repositoryName, typeof(TAggregateRoot))}/{generation}/FindOne";
			cacheKey = $"{cacheKey}/{predicate.ToExpressionString()}/{selector.ToExpressionString()}";

			// Repositories/Books/Acme.Books.Domain.Model.Book/{Generation}/FindOne/{Predicate}/{Selector}/{QueryOptions}
			if(!queryOptions.IsEmpty)
			{
				cacheKey = $"{cacheKey}/{queryOptions}";
			}

			return cacheKey;
		}

		public string GetFindManyCacheKey<TAggregateRoot, TKey>(RepositoryName repositoryName, in long generation,
			Expression<Func<TAggregateRoot, bool>> predicate, IQueryOptions<TAggregateRoot> queryOptions)
			where TAggregateRoot : AggregateRoot<TAggregateRoot, TKey>
		{
			// Repositories/Books/Acme.Books.Domain.Model.Book/{Generation}/FindMany/{Predicate}
			string cacheKey = $"{this.GetCachePrefix(repositoryName, typeof(TAggregateRoot))}/{generation}/FindMany";
			cacheKey = $"{cacheKey}/{predicate.ToExpressionString()}";

			// Repositories/Books/Acme.Books.Domain.Model.Book/{Generation}/FindMany/{Predicate}/{QueryOptions}
			if(!queryOptions.IsEmpty)
			{
				cacheKey = $"{cacheKey}/{queryOptions}";
			}

			return cacheKey;
		}

		public string GetFindManyCacheKey<TAggregateRoot, TKey, TResult>(RepositoryName repositoryName, in long generation,
			Expression<Func<TAggregateRoot, bool>> predicate, Expression<Func<TAggregateRoot, TResult>> selector,
			IQueryOptions<TAggregateRoot> queryOptions) where TAggregateRoot : AggregateRoot<TAggregateRoot, TKey>
		{
			// Repositories/Books/Acme.Books.Domain.Model.Book/{Generation}/FindMany/{Predicate}/{Selector}
			string cacheKey = $"{this.GetCachePrefix(repositoryName, typeof(TAggregateRoot))}/{generation}/FindMany";
			cacheKey = $"{cacheKey}/{predicate.ToExpressionString()}/{selector.ToExpressionString()}";

			// Repositories/Books/Acme.Books.Domain.Model.Book/{Generation}/FindMany/{Predicate}/{Selector}/{QueryOptions}
			if(!queryOptions.IsEmpty)
			{
				cacheKey = $"{cacheKey}/{queryOptions}";
			}

			return cacheKey;
		}

		/// <inheritdoc />
		public string GetExistsCacheKey<TAggregateRoot, TKey>(RepositoryName repositoryName, in long generation,
			Expression<Func<TAggregateRoot, bool>> predicate) where TAggregateRoot : AggregateRoot<TAggregateRoot, TKey>
		{
			// Repositories/Books/Acme.Books.Domain.Model.Book/{Generation}/Count/{Predicate}
			string cacheKey = $"{this.GetCachePrefix(repositoryName, typeof(TAggregateRoot))}/{generation}/Exists";
			cacheKey = $"{cacheKey}/{predicate.ToExpressionString()}";

			return cacheKey;
		}

		/// <inheritdoc />
		public string GetExistsCacheKey<TAggregateRoot, TKey>(RepositoryName repositoryName, in long generation, TKey id)
			where TAggregateRoot : AggregateRoot<TAggregateRoot, TKey>
		{
			// Repositories/Books/Acme.Books.Domain.Model.Book/{Generation}/Exists/{ID}
			string cacheKey = $"{this.GetCachePrefix(repositoryName, typeof(TAggregateRoot))}/{generation}/Exists/{id}";

			return cacheKey;
		}

		/// <summary>
		///     Creates a key prefix like this: Repositories/Books/Acme.Books.Domain.Model.Book
		/// </summary>
		/// <param name="repositoryName"></param>
		/// <param name="type"></param>
		/// <returns></returns>
		protected virtual string GetCachePrefix(RepositoryName repositoryName, Type type)
		{
			long globalCounter = AsyncHelper.RunSync(CacheManager.GetGlobalCounterAsync);

			// Repositories/Books/Acme.Books.Domain.Model.Book
			return $"Repositories/{globalCounter}/{repositoryName}/{type.FullName ?? type.Name}";
		}

		private string GetWriteThroughCacheKey<TAggregateRoot, TKey>(RepositoryName repositoryName, TKey id)
			where TAggregateRoot : AggregateRoot<TAggregateRoot, TKey>
		{
			// Repositories/Books/Acme.Books.Domain.Model.Book/{ID}
			string cacheKey = $"{this.GetCachePrefix(repositoryName, typeof(TAggregateRoot))}/{id}";
			return cacheKey;
		}
	}
}
