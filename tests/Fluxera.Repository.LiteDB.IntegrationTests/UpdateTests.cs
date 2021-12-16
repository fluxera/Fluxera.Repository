namespace Fluxera.Repository.LiteDB.IntegrationTests
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
			repositoryBuilder.AddLiteRepository(repositoryName, options =>
			{
				options.AddSetting("Lite.Database", "test.db");

				configureOptions.Invoke(options);
			});
		}
	}
}
