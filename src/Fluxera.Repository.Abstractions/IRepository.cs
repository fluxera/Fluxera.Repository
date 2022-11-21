namespace Fluxera.Repository
{
	using System;
	using Fluxera.Entity;
	using Fluxera.Repository.Traits;
	using JetBrains.Annotations;

	/// <inheritdoc cref="ICanAdd{TAggregateRoot,TKey}" />
	/// <inheritdoc cref="ICanUpdate{TAggregateRoot,TKey}" />
	/// <inheritdoc cref="ICanRemove{TAggregateRoot,TKey}" />
	/// <inheritdoc cref="IReadOnlyRepository{TAggregateRoot,TKey}" />
	/// <summary>
	///     Contract for a repository that accesses <typeparamref name="TAggregateRoot" /> entities.
	/// </summary>
	/// <typeparam name="TAggregateRoot">Generic repository entity type.</typeparam>
	/// <typeparam name="TKey">The type of the ID.</typeparam>
	[PublicAPI]
	public interface IRepository<TAggregateRoot, in TKey> :
		ICanAdd<TAggregateRoot, TKey>,
		ICanUpdate<TAggregateRoot, TKey>,
		ICanRemove<TAggregateRoot, TKey>,
		IReadOnlyRepository<TAggregateRoot, TKey>,
		IProvideRepositoryName<TAggregateRoot, TKey>
		where TAggregateRoot : AggregateRoot<TAggregateRoot, TKey>
		where TKey : notnull, IComparable<TKey>, IEquatable<TKey>
	{
	}
}
