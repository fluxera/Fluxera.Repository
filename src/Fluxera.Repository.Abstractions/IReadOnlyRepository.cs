namespace Fluxera.Repository
{
	using System;
	using Fluxera.Entity;
	using Fluxera.Repository.Traits;
	using JetBrains.Annotations;

	/// <inheritdoc cref="ICanGet{TEntity,TKey}" />
	/// <inheritdoc cref="ICanFind{TEntity,TKey}" />
	/// <inheritdoc cref="ICanAggregate{TEntity,TKey}" />
	/// <summary>
	///     Contract for a repository that reads <typeparamref name="TEntity" /> entities.
	/// </summary>
	/// <typeparam name="TEntity">The entity type.</typeparam>
	/// <typeparam name="TKey">The type of the ID.</typeparam>
	[PublicAPI]
	public interface IReadOnlyRepository<TEntity, in TKey> : IDisposableRepository,
		ICanGet<TEntity, TKey>,
		ICanFind<TEntity, TKey>,
		ICanAggregate<TEntity, TKey>,
		IProvideRepositoryName<TEntity, TKey>
		where TEntity : Entity<TEntity, TKey>
		where TKey : notnull, IComparable<TKey>, IEquatable<TKey>
	{
	}
}
