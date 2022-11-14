namespace Fluxera.Repository.InMemory.IntegrationTests
{
	using System;
	using Fluxera.Repository.UnitTests.Core;
	using NUnit.Framework;

	[TestFixture]
	public class RepositoryContextTests : RepositoryContextTestsBase
	{
		/// <inheritdoc />
		protected override void AddRepositoryUnderTest(IRepositoryBuilder repositoryBuilder,
			string repositoryName, Action<IRepositoryOptionsBuilder> configureOptions)
		{
			repositoryBuilder.AddInMemoryRepository<RepositoryMultiTenantInMemoryContext>(repositoryName, configureOptions);
		}

		/// <inheritdoc />
		protected override void AddRepositoryUnderTestWithWrongContextBaseClass(IRepositoryBuilder repositoryBuilder, string repositoryName, Action<IRepositoryOptionsBuilder> configureOptions)
		{
			repositoryBuilder.AddInMemoryRepository(repositoryName, typeof(WrongBaseClassContext), configureOptions);
		}
	}
}
