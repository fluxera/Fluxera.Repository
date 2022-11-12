namespace Fluxera.Repository.LiteDB.IntegrationTests
{
	using JetBrains.Annotations;

	[PublicAPI]
	public sealed class RepositoryLiteContext : LiteContext
	{
		/// <inheritdoc />
		public RepositoryLiteContext(
			string repositoryName,
			DatabaseProvider databaseProvider,
			IRepositoryRegistry repositoryRegistry)
			: base(repositoryName, databaseProvider, repositoryRegistry)
		{
		}
	}
}
