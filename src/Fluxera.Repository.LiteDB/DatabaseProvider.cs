namespace Fluxera.Repository.LiteDB
{
	using System.Collections.Concurrent;
	using System.Collections.Generic;
	using Fluxera.Utilities.Extensions;
	using global::LiteDB.Async;
	using JetBrains.Annotations;
	using Microsoft.Extensions.Logging;

	/// <summary>
	///     https://github.com/mlockett42/litedb-async
	/// </summary>
	/// <remarks>
	///     >
	///     With v5.5 the library above will no longer be needed for async support.
	/// </remarks>
	[UsedImplicitly]
	internal sealed class DatabaseProvider : IDatabaseProvider
	{
		private readonly IDictionary<RepositoryName, LiteDatabaseAsync> databases = new ConcurrentDictionary<RepositoryName, LiteDatabaseAsync>();
		private readonly ILogger<DatabaseProvider> logger;

		public DatabaseProvider(ILogger<DatabaseProvider> logger)
		{
			this.logger = logger;
		}

		/// <inheritdoc />
		public LiteDatabaseAsync GetDatabase(RepositoryName repositoryName, string databaseName)
		{
			if(!this.databases.ContainsKey(repositoryName))
			{
				LiteDatabaseAsync database = new LiteDatabaseAsync(databaseName.EnsureEndsWith(".db"));
				this.databases.Add(repositoryName, database);
			}

			return this.databases[repositoryName];
		}

		/// <inheritdoc />
		public void Dispose(RepositoryName repositoryName)
		{
			if(this.databases.ContainsKey(repositoryName))
			{
				this.logger.LogDebug($"Disposed LiteDB database for repository '{repositoryName}'.");

				this.databases[repositoryName].Dispose();
				this.databases.Remove(repositoryName);
			}
		}
	}
}
