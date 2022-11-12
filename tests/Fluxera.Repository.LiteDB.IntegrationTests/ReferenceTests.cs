namespace Fluxera.Repository.LiteDB.IntegrationTests
{
	using System;
	using System.IO;
	using Fluxera.Repository.UnitTests.Core;
	using Microsoft.Extensions.DependencyInjection;
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
			foreach(string file in Directory.EnumerateFiles(".", "*.db"))
			{
				File.Delete(file);
			}

			repositoryBuilder.Services.AddLiteContext(serviceProvider =>
			{
				DatabaseProvider databaseProvider = serviceProvider.GetRequiredService<DatabaseProvider>();
				IRepositoryRegistry repositoryRegistry = serviceProvider.GetRequiredService<IRepositoryRegistry>();

				return new RepositoryLiteContext(repositoryName, databaseProvider, repositoryRegistry);
			});

			repositoryBuilder.AddLiteRepository<RepositoryLiteContext>(repositoryName, options =>
			{
				options.AddSetting("Lite.Database", $"{Guid.NewGuid():N}.db");

				configureOptions.Invoke(options);
			});
		}
	}
}
