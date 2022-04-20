namespace Fluxera.Repository.EntityFrameworkCore.IntegrationTests
{
	using System;
	using Fluxera.Repository.UnitTests.Core;
	using NUnit.Framework;

	[TestFixture]
	public class UpdateTests : UpdateTestBase
	{
		/// <inheritdoc />
		protected override void AddRepositoryUnderTest(IRepositoryBuilder repositoryBuilder,
			string repositoryName, Action<IRepositoryOptionsBuilder> configureOptions)
		{
			repositoryBuilder.AddEntityFrameworkRepository(repositoryName, builder =>
			{
				configureOptions.Invoke(builder);

				builder.AddSetting("EntityFrameworkCore.DbContext", typeof(RepositoryDbContext));
				builder.AddSetting("EntityFrameworkCore.ConnectionString", "Filename=test.db");
				builder.AddSetting("EntityFrameworkCore.LogSQL", false);
			});
		}
	}
}
