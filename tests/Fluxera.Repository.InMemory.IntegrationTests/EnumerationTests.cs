namespace Fluxera.Repository.InMemory.IntegrationTests
{
	using System;
	using Fluxera.Repository.UnitTests.Core;
	using NUnit.Framework;

	[TestFixture(true)]
	[TestFixture(false)]
	public class EnumerationTests : EnumerationTestsBase
	{
		/// <inheritdoc />
		public EnumerationTests(bool isUnitOfWorkEnabled)
			: base(isUnitOfWorkEnabled)
		{
		}

		/// <inheritdoc />
		protected override void AddRepositoryUnderTest(IRepositoryBuilder repositoryBuilder,
			string repositoryName, Action<IRepositoryOptionsBuilder> configureOptions)
		{
			repositoryBuilder.AddInMemoryRepository<RepositoryInMemoryContext>(repositoryName, configureOptions);
		}
	}
}
