namespace Fluxera.Repository.MongoDB.IntegrationTests
{
	using System;
	using System.Threading.Tasks;
	using Fluxera.Repository.UnitTests.Core;
	using global::MongoDB.Driver;
	using NUnit.Framework;

	[TestFixture]
	public class RepositoryContextTests : RepositoryContextTestsBase
	{
		/// <inheritdoc />
		protected override void AddRepositoryUnderTest(IRepositoryBuilder repositoryBuilder,
			string repositoryName, Action<IRepositoryOptionsBuilder> configureOptions)
		{
			repositoryBuilder.AddMongoRepository<RepositoryMultiTenantMongoContext>(repositoryName, configureOptions);
		}

		/// <inheritdoc />
		protected override void AddRepositoryUnderTestWithWrongContextBaseClass(IRepositoryBuilder repositoryBuilder, string repositoryName,
			Action<IRepositoryOptionsBuilder> configureOptions)
		{
			repositoryBuilder.AddMongoRepository(repositoryName, typeof(WrongBaseClassContext), configureOptions);
		}

		/// <inheritdoc />
		protected override async Task TearDownAsync()
		{
			MongoClient client = new MongoClient(GlobalFixture.ConnectionString);
			await client.DropDatabaseAsync(GlobalFixture.Database);
			await client.DropDatabaseAsync($"{GlobalFixture.Database}-1");
			await client.DropDatabaseAsync($"{GlobalFixture.Database}-2");
		}
	}
}
