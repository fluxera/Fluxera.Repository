namespace Fluxera.Repository.EntityFrameworkCore.IntegrationTests
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
			repositoryBuilder.Services.AddDbContext<RepositoryDbContext>();

			repositoryBuilder.AddEntityFrameworkRepository<RepositoryDbContext>(repositoryName, configureOptions.Invoke);
		}
	}
}
