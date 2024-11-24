﻿namespace Fluxera.Repository.LiteDB
{
	using System.Collections.Concurrent;
	using System.IO;
	using Fluxera.Utilities;
	using Fluxera.Utilities.Extensions;
	using global::LiteDB.Async;
	using JetBrains.Annotations;

	/// <summary>
	///     https://github.com/mlockett42/litedb-async
	/// </summary>
	/// <remarks>
	///     With v5.5 the library above will no longer be needed for async support.
	/// </remarks>
	[PublicAPI]
	public sealed class DatabaseProvider : Disposable
	{
		private readonly ConcurrentDictionary<string, LiteDatabaseAsync> databases = new ConcurrentDictionary<string, LiteDatabaseAsync>();

		/// <summary>
		///     Gets a database for the given repository and database names.
		/// </summary>
		/// <param name="repositoryName"></param>
		/// <param name="databaseName"></param>
		/// <param name="isPersistent"></param>
		/// <returns></returns>
		public LiteDatabaseAsync GetDatabase(RepositoryName repositoryName, string databaseName, bool isPersistent)
		{
			string key = $"{repositoryName}_{databaseName}";

			LiteDatabaseAsync database = this.databases.GetOrAdd(key, _ => isPersistent 
				? new LiteDatabaseAsync(databaseName.EnsureEndsWith(".db")) 
				: new LiteDatabaseAsync(new MemoryStream()));

			return database;
		}

		/// <inheritdoc />
		protected override void DisposeManaged()
		{
			foreach(LiteDatabaseAsync database in this.databases.Values)
			{
				database.Dispose();
			}

			this.databases.Clear();
		}
	}
}
