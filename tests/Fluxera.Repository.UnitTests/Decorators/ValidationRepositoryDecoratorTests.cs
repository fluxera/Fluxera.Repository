namespace Fluxera.Repository.UnitTests.Decorators
{
	using System;
	using System.Threading.Tasks;
	using FluentAssertions;
	using Fluxera.Extensions.Validation;
	using Fluxera.Extensions.Validation.DataAnnotations;
	using Fluxera.Repository.Decorators;
	using Fluxera.Repository.UnitTests.Core.PersonAggregate;
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
		protected override Type DecoratorType => typeof(ValidationRepositoryDecorator<,>);

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
		public void ShouldValidate_AddAsync_Multiple()
		{
			Person[] persons =
			{
				new Person(),
				new Person()
			};
			this.ShouldGuardAgainstInvalid(async () => await this.Repository.AddRangeAsync(persons));
		}

		[Test]
		public void ShouldValidate_AddAsync_Single()
		{
			this.ShouldGuardAgainstInvalid(async () => await this.Repository.AddAsync(new Person()));
		}

		[Test]
		public void ShouldValidate_UpdateAsync_Multiple()
		{
			Person[] persons =
			{
				new Person
				{
					ID = Guid.NewGuid()
				},
				new Person
				{
					ID = Guid.NewGuid()
				}
			};
			this.ShouldGuardAgainstInvalid(async () => await this.Repository.UpdateRangeAsync(persons));
		}

		[Test]
		public void ShouldValidate_UpdateAsync_Single()
		{
			this.ShouldGuardAgainstInvalid(async () => await this.Repository.UpdateAsync(new Person
			{
				ID = Guid.NewGuid()
			}));
		}
	}
}
