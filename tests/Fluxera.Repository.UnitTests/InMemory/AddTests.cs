namespace Fluxera.Repository.UnitTests.InMemory
{
	using System;
	using Fluxera.Repository.Storage.InMemory;
	using Fluxera.Repository.UnitTests.PersonAggregate;
	using Microsoft.Extensions.DependencyInjection;
	using NUnit.Framework;

	[TestFixture]
	public class AddTests : TestBase
	{
		[SetUp]
		public void SetUp()
		{
			IServiceProvider serviceProvider = BuildServiceProvider(services =>
			{
				services.AddRepository(rb =>
				{
					rb.AddInMemoryRepository("InMemory", rob =>
					{
						rob.UseFor<Person>();
					});
				});

				services.AddTransient<IPersonRepository, PersonRepository>();
			});

			this.repository = serviceProvider.GetRequiredService<IRepository<Person>>();
			this.readOnlyRepository = serviceProvider.GetRequiredService<IReadOnlyRepository<Person>>();
			this.personRepository = serviceProvider.GetRequiredService<IPersonRepository>();
		}

		private IRepository<Person> repository;
		private IPersonRepository personRepository;
		private IReadOnlyRepository<Person> readOnlyRepository;

		[Test]
		public void ShouldAddItem()
		{
		}
	}
}
