namespace Fluxera.Repository.UnitTests.Interception
{
	using System;
	using System.Collections.Generic;
	using System.Threading.Tasks;
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
			this.serviceProvider = BuildServiceProvider(
				services =>
				{
					services.AddRepository(rb =>
					{
						rb.AddInMemoryRepository<RepositoryInMemoryContext>("Repository", rob =>
						{
							rob.UseFor<Person>();

							rob.EnableInterception(iob =>
							{
								iob.AddInterceptorsFromAssembly(typeof(CountingPersonInterceptor).Assembly);
							});
						});
					});

					services.AddTransient<IPersonRepository, PersonRepository>();
					services.AddSingleton(new InterceptorCounter());
				},
				configuration =>
				{
					configuration.RegisterServicesFromAssembly(RepositoryTestsCore.Assembly);
					configuration.RegisterServicesFromAssembly(RepositoryTests.Assembly);
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
		public async Task ShouldInterceptAdd()
		{
			IPersonRepository personRepository = this.serviceProvider.GetRequiredService<IPersonRepository>();
			await personRepository.AddAsync(new Person
			{
				Name = "Tester",
				Age = 33
			});

			IUnitOfWorkFactory unitOfWorkFactory = this.serviceProvider.GetRequiredService<IUnitOfWorkFactory>();
			IUnitOfWork unitOfWork = unitOfWorkFactory.CreateUnitOfWork("Repository");
			await unitOfWork.SaveChangesAsync();

			InterceptorCounter counter = this.serviceProvider.GetRequiredService<InterceptorCounter>();
			counter.BeforeAddCalled.Should().BeGreaterThan(0);
		}

		[Test]
		public async Task ShouldInterceptRemove()
		{
			IUnitOfWorkFactory unitOfWorkFactory = this.serviceProvider.GetRequiredService<IUnitOfWorkFactory>();
			IUnitOfWork unitOfWork = unitOfWorkFactory.CreateUnitOfWork("Repository");

			IPersonRepository personRepository = this.serviceProvider.GetRequiredService<IPersonRepository>();
			Person person = new Person
			{
				Name = "Tester",
				Age = 33
			};
			await personRepository.AddAsync(person);
			await unitOfWork.SaveChangesAsync();

			await personRepository.RemoveAsync(person);
			await unitOfWork.SaveChangesAsync();

			InterceptorCounter counter = this.serviceProvider.GetRequiredService<InterceptorCounter>();
			counter.BeforeRemoveCalled.Should().BeGreaterThan(0);
		}

		[Test]
		public async Task ShouldInterceptUpdate()
		{
			IUnitOfWorkFactory unitOfWorkFactory = this.serviceProvider.GetRequiredService<IUnitOfWorkFactory>();
			IUnitOfWork unitOfWork = unitOfWorkFactory.CreateUnitOfWork("Repository");

			IPersonRepository personRepository = this.serviceProvider.GetRequiredService<IPersonRepository>();
			Person person = new Person
			{
				Name = "Tester",
				Age = 33
			};
			await personRepository.AddAsync(person);
			await unitOfWork.SaveChangesAsync();

			await personRepository.UpdateAsync(person);
			await unitOfWork.SaveChangesAsync();

			InterceptorCounter counter = this.serviceProvider.GetRequiredService<InterceptorCounter>();
			counter.BeforeUpdateCalled.Should().BeGreaterThan(0);
		}
	}
}
