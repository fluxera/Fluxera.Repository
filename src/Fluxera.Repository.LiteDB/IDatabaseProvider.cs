namespace Fluxera.Repository.LiteDB
{
	using System;
	using global::LiteDB.Async;

	internal interface IDatabaseProvider : IDisposable
	{
		LiteDatabaseAsync GetDatabase(RepositoryName repositoryName, string databaseName);
	}
}
