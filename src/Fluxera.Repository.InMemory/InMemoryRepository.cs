namespace Fluxera.Repository.InMemory
{
	using System;
	using System.Collections.Concurrent;
	using System.Collections.Generic;
	using System.Linq;
	using System.Threading;
	using System.Threading.Tasks;
	using Fluxera.Entity;
	using Fluxera.Guards;
	using Fluxera.Repository.Specifications;
	using Microsoft.Extensions.Logging;

	public sealed class InMemoryRepository<TAggregateRoot, TKey> : LinqRepositoryBase<TAggregateRoot, TKey>
		where TAggregateRoot : AggregateRoot<TAggregateRoot, TKey>
	{
		private readonly ILogger logger;
		private readonly ConcurrentDictionary<TKey, TAggregateRoot> store = new ConcurrentDictionary<TKey, TAggregateRoot>();

		public InMemoryRepository(ILoggerFactory loggerFactory)
		{
			Guard.Against.Null(loggerFactory, nameof(loggerFactory));

			this.logger = loggerFactory.CreateLogger(Name);
		}

		private static string Name => "Fluxera.Repository.InMemoryRepository";

		/// <inheritdoc />
		protected override IQueryable<TAggregateRoot> Queryable => this.store.Values.AsQueryable();

		/// <inheritdoc />
		public override string ToString()
		{
			return Name;
		}

		/// <inheritdoc />
		protected override async Task<long> LongCountAsync(IQueryable<TAggregateRoot> queryable, CancellationToken cancellationToken)
		{
			return queryable.LongCount();
		}

		/// <inheritdoc />
		protected override async Task<IReadOnlyCollection<TAggregateRoot>> ToListAsync(IQueryable<TAggregateRoot> queryable, CancellationToken cancellationToken)
		{
			return queryable.ToList();
		}

		/// <inheritdoc />
		protected override async Task<IReadOnlyCollection<TResult>> ToListAsync<TResult>(IQueryable<TResult> queryable, CancellationToken cancellationToken)
		{
			return queryable.ToList();
		}

		/// <inheritdoc />
		protected override async Task<TAggregateRoot> FirstOrDefaultAsync(IQueryable<TAggregateRoot> queryable, CancellationToken cancellationToken)
		{
			return queryable.FirstOrDefault()!;
		}

		/// <inheritdoc />
		protected override async Task<TResult> FirstOrDefaultAsync<TResult>(IQueryable<TResult> queryable, CancellationToken cancellationToken)
		{
			return queryable.FirstOrDefault()!;
		}

		/// <inheritdoc />
		protected override async Task AddAsync(TAggregateRoot item, CancellationToken cancellationToken)
		{
			item.ID = this.GenerateKey();
			this.store.TryAdd(item.ID, item);
		}

		/// <inheritdoc />
		protected override async Task AddRangeAsync(IEnumerable<TAggregateRoot> items, CancellationToken cancellationToken)
		{
			foreach(TAggregateRoot item in items)
			{
				item.ID = this.GenerateKey();
				this.store.TryAdd(item.ID, item);
			}
		}

		/// <inheritdoc />
		protected override async Task RemoveRangeAsync(ISpecification<TAggregateRoot> specification, CancellationToken cancellationToken)
		{
			IQueryable<TAggregateRoot> items = this.Queryable.Where(specification.Predicate);
			foreach(TAggregateRoot? item in items)
			{
				this.store.TryRemove(item.ID, out _);
			}
		}

		/// <inheritdoc />
		protected override async Task RemoveRangeAsync(IEnumerable<TAggregateRoot> items, CancellationToken cancellationToken)
		{
			foreach(TAggregateRoot item in items)
			{
				this.store.TryRemove(item.ID, out _);
			}
		}

		/// <inheritdoc />
		protected override async Task UpdateAsync(TAggregateRoot item, CancellationToken cancellationToken)
		{
			this.store[item.ID] = item;
		}

		/// <inheritdoc />
		protected override async Task UpdateRangeAsync(IEnumerable<TAggregateRoot> items, CancellationToken cancellationToken)
		{
			foreach(TAggregateRoot item in items)
			{
				this.store[item.ID] = item;
			}
		}

		private TKey GenerateKey()
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
				TKey? pkValue = this.store.Keys.LastOrDefault();

				int nextInt = Convert.ToInt32(pkValue) + 1;
				return (TKey)Convert.ChangeType(nextInt, typeof(TKey));
			}

			if(typeof(TKey) == typeof(long))
			{
				TKey? pkValue = this.store.Keys.LastOrDefault();

				int nextInt = Convert.ToInt32(pkValue) + 1;
				return (TKey)Convert.ChangeType(nextInt, typeof(TKey));
			}

			throw new InvalidOperationException("A key could not be generated. The in-memory repository only supports guid, string, int and long for keys.");
		}
	}
}
