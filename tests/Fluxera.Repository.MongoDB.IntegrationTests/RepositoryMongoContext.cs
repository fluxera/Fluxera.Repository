namespace Fluxera.Repository.MongoDB.IntegrationTests
{
	using JetBrains.Annotations;

	[PublicAPI]
	public sealed class RepositoryMongoContext : MongoContext
	{
		/// <inheritdoc />
		public RepositoryMongoContext(
			string repositoryName,
			IRepositoryRegistry repositoryRegistry,
			IDatabaseNameProvider databaseNameProvider = null)
			: base(repositoryName, repositoryRegistry, databaseNameProvider)
		{
		}
	}
}
