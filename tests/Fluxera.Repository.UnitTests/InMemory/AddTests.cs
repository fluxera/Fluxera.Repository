namespace Fluxera.Repository.UnitTests.InMemory
{
	using System;
	using Fluxera.Repository.Storage.InMemory;
	using Fluxera.Repository.UnitTests.PersonAggregate;
	using JetBrains.Annotations;
	using Microsoft.Extensions.DependencyInjection;
	using NUnit.Framework;

	[TestFixture]
	public class AddTests : TestBase
	{
		private IRepository<Person> repository;
		private IPersonRepository personRepository;
		private IReadOnlyRepository<Person> readOnlyRepository;

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

		[Test]
		public void ShouldAddItem()
		{

		}
	}
}
