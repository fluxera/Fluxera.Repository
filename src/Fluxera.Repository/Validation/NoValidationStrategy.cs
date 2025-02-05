namespace Fluxera.Repository.Validation
{
	using System;
	using System.Collections.Generic;
	using System.Threading;
	using System.Threading.Tasks;
	using Fluxera.Entity;

	internal sealed class NoValidationStrategy<TEntity, TKey> : IValidationStrategy<TEntity, TKey>
		where TEntity : Entity<TEntity, TKey>
		where TKey : IComparable<TKey>, IEquatable<TKey>
	{
		/// <inheritdoc />
		public Task ValidateAsync(TEntity item, CancellationToken cancellationToken = default)
		{
			return Task.CompletedTask;
		}

		/// <inheritdoc />
		public Task ValidateAsync(IEnumerable<TEntity> items, CancellationToken cancellationToken = default)
		{
			return Task.CompletedTask;
		}
	}
}
