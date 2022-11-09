namespace Fluxera.Repository.UnitTests.Core
{
	using System;
	using System.Threading.Tasks;
	using Fluxera.Repository.UnitTests.Core.CompanyAggregate;
	using Fluxera.Repository.UnitTests.Core.EmployeeAggregate;
	using Fluxera.Repository.UnitTests.Core.PersonAggregate;
	using Fluxera.Repository.UnitTests.Core.ReferenceAggregate;
	using JetBrains.Annotations;
	using Microsoft.Extensions.DependencyInjection;
	using NUnit.Framework;

	[PublicAPI]
	public abstract class RepositoryTestBase : TestBase
	{
		private ServiceProvider serviceProvider;

		protected IRepository<Person, Guid> PersonRepository { get; private set; }

		protected IRepository<Company, string> CompanyRepository { get; private set; }

		protected IRepository<Employee, EmployeeId> EmployeeRepository { get; private set; }

		protected IRepository<Reference, string> ReferenceRepository { get; private set; }

		protected IUnitOfWork UnitOfWork { get; private set; }

		[SetUp]
		public async Task SetUp()
		{
			RepositoryName repositoryName = new RepositoryName("RepositoryUnderTest");

			this.serviceProvider = BuildServiceProvider(services =>
			{
				services.AddRepository(rb =>
				{
					this.AddRepositoryUnderTest(rb, (string)repositoryName, rob =>
					{
						rob.UseFor<Person>();
						rob.UseFor<Company>();
						rob.UseFor<Employee>();
						rob.UseFor<Reference>();
					});
				});

				services.AddTransient<IPersonRepository, PersonRepository>();
				services.AddTransient<ICompanyRepository, CompanyRepository>();
				services.AddTransient<IEmployeeRepository, EmployeeRepository>();
				services.AddTransient<IReferenceRepository, ReferenceRepository>();
			});

			this.PersonRepository = this.serviceProvider.GetRequiredService<IPersonRepository>();
			this.CompanyRepository = this.serviceProvider.GetRequiredService<ICompanyRepository>();
			this.EmployeeRepository = this.serviceProvider.GetRequiredService<IEmployeeRepository>();
			this.ReferenceRepository = this.serviceProvider.GetRequiredService<IReferenceRepository>();

			IUnitOfWorkFactory unitOfWorkFactory = this.serviceProvider.GetRequiredService<IUnitOfWorkFactory>();
			this.UnitOfWork = unitOfWorkFactory.CreateUnitOfWork(repositoryName.Name);

			await this.OnSetUpAsync();
		}

		[TearDown]
		public async Task TearDown()
		{
			try
			{
				if(this.ReferenceRepository != null)
				{
					await this.ReferenceRepository.RemoveRangeAsync(x => true);
				}

				if(this.PersonRepository != null)
				{
					await this.PersonRepository.RemoveRangeAsync(x => true);
				}

				if(this.CompanyRepository != null)
				{
					await this.CompanyRepository.RemoveRangeAsync(x => true);
				}

				if(this.EmployeeRepository != null)
				{
					await this.EmployeeRepository.RemoveRangeAsync(x => true);
				}

				await this.UnitOfWork.SaveChangesAsync();

				if(this.ReferenceRepository != null)
				{
					await this.ReferenceRepository.DisposeAsync();
				}

				if(this.PersonRepository != null)
				{
					await this.PersonRepository.DisposeAsync();
				}

				if(this.CompanyRepository != null)
				{
					await this.CompanyRepository.DisposeAsync();
				}

				if(this.EmployeeRepository != null)
				{
					await this.EmployeeRepository.DisposeAsync();
				}

				await this.TearDownAsync();
			}
			catch(Exception ex)
			{
				Console.WriteLine(ex);
				throw;
			}
			finally
			{
				if(this.serviceProvider is not null)
				{
					await this.serviceProvider.DisposeAsync();
				}
			}
		}

		protected virtual Task OnSetUpAsync()
		{
			return Task.CompletedTask;
		}

		protected virtual Task TearDownAsync()
		{
			return Task.CompletedTask;
		}

		protected abstract void AddRepositoryUnderTest(
			IRepositoryBuilder repositoryBuilder,
			string repositoryName,
			Action<IRepositoryOptionsBuilder> configureOptions);
	}
}
