namespace Fluxera.Repository.UnitTests
{
	using System;
	using Fluxera.Repository.Storage.InMemory;
	using Fluxera.Repository.UnitTests.PersonAggregate;
	using Microsoft.Extensions.DependencyInjection;
	using NUnit.Framework;

	public abstract class RepositoryTestBase : TestBase
	{
		protected IRepository<Person> Repository { get; private set; }

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

			this.Repository = serviceProvider.GetRequiredService<IPersonRepository>();
		}
	}
}
