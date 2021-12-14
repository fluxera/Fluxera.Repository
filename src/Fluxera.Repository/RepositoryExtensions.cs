namespace Fluxera.Repository
{
	using System.Collections.Generic;
	using System.Threading.Tasks;
	using Fluxera.Entity;
	using Fluxera.Guards;
	using JetBrains.Annotations;

	[PublicAPI]
	public static class RepositoryExtensions
	{
		/// <summary>
		///     Adds the or update.
		/// </summary>
		/// <typeparam name="TAggregateRoot">The type of the aggregate root.</typeparam>
		/// <param name="repository">The repository.</param>
		/// <param name="item">The item.</param>
		public static async Task AddOrUpdate<TAggregateRoot>(this IRepository<TAggregateRoot> repository, TAggregateRoot item)
			where TAggregateRoot : AggregateRoot<TAggregateRoot>
		{
			Guard.Against.Null(repository, nameof(repository));
			Guard.Against.Null(item, nameof(item));

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
		///     Adds the or update.
		/// </summary>
		/// <typeparam name="TAggregateRoot">The type of the aggregate root.</typeparam>
		/// <param name="repository">The repository.</param>
		/// <param name="items">The items.</param>
		public static async Task AddOrUpdate<TAggregateRoot>(this IRepository<TAggregateRoot> repository, IEnumerable<TAggregateRoot> items)
			where TAggregateRoot : AggregateRoot<TAggregateRoot>
		{
			Guard.Against.Null(repository, nameof(repository));
			Guard.Against.Null(items, nameof(items));

			foreach(TAggregateRoot item in items)
			{
				await repository.AddOrUpdate(item);
			}
		}
	}
}
