namespace Fluxera.Repository.Validation
{
	using System;
	using System.Collections.Generic;
	using System.Threading;
	using System.Threading.Tasks;
	using Fluxera.Entity;
	using JetBrains.Annotations;

	/// <summary>
	///     A contract for a validation strategy.
	/// </summary>
	/// <typeparam name="TEntity"></typeparam>
	/// <typeparam name="TKey"></typeparam>
	[PublicAPI]
	public interface IValidationStrategy<in TEntity, TKey>
		where TEntity : Entity<TEntity, TKey>
		where TKey : IComparable<TKey>, IEquatable<TKey>
	{
		/// <summary>
		///     Validates a single item.
		/// </summary>
		/// <param name="item"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		Task ValidateAsync(TEntity item, CancellationToken cancellationToken = default);

		/// <summary>
		///     Validates multiple items.
		/// </summary>
		/// <param name="items"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		Task ValidateAsync(IEnumerable<TEntity> items, CancellationToken cancellationToken = default);
	}
}
