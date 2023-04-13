namespace Fluxera.Repository.MongoDB.IntegrationTests
{
	using System;
	using System.Threading.Tasks;
	using Fluxera.Repository.UnitTests.Core;
	using global::MongoDB.Driver;
	using MadEyeMatt.MongoDB.DbContext;
	using NUnit.Framework;

	[TestFixture(true)]
	[TestFixture(false)]
	public class AggregateTests : AggregateTestBase
	{
		/// <inheritdoc />
		public AggregateTests(bool isUnitOfWorkEnabled)
			: base(isUnitOfWorkEnabled)
		{
		}

		/// <inheritdoc />
		protected override void AddRepositoryUnderTest(IRepositoryBuilder repositoryBuilder,
			string repositoryName, Action<IRepositoryOptionsBuilder> configureOptions)
		{
			repositoryBuilder.Services.AddMongoDbContext<RepositoryMongoDbContext>();

			repositoryBuilder.AddMongoRepository<RepositoryMongoContext>(repositoryName, configureOptions);
		}

		/// <inheritdoc />
		protected override async Task TearDownAsync()
		{
			MongoClient client = new MongoClient(GlobalFixture.ConnectionString);
			await client.DropDatabaseAsync(GlobalFixture.Database);
		}
	}
}
