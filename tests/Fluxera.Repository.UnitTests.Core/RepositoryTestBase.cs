namespace Fluxera.Repository.UnitTests.Core
{
	using System;
	using System.Threading.Tasks;
	using Fluxera.Repository.UnitTests.Core.CompanyAggregate;
	using Fluxera.Repository.UnitTests.Core.EmployeeAggregate;
	using Fluxera.Repository.UnitTests.Core.PersonAggregate;
	using Microsoft.Extensions.DependencyInjection;
	using NUnit.Framework;

	public abstract class RepositoryTestBase : TestBase
	{
		private ServiceProvider serviceProvider;

		protected IRepository<Person, Guid> PersonRepository { get; private set; }

		protected IRepository<Company, string> CompanyRepository { get; private set; }

		protected IRepository<Employee, EmployeeId> EmployeeRepository { get; private set; }

		[SetUp]
		public void SetUp()
		{
			this.serviceProvider = BuildServiceProvider(services =>
			{
				services.AddRepository(rb =>
				{
					this.AddRepositoryUnderTest(rb, "RepositoryUnderTest", rob =>
					{
						rob.UseFor<Person>();
						rob.UseFor<Company>();
						rob.UseFor<Employee>();
					});
				});

				services.AddTransient<IPersonRepository, PersonRepository>();
				services.AddTransient<ICompanyRepository, CompanyRepository>();
				services.AddTransient<IEmployeeRepository, EmployeeRepository>();
			});

			this.PersonRepository = this.serviceProvider.GetRequiredService<IPersonRepository>();
			this.CompanyRepository = this.serviceProvider.GetRequiredService<ICompanyRepository>();
			this.EmployeeRepository = this.serviceProvider.GetRequiredService<IEmployeeRepository>();
		}

		[TearDown]
		public async Task TearDown()
		{
			try
			{
				if(this.PersonRepository != null!)
				{
					await this.PersonRepository.RemoveRangeAsync(x => true);
					await this.PersonRepository.DisposeAsync();
				}

				if(this.CompanyRepository != null!)
				{
					await this.CompanyRepository.RemoveRangeAsync(x => true);
					await this.CompanyRepository.DisposeAsync();
				}

				if(this.EmployeeRepository != null!)
				{
					await this.EmployeeRepository.RemoveRangeAsync(x => true);
					await this.EmployeeRepository.DisposeAsync();
				}
			}
			catch(Exception ex)
			{
				Console.WriteLine(ex);
			}
			finally
			{
				await this.serviceProvider.DisposeAsync();
			}
		}

		protected abstract void AddRepositoryUnderTest(IRepositoryBuilder repositoryBuilder,
			string repositoryName, Action<IRepositoryOptionsBuilder> configureOptions);
	}
}
