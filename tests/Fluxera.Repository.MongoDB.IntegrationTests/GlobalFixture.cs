namespace Fluxera.Repository.MongoDB.IntegrationTests
{
	using System.Threading.Tasks;
	using DotNet.Testcontainers.Builders;
	using DotNet.Testcontainers.Configurations;
	using DotNet.Testcontainers.Containers;
	using NUnit.Framework;

	[SetUpFixture]
	public class GlobalFixture
	{
		private static TestcontainerDatabase container;

		private readonly MongoDbTestcontainerConfiguration configuration = new MongoDbTestcontainerConfiguration
		{
			Database = "test",
			Username = null,
			Password = null
		};

		public GlobalFixture()
		{
			container = new TestcontainersBuilder<MongoDbTestcontainer>()
				.WithDatabase(this.configuration)
				.WithPortBinding(37017, 27017)
				.WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(27017))
				.Build();
		}

		public static string ConnectionString => container.ConnectionString;

		public static string Database => container.Database;

		[OneTimeSetUp]
		public async Task OneTimeSetUp()
		{
			await container.StartAsync();
		}

		[OneTimeTearDown]
		public async Task OneTimeTearDown()
		{
			await container.DisposeAsync();
		}
	}
}
