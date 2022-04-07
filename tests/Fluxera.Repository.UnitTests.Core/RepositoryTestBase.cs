namespace Fluxera.Repository.UnitTests.Core
{
	using System;
	using System.Threading.Tasks;
	using Fluxera.Repository.UnitTests.Core.CompanyAggregate;
	using Fluxera.Repository.UnitTests.Core.PersonAggregate;
	using Microsoft.Extensions.DependencyInjection;
	using NUnit.Framework;

	public abstract class RepositoryTestBase : TestBase
	{
		protected IRepository<Person, Guid> PersonRepository { get; private set; }

		protected IRepository<Company, Guid> CompanyRepository { get; private set; }

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
						rob.UseFor<Company>();
					});
				});

				services.AddTransient<IPersonRepository, PersonRepository>();
				services.AddTransient<ICompanyRepository, CompanyRepository>();
			});

			this.PersonRepository = serviceProvider.GetRequiredService<IPersonRepository>();
			this.CompanyRepository = serviceProvider.GetRequiredService<ICompanyRepository>();
		}

		[TearDown]
		public async Task TearDown()
		{
			if(this.PersonRepository != null!)
			{
				await this.PersonRepository.RemoveRangeAsync(x => true);
				await this.PersonRepository.DisposeAsync();
			}
		}

		protected abstract void AddRepositoryUnderTest(IRepositoryBuilder repositoryBuilder,
			string repositoryName, Action<IRepositoryOptionsBuilder> configureOptions);
	}
}
