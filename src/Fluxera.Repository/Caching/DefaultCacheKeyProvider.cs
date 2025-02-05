namespace Fluxera.Repository.Caching
{
	using System;
	using System.Linq.Expressions;
	using Fluxera.Entity;
	using Fluxera.Linq.Expressions;
	using Fluxera.Repository.Query;
	using Fluxera.Utilities;
	using JetBrains.Annotations;

	/// <summary>
	///     The default implementation of <see cref="ICacheKeyProvider" /> which provides
	///     the protected virtual method <see cref="GetCachePrefix" /> for use in derived
	///     classes.
	/// </summary>
	[PublicAPI]
	public class DefaultCacheKeyProvider : ICacheKeyProvider
	{
		/// <inheritdoc />
		public string GetGenerationCacheKey<TEntity, TKey>(RepositoryName repositoryName)
			where TEntity : Entity<TEntity, TKey>
			where TKey : IComparable<TKey>, IEquatable<TKey>
		{
			// Repositories/{GlobalCounter}/Books/Acme.Books.Domain.Model.Book/Generation
			string cacheKey = $"{this.GetCachePrefix(repositoryName, typeof(TEntity))}/Generation";
			return cacheKey;
		}

		/// <inheritdoc />
		public string GetCountCacheKey<TEntity, TKey>(RepositoryName repositoryName, in long generation)
			where TEntity : Entity<TEntity, TKey>
			where TKey : IComparable<TKey>, IEquatable<TKey>
		{
			// Repositories/Books/Acme.Books.Domain.Model.Book/{Generation}/Count
			string cacheKey = $"{this.GetCachePrefix(repositoryName, typeof(TEntity))}/{generation}/Count";

			return cacheKey;
		}

		/// <inheritdoc />
		public string GetCountCacheKey<TEntity, TKey>(RepositoryName repositoryName, in long generation,
			Expression<Func<TEntity, bool>> predicate)
			where TEntity : Entity<TEntity, TKey>
			where TKey : IComparable<TKey>, IEquatable<TKey>
		{
			// Repositories/Books/Acme.Books.Domain.Model.Book/{Generation}/Count/{Predicate}
			string cacheKey = $"{this.GetCachePrefix(repositoryName, typeof(TEntity))}/{generation}/Count";
			cacheKey = $"{cacheKey}/{predicate.ToExpressionString()}";

			return cacheKey;
		}

		/// <inheritdoc />
		public string GetSumCacheKey<TEntity, TKey>(RepositoryName repositoryName, in long generation)
			where TEntity : Entity<TEntity, TKey>
			where TKey : notnull, IComparable<TKey>, IEquatable<TKey>
		{
			// Repositories/Books/Acme.Books.Domain.Model.Book/{Generation}/Sum
			string cacheKey = $"{this.GetCachePrefix(repositoryName, typeof(TEntity))}/{generation}/Sum";

			return cacheKey;
		}

		/// <inheritdoc />
		public string GetAverageCacheKey<TEntity, TKey>(RepositoryName repositoryName, in long generation,
			Expression<Func<TEntity, bool>> predicate)
			where TEntity : Entity<TEntity, TKey>
			where TKey : notnull, IComparable<TKey>, IEquatable<TKey>
		{
			// Repositories/Books/Acme.Books.Domain.Model.Book/{Generation}/Average/{Predicate}
			string cacheKey = $"{this.GetCachePrefix(repositoryName, typeof(TEntity))}/{generation}/Average";
			cacheKey = $"{cacheKey}/{predicate.ToExpressionString()}";

			return cacheKey;
		}

		/// <inheritdoc />
		public string GetAverageCacheKey<TEntity, TKey>(RepositoryName repositoryName, in long generation)
			where TEntity : Entity<TEntity, TKey>
			where TKey : notnull, IComparable<TKey>, IEquatable<TKey>
		{
			// Repositories/Books/Acme.Books.Domain.Model.Book/{Generation}/Average
			string cacheKey = $"{this.GetCachePrefix(repositoryName, typeof(TEntity))}/{generation}/Average";

			return cacheKey;
		}

		/// <inheritdoc />
		public string GetSumCacheKey<TEntity, TKey>(RepositoryName repositoryName, in long generation,
			Expression<Func<TEntity, bool>> predicate)
			where TEntity : Entity<TEntity, TKey>
			where TKey : notnull, IComparable<TKey>, IEquatable<TKey>
		{
			// Repositories/Books/Acme.Books.Domain.Model.Book/{Generation}/Sum/{Predicate}
			string cacheKey = $"{this.GetCachePrefix(repositoryName, typeof(TEntity))}/{generation}/Sum";
			cacheKey = $"{cacheKey}/{predicate.ToExpressionString()}";

			return cacheKey;
		}

		/// <inheritdoc />
		public string GetAddCacheKey<TEntity, TKey>(RepositoryName repositoryName, TKey id)
			where TEntity : Entity<TEntity, TKey>
			where TKey : IComparable<TKey>, IEquatable<TKey>
		{
			// Repositories/Books/Acme.Books.Domain.Model.Book/{ID}
			return this.GetWriteThroughCacheKey<TEntity, TKey>(repositoryName, id);
		}

		/// <inheritdoc />
		public string GetUpdateCacheKey<TEntity, TKey>(RepositoryName repositoryName, TKey id)
			where TEntity : Entity<TEntity, TKey>
			where TKey : IComparable<TKey>, IEquatable<TKey>
		{
			// Repositories/Books/Acme.Books.Domain.Model.Book/{ID}
			return this.GetWriteThroughCacheKey<TEntity, TKey>(repositoryName, id);
		}

		/// <inheritdoc />
		public string GetDeleteCacheKey<TEntity, TKey>(RepositoryName repositoryName, TKey id)
			where TEntity : Entity<TEntity, TKey>
			where TKey : IComparable<TKey>, IEquatable<TKey>
		{
			// Repositories/Books/Acme.Books.Domain.Model.Book/{ID}
			return this.GetWriteThroughCacheKey<TEntity, TKey>(repositoryName, id);
		}

		/// <inheritdoc />
		public string GetGetCacheKey<TEntity, TKey>(RepositoryName repositoryName, TKey id)
			where TEntity : Entity<TEntity, TKey>
			where TKey : IComparable<TKey>, IEquatable<TKey>
		{
			// Repositories/Books/Acme.Books.Domain.Model.Book/{ID}
			return this.GetWriteThroughCacheKey<TEntity, TKey>(repositoryName, id);
		}

		/// <inheritdoc />
		public string GetGetCacheKey<TEntity, TKey, TResult>(RepositoryName repositoryName, TKey id,
			Expression<Func<TEntity, TResult>> selector)
			where TEntity : Entity<TEntity, TKey>
			where TKey : IComparable<TKey>, IEquatable<TKey>
		{
			// Repositories/Books/Acme.Books.Domain.Model.Book/{ID}/{Selector}
			string cacheKey = this.GetWriteThroughCacheKey<TEntity, TKey>(repositoryName, id);
			cacheKey = $"{cacheKey}/{selector.ToExpressionString()}";

			return cacheKey;
		}

		/// <inheritdoc />
		public string GetFindOneCacheKey<TEntity, TKey>(RepositoryName repositoryName, in long generation,
			Expression<Func<TEntity, bool>> predicate,
			IQueryOptions<TEntity> queryOptions)
			where TEntity : Entity<TEntity, TKey>
			where TKey : IComparable<TKey>, IEquatable<TKey>
		{
			// Repositories/Books/Acme.Books.Domain.Model.Book/{Generation}/FindOne/{Predicate}
			string cacheKey = $"{this.GetCachePrefix(repositoryName, typeof(TEntity))}/{generation}/FindOne";
			cacheKey = $"{cacheKey}/{predicate.ToExpressionString()}";

			// Repositories/Books/Acme.Books.Domain.Model.Book/{Generation}/FindOne/{Predicate}/{QueryOptions}
			if(queryOptions is not null)
			{
				cacheKey = $"{cacheKey}/{queryOptions}";
			}

			return cacheKey;
		}

		/// <inheritdoc />
		public string GetFindOneCacheKey<TEntity, TKey, TResult>(RepositoryName repositoryName, in long generation,
			Expression<Func<TEntity, bool>> predicate,
			Expression<Func<TEntity, TResult>> selector,
			IQueryOptions<TEntity> queryOptions)
			where TEntity : Entity<TEntity, TKey>
			where TKey : IComparable<TKey>, IEquatable<TKey>
		{
			// Repositories/Books/Acme.Books.Domain.Model.Book/{Generation}/FindOne/{Predicate}/{Selector}
			string cacheKey = $"{this.GetCachePrefix(repositoryName, typeof(TEntity))}/{generation}/FindOne";
			cacheKey = $"{cacheKey}/{predicate.ToExpressionString()}/{selector.ToExpressionString()}";

			// Repositories/Books/Acme.Books.Domain.Model.Book/{Generation}/FindOne/{Predicate}/{Selector}/{QueryOptions}
			if(queryOptions is not null)
			{
				cacheKey = $"{cacheKey}/{queryOptions}";
			}

			return cacheKey;
		}

		/// <inheritdoc />
		public string GetFindManyCacheKey<TEntity, TKey>(RepositoryName repositoryName, in long generation,
			Expression<Func<TEntity, bool>> predicate, IQueryOptions<TEntity> queryOptions)
			where TEntity : Entity<TEntity, TKey>
			where TKey : IComparable<TKey>, IEquatable<TKey>
		{
			// Repositories/Books/Acme.Books.Domain.Model.Book/{Generation}/FindMany/{Predicate}
			string cacheKey = $"{this.GetCachePrefix(repositoryName, typeof(TEntity))}/{generation}/FindMany";
			cacheKey = $"{cacheKey}/{predicate.ToExpressionString()}";

			// Repositories/Books/Acme.Books.Domain.Model.Book/{Generation}/FindMany/{Predicate}/{QueryOptions}
			if(queryOptions is not null)
			{
				cacheKey = $"{cacheKey}/{queryOptions}";
			}

			return cacheKey;
		}

		/// <inheritdoc />
		public string GetFindManyCacheKey<TEntity, TKey, TResult>(RepositoryName repositoryName, in long generation,
			Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TResult>> selector,
			IQueryOptions<TEntity> queryOptions)
			where TEntity : Entity<TEntity, TKey>
			where TKey : IComparable<TKey>, IEquatable<TKey>
		{
			// Repositories/Books/Acme.Books.Domain.Model.Book/{Generation}/FindMany/{Predicate}/{Selector}
			string cacheKey = $"{this.GetCachePrefix(repositoryName, typeof(TEntity))}/{generation}/FindMany";
			cacheKey = $"{cacheKey}/{predicate.ToExpressionString()}/{selector.ToExpressionString()}";

			// Repositories/Books/Acme.Books.Domain.Model.Book/{Generation}/FindMany/{Predicate}/{Selector}/{QueryOptions}
			if(queryOptions is not null)
			{
				cacheKey = $"{cacheKey}/{queryOptions}";
			}

			return cacheKey;
		}

		/// <inheritdoc />
		public string GetExistsCacheKey<TEntity, TKey>(RepositoryName repositoryName, in long generation,
			Expression<Func<TEntity, bool>> predicate)
			where TEntity : Entity<TEntity, TKey>
			where TKey : IComparable<TKey>, IEquatable<TKey>
		{
			// Repositories/Books/Acme.Books.Domain.Model.Book/{Generation}/Count/{Predicate}
			string cacheKey = $"{this.GetCachePrefix(repositoryName, typeof(TEntity))}/{generation}/Exists";
			cacheKey = $"{cacheKey}/{predicate.ToExpressionString()}";

			return cacheKey;
		}

		/// <inheritdoc />
		public string GetExistsCacheKey<TEntity, TKey>(RepositoryName repositoryName, in long generation, TKey id)
			where TEntity : Entity<TEntity, TKey>
			where TKey : IComparable<TKey>, IEquatable<TKey>
		{
			// Repositories/Books/Acme.Books.Domain.Model.Book/{Generation}/Exists/{ID}
			string cacheKey = $"{this.GetCachePrefix(repositoryName, typeof(TEntity))}/{generation}/Exists/{id}";

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

		private string GetWriteThroughCacheKey<TEntity, TKey>(RepositoryName repositoryName, TKey id)
			where TEntity : Entity<TEntity, TKey>
			where TKey : IComparable<TKey>, IEquatable<TKey>
		{
			// Repositories/Books/Acme.Books.Domain.Model.Book/{ID}
			string cacheKey = $"{this.GetCachePrefix(repositoryName, typeof(TEntity))}/{id}";
			return cacheKey;
		}
	}
}
