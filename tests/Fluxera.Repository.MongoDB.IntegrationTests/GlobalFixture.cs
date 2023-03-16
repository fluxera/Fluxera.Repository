namespace Fluxera.Repository.MongoDB.IntegrationTests
{
	using System.Threading.Tasks;
	using DotNet.Testcontainers.Builders;
	using NUnit.Framework;
	using Testcontainers.MongoDb;

	[SetUpFixture]
	public class GlobalFixture
	{
		private static MongoDbContainer container;

		public GlobalFixture()
		{
			container = new MongoDbBuilder()
				.WithPortBinding(37017, MongoDbBuilder.MongoDbPort)
				.WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(27017))
				.WithImage("mongo:latest")
				.Build();
		}

		public static string ConnectionString => container.GetConnectionString();

		public static string Database => "test";

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
