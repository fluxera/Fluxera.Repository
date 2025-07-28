namespace Fluxera.Repository.UnitTests.Options
{
	using System;
	using System.Linq;
	using FluentAssertions;
	using FluentValidation;
	using Fluxera.DomainEvents;
	using Fluxera.Repository.Caching;
	using Fluxera.Repository.InMemory;
	using Fluxera.Repository.Interception;
	using Fluxera.Repository.Options;
	using Fluxera.Repository.UnitTests.Core;
	using Fluxera.Repository.UnitTests.Core.PersonAggregate;
	using Mediator;
	using Microsoft.Extensions.DependencyInjection;
	using NUnit.Framework;

	[TestFixture]
	public class RepositoryOptionsTests : TestBase
	{
		[Test]
		public void ShouldAddInterceptorsFromAssembly()
		{
			IServiceProvider serviceProvider = BuildServiceProvider(
				services =>
				{
					services.AddRepository(rb =>
					{
						rb.AddInMemoryRepository<RepositoryInMemoryContext>(builder =>
						{
							builder.UseFor<Person>();

							builder.EnableInterception(x =>
							{
								x.AddInterceptorsFromAssembly(typeof(Person).Assembly);
							});
						});
					});
				});

			IRepositoryRegistry repositoryRegistry = serviceProvider.GetRequiredService<IRepositoryRegistry>();
			RepositoryOptions options = repositoryRegistry.GetRepositoryOptionsFor("Default");
			options.InterceptionOptions.IsEnabled.Should().BeTrue();

			IInterceptor<Person, Guid>[] interceptors = serviceProvider.GetServices<IInterceptor<Person, Guid>>().ToArray();
			interceptors.Should().NotBeNullOrEmpty().And.HaveCount(1);
		}

		[Test]
		public void ShouldNotAddDuplicateInterceptorsFromAssemblies()
		{
			IServiceProvider serviceProvider = BuildServiceProvider(
				services =>
				{
					services.AddRepository(rb =>
					{
						rb.AddInMemoryRepository<RepositoryInMemoryContext>(builder =>
						{
							builder.UseFor<Person>();

							// Add interceptors twice from the same assembly.
							builder.EnableInterception(x =>
							{
								x.AddInterceptorsFromAssembly(typeof(Person).Assembly);
								x.AddInterceptorsFromAssembly(typeof(Person).Assembly);
							});
						});
					});
				});

			IRepositoryRegistry repositoryRegistry = serviceProvider.GetRequiredService<IRepositoryRegistry>();
			RepositoryOptions options = repositoryRegistry.GetRepositoryOptionsFor("Default");
			options.InterceptionOptions.IsEnabled.Should().BeTrue();

			IInterceptor<Person, Guid>[] interceptors = serviceProvider.GetServices<IInterceptor<Person, Guid>>().ToArray();
			interceptors.Should().NotBeNullOrEmpty().And.HaveCount(1);
		}

		[Test]
		public void ShouldAddValidatorsFromAssembly()
		{
			IServiceProvider serviceProvider = BuildServiceProvider(
				services =>
				{
					services.AddRepository(rb =>
					{
						rb.AddInMemoryRepository<RepositoryInMemoryContext>(builder =>
						{
							builder.UseFor<Person>();

							builder.EnableValidation(x =>
							{
								x.AddValidatorsFromAssembly(typeof(Person).Assembly);
							});
						});
					});
				});

			IRepositoryRegistry repositoryRegistry = serviceProvider.GetRequiredService<IRepositoryRegistry>();
			RepositoryOptions options = repositoryRegistry.GetRepositoryOptionsFor("Default");
			options.ValidationOptions.IsEnabled.Should().BeTrue();

			IValidator<Person>[] validators = serviceProvider.GetServices<IValidator<Person>>().ToArray();
			validators.Should().NotBeNullOrEmpty().And.HaveCount(1);
		}

		[Test]
		public void ShouldNotAddDuplicateValidatorsFromAssemblies()
		{
			IServiceProvider serviceProvider = BuildServiceProvider(
				services =>
				{
					services.AddRepository(rb =>
					{
						rb.AddInMemoryRepository<RepositoryInMemoryContext>(builder =>
						{
							builder.UseFor<Person>();

							builder.EnableValidation(x =>
							{
								x.AddValidatorsFromAssembly(typeof(Person).Assembly);
								x.AddValidatorsFromAssembly(typeof(Person).Assembly);
							});
						});
					});
				});

			IRepositoryRegistry repositoryRegistry = serviceProvider.GetRequiredService<IRepositoryRegistry>();
			RepositoryOptions options = repositoryRegistry.GetRepositoryOptionsFor("Default");
			options.ValidationOptions.IsEnabled.Should().BeTrue();

			IValidator<Person>[] validators = serviceProvider.GetServices<IValidator<Person>>().ToArray();
			validators.Should().NotBeNullOrEmpty().And.HaveCount(1);
		}

		[Test]
		public void ShouldAddDomainEventHandlersFromAssembly()
		{
			IServiceProvider serviceProvider = BuildServiceProvider(
				services =>
				{
					services.AddRepository(rb =>
					{
						rb.AddInMemoryRepository<RepositoryInMemoryContext>(builder =>
						{
							builder.UseFor<Person>();

							builder.EnableDomainEventHandling();
						});
					});
				});

			IRepositoryRegistry repositoryRegistry = serviceProvider.GetRequiredService<IRepositoryRegistry>();
			RepositoryOptions options = repositoryRegistry.GetRepositoryOptionsFor("Default");
			options.DomainEventsOptions.IsEnabled.Should().BeTrue();

			INotificationHandler<PersonDomainEvent>[] domainEventHandlers = serviceProvider.GetServices<INotificationHandler<PersonDomainEvent>>().ToArray();
			domainEventHandlers.Should().NotBeNullOrEmpty().And.HaveCount(1);
			domainEventHandlers.Any(x => x is IDomainEventHandler<PersonDomainEvent>).Should().BeTrue();
		} 

		[Test]
		public void ShouldNotAddDuplicateDomainEventHandlersFromAssemblies()
		{
			IServiceProvider serviceProvider = BuildServiceProvider(
				services =>
				{
					services.AddRepository(rb =>
					{
						rb.AddInMemoryRepository<RepositoryInMemoryContext>(builder =>
						{
							builder.UseFor<Person>();

							builder.EnableDomainEventHandling();
						});
					});
				});

			IRepositoryRegistry repositoryRegistry = serviceProvider.GetRequiredService<IRepositoryRegistry>();
			RepositoryOptions options = repositoryRegistry.GetRepositoryOptionsFor("Default");
			options.DomainEventsOptions.IsEnabled.Should().BeTrue();

			INotificationHandler<PersonDomainEvent>[] domainEventHandlers = serviceProvider.GetServices<INotificationHandler<PersonDomainEvent>>().ToArray();
			domainEventHandlers.Should().NotBeNullOrEmpty().And.HaveCount(1);
			domainEventHandlers.Any(x => x is IDomainEventHandler<PersonDomainEvent>).Should().BeTrue();
		}

		[Test]
		[TestCase(CachingStrategyNames.NoCaching)]
		[TestCase(CachingStrategyNames.Standard)]
		[TestCase(CachingStrategyNames.Timeout)]
		public void ShouldAddCaching(string cachingStrategyName)
		{
			IServiceProvider serviceProvider = BuildServiceProvider(
				services =>
				{
					services.AddRepository(rb =>
					{
						rb.AddInMemoryRepository<RepositoryInMemoryContext>(builder =>
						{
							builder.UseFor<Person>();

							builder.EnableCaching(x =>
							{
								switch(cachingStrategyName)
								{
									case CachingStrategyNames.NoCaching:
										x.UseNoCaching();
										break;
									case CachingStrategyNames.Standard:
										x.UseStandard();
										break;
									case CachingStrategyNames.Timeout:
										x.UseTimeout(TimeSpan.FromMinutes(1));
										break;
								}
							});
						});
					});
				});

			IRepositoryRegistry repositoryRegistry = serviceProvider.GetRequiredService<IRepositoryRegistry>();
			RepositoryOptions options = repositoryRegistry.GetRepositoryOptionsFor("Default");
			options.CachingOptions.IsEnabled.Should().BeTrue();

			ICachingStrategyFactory cachingStrategyFactory = serviceProvider.GetRequiredService<ICachingStrategyFactory>();
			ICachingStrategy<Person, Guid> cachingStrategy = cachingStrategyFactory.CreateStrategy<Person, Guid>();

			cachingStrategy.Should().NotBeNull();

			switch(cachingStrategyName)
			{
				case CachingStrategyNames.NoCaching:
					cachingStrategy.Should().BeOfType<NoCachingStrategy<Person, Guid>>();
					break;
				case CachingStrategyNames.Standard:
					cachingStrategy.Should().BeOfType<StandardCachingStrategy<Person, Guid>>();
					break;
				case CachingStrategyNames.Timeout:
					cachingStrategy.Should().BeOfType<TimeoutCachingStrategy<Person, Guid>>();
					break;
			}
		}

		[Test]
		[TestCase(true)]
		[TestCase(false)]
		public void ShouldEnableUnitOfWork(bool enableUnitOfWork)
		{
			IServiceProvider serviceProvider = BuildServiceProvider(
				services =>
				{
					services.AddRepository(rb =>
					{
						rb.AddInMemoryRepository<RepositoryInMemoryContext>(builder =>
						{
							builder.UseFor<Person>();

							if(enableUnitOfWork)
							{
								builder.EnableUnitOfWork();
							}
						});
					});
				});

			IRepositoryRegistry repositoryRegistry = serviceProvider.GetRequiredService<IRepositoryRegistry>();
			RepositoryOptions options = repositoryRegistry.GetRepositoryOptionsFor("Default");
			options.IsUnitOfWorkEnabled.Should().Be(enableUnitOfWork);
		}
	}
}
