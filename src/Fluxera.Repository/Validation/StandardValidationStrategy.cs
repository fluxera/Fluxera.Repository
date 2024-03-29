﻿namespace Fluxera.Repository.Validation
{
	using System;
	using System.Collections.Generic;
	using System.Threading.Tasks;
	using Fluxera.Entity;
	using Fluxera.Extensions.Validation;
	using Fluxera.Guards;

	internal sealed class StandardValidationStrategy<TAggregateRoot, TKey> : IValidationStrategy<TAggregateRoot, TKey>
		where TAggregateRoot : AggregateRoot<TAggregateRoot, TKey>
		where TKey : IComparable<TKey>, IEquatable<TKey>
	{
		private readonly IReadOnlyCollection<IValidator> validators;

		public StandardValidationStrategy(IReadOnlyCollection<IValidator> validators)
		{
			Guard.Against.Null(validators, nameof(validators));

			this.validators = validators;
		}

		public async Task ValidateAsync(TAggregateRoot item)
		{
			ValidationResult validationResult = await this.validators.ValidateAsync(item);
			if(!validationResult.IsValid)
			{
				throw Errors.ItemNotValid(validationResult.ValidationErrors);
			}
		}

		public async Task ValidateAsync(IEnumerable<TAggregateRoot> items)
		{
			foreach(TAggregateRoot item in items)
			{
				ValidationResult validationResult = await this.validators.ValidateAsync(item);
				if(!validationResult.IsValid)
				{
					throw Errors.ItemNotValid(validationResult.ValidationErrors);
				}
			}
		}
	}
}
