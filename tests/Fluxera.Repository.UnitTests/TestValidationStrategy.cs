namespace Fluxera.Repository.UnitTests
{
	using System.Collections.Generic;
	using System.Threading.Tasks;
	using Fluxera.Entity;
	using Fluxera.Extensions.Validation;
	using Fluxera.Repository.Validation;
	using Fluxera.Utilities.Extensions;

	public class TestValidationStrategy<T> : IValidationStrategy<T> where T : AggregateRoot<T>
	{
		private readonly IReadOnlyCollection<IValidator> validators;

		public TestValidationStrategy(IReadOnlyCollection<IValidator> validators)
		{
			this.validators = validators;
		}

		public async Task ValidateAsync(T item)
		{
			ValidationResult validationResult = await this.validators.ValidateAsync(item);
			if(!validationResult.IsValid)
			{
				ValidationException exception = new ValidationException("Validation failed");
				exception.Errors.AddRange(validationResult.ValidationErrors);
				throw exception;
			}
		}

		public async Task ValidateAsync(IEnumerable<T> items)
		{
			foreach(T item in items)
			{
				ValidationResult validationResult = await this.validators.ValidateAsync(item);
				if(!validationResult.IsValid)
				{
					ValidationException exception = new ValidationException("Validation failed");
					exception.Errors.AddRange(validationResult.ValidationErrors);
					throw exception;
				}
			}
		}
	}
}
