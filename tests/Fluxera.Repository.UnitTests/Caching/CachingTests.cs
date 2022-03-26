namespace Fluxera.Repository.UnitTests.Caching
{
	using System;
	using FluentAssertions;
	using Fluxera.Repository.Caching;
	using Fluxera.Repository.InMemory;
	using Fluxera.Repository.Options;
	using Fluxera.Repository.UnitTests.Core;
	using Fluxera.Repository.UnitTests.Core.CompanyAggregate;
	using Fluxera.Repository.UnitTests.Core.PersonAggregate;
	using Microsoft.Extensions.DependencyInjection;
	using NUnit.Framework;

	[TestFixture]
	public class CachingTests : TestBase
	{
		[SetUp]
		public void SetUp()
		{
			this.serviceProvider = BuildServiceProvider(services =>
			{
				services.AddRepository(rb =>
				{
					rb.AddInMemoryRepository("Repository", rob =>
					{
						rob.UseFor<Company>();
						rob.UseFor<Person>();

						rob.AddCaching(cob =>
						{
							cob
								.UseStandard()
								.UseTimeoutFor<Person>(TimeSpan.FromSeconds(20));
						});
					});
				});

				services.AddTransient<IPersonRepository, PersonRepository>();
			});
		}

		[TearDown]
		public void TearDown()
		{
			this.serviceProvider = null;
		}

		private IServiceProvider serviceProvider;

		[Test]
		public void ShouldEnableCaching()
		{
			IRepositoryRegistry repositoryRegistry = this.serviceProvider.GetRequiredService<IRepositoryRegistry>();
			RepositoryOptions options = repositoryRegistry.GetRepositoryOptionsFor((RepositoryName)"Repository");

			options.CachingOptions.IsEnabled.Should().BeTrue();

			ICachingStrategyFactory cachingStrategyFactory = this.serviceProvider.GetRequiredService<ICachingStrategyFactory>();

			ICachingStrategy<Company, Guid> companyStrategy = cachingStrategyFactory.CreateStrategy<Company, Guid>();
			companyStrategy.Should().NotBeNull();
			companyStrategy.Should().BeOfType<StandardCachingStrategy<Company, Guid>>();

			ICachingStrategy<Person, Guid> personStrategy = cachingStrategyFactory.CreateStrategy<Person, Guid>();
			personStrategy.Should().NotBeNull();
			personStrategy.Should().BeOfType<TimeoutCachingStrategy<Person, Guid>>();
		}
	}
}
