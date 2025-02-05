namespace Fluxera.Repository.Validation
{
	using System;
	using Fluxera.Entity;
	using JetBrains.Annotations;

	/// <summary>
	///     A contract for a validation strategy factory.
	/// </summary>
	[PublicAPI]
	public interface IValidationStrategyFactory
	{
		/// <summary>
		///     Creates the validation strategy to use for the repository and aggregate root.
		/// </summary>
		/// <typeparam name="TEntity"></typeparam>
		/// <typeparam name="TKey">The type of the ID.</typeparam>
		/// <returns></returns>
		IValidationStrategy<TEntity, TKey> CreateStrategy<TEntity, TKey>()
			where TEntity : Entity<TEntity, TKey>
			where TKey : IComparable<TKey>, IEquatable<TKey>;
	}
}
