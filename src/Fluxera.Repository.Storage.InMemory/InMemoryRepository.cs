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
	using Fluxera.Repository.Query;
	using Fluxera.Utilities.Extensions;
	using Microsoft.Extensions.Logging;

	public sealed class InMemoryRepository<TAggregateRoot> : RepositoryBase<TAggregateRoot>
		where TAggregateRoot : AggregateRoot<TAggregateRoot>
	{
		private static readonly object syncRoot = new object();

		private readonly ConcurrentDictionary<string, TAggregateRoot> store = new ConcurrentDictionary<string, TAggregateRoot>();

		/// <inheritdoc />
		public InMemoryRepository(ILoggerFactory loggerFactory)
			: base(loggerFactory)
		{
		}

		protected override string Name => "Fluxera.Repository.InMemoryRepository";

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

		/// <inheritdoc />
		protected override async Task OnAddAsync(TAggregateRoot item, CancellationToken cancellationToken = default)
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
		protected override async Task OnAddAsync(IEnumerable<TAggregateRoot> items, CancellationToken cancellationToken = default)
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
		protected override async Task OnUpdateAsync(TAggregateRoot item, CancellationToken cancellationToken = default)
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
		protected override async Task OnUpdateAsync(IEnumerable<TAggregateRoot> items, CancellationToken cancellationToken = default)
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
		protected override async Task OnRemoveAsync(TAggregateRoot item, CancellationToken cancellationToken = default)
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
		protected override async Task OnRemoveAsync(string id, CancellationToken cancellationToken = default)
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
		protected override async Task OnRemoveAsync(Expression<Func<TAggregateRoot, bool>> predicate, CancellationToken cancellationToken = default)
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
		protected override async Task OnRemoveAsync(IEnumerable<TAggregateRoot> items, CancellationToken cancellationToken = default)
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
		protected override async Task<TAggregateRoot> OnGetAsync(string id, CancellationToken cancellationToken = default)
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
		protected override async Task<TResult> OnGetAsync<TResult>(string id, Expression<Func<TAggregateRoot, TResult>> selector, CancellationToken cancellationToken = default)
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
		protected override async Task<long> OnCountAsync(Expression<Func<TAggregateRoot, bool>> predicate, CancellationToken cancellationToken = default)
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
		protected override async Task<TAggregateRoot> OnFindOneAsync(Expression<Func<TAggregateRoot, bool>> predicate, IQueryOptions<TAggregateRoot> queryOptions, CancellationToken cancellationToken = default)
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
		protected override async Task<TResult> OnFindOneAsync<TResult>(Expression<Func<TAggregateRoot, bool>> predicate, Expression<Func<TAggregateRoot, TResult>> selector, IQueryOptions<TAggregateRoot> queryOptions, CancellationToken cancellationToken = default)
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
		protected override async Task<IReadOnlyCollection<TAggregateRoot>> OnFindManyAsync(Expression<Func<TAggregateRoot, bool>> predicate, IQueryOptions<TAggregateRoot> queryOptions, CancellationToken cancellationToken = default)
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
		protected override async Task<IReadOnlyCollection<TResult>> OnFindManyAsync<TResult>(Expression<Func<TAggregateRoot, bool>> predicate, Expression<Func<TAggregateRoot, TResult>> selector, IQueryOptions<TAggregateRoot> queryOptions, CancellationToken cancellationToken = default)
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
	}
}
