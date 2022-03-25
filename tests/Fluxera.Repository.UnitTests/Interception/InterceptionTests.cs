namespace Fluxera.Repository.UnitTests.Interception
{
	using System;
	using System.Collections.Generic;
	using FluentAssertions;
	using Fluxera.Repository.InMemory;
	using Fluxera.Repository.Interception;
	using Fluxera.Repository.Options;
	using Fluxera.Repository.UnitTests.Core;
	using Fluxera.Repository.UnitTests.Core.PersonAggregate;
	using Microsoft.Extensions.DependencyInjection;
	using NUnit.Framework;

	[TestFixture]
	public class InterceptionTests : TestBase
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
						rob.UseFor<Person>();

						rob.AddInterception(iob =>
						{
							iob.AddInterceptor<PersonInterceptor>();
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
			interceptors.Should().NotBeEmpty();
		}

		[Test]
		public void ShouldInterceptAdd()
		{
			IPersonRepository personRepository = this.serviceProvider.GetRequiredService<IPersonRepository>();
			personRepository.AddAsync(new Person
			{
				Name = "Tester",
				Age = 33
			});

			InterceptorCounter counter = this.serviceProvider.GetRequiredService<InterceptorCounter>();
			counter.BeforeAddCalled.Should().BeGreaterThan(0);
			counter.AfterAddCalled.Should().BeGreaterThan(0);
		}

		[Test]
		public void ShouldInterceptRemove()
		{
			IPersonRepository personRepository = this.serviceProvider.GetRequiredService<IPersonRepository>();
			Person person = new Person
			{
				Name = "Tester",
				Age = 33
			};
			personRepository.AddAsync(person);
			personRepository.RemoveAsync(person);

			InterceptorCounter counter = this.serviceProvider.GetRequiredService<InterceptorCounter>();
			counter.BeforeRemoveCalled.Should().BeGreaterThan(0);
			counter.AfterRemoveCalled.Should().BeGreaterThan(0);
		}

		[Test]
		public void ShouldInterceptUpdate()
		{
			IPersonRepository personRepository = this.serviceProvider.GetRequiredService<IPersonRepository>();
			Person person = new Person
			{
				Name = "Tester",
				Age = 33
			};
			personRepository.AddAsync(person);
			personRepository.UpdateAsync(person);

			InterceptorCounter counter = this.serviceProvider.GetRequiredService<InterceptorCounter>();
			counter.BeforeUpdateCalled.Should().BeGreaterThan(0);
			counter.AfterUpdateCalled.Should().BeGreaterThan(0);
		}
	}
}
