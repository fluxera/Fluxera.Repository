﻿namespace Fluxera.Repository.UnitTests.Core
{
	using System;
	using System.Threading.Tasks;
	using FluentAssertions;
	using Fluxera.Repository.UnitTests.Core.PersonAggregate;
	using JetBrains.Annotations;
	using Microsoft.Extensions.DependencyInjection;
	using NUnit.Framework;

	[PublicAPI]
	public abstract class RepositoryContextTestsBase : TestBase
	{
		private ServiceProvider serviceProvider;

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

						rob.EnableUnitOfWork();
					});
				});

				services.AddTransient<IPersonRepository, PersonRepository>();
				services.AddScoped<TenantNameProvider>();
			});

			await this.OnSetUpAsync();
		}

		[TearDown]
		public async Task TearDown()
		{
			try
			{
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

		[Test]
		public async Task ShouldChangeTargetDatabaseBetweenScopes()
		{
			// Add person for tenant 1
			using(IServiceScope scope = this.serviceProvider.CreateScope())
			{
				await this.AddPerson(scope.ServiceProvider, "1");
				await this.AddPerson(scope.ServiceProvider, "1");
			}

			// Add person for tenant 2
			using(IServiceScope scope = this.serviceProvider.CreateScope())
			{
				await this.AddPerson(scope.ServiceProvider, "2");
				await this.AddPerson(scope.ServiceProvider, "2");
				await this.AddPerson(scope.ServiceProvider, "2");
				await this.AddPerson(scope.ServiceProvider, "2");
				await this.AddPerson(scope.ServiceProvider, "2");
			}

			// Check for person count of tenant 1.
			using(IServiceScope scope = this.serviceProvider.CreateScope())
			{
				long count = await this.GetPersonCount(scope.ServiceProvider, "1");
				count.Should().Be(2);
			}

			// Check for person count of tenant 2.
			using(IServiceScope scope = this.serviceProvider.CreateScope())
			{
				long count = await this.GetPersonCount(scope.ServiceProvider, "2");
				count.Should().Be(5);
			}
		}

		private async Task<long> GetPersonCount(IServiceProvider scopedServiceProvider, string tenantName)
		{
			// Store the current tenant in the scope.
			TenantNameProvider tenantNameProvider = scopedServiceProvider.GetRequiredService<TenantNameProvider>();
			tenantNameProvider.Name = tenantName;

			IPersonRepository personRepository = scopedServiceProvider.GetRequiredService<IPersonRepository>();
			long count = await personRepository.CountAsync();

			return count;
		}

		private async Task AddPerson(IServiceProvider scopedServiceProvider, string tenantName)
		{
			// Store the current tenant in the scope.
			TenantNameProvider tenantNameProvider = scopedServiceProvider.GetRequiredService<TenantNameProvider>();
			tenantNameProvider.Name = tenantName;

			// Add a person for the tenant.
			Person person = new Person
			{
				Name = $"Tenant-{tenantName}"
			};

			IPersonRepository personRepository = scopedServiceProvider.GetRequiredService<IPersonRepository>();
			IUnitOfWorkFactory unitOfWorkFactory = scopedServiceProvider.GetRequiredService<IUnitOfWorkFactory>();
			IUnitOfWork unitOfWork = unitOfWorkFactory.CreateUnitOfWork("RepositoryUnderTest");

			await personRepository.AddAsync(person);
			await unitOfWork.SaveChangesAsync();
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