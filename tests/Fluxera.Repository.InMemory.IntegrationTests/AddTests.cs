namespace Fluxera.Repository.InMemory.IntegrationTests
{
	using System;
	using Fluxera.Repository.UnitTests.Core;
	using NUnit.Framework;

	[TestFixture]
	public class AddTests : AddTestBase
	{
		/// <inheritdoc />
		protected override void AddRepositoryUnderTest(IRepositoryBuilder repositoryBuilder,
			string repositoryName, Action<IRepositoryOptionsBuilder> configureOptions)
		{
			repositoryBuilder.AddInMemoryRepository(repositoryName, configureOptions);
		}
	}
}
