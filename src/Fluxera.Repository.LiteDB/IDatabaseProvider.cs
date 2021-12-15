namespace Fluxera.Repository.LiteDB
{
	using global::LiteDB.Async;

	internal interface IDatabaseProvider
	{
		LiteDatabaseAsync GetDatabase(RepositoryName repositoryName, string databaseName);

		void Dispose(RepositoryName repositoryName);
	}
}
