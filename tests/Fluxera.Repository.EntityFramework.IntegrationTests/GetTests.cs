namespace Fluxera.Repository.EntityFrameworkCore.IntegrationTests
{
	using System;
	using Fluxera.Repository.UnitTests.Core;
	using NUnit.Framework;

	[TestFixture]
	public class GetTests : GetTestBase
	{
		/// <inheritdoc />
		protected override void AddRepositoryUnderTest(IRepositoryBuilder repositoryBuilder,
			string repositoryName, Action<IRepositoryOptionsBuilder> configureOptions)
		{
			repositoryBuilder.AddEntityFrameworkRepository(repositoryName, builder =>
			{
				configureOptions.Invoke(builder);

				builder.AddSetting("EntityFramework.DbContext", typeof(RepositoryDbContext));
				builder.AddSetting("EntityFramework.ConnectionString", "Filename=test.db");
				builder.AddSetting("EntityFramework.LogSQL", false);
			});
		}
	}
}
