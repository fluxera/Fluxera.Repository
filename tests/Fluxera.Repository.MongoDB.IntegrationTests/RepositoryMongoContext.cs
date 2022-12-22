namespace Fluxera.Repository.MongoDB.IntegrationTests
{
	using JetBrains.Annotations;

	[PublicAPI]
	public sealed class RepositoryMongoContext : MongoContext
	{
		/// <inheritdoc />
		protected override void ConfigureOptions(MongoContextOptions options)
		{
			options.ConnectionString = GlobalFixture.ConnectionString;
			options.Database = GlobalFixture.Database;
		}
	}
}
