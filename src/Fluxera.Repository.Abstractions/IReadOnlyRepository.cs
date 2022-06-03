namespace Fluxera.Repository
{
	using System;
	using Fluxera.Entity;
	using Fluxera.Repository.Traits;
	using JetBrains.Annotations;

	/// <inheritdoc cref="ICanGet{TAggregateRoot,TKey}" />
	/// <inheritdoc cref="ICanFind{TAggregateRoot,TKey}" />
	/// <inheritdoc cref="ICanAggregate{TAggregateRoot,TKey}" />
	/// <summary>
	///     Contract for a repository that reads <typeparamref name="TAggregateRoot" /> entities.
	/// </summary>
	/// <typeparam name="TAggregateRoot">The entity type.</typeparam>
	/// <typeparam name="TKey">The type of the ID.</typeparam>
	[PublicAPI]
	public interface IReadOnlyRepository<TAggregateRoot, TKey> : IDisposableRepository,
		ICanGet<TAggregateRoot, TKey>,
		ICanFind<TAggregateRoot, TKey>,
		ICanAggregate<TAggregateRoot, TKey>
		where TAggregateRoot : AggregateRoot<TAggregateRoot, TKey>
		where TKey : notnull, IComparable<TKey>, IEquatable<TKey>
	{
	}
}
