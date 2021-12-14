namespace Fluxera.Repository.Storage.InMemory
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
		private static readonly object syncRoot = new object();

		private readonly ILogger logger;
		private readonly ConcurrentDictionary<string, TAggregateRoot> store = new ConcurrentDictionary<string, TAggregateRoot>();

		public InMemoryRepository(ILoggerFactory loggerFactory)
		{
			Guard.Against.Null(loggerFactory, nameof(loggerFactory));

			this.logger = loggerFactory.CreateLogger(Name);
		}

		private static string Name => "Fluxera.Repository.InMemoryRepository";

		private IQueryable<TAggregateRoot> Queryable
		{
			get
			{
				lock(syncRoot)
				{
					return this.store.Values.AsQueryable();
				}
			}
		}

		private bool IsDisposed { get; set; }

		/// <inheritdoc />
		async Task ICanAdd<TAggregateRoot>.AddAsync(TAggregateRoot item, CancellationToken cancellationToken = default)
		{
			await Task.Factory.StartNew(() =>
			{
				lock(syncRoot)
				{
					item.ID = Guid.NewGuid().ToString("N");
					this.store.TryAdd(item.ID, item);
				}
			}, cancellationToken).ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task ICanAdd<TAggregateRoot>.AddAsync(IEnumerable<TAggregateRoot> items, CancellationToken cancellationToken = default)
		{
			await Task.Factory.StartNew(() =>
			{
				lock(syncRoot)
				{
					foreach(TAggregateRoot item in items)
					{
						item.ID = Guid.NewGuid().ToString("N");
						this.store.TryAdd(item.ID, item);
					}
				}
			}, cancellationToken).ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task ICanUpdate<TAggregateRoot>.UpdateAsync(TAggregateRoot item, CancellationToken cancellationToken = default)
		{
			await Task.Factory.StartNew(() =>
			{
				lock(syncRoot)
				{
					this.store[item.ID] = item;
				}
			}, cancellationToken).ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task ICanUpdate<TAggregateRoot>.UpdateAsync(IEnumerable<TAggregateRoot> items, CancellationToken cancellationToken = default)
		{
			await Task.Factory.StartNew(() =>
			{
				lock(syncRoot)
				{
					foreach(TAggregateRoot item in items)
					{
						this.store[item.ID] = item;
					}
				}
			}, cancellationToken).ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task ICanRemove<TAggregateRoot>.RemoveAsync(TAggregateRoot item, CancellationToken cancellationToken = default)
		{
			await Task.Factory.StartNew(() =>
			{
				lock(syncRoot)
				{
					this.store.TryRemove(item.ID, out _);
				}
			}, cancellationToken).ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task ICanRemove<TAggregateRoot>.RemoveAsync(string id, CancellationToken cancellationToken = default)
		{
			await Task.Factory.StartNew(() =>
			{
				lock(syncRoot)
				{
					this.store.TryRemove(id, out _);
				}
			}, cancellationToken).ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task ICanRemove<TAggregateRoot>.RemoveAsync(Expression<Func<TAggregateRoot, bool>> predicate, CancellationToken cancellationToken = default)
		{
			await Task.Factory.StartNew(() =>
			{
				lock(syncRoot)
				{
					TAggregateRoot item = this.Queryable.Where(predicate).FirstOrDefault();
					this.store.TryRemove(item.ID, out _);
				}
			}, cancellationToken).ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task ICanRemove<TAggregateRoot>.RemoveAsync(IEnumerable<TAggregateRoot> items, CancellationToken cancellationToken = default)
		{
			await Task.Factory.StartNew(() =>
			{
				lock(syncRoot)
				{
					foreach(TAggregateRoot item in items)
					{
						this.store.TryRemove(item.ID, out _);
					}
				}
			}, cancellationToken).ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task<TAggregateRoot> ICanGet<TAggregateRoot>.GetAsync(string id, CancellationToken cancellationToken = default)
		{
			return await Task.Factory.StartNew(() =>
			{
				lock(syncRoot)
				{
					return this.Queryable.FirstOrDefault(x => x.ID == id);
				}
			}, cancellationToken).ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task<TResult> ICanGet<TAggregateRoot>.GetAsync<TResult>(string id, Expression<Func<TAggregateRoot, TResult>> selector, CancellationToken cancellationToken = default)
		{
			return await Task.Factory.StartNew(() =>
			{
				lock(syncRoot)
				{
					return this.Queryable
						.Where(x => x.ID == id)
						.Select(selector)
						.FirstOrDefault();
				}
			}, cancellationToken).ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task<long> ICanAggregate<TAggregateRoot>.CountAsync(CancellationToken cancellationToken)
		{
			return await Task.Factory.StartNew(() =>
			{
				lock(syncRoot)
				{
					return this.Queryable.LongCount();
				}
			}, cancellationToken).ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task<long> ICanAggregate<TAggregateRoot>.CountAsync(Expression<Func<TAggregateRoot, bool>> predicate, CancellationToken cancellationToken = default)
		{
			return await Task.Factory.StartNew(() =>
			{
				lock(syncRoot)
				{
					return this.Queryable.LongCount(predicate);
				}
			}, cancellationToken).ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task<bool> ICanGet<TAggregateRoot>.ExistsAsync(string id, CancellationToken cancellationToken)
		{
			return await Task.Factory.StartNew(() =>
			{
				lock(syncRoot)
				{
					return this.Queryable.LongCount(x => x.ID == id) > 0;
				}
			}, cancellationToken).ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task<bool> ICanFind<TAggregateRoot>.ExistsAsync(Expression<Func<TAggregateRoot, bool>> predicate, CancellationToken cancellationToken)
		{
			return await Task.Factory.StartNew(() =>
			{
				lock(syncRoot)
				{
					return this.Queryable.LongCount(predicate) > 0;
				}
			}, cancellationToken).ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task<TAggregateRoot> ICanFind<TAggregateRoot>.FindOneAsync(Expression<Func<TAggregateRoot, bool>> predicate, IQueryOptions<TAggregateRoot> queryOptions, CancellationToken cancellationToken = default)
		{
			return await Task.Factory.StartNew(() =>
			{
				lock(syncRoot)
				{
					return this.Queryable
						.ApplyOptions(queryOptions)
						.FirstOrDefault(predicate);
				}
			}, cancellationToken).ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task<TResult> ICanFind<TAggregateRoot>.FindOneAsync<TResult>(Expression<Func<TAggregateRoot, bool>> predicate, Expression<Func<TAggregateRoot, TResult>> selector, IQueryOptions<TAggregateRoot> queryOptions, CancellationToken cancellationToken = default)
		{
			return await Task.Factory.StartNew(() =>
			{
				lock(syncRoot)
				{
					return this.Queryable
						.ApplyOptions(queryOptions)
						.Where(predicate)
						.Select(selector)
						.FirstOrDefault();
				}
			}, cancellationToken).ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task<IReadOnlyCollection<TAggregateRoot>> ICanFind<TAggregateRoot>.FindManyAsync(Expression<Func<TAggregateRoot, bool>> predicate, IQueryOptions<TAggregateRoot> queryOptions, CancellationToken cancellationToken = default)
		{
			return await Task.Factory.StartNew(() =>
			{
				lock(syncRoot)
				{
					return this.Queryable
						.ApplyOptions(queryOptions)
						.Where(predicate)
						.AsReadOnly();
				}
			}, cancellationToken).ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task<IReadOnlyCollection<TResult>> ICanFind<TAggregateRoot>.FindManyAsync<TResult>(Expression<Func<TAggregateRoot, bool>> predicate, Expression<Func<TAggregateRoot, TResult>> selector, IQueryOptions<TAggregateRoot> queryOptions, CancellationToken cancellationToken = default)
		{
			return await Task.Factory.StartNew(() =>
			{
				lock(syncRoot)
				{
					return this.Queryable
						.ApplyOptions(queryOptions)
						.Where(predicate)
						.Select(selector)
						.AsReadOnly();
				}
			}, cancellationToken).ConfigureAwait(false);
		}

		/// <inheritdoc />
		void IDisposable.Dispose()
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
		public override string ToString()
		{
			return Name;
		}
	}
}
