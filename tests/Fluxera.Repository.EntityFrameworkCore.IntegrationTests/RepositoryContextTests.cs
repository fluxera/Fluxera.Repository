namespace Fluxera.Repository.EntityFrameworkCore.IntegrationTests
{
	using System;
	using Fluxera.Repository.UnitTests.Core;
	using Microsoft.Extensions.DependencyInjection;
	using NUnit.Framework;

	[TestFixture]
	public class RepositoryContextTests : RepositoryContextTestsBase
	{
		/// <inheritdoc />
		protected override void AddRepositoryUnderTest(IRepositoryBuilder repositoryBuilder,
			string repositoryName, Action<IRepositoryOptionsBuilder> configureOptions)
		{
			repositoryBuilder.Services.AddDbContext<RepositoryMultiTenantDbContext>();

			repositoryBuilder.AddEntityFrameworkRepository<RepositoryMultiTenantContext>(repositoryName, configureOptions.Invoke);
		}

		/// <inheritdoc />
		protected override void AddRepositoryUnderTestWithWrongContextBaseClass(IRepositoryBuilder repositoryBuilder, string repositoryName, Action<IRepositoryOptionsBuilder> configureOptions)
		{
			repositoryBuilder.Services.AddDbContext<WrongBaseClassContext>();

			repositoryBuilder.AddEntityFrameworkRepository(repositoryName, typeof(WrongBaseClassContext), configureOptions.Invoke);
		}
	}
}
