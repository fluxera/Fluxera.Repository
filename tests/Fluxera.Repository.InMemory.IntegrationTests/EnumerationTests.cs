namespace Fluxera.Repository.InMemory.IntegrationTests
{
	using System;
	using Fluxera.Repository.UnitTests.Core;
	using Microsoft.Extensions.DependencyInjection;
	using NUnit.Framework;

	[TestFixture]
	public class EnumerationTests : EnumerationTestsBase
	{
		/// <inheritdoc />
		protected override void AddRepositoryUnderTest(IRepositoryBuilder repositoryBuilder,
			string repositoryName, Action<IRepositoryOptionsBuilder> configureOptions)
		{
			repositoryBuilder.Services.AddInMemoryContext(serviceProvider =>
			{
				IRepositoryRegistry repositoryRegistry = serviceProvider.GetRequiredService<IRepositoryRegistry>();

				return new RepositoryInMemoryContext(repositoryName, repositoryRegistry);
			});

			repositoryBuilder.AddInMemoryRepository<RepositoryInMemoryContext>(repositoryName, configureOptions);
		}
	}
}
