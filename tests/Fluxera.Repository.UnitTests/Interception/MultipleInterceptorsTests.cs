namespace Fluxera.Repository.UnitTests.Interception
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using FluentAssertions;
	using Fluxera.Repository.InMemory;
	using Fluxera.Repository.Interception;
	using Fluxera.Repository.Options;
	using Fluxera.Repository.UnitTests.Core;
	using Fluxera.Repository.UnitTests.Core.PersonAggregate;
	using Microsoft.Extensions.DependencyInjection;
	using NUnit.Framework;

	[TestFixture]
	public class MultipleInterceptorsTests : TestBase
	{
		[SetUp]
		public void SetUp()
		{
			this.serviceProvider = BuildServiceProvider(services =>
			{
				services.AddRepository(rb =>
				{
					rb.Services.AddInMemoryContext(sp =>
					{
						IRepositoryRegistry repositoryRegistry = sp.GetRequiredService<IRepositoryRegistry>();

						return new RepositoryInMemoryContext("Repository", repositoryRegistry);
					});

					rb.AddInMemoryRepository<RepositoryInMemoryContext>("Repository", rob =>
					{
						rob.UseFor<Person>();

						rob.AddInterception(iob =>
						{
							iob.AddInterceptor<FirstPersonInterceptor>();
							iob.AddInterceptor<LastPersonInterceptor>();
							iob.AddInterceptor<MiddlePersonInterceptor>();
						});
					});
				});

				services.AddTransient<IPersonRepository, PersonRepository>();
				services.AddSingleton(new InterceptorCounter());
			});
		}

		[TearDown]
		public void TearDown()
		{
			this.serviceProvider = null;
		}

		private IServiceProvider serviceProvider;

		[Test]
		public void ShouldEnableInterception()
		{
			IRepositoryRegistry repositoryRegistry = this.serviceProvider.GetRequiredService<IRepositoryRegistry>();
			RepositoryOptions options = repositoryRegistry.GetRepositoryOptionsFor((RepositoryName)"Repository");

			options.InterceptionOptions.IsEnabled.Should().BeTrue();
			IEnumerable<IInterceptor<Person, Guid>> interceptors = this.serviceProvider.GetServices<IInterceptor<Person, Guid>>();
			interceptors.Count().Should().Be(3);
		}

		[Test]
		public void ShouldInterceptAll()
		{
			IPersonRepository personRepository = this.serviceProvider.GetRequiredService<IPersonRepository>();
			personRepository.AddAsync(new Person
			{
				Name = "Tester",
				Age = 33
			});

			InterceptorCounter counter = this.serviceProvider.GetRequiredService<InterceptorCounter>();
			counter.BeforeAddCalled.Should().Be(3);
			//counter.AfterAddCalled.Should().Be(3);

			counter.BeforeAddCall[0].Should().Be("First");
			counter.BeforeAddCall[1].Should().Be("Middle");
			counter.BeforeAddCall[2].Should().Be("Last");
		}
	}
}
