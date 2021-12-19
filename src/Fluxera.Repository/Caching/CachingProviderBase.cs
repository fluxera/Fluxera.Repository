namespace Fluxera.Repository.Caching
{
	using System;
	using System.Text;
	using System.Threading.Tasks;
	using Fluxera.Extensions.Common;
	using Fluxera.Guards;
	using JetBrains.Annotations;
	using Microsoft.Extensions.Logging;

	/// <summary>
	///     Caching provider base.
	/// </summary>
	[PublicAPI]
	internal abstract class CachingProviderBase : ICachingProvider
	{
		private readonly IHashCalculator hashCalculator;

		protected CachingProviderBase(ILoggerFactory loggerFactory, IHashCalculator hashCalculator)
		{
			this.Logger = loggerFactory.CreateLogger("CachingProviderBase");
			this.hashCalculator = hashCalculator;
		}

		protected ILogger Logger { get; }

		async Task ICachingProvider.SetAsync<T>(string key, T value, TimeSpan? expiration)
		{
			Guard.Against.NullOrEmpty(key, nameof(key));

			this.Logger.LogTrace($"Setting cache item: Type = {typeof(T)}, Key = {key}");
			await this.SetAsync(this.CreateHash(key), value, expiration).ConfigureAwait(false);
		}

		async Task<T> ICachingProvider.GetAsync<T>(string key)
		{
			Guard.Against.NullOrEmpty(key, nameof(key));

			this.Logger.LogTrace($"Getting cache item: Type = {typeof(T)}, Key = {key}");
			return await this.GetAsync<T>(this.CreateHash(key)).ConfigureAwait(false);
		}

		async Task ICachingProvider.RemoveAsync(string key)
		{
			Guard.Against.NullOrEmpty(key, nameof(key));

			this.Logger.LogTrace($"Removing cache item: Key = {key}");
			await this.RemoveAsync(this.CreateHash(key)).ConfigureAwait(false);
		}

		async Task<bool> ICachingProvider.ExistsAsync(string key)
		{
			Guard.Against.NullOrEmpty(key, nameof(key));

			this.Logger.LogTrace($"Querying if cache item exists: Key = {key}");
			return await this.ExistsAsync(this.CreateHash(key)).ConfigureAwait(false);
		}

		async Task<long> ICachingProvider.IncrementAsync(string key, long incrementValue)
		{
			Guard.Against.NullOrEmpty(key, nameof(key));

			this.Logger.LogTrace($"Increment cache item: Increment = {incrementValue}, Key = {key}");
			return await this.IncrementAsync(this.CreateHash(key), incrementValue).ConfigureAwait(false);
		}

		protected abstract Task SetAsync<T>(string key, T value, TimeSpan? expiration = null);

		protected abstract Task<T> GetAsync<T>(string key);

		protected abstract Task RemoveAsync(string key);

		protected abstract Task<bool> ExistsAsync(string key);

		protected abstract Task<long> IncrementAsync(string key, long incrementValue);

		private string CreateHash(string key)
		{
			string hash = this.hashCalculator.ComputeHash(key, Encoding.UTF8);
			this.Logger.LogTrace($"Computed MD5: Key = {key}, Hash = {hash}");
			return hash;
		}
	}
}
