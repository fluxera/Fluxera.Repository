namespace Fluxera.Repository.Storage
{
	using System;
	using System.Collections.Generic;
	using System.Linq.Expressions;
	using System.Threading;
	using System.Threading.Tasks;
	using Fluxera.Entity;
	using Fluxera.Repository.Query;
	using Fluxera.Repository.Traits;
	using JetBrains.Annotations;
	using Microsoft.Extensions.Logging;

	/// <summary>
	///     An abstract base class for storage provider repository implementations.
	/// </summary>
	/// <typeparam name="TAggregateRoot">The aggregate root type.</typeparam>
	[PublicAPI]
	public abstract class RepositoryBase<TAggregateRoot> : IRepository<TAggregateRoot>
		where TAggregateRoot : AggregateRoot<TAggregateRoot>
	{
		protected RepositoryBase(ILoggerFactory loggerFactory)
		{
			this.Logger = loggerFactory.CreateLogger(this.ToString());
		}

		protected ILogger Logger { get; }

		protected abstract string Name { get; }

		/// <inheritdoc />
		async Task ICanAdd<TAggregateRoot>.AddAsync(TAggregateRoot item, CancellationToken cancellationToken)
		{
			await this.OnAddAsync(item, cancellationToken).ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task ICanAdd<TAggregateRoot>.AddAsync(IEnumerable<TAggregateRoot> items, CancellationToken cancellationToken)
		{
			await this.OnAddAsync(items, cancellationToken).ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task ICanUpdate<TAggregateRoot>.UpdateAsync(TAggregateRoot item, CancellationToken cancellationToken)
		{
			await this.OnUpdateAsync(item, cancellationToken).ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task ICanUpdate<TAggregateRoot>.UpdateAsync(IEnumerable<TAggregateRoot> items, CancellationToken cancellationToken)
		{
			await this.OnUpdateAsync(items, cancellationToken).ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task ICanRemove<TAggregateRoot>.RemoveAsync(TAggregateRoot item, CancellationToken cancellationToken)
		{
			await this.OnRemoveAsync(item, cancellationToken).ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task ICanRemove<TAggregateRoot>.RemoveAsync(string id, CancellationToken cancellationToken)
		{
			await this.OnRemoveAsync(id, cancellationToken).ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task ICanRemove<TAggregateRoot>.RemoveAsync(Expression<Func<TAggregateRoot, bool>> predicate, CancellationToken cancellationToken)
		{
			await this.OnRemoveAsync(predicate, cancellationToken).ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task ICanRemove<TAggregateRoot>.RemoveAsync(IEnumerable<TAggregateRoot> items, CancellationToken cancellationToken)
		{
			await this.OnRemoveAsync(items, cancellationToken).ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task<TAggregateRoot> ICanGet<TAggregateRoot>.GetAsync(string id, CancellationToken cancellationToken)
		{
			return await this.OnGetAsync(id, cancellationToken).ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task<TResult> ICanGet<TAggregateRoot>.GetAsync<TResult>(string id, Expression<Func<TAggregateRoot, TResult>> selector, CancellationToken cancellationToken)
		{
			return await this.OnGetAsync(id, selector, cancellationToken).ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task<bool> ICanGet<TAggregateRoot>.ExistsAsync(string id, CancellationToken cancellationToken)
		{
			TAggregateRoot result = await this.OnGetAsync(id, cancellationToken).ConfigureAwait(false);
			return result is not null;
		}

		/// <inheritdoc />
		async Task<TAggregateRoot> ICanFind<TAggregateRoot>.FindOneAsync(Expression<Func<TAggregateRoot, bool>> predicate, IQueryOptions<TAggregateRoot>? queryOptions, CancellationToken cancellationToken)
		{
			return await this.OnFindOneAsync(predicate, queryOptions, cancellationToken).ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task<TResult> ICanFind<TAggregateRoot>.FindOneAsync<TResult>(Expression<Func<TAggregateRoot, bool>> predicate, Expression<Func<TAggregateRoot, TResult>> selector, IQueryOptions<TAggregateRoot>? queryOptions, CancellationToken cancellationToken)
		{
			return await this.OnFindOneAsync(predicate, selector, queryOptions, cancellationToken).ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task<bool> ICanFind<TAggregateRoot>.ExistsAsync(Expression<Func<TAggregateRoot, bool>> predicate, CancellationToken cancellationToken)
		{
			TAggregateRoot result = await this.OnFindOneAsync(predicate, null, cancellationToken).ConfigureAwait(false);
			return result is not null;
		}

		/// <inheritdoc />
		async Task<IReadOnlyCollection<TAggregateRoot>> ICanFind<TAggregateRoot>.FindManyAsync(Expression<Func<TAggregateRoot, bool>> predicate, IQueryOptions<TAggregateRoot>? queryOptions, CancellationToken cancellationToken)
		{
			return await this.OnFindManyAsync(predicate, queryOptions, cancellationToken).ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task<IReadOnlyCollection<TResult>> ICanFind<TAggregateRoot>.FindManyAsync<TResult>(Expression<Func<TAggregateRoot, bool>> predicate, Expression<Func<TAggregateRoot, TResult>> selector, IQueryOptions<TAggregateRoot>? queryOptions, CancellationToken cancellationToken)
		{
			return await this.OnFindManyAsync(predicate, selector, queryOptions, cancellationToken).ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task<long> ICanAggregate<TAggregateRoot>.CountAsync(CancellationToken cancellationToken)
		{
			return await this.OnCountAsync(x => true, cancellationToken).ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task<long> ICanAggregate<TAggregateRoot>.CountAsync(Expression<Func<TAggregateRoot, bool>> predicate, CancellationToken cancellationToken)
		{
			return await this.OnCountAsync(predicate, cancellationToken).ConfigureAwait(false);
		}

		/// <inheritdoc />
		void IDisposable.Dispose()
		{
			if(this.IsDisposed)
			{
				return;
			}

			// Dispose of unmanaged resources.
			this.Dispose(true);

			// Suppress finalization.
			GC.SuppressFinalize(this);
		}

		/// <inheritdoc />
		public bool IsDisposed { get; private set; }

		public sealed override string ToString()
		{
			return this.Name;
		}

		protected abstract Task OnAddAsync(TAggregateRoot item, CancellationToken cancellationToken = default);

		protected abstract Task OnAddAsync(IEnumerable<TAggregateRoot> items, CancellationToken cancellationToken = default);

		protected abstract Task OnUpdateAsync(TAggregateRoot item, CancellationToken cancellationToken = default);

		protected abstract Task OnUpdateAsync(IEnumerable<TAggregateRoot> items, CancellationToken cancellationToken = default);

		protected abstract Task OnRemoveAsync(TAggregateRoot item, CancellationToken cancellationToken = default);

		protected abstract Task OnRemoveAsync(string id, CancellationToken cancellationToken = default);

		protected abstract Task OnRemoveAsync(Expression<Func<TAggregateRoot, bool>> predicate, CancellationToken cancellationToken = default);

		protected abstract Task OnRemoveAsync(IEnumerable<TAggregateRoot> items, CancellationToken cancellationToken = default);

		protected abstract Task<TAggregateRoot> OnGetAsync(string id, CancellationToken cancellationToken = default);

		protected abstract Task<TResult> OnGetAsync<TResult>(string id, Expression<Func<TAggregateRoot, TResult>> selector, CancellationToken cancellationToken = default);

		protected abstract Task<long> OnCountAsync(Expression<Func<TAggregateRoot, bool>> predicate, CancellationToken cancellationToken = default);

		protected abstract Task<TAggregateRoot> OnFindOneAsync(Expression<Func<TAggregateRoot, bool>> predicate, IQueryOptions<TAggregateRoot>? queryOptions, CancellationToken cancellationToken = default);

		protected abstract Task<TResult> OnFindOneAsync<TResult>(Expression<Func<TAggregateRoot, bool>> predicate, Expression<Func<TAggregateRoot, TResult>> selector, IQueryOptions<TAggregateRoot>? queryOptions, CancellationToken cancellationToken = default);

		protected abstract Task<IReadOnlyCollection<TAggregateRoot>> OnFindManyAsync(Expression<Func<TAggregateRoot, bool>> predicate, IQueryOptions<TAggregateRoot>? queryOptions, CancellationToken cancellationToken = default);

		protected abstract Task<IReadOnlyCollection<TResult>> OnFindManyAsync<TResult>(Expression<Func<TAggregateRoot, bool>> predicate, Expression<Func<TAggregateRoot, TResult>> selector, IQueryOptions<TAggregateRoot>? queryOptions, CancellationToken cancellationToken = default);

		protected virtual void OnDispose()
		{
		}

		private void Dispose(bool disposing)
		{
			if(disposing)
			{
				// Free any other managed objects here.
				this.OnDispose();
			}

			this.IsDisposed = true;
		}
	}
}
