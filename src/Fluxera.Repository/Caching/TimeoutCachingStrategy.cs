namespace Fluxera.Repository.Caching
{
	using System;
	using System.Collections.Generic;
	using System.Linq.Expressions;
	using System.Threading.Tasks;
	using Fluxera.Entity;
	using Fluxera.Repository.Query;
	using JetBrains.Annotations;
	using Microsoft.Extensions.Logging;

	[UsedImplicitly]
	internal sealed class TimeoutCachingStrategy<TAggregateRoot, TKey> : ICachingStrategy<TAggregateRoot, TKey>
		where TAggregateRoot : AggregateRoot<TAggregateRoot, TKey>
	{
		private readonly TimeSpan expiration;

		public TimeoutCachingStrategy(
			RepositoryName repositoryName,
			ICachingProvider cachingProvider,
			ICacheKeyProvider cacheKeyProvider,
			ILoggerFactory loggerFactory,
			TimeSpan? expiration)
		{
			this.RepositoryName = repositoryName;
			this.CachingProvider = cachingProvider;
			this.CacheKeyProvider = cacheKeyProvider;
			this.Logger = loggerFactory.CreateLogger(LoggerNames.Caching);

			this.expiration = expiration.GetValueOrDefault(TimeSpan.FromSeconds(10));
		}

		private ICachingProvider CachingProvider { get; }

		private ICacheKeyProvider CacheKeyProvider { get; }

		private ILogger Logger { get; }

		private RepositoryName RepositoryName { get; }

		/// <inheritdoc />
		public async Task AddAsync(TAggregateRoot item)
		{
			string cacheKey = this.CacheKeyProvider.GetAddCacheKey<TAggregateRoot, TKey>(this.RepositoryName, item.ID!);
			await this.SetSafeAsync(cacheKey, item).ConfigureAwait(false);
		}

		/// <inheritdoc />
		public async Task AddAsync(IEnumerable<TAggregateRoot> items)
		{
			foreach(TAggregateRoot item in items)
			{
				await this.AddAsync(item).ConfigureAwait(false);
			}
		}

		/// <inheritdoc />
		public async Task UpdateAsync(TAggregateRoot item)
		{
			string cacheKey = this.CacheKeyProvider.GetUpdateCacheKey<TAggregateRoot, TKey>(this.RepositoryName, item.ID!);
			await this.SetSafeAsync(cacheKey, item).ConfigureAwait(false);
		}

		/// <inheritdoc />
		public async Task UpdateAsync(IEnumerable<TAggregateRoot> items)
		{
			foreach(TAggregateRoot item in items)
			{
				await this.UpdateAsync(item).ConfigureAwait(false);
			}
		}

		/// <inheritdoc />
		public async Task RemoveAsync(TKey id)
		{
			string cacheKey = this.CacheKeyProvider.GetDeleteCacheKey<TAggregateRoot, TKey>(this.RepositoryName, id);
			await this.RemoveSafeAsync(cacheKey).ConfigureAwait(false);
		}

		/// <inheritdoc />
		public async Task RemoveAsync(IEnumerable<TKey> ids)
		{
			foreach(TKey id in ids)
			{
				await this.RemoveAsync(id).ConfigureAwait(false);
			}
		}

		/// <inheritdoc />
		public async Task<TAggregateRoot> GetAsync(TKey id, Func<Task<TAggregateRoot>> setter)
		{
			try
			{
				string cacheKey = this.CacheKeyProvider.GetGetCacheKey<TAggregateRoot, TKey>(this.RepositoryName, id);
				bool exists = await this.ExistsSafeAsync(cacheKey).ConfigureAwait(false);

				TAggregateRoot item;
				if(exists)
				{
					item = await this.GetSafeAsync<TAggregateRoot>(cacheKey).ConfigureAwait(true);
				}
				else
				{
					item = await setter.Invoke().ConfigureAwait(false);
					await this.SetSafeAsync(cacheKey, item).ConfigureAwait(false);
				}

				return item;
			}
			catch(Exception e)
			{
				this.Logger.LogError(e, e.Message);
				throw;
			}
		}

		/// <inheritdoc />
		public async Task<TResult> GetAsync<TResult>(TKey id, Expression<Func<TAggregateRoot, TResult>> selector, Func<Task<TResult>> setter)
		{
			try
			{
				string cacheKey = this.CacheKeyProvider.GetGetCacheKey(this.RepositoryName, id, selector);
				bool exists = await this.ExistsSafeAsync(cacheKey).ConfigureAwait(false);

				TResult item;
				if(exists)
				{
					item = await this.GetSafeAsync<TResult>(cacheKey).ConfigureAwait(true);
				}
				else
				{
					item = await setter.Invoke().ConfigureAwait(false);
					await this.SetSafeAsync(cacheKey, item).ConfigureAwait(false);
				}

				return item;
			}
			catch(Exception e)
			{
				this.Logger.LogError(e, e.Message);
				throw;
			}
		}

		/// <inheritdoc />
		public async Task<long> CountAsync(Func<Task<long>> setter)
		{
			try
			{
				long generation = GetGenerationAsync();
				string cacheKey = this.CacheKeyProvider.GetCountCacheKey<TAggregateRoot, TKey>(this.RepositoryName, generation);
				bool exists = await this.ExistsSafeAsync(cacheKey).ConfigureAwait(false);

				long count;
				if(exists)
				{
					count = await this.GetSafeAsync<long>(cacheKey).ConfigureAwait(true);
				}
				else
				{
					count = await setter.Invoke().ConfigureAwait(false);
					await this.SetSafeAsync(cacheKey, count).ConfigureAwait(false);
				}

				return count;
			}
			catch(Exception e)
			{
				this.Logger.LogError(e, e.Message);
				throw;
			}
		}

		/// <inheritdoc />
		public async Task<long> CountAsync(Expression<Func<TAggregateRoot, bool>> predicate, Func<Task<long>> setter)
		{
			try
			{
				long generation = GetGenerationAsync();
				string cacheKey = this.CacheKeyProvider.GetCountCacheKey<TAggregateRoot, TKey>(this.RepositoryName, generation, predicate);
				bool exists = await this.ExistsSafeAsync(cacheKey).ConfigureAwait(false);

				long count;
				if(exists)
				{
					count = await this.GetSafeAsync<long>(cacheKey).ConfigureAwait(true);
				}
				else
				{
					count = await setter.Invoke().ConfigureAwait(false);
					await this.SetSafeAsync(cacheKey, count).ConfigureAwait(false);
				}

				return count;
			}
			catch(Exception e)
			{
				this.Logger.LogError(e, e.Message);
				throw;
			}
		}

		/// <inheritdoc />
		public async Task<TAggregateRoot> FindOneAsync(Expression<Func<TAggregateRoot, bool>> predicate, IQueryOptions<TAggregateRoot>? queryOptions, Func<Task<TAggregateRoot>> setter)
		{
			try
			{
				long generation = GetGenerationAsync();
				string cacheKey = this.CacheKeyProvider.GetFindOneCacheKey<TAggregateRoot, TKey>(this.RepositoryName, generation, predicate, queryOptions);
				bool exists = await this.ExistsSafeAsync(cacheKey).ConfigureAwait(false);

				TAggregateRoot item;
				if(exists)
				{
					item = await this.GetSafeAsync<TAggregateRoot>(cacheKey).ConfigureAwait(true);
				}
				else
				{
					item = await setter.Invoke().ConfigureAwait(false);
					await this.SetSafeAsync(cacheKey, item).ConfigureAwait(false);
				}

				return item;
			}
			catch(Exception e)
			{
				this.Logger.LogError(e, e.Message);
				throw;
			}
		}

		/// <inheritdoc />
		public async Task<TResult> FindOneAsync<TResult>(Expression<Func<TAggregateRoot, bool>> predicate, Expression<Func<TAggregateRoot, TResult>> selector, IQueryOptions<TAggregateRoot>? queryOptions, Func<Task<TResult>> setter)
		{
			try
			{
				long generation = GetGenerationAsync();
				string cacheKey = this.CacheKeyProvider.GetFindOneCacheKey<TAggregateRoot, TKey, TResult>(this.RepositoryName, generation, predicate, selector, queryOptions);
				bool exists = await this.ExistsSafeAsync(cacheKey).ConfigureAwait(false);

				TResult item;
				if(exists)
				{
					item = await this.GetSafeAsync<TResult>(cacheKey).ConfigureAwait(true);
				}
				else
				{
					item = await setter.Invoke().ConfigureAwait(false);
					await this.SetSafeAsync(cacheKey, item).ConfigureAwait(false);
				}

				return item;
			}
			catch(Exception e)
			{
				this.Logger.LogError(e, e.Message);
				throw;
			}
		}

		/// <inheritdoc />
		public async Task<IReadOnlyCollection<TAggregateRoot>> FindManyAsync(Expression<Func<TAggregateRoot, bool>> predicate, IQueryOptions<TAggregateRoot>? queryOptions, Func<Task<IReadOnlyCollection<TAggregateRoot>>> setter)
		{
			try
			{
				long generation = GetGenerationAsync();
				string cacheKey = this.CacheKeyProvider.GetFindManyCacheKey<TAggregateRoot, TKey>(this.RepositoryName, generation, predicate, queryOptions);
				bool exists = await this.ExistsSafeAsync(cacheKey).ConfigureAwait(false);

				IReadOnlyCollection<TAggregateRoot> items;
				if(exists)
				{
					items = await this.GetSafeAsync<IReadOnlyList<TAggregateRoot>>(cacheKey).ConfigureAwait(true);
				}
				else
				{
					items = await setter.Invoke().ConfigureAwait(false);
					await this.SetSafeAsync(cacheKey, items).ConfigureAwait(false);
				}

				return items;
			}
			catch(Exception e)
			{
				this.Logger.LogError(e, e.Message);
				throw;
			}
		}

		/// <inheritdoc />
		public async Task<IReadOnlyCollection<TResult>> FindManyAsync<TResult>(Expression<Func<TAggregateRoot, bool>> predicate, Expression<Func<TAggregateRoot, TResult>> selector, IQueryOptions<TAggregateRoot>? queryOptions, Func<Task<IReadOnlyCollection<TResult>>> setter)
		{
			try
			{
				long generation = GetGenerationAsync();
				string cacheKey = this.CacheKeyProvider.GetFindManyCacheKey<TAggregateRoot, TKey, TResult>(this.RepositoryName, generation, predicate, selector, queryOptions);
				bool exists = await this.ExistsSafeAsync(cacheKey).ConfigureAwait(false);

				IReadOnlyCollection<TResult> items;
				if(exists)
				{
					items = await this.GetSafeAsync<IReadOnlyList<TResult>>(cacheKey).ConfigureAwait(true);
				}
				else
				{
					items = await setter.Invoke().ConfigureAwait(false);
					await this.SetSafeAsync(cacheKey, items).ConfigureAwait(false);
				}

				return items;
			}
			catch(Exception e)
			{
				this.Logger.LogError(e, e.Message);
				throw;
			}
		}

		/// <inheritdoc />
		public async Task<bool> ExistsAsync(TKey id, Func<Task<bool>> setter)
		{
			try
			{
				long generation = GetGenerationAsync();
				string cacheKey = this.CacheKeyProvider.GetExistsCacheKey<TAggregateRoot, TKey>(this.RepositoryName, generation, id);
				bool exists = await this.ExistsSafeAsync(cacheKey).ConfigureAwait(false);

				bool result;
				if(exists)
				{
					result = await this.GetSafeAsync<bool>(cacheKey).ConfigureAwait(true);
				}
				else
				{
					result = await setter.Invoke().ConfigureAwait(false);
					await this.SetSafeAsync(cacheKey, result).ConfigureAwait(false);
				}

				return result;
			}
			catch(Exception e)
			{
				this.Logger.LogError(e, e.Message);
				throw;
			}
		}

		/// <inheritdoc />
		public async Task<bool> ExistsAsync(Expression<Func<TAggregateRoot, bool>> predicate, Func<Task<bool>> setter)
		{
			try
			{
				long generation = GetGenerationAsync();
				string cacheKey = this.CacheKeyProvider.GetExistsCacheKey<TAggregateRoot, TKey>(this.RepositoryName, generation, predicate);
				bool exists = await this.ExistsSafeAsync(cacheKey).ConfigureAwait(false);

				bool result;
				if(exists)
				{
					result = await this.GetSafeAsync<bool>(cacheKey).ConfigureAwait(true);
				}
				else
				{
					result = await setter.Invoke().ConfigureAwait(false);
					await this.SetSafeAsync(cacheKey, result).ConfigureAwait(false);
				}

				return result;
			}
			catch(Exception e)
			{
				this.Logger.LogError(e, e.Message);
				throw;
			}
		}

		private static long GetGenerationAsync()
		{
			return 0;
		}

		private async Task<TCacheItem> GetSafeAsync<TCacheItem>(string cacheKey)
		{
			try
			{
				return await this.CachingProvider
					.GetAsync<TCacheItem>(cacheKey)
					.ConfigureAwait(false);
			}
			catch(Exception e)
			{
				// Don't let caching errors mess with the repository.
				this.Logger.LogError(e, e.Message);
				return default!;
			}
		}

		private async Task<bool> ExistsSafeAsync(string cacheKey)
		{
			try
			{
				bool exists = await this.CachingProvider
					.ExistsAsync(cacheKey)
					.ConfigureAwait(false);

				this.Logger.LogTrace(exists
					? $"Entry found in cache for key: {cacheKey}"
					: $"No entry found in cache key: {cacheKey}");

				return exists;
			}
			catch(Exception e)
			{
				// Don't let caching errors mess with the repository.
				this.Logger.LogError(e, e.Message);
				return false;
			}
		}

		private async Task RemoveSafeAsync(string cacheKey)
		{
			try
			{
				await this.CachingProvider
					.RemoveAsync(cacheKey)
					.ConfigureAwait(false);
			}
			catch(Exception e)
			{
				// Don't let caching errors mess with the repository.
				this.Logger.LogError(e, e.Message);
			}
		}

		private async Task SetSafeAsync<TCacheItem>(string cacheKey, TCacheItem item)
		{
			try
			{
				await this.CachingProvider
					.SetAsync(cacheKey, item, this.expiration)
					.ConfigureAwait(false);
			}
			catch(Exception e)
			{
				// Don't let caching errors mess with the repository.
				this.Logger.LogError(e, e.Message);
			}
		}
	}
}
