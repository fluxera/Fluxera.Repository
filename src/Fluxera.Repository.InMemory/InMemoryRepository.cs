namespace Fluxera.Repository.InMemory
{
	using System;
	using System.Collections.Concurrent;
	using System.Collections.Generic;
	using System.Linq;
	using System.Threading;
	using System.Threading.Tasks;
	using Fluxera.Entity;
	using Fluxera.Repository.Specifications;

	internal sealed class InMemoryRepository<TAggregateRoot, TKey> : LinqRepositoryBase<TAggregateRoot, TKey>
		where TAggregateRoot : AggregateRoot<TAggregateRoot, TKey>
	{
		private static readonly ConcurrentDictionary<TKey, TAggregateRoot> store = new ConcurrentDictionary<TKey, TAggregateRoot>();

		private static string Name => "Fluxera.Repository.InMemoryRepository";

		/// <inheritdoc />
		protected override IQueryable<TAggregateRoot> Queryable => store.Values.AsQueryable();

		/// <inheritdoc />
		public override string ToString()
		{
			return Name;
		}

		/// <inheritdoc />
		protected override Task<long> LongCountAsync(IQueryable<TAggregateRoot> queryable, CancellationToken cancellationToken)
		{
			return Task.FromResult(queryable.LongCount());
		}

		/// <inheritdoc />
		protected override Task<IReadOnlyCollection<TAggregateRoot>> ToListAsync(IQueryable<TAggregateRoot> queryable, CancellationToken cancellationToken)
		{
			return Task.FromResult<IReadOnlyCollection<TAggregateRoot>>(queryable.ToList());
		}

		/// <inheritdoc />
		protected override Task<IReadOnlyCollection<TResult>> ToListAsync<TResult>(IQueryable<TResult> queryable, CancellationToken cancellationToken)
		{
			return Task.FromResult<IReadOnlyCollection<TResult>>(queryable.ToList());
		}

		/// <inheritdoc />
		protected override Task<TAggregateRoot> FirstOrDefaultAsync(IQueryable<TAggregateRoot> queryable, CancellationToken cancellationToken)
		{
			return Task.FromResult(queryable.FirstOrDefault()!);
		}

		/// <inheritdoc />
		protected override Task<TResult> FirstOrDefaultAsync<TResult>(IQueryable<TResult> queryable, CancellationToken cancellationToken)
		{
			return Task.FromResult(queryable.FirstOrDefault()!);
		}

		/// <inheritdoc />
		protected override Task AddAsync(TAggregateRoot item, CancellationToken cancellationToken)
		{
			item.ID = GenerateKey();
			store.TryAdd(item.ID, item);
			return Task.CompletedTask;
		}

		/// <inheritdoc />
		protected override Task AddRangeAsync(IEnumerable<TAggregateRoot> items, CancellationToken cancellationToken)
		{
			foreach(TAggregateRoot item in items)
			{
				item.ID = GenerateKey();
				store.TryAdd(item.ID, item);
			}

			return Task.CompletedTask;
		}

		/// <inheritdoc />
		protected override Task RemoveRangeAsync(ISpecification<TAggregateRoot> specification, CancellationToken cancellationToken)
		{
			IQueryable<TAggregateRoot> items = this.Queryable.Where(specification.Predicate);
			foreach(TAggregateRoot item in items)
			{
				store.TryRemove(item.ID, out _);
			}

			return Task.CompletedTask;
		}

		/// <inheritdoc />
		protected override Task RemoveRangeAsync(IEnumerable<TAggregateRoot> items, CancellationToken cancellationToken)
		{
			foreach(TAggregateRoot item in items)
			{
				store.TryRemove(item.ID, out _);
			}

			return Task.CompletedTask;
		}

		/// <inheritdoc />
		protected override Task UpdateAsync(TAggregateRoot item, CancellationToken cancellationToken)
		{
			store[item.ID] = item;
			return Task.CompletedTask;
		}

		/// <inheritdoc />
		protected override Task UpdateRangeAsync(IEnumerable<TAggregateRoot> items, CancellationToken cancellationToken)
		{
			foreach(TAggregateRoot item in items)
			{
				store[item.ID] = item;
			}

			return Task.CompletedTask;
		}

		private static TKey GenerateKey()
		{
			if(typeof(TKey) == typeof(string))
			{
				return (TKey)Convert.ChangeType(Guid.NewGuid().ToString("N"), typeof(TKey));
			}

			if(typeof(TKey) == typeof(Guid))
			{
				return (TKey)Convert.ChangeType(Guid.NewGuid(), typeof(TKey));
			}

			if(typeof(TKey) == typeof(int))
			{
				TKey pkValue = store.Keys.LastOrDefault();

				int nextInt = Convert.ToInt32(pkValue) + 1;
				return (TKey)Convert.ChangeType(nextInt, typeof(TKey));
			}

			if(typeof(TKey) == typeof(long))
			{
				TKey pkValue = store.Keys.LastOrDefault();

				int nextInt = Convert.ToInt32(pkValue) + 1;
				return (TKey)Convert.ChangeType(nextInt, typeof(TKey));
			}

			throw new InvalidOperationException("A key could not be generated. The in-memory repository only supports guid, string, int and long for keys.");
		}
	}
}
