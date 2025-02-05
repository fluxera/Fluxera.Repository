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

	internal sealed class StandardValidationStrategy<TEntity, TKey> : IValidationStrategy<TEntity, TKey>
		where TEntity : Entity<TEntity, TKey>
		where TKey : IComparable<TKey>, IEquatable<TKey>
	{
		private readonly IValidationService validationService;

		public StandardValidationStrategy(IValidationService validationService)
		{
			this.validationService = Guard.Against.Null(validationService);
		}

		public async Task ValidateAsync(TEntity item, CancellationToken cancellationToken = default)
		{
			ValidationResult validationResult = await this.validationService.ValidateAsync(item, cancellationToken);
			if(!validationResult.IsValid)
			{
				throw Errors.ItemNotValid(validationResult.Errors);
			}
		}

		public async Task ValidateAsync(IEnumerable<TEntity> items, CancellationToken cancellationToken = default)
		{
			foreach(TEntity item in items)
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
