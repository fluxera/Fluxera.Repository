namespace Fluxera.Repository.InMemory
{
	using System;
	using System.Collections.Concurrent;
	using System.Collections.Generic;
	using System.Linq;
	using System.Linq.Expressions;
	using System.Threading;
	using System.Threading.Tasks;
	using Fluxera.Entity;
	using Fluxera.Guards;
	using Fluxera.Repository.Query;
	using Fluxera.Repository.Traits;
	using Fluxera.Utilities.Extensions;
	using Microsoft.Extensions.Logging;

	public sealed class InMemoryRepository<TAggregateRoot, TKey> : IRepository<TAggregateRoot, TKey>
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

		private IQueryable<TAggregateRoot> Queryable => this.store.Values.AsQueryable();

		private bool IsDisposed { get; set; }

		/// <inheritdoc />
		async Task ICanAdd<TAggregateRoot, TKey>.AddAsync(TAggregateRoot item, CancellationToken cancellationToken)
		{
			item.ID = this.GenerateKey();
			this.store.TryAdd(item.ID, item);
		}

		/// <inheritdoc />
		async Task ICanAdd<TAggregateRoot, TKey>.AddAsync(IEnumerable<TAggregateRoot> items, CancellationToken cancellationToken)
		{
			foreach(TAggregateRoot item in items)
			{
				item.ID = this.GenerateKey();
				this.store.TryAdd(item.ID, item);
			}
		}

		/// <inheritdoc />
		async Task ICanUpdate<TAggregateRoot, TKey>.UpdateAsync(TAggregateRoot item, CancellationToken cancellationToken)
		{
			this.store[item.ID] = item;
		}

		/// <inheritdoc />
		async Task ICanUpdate<TAggregateRoot, TKey>.UpdateAsync(IEnumerable<TAggregateRoot> items, CancellationToken cancellationToken)
		{
			foreach(TAggregateRoot item in items)
			{
				this.store[item.ID] = item;
			}
		}

		/// <inheritdoc />
		async Task ICanRemove<TAggregateRoot, TKey>.RemoveAsync(TAggregateRoot item, CancellationToken cancellationToken)
		{
			this.store.TryRemove(item.ID, out _);
		}

		/// <inheritdoc />
		async Task ICanRemove<TAggregateRoot, TKey>.RemoveAsync(Expression<Func<TAggregateRoot, bool>> predicate, CancellationToken cancellationToken)
		{
			IQueryable<TAggregateRoot> items = this.Queryable.Where(predicate);
			foreach(TAggregateRoot? item in items)
			{
				this.store.TryRemove(item.ID, out _);
			}
		}

		/// <inheritdoc />
		async Task ICanRemove<TAggregateRoot, TKey>.RemoveAsync(IEnumerable<TAggregateRoot> items, CancellationToken cancellationToken)
		{
			foreach(TAggregateRoot item in items)
			{
				this.store.TryRemove(item.ID, out _);
			}
		}

		/// <inheritdoc />
		async Task<long> ICanAggregate<TAggregateRoot, TKey>.CountAsync(CancellationToken cancellationToken)
		{
			return this.Queryable.LongCount();
		}

		/// <inheritdoc />
		async Task<long> ICanAggregate<TAggregateRoot, TKey>.CountAsync(Expression<Func<TAggregateRoot, bool>> predicate, CancellationToken cancellationToken)
		{
			return this.Queryable.LongCount(predicate);
		}

		/// <inheritdoc />
		async Task<bool> ICanFind<TAggregateRoot, TKey>.ExistsAsync(Expression<Func<TAggregateRoot, bool>> predicate, CancellationToken cancellationToken)
		{
			return this.Queryable.LongCount(predicate) > 0;
		}

		/// <inheritdoc />
		async Task<TAggregateRoot> ICanFind<TAggregateRoot, TKey>.FindOneAsync(Expression<Func<TAggregateRoot, bool>> predicate, IQueryOptions<TAggregateRoot>? queryOptions, CancellationToken cancellationToken)
		{
			return this.Queryable
				.ApplyOptions<TAggregateRoot, TKey>(queryOptions)
				.FirstOrDefault(predicate)!;
		}

		/// <inheritdoc />
		async Task<TResult> ICanFind<TAggregateRoot, TKey>.FindOneAsync<TResult>(Expression<Func<TAggregateRoot, bool>> predicate, Expression<Func<TAggregateRoot, TResult>> selector, IQueryOptions<TAggregateRoot>? queryOptions, CancellationToken cancellationToken)
		{
			return this.Queryable
				.ApplyOptions<TAggregateRoot, TKey>(queryOptions)
				.Where(predicate)
				.Select(selector)
				.FirstOrDefault();
		}

		/// <inheritdoc />
		async Task<IReadOnlyCollection<TAggregateRoot>> ICanFind<TAggregateRoot, TKey>.FindManyAsync(Expression<Func<TAggregateRoot, bool>> predicate, IQueryOptions<TAggregateRoot>? queryOptions, CancellationToken cancellationToken)
		{
			return this.Queryable
				.ApplyOptions<TAggregateRoot, TKey>(queryOptions)
				.Where(predicate)
				.AsReadOnly();
		}

		/// <inheritdoc />
		async Task<IReadOnlyCollection<TResult>> ICanFind<TAggregateRoot, TKey>.FindManyAsync<TResult>(Expression<Func<TAggregateRoot, bool>> predicate, Expression<Func<TAggregateRoot, TResult>> selector, IQueryOptions<TAggregateRoot>? queryOptions, CancellationToken cancellationToken)
		{
			return this.Queryable
				.ApplyOptions<TAggregateRoot, TKey>(queryOptions)
				.Where(predicate)
				.Select(selector)
				.AsReadOnly();
		}

		/// <inheritdoc />
		public void Dispose()
		{
			if(!this.IsDisposed)
			{
				this.store.Clear();
			}

			this.IsDisposed = true;
		}

		/// <inheritdoc />
		bool IReadOnlyRepository<TAggregateRoot, TKey>.IsDisposed => this.IsDisposed;

		/// <inheritdoc />
		ValueTask IAsyncDisposable.DisposeAsync()
		{
			try
			{
				this.Dispose();
				return default;
			}
			catch(Exception exception)
			{
				return new ValueTask(Task.FromException(exception));
			}
		}

		/// <inheritdoc />
		async Task ICanRemove<TAggregateRoot, TKey>.RemoveAsync(TKey id, CancellationToken cancellationToken)
		{
			this.store.TryRemove(id, out TAggregateRoot item);
		}

		/// <inheritdoc />
		async Task<TAggregateRoot> ICanGet<TAggregateRoot, TKey>.GetAsync(TKey id, CancellationToken cancellationToken)
		{
			return this.Queryable.FirstOrDefault(x => Equals(x.ID, id))!;
		}

		/// <inheritdoc />
		async Task<TResult> ICanGet<TAggregateRoot, TKey>.GetAsync<TResult>(TKey id, Expression<Func<TAggregateRoot, TResult>> selector, CancellationToken cancellationToken)
		{
			return this.Queryable
				.Where(x => Equals(x.ID, id))
				.Select(selector)
				.FirstOrDefault();
		}

		/// <inheritdoc />
		async Task<bool> ICanGet<TAggregateRoot, TKey>.ExistsAsync(TKey id, CancellationToken cancellationToken)
		{
			return this.Queryable.LongCount(x => Equals(x.ID, id)) > 0;
		}

		private TKey GenerateKey()
		{
			if(typeof(TKey) == typeof(Guid))
			{
				return (TKey)Convert.ChangeType(Guid.NewGuid(), typeof(TKey));
			}

			if(typeof(TKey) == typeof(string))
			{
				return (TKey)Convert.ChangeType(Guid.NewGuid().ToString("N"), typeof(TKey));
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

		/// <inheritdoc />
		public override string ToString()
		{
			return Name;
		}
	}
}
