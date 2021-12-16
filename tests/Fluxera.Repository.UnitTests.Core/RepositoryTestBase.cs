namespace Fluxera.Repository.UnitTests.Core
{
	using System;
	using Fluxera.Repository.UnitTests.Core.PersonAggregate;
	using Microsoft.Extensions.DependencyInjection;
	using NUnit.Framework;

	public abstract class RepositoryTestBase : TestBase
	{
		protected IRepository<Person>? Repository { get; private set; }

		[SetUp]
		public void SetUp()
		{
			IServiceProvider serviceProvider = BuildServiceProvider(services =>
			{
				services.AddRepository(rb =>
				{
					this.AddRepositoryUnderTest(rb, "RepositoryUnderTest", rob =>
					{
						rob.UseFor<Person>();
					});
				});

				services.AddTransient<IPersonRepository, PersonRepository>();
			});

			this.Repository = serviceProvider.GetRequiredService<IPersonRepository>();
		}

		[TearDown]
		public void TearDown()
		{
			this.Repository?.Dispose();
		}

		protected abstract void AddRepositoryUnderTest(IRepositoryBuilder repositoryBuilder,
			string repositoryName, Action<IRepositoryOptionsBuilder> configureOptions);
	}
}
