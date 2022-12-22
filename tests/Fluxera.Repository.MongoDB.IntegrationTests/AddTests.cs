namespace Fluxera.Repository.MongoDB.IntegrationTests
{
	using System;
	using System.Threading.Tasks;
	using DotNet.Testcontainers.Builders;
	using DotNet.Testcontainers.Configurations;
	using DotNet.Testcontainers.Containers;
	using Fluxera.Repository.UnitTests.Core;
	using global::MongoDB.Driver;
	using NUnit.Framework;

	[TestFixture(true)]
	[TestFixture(false)]
	public class AddTests : AddTestBase
	{
		private readonly TestcontainerDatabase container;

		/// <inheritdoc />
		public AddTests(bool isUnitOfWorkEnabled)
			: base(isUnitOfWorkEnabled)
		{
			this.container = new TestcontainersBuilder<MongoDbTestcontainer>()
				.WithDatabase(new MongoDbTestcontainerConfiguration
				{
					Database = "test",
					Port = 37017,
					Username = null,
					Password = null
				})
				.WithImage("mongo:latest")
				//.WithCommand("--replSet", "test")
				.Build();
		}

		/// <inheritdoc />
		protected override void AddRepositoryUnderTest(IRepositoryBuilder repositoryBuilder,
			string repositoryName, Action<IRepositoryOptionsBuilder> configureOptions)
		{
			repositoryBuilder.AddMongoRepository<RepositoryMongoContext>(repositoryName, configureOptions);
		}

		/// <inheritdoc />
		protected override async Task OnSetUpAsync()
		{
			await this.container.StartAsync();
		}

		/// <inheritdoc />
		protected override async Task TearDownAsync()
		{
			MongoClient client = new MongoClient("mongodb://localhost:37017");
			await client.DropDatabaseAsync("test");

			await this.container.DisposeAsync();
		}
	}
}
