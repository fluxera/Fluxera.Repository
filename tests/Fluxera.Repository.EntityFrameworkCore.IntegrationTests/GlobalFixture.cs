namespace Fluxera.Repository.EntityFrameworkCore.IntegrationTests
{
	using System.Threading.Tasks;
	using DotNet.Testcontainers.Builders;
	using Microsoft.EntityFrameworkCore;
	using NUnit.Framework;
	using Testcontainers.MsSql;

	[SetUpFixture]
	public class GlobalFixture
	{
		private static MsSqlContainer container;

		public GlobalFixture()
		{
			container = new MsSqlBuilder()
				.WithPortBinding(3433, MsSqlBuilder.MsSqlPort)
				.WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(1433))
				.WithImage("mcr.microsoft.com/mssql/server:2019-latest")
				.Build();
		}

		public static string ConnectionString => container?.GetConnectionString();

		public static string Database => MsSqlBuilder.DefaultDatabase;

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
