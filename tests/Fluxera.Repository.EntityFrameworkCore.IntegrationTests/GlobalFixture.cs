namespace Fluxera.Repository.EntityFrameworkCore.IntegrationTests
{
	using System.Threading.Tasks;
	using DotNet.Testcontainers.Builders;
	using DotNet.Testcontainers.Configurations;
	using DotNet.Testcontainers.Containers;
	using Microsoft.EntityFrameworkCore;
	using NUnit.Framework;

	[SetUpFixture]
	public class GlobalFixture
	{
		private static TestcontainerDatabase container;

		private readonly TestcontainerDatabaseConfiguration configuration = new MsSqlTestcontainerConfiguration
		{
			Database = "test",
			Password = "yourStrong(!)Password"
		};

		public GlobalFixture()
		{
			container = new ContainerBuilder<MsSqlTestcontainer>()
				.WithDatabase(this.configuration)
				.WithPortBinding(3433, 1433)
				.WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(1433))
				.WithImage("mcr.microsoft.com/mssql/server:2019-latest")
				.Build();
		}

		public static string ConnectionString => container is not null
			? $"{container?.ConnectionString}TrustServerCertificate=True;"
			: null;

		public static string Database => container?.Database;

		[OneTimeSetUp]
		public async Task OneTimeSetUp()
		{
			await container.StartAsync();

			await using(RepositoryDbContext context = new RepositoryDbContext())
			{
				await context.Database.MigrateAsync();
			}

			await using(RepositoryDbContext context = new RepositoryDbContext($"{Database}-1"))
			{
				await context.Database.MigrateAsync();
			}

			await using(RepositoryDbContext context = new RepositoryDbContext($"{Database}-2"))
			{
				await context.Database.MigrateAsync();
			}
		}

		[OneTimeTearDown]
		public async Task OneTimeTearDown()
		{
			await container.DisposeAsync();
		}
	}
}
