﻿namespace Fluxera.Repository.Caching
{
	using System;
	using System.Collections.Generic;
	using System.Linq.Expressions;
	using System.Threading.Tasks;
	using Fluxera.Entity;
	using Fluxera.Repository.Query;
	using JetBrains.Annotations;
	using Microsoft.Extensions.Logging;

	// References that were helpful in developing the Write Through Caching and Generational Caching logic.
	//  https://blog.fairwaytech.com/2012/09/write-through-and-generational-caching
	//  http://www.regexprn.com/2011/06/web-application-caching-strategies.html
	//  http://www.regexprn.com/2011/06/web-application-caching-strategies_05.html
	//  http://37signals.com/svn/posts/3113-how-key-based-cache-expiration-works
	//  http://assets.en.oreilly.com/1/event/27/Accelerate%20your%20Rails%20Site%20with%20Automatic%20Generation-based%20Action%20Caching%20Presentation%201.pdf
	[UsedImplicitly]
	internal sealed class StandardCachingStrategy<TAggregateRoot> : ICachingStrategy<TAggregateRoot>
		where TAggregateRoot : AggregateRoot<TAggregateRoot>
	{
		public StandardCachingStrategy(
			RepositoryName repositoryName,
			ICachingProvider cachingProvider,
			ICacheKeyProvider cacheKeyProvider,
			ILoggerFactory loggerFactory)
		{
			this.RepositoryName = repositoryName;
			this.CachingProvider = cachingProvider;
			this.CacheKeyProvider = cacheKeyProvider;
			this.Logger = loggerFactory.CreateLogger(LoggerNames.Caching);
		}

		private ICachingProvider CachingProvider { get; }

		private ICacheKeyProvider CacheKeyProvider { get; }

		private ILogger Logger { get; }

		private RepositoryName RepositoryName { get; }

		/// <inheritdoc />
		public async Task AddAsync(TAggregateRoot item)
		{
			string cacheKey = this.CacheKeyProvider.GetAddCacheKey<TAggregateRoot>(this.RepositoryName, item.ID);
			await this.SetSafeAsync(cacheKey, item).ConfigureAwait(false);

			await this.IncrementGenerationAsync().ConfigureAwait(false);
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
			string cacheKey = this.CacheKeyProvider.GetUpdateCacheKey<TAggregateRoot>(this.RepositoryName, item.ID);
			await this.SetSafeAsync(cacheKey, item).ConfigureAwait(false);

			await this.IncrementGenerationAsync().ConfigureAwait(false);
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
		public async Task DeleteAsync(string id)
		{
			string cacheKey = this.CacheKeyProvider.GetDeleteCacheKey<TAggregateRoot>(this.RepositoryName, id);
			await this.RemoveSafeAsync(cacheKey).ConfigureAwait(false);

			await this.IncrementGenerationAsync().ConfigureAwait(false);
		}

		/// <inheritdoc />
		public async Task DeleteAsync(IEnumerable<string> ids)
		{
			foreach(string id in ids)
			{
				await this.DeleteAsync(id).ConfigureAwait(false);
			}
		}

		/// <inheritdoc />
		public async Task<TAggregateRoot> GetAsync(string id, Func<Task<TAggregateRoot>> setter)
		{
			try
			{
				string cacheKey = this.CacheKeyProvider.GetGetCacheKey<TAggregateRoot>(this.RepositoryName, id);
				bool exists = await this.ExistsSafeTask(cacheKey).ConfigureAwait(false);

				TAggregateRoot item;
				if (exists)
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
			catch (Exception e)
			{
				this.Logger.LogError(e, e.Message);
				throw;
			}
		}

		/// <inheritdoc />
		public async Task<TResult> GetAsync<TResult>(string id, Expression<Func<TAggregateRoot, TResult>> selector,
			Func<Task<TResult>> setter)
		{
			try
			{
				string cacheKey = this.CacheKeyProvider.GetGetCacheKey(this.RepositoryName, id, selector);
				bool exists = await this.ExistsSafeTask(cacheKey).ConfigureAwait(false);

				TResult item;
				if (exists)
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
			catch (Exception e)
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
				long generation = await this.GetGenerationAsync().ConfigureAwait(false);
				string cacheKey = this.CacheKeyProvider.GetCountCacheKey<TAggregateRoot>(this.RepositoryName, generation);
				bool exists = await this.ExistsSafeTask(cacheKey).ConfigureAwait(false);

				long count;
				if (exists)
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
			catch (Exception e)
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
				long generation = await this.GetGenerationAsync().ConfigureAwait(false);
				string cacheKey = this.CacheKeyProvider.GetCountCacheKey(this.RepositoryName, generation, predicate);
				bool exists = await this.ExistsSafeTask(cacheKey).ConfigureAwait(false);

				long count;
				if (exists)
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
			catch (Exception e)
			{
				this.Logger.LogError(e, e.Message);
				throw;
			}
		}

		/// <inheritdoc />
		public async Task<TAggregateRoot> FindOneAsync(Expression<Func<TAggregateRoot, bool>> predicate,
			IQueryOptions<TAggregateRoot>? queryOptions, Func<Task<TAggregateRoot>> setter)
		{
			try
			{
				long generation = await this.GetGenerationAsync().ConfigureAwait(false);
				string cacheKey = this.CacheKeyProvider.GetFindOneCacheKey(this.RepositoryName, generation, predicate, queryOptions);
				bool exists = await this.ExistsSafeTask(cacheKey).ConfigureAwait(false);

				TAggregateRoot item;
				if (exists)
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
			catch (Exception e)
			{
				this.Logger.LogError(e, e.Message);
				throw;
			}
		}

		public async Task<TResult> FindOneAsync<TResult>(Expression<Func<TAggregateRoot, bool>> predicate,
			Expression<Func<TAggregateRoot, TResult>> selector, IQueryOptions<TAggregateRoot>? queryOptions,
			Func<Task<TResult>> setter)
		{
			try
			{
				long generation = await this.GetGenerationAsync().ConfigureAwait(false);
				string cacheKey = this.CacheKeyProvider.GetFindOneCacheKey(this.RepositoryName, generation, predicate, selector, queryOptions);
				bool exists = await this.ExistsSafeTask(cacheKey).ConfigureAwait(false);

				TResult item;
				if (exists)
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
			catch (Exception e)
			{
				this.Logger.LogError(e, e.Message);
				throw;
			}
		}

		public async Task<IReadOnlyCollection<TAggregateRoot>> FindManyAsync(Expression<Func<TAggregateRoot, bool>> predicate,
			IQueryOptions<TAggregateRoot>? queryOptions, Func<Task<IReadOnlyCollection<TAggregateRoot>>> setter)
		{
			try
			{
				long generation = await this.GetGenerationAsync().ConfigureAwait(false);
				string cacheKey = this.CacheKeyProvider.GetFindManyCacheKey(this.RepositoryName, generation, predicate, queryOptions);
				bool exists = await this.ExistsSafeTask(cacheKey).ConfigureAwait(false);

				IReadOnlyCollection<TAggregateRoot> items;
				if (exists)
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
			catch (Exception e)
			{
				this.Logger.LogError(e, e.Message);
				throw;
			}
		}

		public async Task<IReadOnlyCollection<TResult>> FindManyAsync<TResult>(
			Expression<Func<TAggregateRoot, bool>> predicate, Expression<Func<TAggregateRoot, TResult>> selector,
			IQueryOptions<TAggregateRoot>? queryOptions, Func<Task<IReadOnlyCollection<TResult>>> setter)
		{
			try
			{
				long generation = await this.GetGenerationAsync().ConfigureAwait(false);
				string cacheKey = this.CacheKeyProvider.GetFindManyCacheKey(this.RepositoryName, generation, predicate, selector, queryOptions);
				bool exists = await this.ExistsSafeTask(cacheKey).ConfigureAwait(false);

				IReadOnlyCollection<TResult> items;
				if (exists)
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
			catch (Exception e)
			{
				this.Logger.LogError(e, e.Message);
				throw;
			}
		}

		/// <inheritdoc />
		public async Task<bool> ExistsAsync(string id, Func<Task<bool>> setter)
		{
			try
			{
				long generation = await this.GetGenerationAsync().ConfigureAwait(false);
				string cacheKey = this.CacheKeyProvider.GetExistsCacheKey<TAggregateRoot>(this.RepositoryName, generation, id);
				bool exists = await this.ExistsSafeTask(cacheKey).ConfigureAwait(false);

				bool result;
				if (exists)
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
			catch (Exception e)
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
				long generation = await this.GetGenerationAsync().ConfigureAwait(false);
				string cacheKey = this.CacheKeyProvider.GetExistsCacheKey(this.RepositoryName, generation, predicate);
				bool exists = await this.ExistsSafeTask(cacheKey).ConfigureAwait(false);

				bool result;
				if (exists)
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
			catch (Exception e)
			{
				this.Logger.LogError(e, e.Message);
				throw;
			}
		}

		private async Task SetSafeAsync<TCacheItem>(string cacheKey, TCacheItem item)
		{
			try
			{
				await this.CachingProvider
						  .SetAsync(cacheKey, item)
						  .ConfigureAwait(false);
			}
			catch (Exception e)
			{
				// Don't let caching errors mess with the repository.
				this.Logger.LogError(e, e.Message);
			}
		}

		private async Task<TCacheItem> GetSafeAsync<TCacheItem>(string cacheKey)
		{
			try
			{
				return await this.CachingProvider
								 .GetAsync<TCacheItem>(cacheKey)
								 .ConfigureAwait(false);
			}
			catch (Exception e)
			{
				// Don't let caching errors mess with the repository.
				this.Logger.LogError(e, e.Message);
				return default;
			}
		}

		private async Task IncrementSafeAsync(string cacheKey)
		{
			try
			{
				await this.CachingProvider
						  .IncrementAsync(cacheKey, 1)
						  .ConfigureAwait(false);
			}
			catch (Exception e)
			{
				// Don't let caching errors mess with the repository.
				this.Logger.LogError(e, e.Message);
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
			catch (Exception e)
			{
				// Don't let caching errors mess with the repository.
				this.Logger.LogError(e, e.Message);
			}
		}

		private async Task<bool> ExistsSafeTask(string cacheKey)
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
			catch (Exception e)
			{
				// Don't let caching errors mess with the repository.
				this.Logger.LogError(e, e.Message);
				return false;
			}
		}

		private async Task IncrementGenerationAsync()
		{
			string cacheKey = this.CacheKeyProvider.GetGenerationCacheKey<TAggregateRoot>(this.RepositoryName);
			await this.IncrementSafeAsync(cacheKey).ConfigureAwait(false);
		}

		private async Task<long> GetGenerationAsync()
		{
			string cacheKey = this.CacheKeyProvider.GetGenerationCacheKey<TAggregateRoot>(this.RepositoryName);
			long generation = await this.GetSafeAsync<long>(cacheKey).ConfigureAwait(false);

			return generation;
		}
	}
}
