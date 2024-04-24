namespace Fluxera.Repository.Validation
{
	using System;
	using System.Collections.Generic;
	using System.Threading;
	using System.Threading.Tasks;
	using FluentValidation.Results;
	using Fluxera.Entity;
	using Fluxera.Extensions.Validation;
	using Fluxera.Guards;

	internal sealed class StandardValidationStrategy<TAggregateRoot, TKey> : IValidationStrategy<TAggregateRoot, TKey>
		where TAggregateRoot : AggregateRoot<TAggregateRoot, TKey>
		where TKey : IComparable<TKey>, IEquatable<TKey>
	{
		private readonly IValidationService validationService;

		public StandardValidationStrategy(IValidationService validationService)
		{
			this.validationService = Guard.Against.Null(validationService);
		}

		public async Task ValidateAsync(TAggregateRoot item, CancellationToken cancellationToken = default)
		{
			ValidationResult validationResult = await this.validationService.ValidateAsync(item, cancellationToken);
			if(!validationResult.IsValid)
			{
				throw Errors.ItemNotValid(validationResult.Errors);
			}
		}

		public async Task ValidateAsync(IEnumerable<TAggregateRoot> items, CancellationToken cancellationToken = default)
		{
			foreach(TAggregateRoot item in items)
			{
				ValidationResult validationResult = await this.validationService.ValidateAsync(item, cancellationToken);
				if(!validationResult.IsValid)
				{
					throw Errors.ItemNotValid(validationResult.Errors);
				}
			}
		}
	}
}
