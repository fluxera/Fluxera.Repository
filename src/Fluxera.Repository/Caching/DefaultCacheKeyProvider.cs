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
		public string GetGenerationCacheKey<TAggregateRoot>(RepositoryName repositoryName)
			where TAggregateRoot : AggregateRoot<TAggregateRoot>
		{
			// Repositories/{GlobalCounter}/Books/Acme.Books.Domain.Model.Book/Generation
			string cacheKey = $"{this.GetCachePrefix(repositoryName, typeof(TAggregateRoot))}/Generation";
			return cacheKey;
		}

		/// <inheritdoc />
		public string GetAddCacheKey<TAggregateRoot>(RepositoryName repositoryName, string id)
			where TAggregateRoot : AggregateRoot<TAggregateRoot>
		{
			// Repositories/Books/Acme.Books.Domain.Model.Book/{ID}
			return this.GetWriteThroughCacheKey<TAggregateRoot>(repositoryName, id);
		}

		/// <inheritdoc />
		public string GetUpdateCacheKey<TAggregateRoot>(RepositoryName repositoryName, string id)
			where TAggregateRoot : AggregateRoot<TAggregateRoot>
		{
			// Repositories/Books/Acme.Books.Domain.Model.Book/{ID}
			return this.GetWriteThroughCacheKey<TAggregateRoot>(repositoryName, id);
		}

		/// <inheritdoc />
		public string GetDeleteCacheKey<TAggregateRoot>(RepositoryName repositoryName, string id)
			where TAggregateRoot : AggregateRoot<TAggregateRoot>
		{
			// Repositories/Books/Acme.Books.Domain.Model.Book/{ID}
			return this.GetWriteThroughCacheKey<TAggregateRoot>(repositoryName, id);
		}

		/// <inheritdoc />
		public string GetGetCacheKey<TAggregateRoot>(RepositoryName repositoryName, string id)
			where TAggregateRoot : AggregateRoot<TAggregateRoot>
		{
			// Repositories/Books/Acme.Books.Domain.Model.Book/{ID}
			return this.GetWriteThroughCacheKey<TAggregateRoot>(repositoryName, id);
		}

		public string GetGetCacheKey<TAggregateRoot, TResult>(RepositoryName repositoryName, string id,
			Expression<Func<TAggregateRoot, TResult>> selector)
			where TAggregateRoot : AggregateRoot<TAggregateRoot>
		{
			// Repositories/Books/Acme.Books.Domain.Model.Book/{ID}/{Selector}
			string cacheKey = this.GetWriteThroughCacheKey<TAggregateRoot>(repositoryName, id);
			cacheKey = $"{cacheKey}/{selector.ToExpressionString()}";

			return cacheKey;
		}

		/// <inheritdoc />
		public string GetCountCacheKey<TAggregateRoot>(RepositoryName repositoryName, in long generation)
		{
			// Repositories/Books/Acme.Books.Domain.Model.Book/{Generation}/Count
			string cacheKey = $"{this.GetCachePrefix(repositoryName, typeof(TAggregateRoot))}/{generation}/Count";

			return cacheKey;
		}

		/// <inheritdoc />
		public string GetCountCacheKey<TAggregateRoot>(RepositoryName repositoryName, in long generation,
			Expression<Func<TAggregateRoot, bool>> predicate)
			where TAggregateRoot : AggregateRoot<TAggregateRoot>
		{
			// Repositories/Books/Acme.Books.Domain.Model.Book/{Generation}/Count/{Predicate}
			string cacheKey = $"{this.GetCachePrefix(repositoryName, typeof(TAggregateRoot))}/{generation}/Count";
			cacheKey = $"{cacheKey}/{predicate.ToExpressionString()}";

			return cacheKey;
		}

		/// <inheritdoc />
		public string GetFindOneCacheKey<TAggregateRoot>(RepositoryName repositoryName, in long generation,
			Expression<Func<TAggregateRoot, bool>> predicate, IQueryOptions<TAggregateRoot> queryOptions)
			where TAggregateRoot : AggregateRoot<TAggregateRoot>
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

		public string GetFindOneCacheKey<TAggregateRoot, TResult>(RepositoryName repositoryName, in long generation,
			Expression<Func<TAggregateRoot, bool>> predicate, Expression<Func<TAggregateRoot, TResult>> selector,
			IQueryOptions<TAggregateRoot> queryOptions) where TAggregateRoot : AggregateRoot<TAggregateRoot>
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

		public string GetFindManyCacheKey<TAggregateRoot>(RepositoryName repositoryName, in long generation,
			Expression<Func<TAggregateRoot, bool>> predicate, IQueryOptions<TAggregateRoot> queryOptions)
			where TAggregateRoot : AggregateRoot<TAggregateRoot>
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

		public string GetFindManyCacheKey<TAggregateRoot, TResult>(RepositoryName repositoryName, in long generation,
			Expression<Func<TAggregateRoot, bool>> predicate, Expression<Func<TAggregateRoot, TResult>> selector,
			IQueryOptions<TAggregateRoot> queryOptions) where TAggregateRoot : AggregateRoot<TAggregateRoot>
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
		public string GetExistsCacheKey<TAggregateRoot>(RepositoryName repositoryName, in long generation, string id)
			where TAggregateRoot : AggregateRoot<TAggregateRoot>
		{
			// Repositories/Books/Acme.Books.Domain.Model.Book/{Generation}/Exists/{ID}
			string cacheKey = $"{this.GetCachePrefix(repositoryName, typeof(TAggregateRoot))}/{generation}/Exists/{id}";

			return cacheKey;
		}

		/// <inheritdoc />
		public string GetExistsCacheKey<TAggregateRoot>(RepositoryName repositoryName, in long generation,
			Expression<Func<TAggregateRoot, bool>> predicate) where TAggregateRoot : AggregateRoot<TAggregateRoot>
		{
			// Repositories/Books/Acme.Books.Domain.Model.Book/{Generation}/Count/{Predicate}
			string cacheKey = $"{this.GetCachePrefix(repositoryName, typeof(TAggregateRoot))}/{generation}/Exists";
			cacheKey = $"{cacheKey}/{predicate.ToExpressionString()}";

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

		private string GetWriteThroughCacheKey<TAggregateRoot>(RepositoryName repositoryName, string id)
			where TAggregateRoot : AggregateRoot<TAggregateRoot>
		{
			// Repositories/Books/Acme.Books.Domain.Model.Book/{ID}
			string cacheKey = $"{this.GetCachePrefix(repositoryName, typeof(TAggregateRoot))}/{id}";
			return cacheKey;
		}
	}
}
