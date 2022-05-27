namespace Fluxera.Repository.Caching
{
	using System;
	using System.Threading;
	using System.Threading.Tasks;
	using Fluxera.Extensions.Caching;
	using Fluxera.Extensions.Common;
	using Microsoft.Extensions.Caching.Distributed;
	using Microsoft.Extensions.Logging;

	internal sealed class CachingProvider : CachingProviderBase
	{
		// Instantiate a Singleton of the Semaphore with a value of 1.
		// This means that only 1 thread can be granted access at a time.
		private static readonly SemaphoreSlim Semaphore = new SemaphoreSlim(1, 1);

		private readonly IDistributedCache distributedCache;

		public CachingProvider(
			ILoggerFactory loggerFactory,
			IHashCalculator hashCalculator,
			IDistributedCache distributedCache)
			: base(loggerFactory, hashCalculator)
		{
			this.distributedCache = distributedCache;
		}

		/// <inheritdoc />
		protected override async Task SetAsync<T>(string key, T value, TimeSpan? expiration = null)
		{
			DistributedCacheEntryOptions options = new DistributedCacheEntryOptions();
			if(expiration.HasValue)
			{
				options.AbsoluteExpirationRelativeToNow = expiration;
			}

			await this.distributedCache
				.SetAsJsonAsync(key, value, options)
				.ConfigureAwait(false);
		}

		/// <inheritdoc />
		protected override async Task<T> GetAsync<T>(string key)
		{
			T item = await this.distributedCache
				.GetAsJsonAsync<T>(key)
				.ConfigureAwait(false);

			return item!;
		}

		/// <inheritdoc />
		protected override async Task RemoveAsync(string key)
		{
			await this.distributedCache
				.RemoveAsync(key)
				.ConfigureAwait(false);
		}

		/// <inheritdoc />
		protected override async Task<bool> ExistsAsync(string key)
		{
			byte[] item = await this.distributedCache
				.GetAsync(key)
				.ConfigureAwait(false);

			return item != null;
		}

		/// <inheritdoc />
		protected override async Task<long> IncrementAsync(string key, long incrementValue)
		{
			// https://gist.github.com/JCKodel/d3467a66350af98ee61c74f5ebd804be

			// https://blog.cdemi.io/async-waiting-inside-c-sharp-locks/
			// Asynchronously wait to enter the Semaphore.
			// If no-one has been granted access to the Semaphore, code execution will proceed,
			// otherwise this thread waits here until the semaphore is released.
			await Semaphore.WaitAsync();
			try
			{
				long value = await this.GetAsync<long>(key).ConfigureAwait(false);
				value += incrementValue;
				await this.SetAsync(key, value, TimeSpan.MaxValue).ConfigureAwait(false);

				return value;
			}
			finally
			{
				// When the task is ready, release the semaphore.
				// It is vital to ALWAYS release the semaphore when we are ready, or else we will
				// end up with a Semaphore that is forever locked. This is why it is important to
				// do the Release within a try...finally clause; program execution may crash or
				// take a different path, this way you are guaranteed execution.
				Semaphore.Release();
			}
		}
	}
}
