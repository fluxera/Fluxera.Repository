namespace Fluxera.Repository.UnitTests
{
	using System;
	using System.Threading.Tasks;
	using FluentAssertions;
	using Fluxera.Extensions.Validation;
	using Fluxera.Extensions.Validation.DataAnnotations;
	using Fluxera.Repository.InMemory;
	using Fluxera.Repository.UnitTests.PersonAggregate;
	using Microsoft.Extensions.DependencyInjection;
	using NUnit.Framework;

	[TestFixture]
	public class ValidationRepositoryDecoratorTests : TestBase
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

						rob.AddValidation(vob =>
						{
							vob.AddValidatorFactory(vb =>
							{
								vb.AddDataAnnotations(vob.RepositoryName);
							});
						});

						rob.AddCaching(cob =>
						{
						});
					});
				});
			});

			this.repository = serviceProvider.GetRequiredService<IRepository<Person>>();
		}

		private IRepository<Person> repository;

		private void ShouldGuardAgainstInvalid(Func<Task> innerFunc)
		{
			Func<Task> func = async () =>
			{
				await innerFunc.Invoke();
			};
			func.Should().ThrowAsync<ValidationException>();
		}

		[Test]
		public void ShouldGuard_AddAsync_Multiple()
		{
			this.ShouldGuardAgainstInvalid(async () => await this.repository.AddAsync(Persons.Invalid));
		}

		[Test]
		public void ShouldGuard_AddAsync_Single()
		{
			this.ShouldGuardAgainstInvalid(async () => await this.repository.AddAsync(Person.Invalid));
		}

		[Test]
		public void ShouldGuard_UpdateAsync_Multiple()
		{
			this.ShouldGuardAgainstInvalid(async () => await this.repository.UpdateAsync(Persons.Invalid));
		}

		[Test]
		public void ShouldGuard_UpdateAsync_Single()
		{
			this.ShouldGuardAgainstInvalid(async () => await this.repository.UpdateAsync(Person.Invalid));
		}
	}
}
