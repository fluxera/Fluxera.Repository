namespace Fluxera.Repository.Validation
{
	using System;
	using System.Collections.Generic;
	using System.Threading.Tasks;
	using Fluxera.Entity;
	using JetBrains.Annotations;

	/// <summary>
	///     A contract for a validation strategy.
	/// </summary>
	/// <typeparam name="TAggregateRoot"></typeparam>
	/// <typeparam name="TKey"></typeparam>
	[PublicAPI]
	public interface IValidationStrategy<in TAggregateRoot, TKey>
		where TAggregateRoot : AggregateRoot<TAggregateRoot, TKey>
		where TKey : IComparable<TKey>, IEquatable<TKey>
	{
		/// <summary>
		///     Validates a single item.
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		Task ValidateAsync(TAggregateRoot item);

		/// <summary>
		///     Validates multiple items.
		/// </summary>
		/// <param name="items"></param>
		/// <returns></returns>
		Task ValidateAsync(IEnumerable<TAggregateRoot> items);
	}
}
