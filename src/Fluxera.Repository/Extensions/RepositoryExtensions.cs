namespace Fluxera.Repository.Extensions
{
	using System;
	using System.Collections.Generic;
	using System.Threading.Tasks;
	using Fluxera.Entity;
	using Fluxera.Guards;
	using Fluxera.Repository;
	using JetBrains.Annotations;

	/// <summary>
	///     Provides extension methods for the <see cref="IRepository{TAggregateRoot,TKey}" /> type.
	/// </summary>
	[PublicAPI]
	public static class RepositoryExtensions
	{
		/// <summary>
		///     Adds or updates the given item.
		/// </summary>
		/// <typeparam name="TAggregateRoot">The type of the aggregate root.</typeparam>
		/// <typeparam name="TKey">The type of the ID.</typeparam>
		/// <param name="repository">The repository.</param>
		/// <param name="item">The item.</param>
		public static async Task AddOrUpdate<TAggregateRoot, TKey>(this IRepository<TAggregateRoot, TKey> repository, TAggregateRoot item)
			where TAggregateRoot : AggregateRoot<TAggregateRoot, TKey>
			where TKey : IComparable<TKey>, IEquatable<TKey>
		{
			Guard.Against.Null(repository);
			Guard.Against.Null(item);

			if(item.IsTransient)
			{
				await repository.AddAsync(item);
			}
			else
			{
				await repository.UpdateAsync(item);
			}
		}

		/// <summary>
		///     Adds or updates the given items.
		/// </summary>
		/// <typeparam name="TAggregateRoot">The type of the aggregate root.</typeparam>
		/// <typeparam name="TKey">The type of the ID.</typeparam>
		/// <param name="repository">The repository.</param>
		/// <param name="items">The items.</param>
		public static async Task AddOrUpdate<TAggregateRoot, TKey>(this IRepository<TAggregateRoot, TKey> repository, IEnumerable<TAggregateRoot> items)
			where TAggregateRoot : AggregateRoot<TAggregateRoot, TKey>
			where TKey : IComparable<TKey>, IEquatable<TKey>
		{
			// ReSharper disable PossibleMultipleEnumeration
			Guard.Against.Null(repository);
			Guard.Against.Null(items);

			foreach(TAggregateRoot item in items)
			{
				await repository.AddOrUpdate(item);
			}
			// ReSharper enable PossibleMultipleEnumeration
		}
	}
}
