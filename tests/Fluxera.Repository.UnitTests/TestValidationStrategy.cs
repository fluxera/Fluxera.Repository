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

	public class TestValidationStrategy<TAggregateRoot, TKey> : IValidationStrategy<TAggregateRoot, TKey>
		where TAggregateRoot : AggregateRoot<TAggregateRoot, TKey>
		where TKey : IComparable<TKey>, IEquatable<TKey>
	{
		private readonly IValidationService validationService;

		public TestValidationStrategy(IValidationService validationService)
		{
			this.validationService = validationService;
		}

		public async Task ValidateAsync(TAggregateRoot item, CancellationToken cancellationToken = default)
		{
			ValidationResult validationResult = await this.validationService.ValidateAsync(item, cancellationToken);
			if(!validationResult.IsValid)
			{
				ValidationException exception = new ValidationException("Validation failed.", validationResult.Errors);
				throw exception;
			}
		}

		public async Task ValidateAsync(IEnumerable<TAggregateRoot> items, CancellationToken cancellationToken = default)
		{
			foreach(TAggregateRoot item in items)
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
