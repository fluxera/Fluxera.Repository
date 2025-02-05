namespace Fluxera.Repository
{
	using System;
	using Fluxera.Entity;
	using Fluxera.Repository.Traits;
	using JetBrains.Annotations;

	/// <inheritdoc cref="ICanAdd{TEntity,TKey}" />
	/// <inheritdoc cref="ICanUpdate{TEntity,TKey}" />
	/// <inheritdoc cref="ICanRemove{TEntity,TKey}" />
	/// <inheritdoc cref="IReadOnlyRepository{TEntity,TKey}" />
	/// <summary>
	///     Contract for a repository that accesses <typeparamref name="TEntity" /> entities.
	/// </summary>
	/// <typeparam name="TEntity">Generic repository entity type.</typeparam>
	/// <typeparam name="TKey">The type of the ID.</typeparam>
	[PublicAPI]
	public interface IRepository<TEntity, in TKey> :
		ICanAdd<TEntity, TKey>,
		ICanUpdate<TEntity, TKey>,
		ICanRemove<TEntity, TKey>,
		IReadOnlyRepository<TEntity, TKey>,
		IProvideRepositoryName<TEntity, TKey>
		where TEntity : Entity<TEntity, TKey>
		where TKey : notnull, IComparable<TKey>, IEquatable<TKey>
	{
	}
}
