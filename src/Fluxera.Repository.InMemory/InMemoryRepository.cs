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

	public sealed class InMemoryRepository<TAggregateRoot> : IRepository<TAggregateRoot>
		where TAggregateRoot : AggregateRoot<TAggregateRoot>
	{
		private readonly ILogger logger;
		private readonly ConcurrentDictionary<string, TAggregateRoot> store = new ConcurrentDictionary<string, TAggregateRoot>();

		public InMemoryRepository(ILoggerFactory loggerFactory)
		{
			Guard.Against.Null(loggerFactory, nameof(loggerFactory));

			this.logger = loggerFactory.CreateLogger(Name);
		}

		private static string Name => "Fluxera.Repository.InMemoryRepository";

		private IQueryable<TAggregateRoot> Queryable => this.store.Values.AsQueryable();

		private bool IsDisposed { get; set; }

		/// <inheritdoc />
		async Task ICanAdd<TAggregateRoot>.AddAsync(TAggregateRoot item, CancellationToken cancellationToken)
		{
			item.ID = Guid.NewGuid().ToString("N");
			this.store.TryAdd(item.ID, item);
		}

		/// <inheritdoc />
		async Task ICanAdd<TAggregateRoot>.AddAsync(IEnumerable<TAggregateRoot> items, CancellationToken cancellationToken)
		{
			foreach(TAggregateRoot item in items)
			{
				item.ID = Guid.NewGuid().ToString("N");
				this.store.TryAdd(item.ID, item);
			}
		}

		/// <inheritdoc />
		async Task ICanUpdate<TAggregateRoot>.UpdateAsync(TAggregateRoot item, CancellationToken cancellationToken)
		{
			this.store[item.ID] = item;
		}

		/// <inheritdoc />
		async Task ICanUpdate<TAggregateRoot>.UpdateAsync(IEnumerable<TAggregateRoot> items, CancellationToken cancellationToken)
		{
			foreach(TAggregateRoot item in items)
			{
				this.store[item.ID] = item;
			}
		}

		/// <inheritdoc />
		async Task ICanRemove<TAggregateRoot>.RemoveAsync(TAggregateRoot item, CancellationToken cancellationToken)
		{
			this.store.TryRemove(item.ID, out _);
		}

		/// <inheritdoc />
		async Task ICanRemove<TAggregateRoot>.RemoveAsync(string id, CancellationToken cancellationToken)
		{
			this.store.TryRemove(id, out TAggregateRoot item);
		}

		/// <inheritdoc />
		async Task ICanRemove<TAggregateRoot>.RemoveAsync(Expression<Func<TAggregateRoot, bool>> predicate, CancellationToken cancellationToken)
		{
			IQueryable<TAggregateRoot> items = this.Queryable.Where(predicate);
			foreach(TAggregateRoot? item in items)
			{
				this.store.TryRemove(item.ID, out _);
			}
		}

		/// <inheritdoc />
		async Task ICanRemove<TAggregateRoot>.RemoveAsync(IEnumerable<TAggregateRoot> items, CancellationToken cancellationToken)
		{
			foreach(TAggregateRoot item in items)
			{
				this.store.TryRemove(item.ID, out _);
			}
		}

		/// <inheritdoc />
		async Task<TAggregateRoot> ICanGet<TAggregateRoot>.GetAsync(string id, CancellationToken cancellationToken)
		{
			return this.Queryable.FirstOrDefault(x => x.ID == id)!;
		}

		/// <inheritdoc />
		async Task<TResult> ICanGet<TAggregateRoot>.GetAsync<TResult>(string id, Expression<Func<TAggregateRoot, TResult>> selector, CancellationToken cancellationToken)
		{
			return this.Queryable
				.Where(x => x.ID == id)
				.Select(selector)
				.FirstOrDefault();
		}

		/// <inheritdoc />
		async Task<long> ICanAggregate<TAggregateRoot>.CountAsync(CancellationToken cancellationToken)
		{
			return this.Queryable.LongCount();
		}

		/// <inheritdoc />
		async Task<long> ICanAggregate<TAggregateRoot>.CountAsync(Expression<Func<TAggregateRoot, bool>> predicate, CancellationToken cancellationToken)
		{
			return this.Queryable.LongCount(predicate);
		}

		/// <inheritdoc />
		async Task<bool> ICanGet<TAggregateRoot>.ExistsAsync(string id, CancellationToken cancellationToken)
		{
			return this.Queryable.LongCount(x => x.ID == id) > 0;
		}

		/// <inheritdoc />
		async Task<bool> ICanFind<TAggregateRoot>.ExistsAsync(Expression<Func<TAggregateRoot, bool>> predicate, CancellationToken cancellationToken)
		{
			return this.Queryable.LongCount(predicate) > 0;
		}

		/// <inheritdoc />
		async Task<TAggregateRoot> ICanFind<TAggregateRoot>.FindOneAsync(Expression<Func<TAggregateRoot, bool>> predicate, IQueryOptions<TAggregateRoot>? queryOptions, CancellationToken cancellationToken)
		{
			return this.Queryable
				.ApplyOptions(queryOptions)
				.FirstOrDefault(predicate)!;
		}

		/// <inheritdoc />
		async Task<TResult> ICanFind<TAggregateRoot>.FindOneAsync<TResult>(Expression<Func<TAggregateRoot, bool>> predicate, Expression<Func<TAggregateRoot, TResult>> selector, IQueryOptions<TAggregateRoot>? queryOptions, CancellationToken cancellationToken)
		{
			return this.Queryable
				.ApplyOptions(queryOptions)
				.Where(predicate)
				.Select(selector)
				.FirstOrDefault();
		}

		/// <inheritdoc />
		async Task<IReadOnlyCollection<TAggregateRoot>> ICanFind<TAggregateRoot>.FindManyAsync(Expression<Func<TAggregateRoot, bool>> predicate, IQueryOptions<TAggregateRoot>? queryOptions, CancellationToken cancellationToken)
		{
			return this.Queryable
				.ApplyOptions(queryOptions)
				.Where(predicate)
				.AsReadOnly();
		}

		/// <inheritdoc />
		async Task<IReadOnlyCollection<TResult>> ICanFind<TAggregateRoot>.FindManyAsync<TResult>(Expression<Func<TAggregateRoot, bool>> predicate, Expression<Func<TAggregateRoot, TResult>> selector, IQueryOptions<TAggregateRoot>? queryOptions, CancellationToken cancellationToken)
		{
			return this.Queryable
				.ApplyOptions(queryOptions)
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
		bool IReadOnlyRepository<TAggregateRoot>.IsDisposed => this.IsDisposed;

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
		public override string ToString()
		{
			return Name;
		}
	}
}
