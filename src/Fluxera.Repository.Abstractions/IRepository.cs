﻿namespace Fluxera.Repository
{
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
	///// <typeparam name="TKey">The type of the ID.</typeparam>
	[PublicAPI]
	public interface IRepository<TAggregateRoot /*, TKey*/> :
		ICanAdd<TAggregateRoot>,
		ICanUpdate<TAggregateRoot>,
		ICanRemove<TAggregateRoot>,
		IReadOnlyRepository<TAggregateRoot>
		where TAggregateRoot : AggregateRoot<TAggregateRoot>
	{
	}
}
