namespace Fluxera.Repository.MongoDB.IntegrationTests
{
	using System;
	using System.Threading.Tasks;
	using Fluxera.Repository.UnitTests.Core;
	using global::MongoDB.Driver;
	using NUnit.Framework;

	[TestFixture(true)]
	[TestFixture(false)]
	public class ReferenceTests : ReferenceTestsBase
	{
		/// <inheritdoc />
		public ReferenceTests(bool isUnitOfWorkEnabled)
			: base(isUnitOfWorkEnabled)
		{
		}

		/// <inheritdoc />
		protected override void AddRepositoryUnderTest(IRepositoryBuilder repositoryBuilder,
			string repositoryName, Action<IRepositoryOptionsBuilder> configureOptions)
		{
			repositoryBuilder.AddMongoRepository<RepositoryMongoContext>(repositoryName, options =>
			{
				options.AddSetting("Mongo.ConnectionString", "mongodb://localhost:27017");
				options.AddSetting("Mongo.Database", "test");

				configureOptions.Invoke(options);
			});
		}

		/// <inheritdoc />
		protected override async Task TearDownAsync()
		{
			MongoClient client = new MongoClient("mongodb://localhost:27017");
			await client.DropDatabaseAsync("test");
		}
	}
}
