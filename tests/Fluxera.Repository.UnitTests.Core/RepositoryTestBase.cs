namespace Fluxera.Repository.UnitTests.Core
{
	using System;
	using System.Threading.Tasks;
	using Fluxera.Repository.UnitTests.Core.PersonAggregate;
	using Microsoft.Extensions.DependencyInjection;
	using NUnit.Framework;

	public abstract class RepositoryTestBase : TestBase
	{
		protected IRepository<Person, Guid> Repository { get; private set; }

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
		public async Task TearDown()
		{
			if(this.Repository != null!)
			{
				await this.Repository.RemoveAsync(x => true);
				await this.Repository.DisposeAsync();
			}
		}

		protected abstract void AddRepositoryUnderTest(IRepositoryBuilder repositoryBuilder,
			string repositoryName, Action<IRepositoryOptionsBuilder> configureOptions);
	}
}
