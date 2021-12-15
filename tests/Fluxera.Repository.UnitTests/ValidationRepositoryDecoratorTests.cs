namespace Fluxera.Repository.UnitTests
{
	using System;
	using System.Threading.Tasks;
	using FluentAssertions;
	using Fluxera.Extensions.Validation;
	using Fluxera.Extensions.Validation.DataAnnotations;
	using Fluxera.Repository.Decorators;
	using Fluxera.Repository.UnitTests.PersonAggregate;
	using Fluxera.Repository.Validation;
	using Microsoft.Extensions.DependencyInjection;
	using NUnit.Framework;

	[TestFixture]
	public class ValidationRepositoryDecoratorTests : DecoratorTestBase
	{
		private void ShouldGuardAgainstInvalid(Func<Task> innerFunc)
		{
			Func<Task> func = async () =>
			{
				await innerFunc.Invoke();
			};
			func.Should().ThrowAsync<ValidationException>();
		}

		/// <inheritdoc />
		protected override Type DecoratorType => typeof(ValidationRepositoryDecorator<>);

		/// <inheritdoc />
		protected override void ConfigureServices(IServiceCollection services)
		{
			services.AddValidation(builder =>
			{
				builder.AddDataAnnotations();
			});

			services.AddTransient<IValidationStrategyFactory, TesValidationStrategyFactory>();
		}

		[Test]
		public void ShouldGuard_AddAsync_Multiple()
		{
			this.ShouldGuardAgainstInvalid(async () => await this.Repository.AddAsync(Persons.Invalid));
		}

		[Test]
		public void ShouldGuard_AddAsync_Single()
		{
			this.ShouldGuardAgainstInvalid(async () => await this.Repository.AddAsync(Person.Invalid));
		}

		[Test]
		public void ShouldGuard_UpdateAsync_Multiple()
		{
			this.ShouldGuardAgainstInvalid(async () => await this.Repository.UpdateAsync(Persons.Invalid));
		}

		[Test]
		public void ShouldGuard_UpdateAsync_Single()
		{
			this.ShouldGuardAgainstInvalid(async () => await this.Repository.UpdateAsync(Person.Invalid));
		}
	}
}
