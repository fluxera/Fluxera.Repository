namespace Fluxera.Repository.UnitTests
{
	using System;
	using System.Collections.Generic;
	using System.Threading;
	using System.Threading.Tasks;
	using FluentValidation;
	using FluentValidation.Results;
	using Fluxera.Entity;
	using Fluxera.Extensions.Validation;
	using Fluxera.Repository.Validation;

	public class TestValidationStrategy<TEntity, TKey> : IValidationStrategy<TEntity, TKey>
		where TEntity : Entity<TEntity, TKey>
		where TKey : IComparable<TKey>, IEquatable<TKey>
	{
		private readonly IValidationService validationService;

		public TestValidationStrategy(IValidationService validationService)
		{
			this.validationService = validationService;
		}

		public async Task ValidateAsync(TEntity item, CancellationToken cancellationToken = default)
		{
			ValidationResult validationResult = await this.validationService.ValidateAsync(item, cancellationToken);
			if(!validationResult.IsValid)
			{
				ValidationException exception = new ValidationException("Validation failed.", validationResult.Errors);
				throw exception;
			}
		}

		public async Task ValidateAsync(IEnumerable<TEntity> items, CancellationToken cancellationToken = default)
		{
			foreach(TEntity item in items)
			{
				ValidationResult validationResult = await this.validationService.ValidateAsync(item, cancellationToken);
				if(!validationResult.IsValid)
				{
					ValidationException exception = new ValidationException("Validation failed.", validationResult.Errors);
					throw exception;
				}
			}
		}
	}
}
