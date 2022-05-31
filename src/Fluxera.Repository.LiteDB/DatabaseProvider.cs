namespace Fluxera.Repository.LiteDB
{
	using System.Collections.Concurrent;
	using Fluxera.Utilities.Extensions;
	using global::LiteDB.Async;
	using JetBrains.Annotations;

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
		private readonly ConcurrentDictionary<RepositoryName, LiteDatabaseAsync> databases = new ConcurrentDictionary<RepositoryName, LiteDatabaseAsync>();

		/// <inheritdoc />
		public LiteDatabaseAsync GetDatabase(RepositoryName repositoryName, string databaseName)
		{
			LiteDatabaseAsync database = this.databases.GetOrAdd(repositoryName,
				_ => new LiteDatabaseAsync(databaseName.EnsureEndsWith(".db")));

			return database;
		}

		/// <inheritdoc />
		public void Dispose()
		{
			foreach(LiteDatabaseAsync database in this.databases.Values)
			{
				database.Dispose();
			}

			this.databases.Clear();
		}
	}
}
