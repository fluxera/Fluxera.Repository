namespace Fluxera.Repository.Decorators
{
	using System;
	using System.Collections.Generic;
	using System.Linq.Expressions;
	using System.Threading;
	using System.Threading.Tasks;
	using Fluxera.Entity;
	using Fluxera.Guards;
	using Fluxera.Repository.Query;
	using Fluxera.Repository.Specifications;
	using Fluxera.Repository.Traits;

	public sealed class GuardRepositoryDecorator<TAggregateRoot, TKey> : IRepository<TAggregateRoot, TKey>
		where TAggregateRoot : AggregateRoot<TAggregateRoot, TKey>
	{
		private readonly IRepository<TAggregateRoot, TKey> innerRepository;

		public GuardRepositoryDecorator(IRepository<TAggregateRoot, TKey> innerRepository)
		{
			Guard.Against.Null(innerRepository, nameof(innerRepository));

			this.innerRepository = innerRepository;
		}

		/// <inheritdoc />
		async Task ICanAdd<TAggregateRoot, TKey>.AddAsync(TAggregateRoot item, CancellationToken cancellationToken)
		{
			Guard.Against.Disposed(this);
			Guard.Against.Null(item, nameof(item));
			Guard.Against.NotTransient<TAggregateRoot, TKey>(item, nameof(item), "A non-transient item can not be added.");

			await this.innerRepository.AddAsync(item, cancellationToken).ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task ICanAdd<TAggregateRoot, TKey>.AddAsync(IEnumerable<TAggregateRoot> items, CancellationToken cancellationToken)
		{
			Guard.Against.Disposed(this);
			Guard.Against.Null(items, nameof(items));
			Guard.Against.NotTransient<TAggregateRoot, TKey>(items, nameof(items), "A non-transient item can not be added.");

			await this.innerRepository.AddAsync(items, cancellationToken).ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task ICanUpdate<TAggregateRoot, TKey>.UpdateAsync(TAggregateRoot item, CancellationToken cancellationToken)
		{
			Guard.Against.Disposed(this);
			Guard.Against.Null(item, nameof(item));
			Guard.Against.Transient<TAggregateRoot, TKey>(item, nameof(item), "A transient item can not be updated. Add the item first.");

			await this.innerRepository.UpdateAsync(item, cancellationToken).ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task ICanUpdate<TAggregateRoot, TKey>.UpdateAsync(IEnumerable<TAggregateRoot> items, CancellationToken cancellationToken)
		{
			Guard.Against.Disposed(this);
			Guard.Against.Null(items, nameof(items));
			Guard.Against.Transient<TAggregateRoot, TKey>(items, nameof(items), "A transient item can not be updated. Add the item first.");

			await this.innerRepository.UpdateAsync(items, cancellationToken).ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task ICanRemove<TAggregateRoot, TKey>.RemoveAsync(TAggregateRoot item, CancellationToken cancellationToken)
		{
			Guard.Against.Disposed(this);
			Guard.Against.Null(item, nameof(item));
			Guard.Against.Transient<TAggregateRoot, TKey>(item, nameof(item), "A transient item can not be removed. Add the item first.");

			await this.innerRepository.RemoveAsync(item, cancellationToken).ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task ICanRemove<TAggregateRoot, TKey>.RemoveAsync(Expression<Func<TAggregateRoot, bool>> predicate, CancellationToken cancellationToken)
		{
			Guard.Against.Disposed(this);
			Guard.Against.Null(predicate, nameof(predicate));

			await this.innerRepository.RemoveAsync(predicate, cancellationToken).ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task ICanRemove<TAggregateRoot, TKey>.RemoveAsync(ISpecification<TAggregateRoot> specification, CancellationToken cancellationToken)
		{
			Guard.Against.Disposed(this);
			Guard.Against.Null(specification, nameof(specification));

			await this.innerRepository.RemoveAsync(specification, cancellationToken);
		}

		/// <inheritdoc />
		async Task ICanRemove<TAggregateRoot, TKey>.RemoveAsync(IEnumerable<TAggregateRoot> items, CancellationToken cancellationToken)
		{
			Guard.Against.Disposed(this);
			Guard.Against.Null(items, nameof(items));
			Guard.Against.Transient<TAggregateRoot, TKey>(items, nameof(items), "A transient item can not be removed. Add the item first.");

			await this.innerRepository.RemoveAsync(items, cancellationToken).ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task<TAggregateRoot> ICanFind<TAggregateRoot, TKey>.FindOneAsync(Expression<Func<TAggregateRoot, bool>> predicate, IQueryOptions<TAggregateRoot>? queryOptions, CancellationToken cancellationToken)
		{
			Guard.Against.Disposed(this);
			Guard.Against.Null(predicate, nameof(predicate));

			queryOptions ??= QueryOptions<TAggregateRoot>.Empty();
			return await this.innerRepository.FindOneAsync(predicate, queryOptions, cancellationToken).ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task<TAggregateRoot> ICanFind<TAggregateRoot, TKey>.FindOneAsync(ISpecification<TAggregateRoot> specification, IQueryOptions<TAggregateRoot>? queryOptions, CancellationToken cancellationToken)
		{
			Guard.Against.Disposed(this);
			Guard.Against.Null(specification, nameof(specification));

			queryOptions ??= QueryOptions<TAggregateRoot>.Empty();
			return await this.innerRepository.FindOneAsync(specification, queryOptions, cancellationToken);
		}

		/// <inheritdoc />
		async Task<TResult> ICanFind<TAggregateRoot, TKey>.FindOneAsync<TResult>(Expression<Func<TAggregateRoot, bool>> predicate, Expression<Func<TAggregateRoot, TResult>> selector, IQueryOptions<TAggregateRoot>? queryOptions, CancellationToken cancellationToken)
		{
			Guard.Against.Disposed(this);
			Guard.Against.Null(predicate, nameof(predicate));
			Guard.Against.Null(selector, nameof(selector));

			queryOptions ??= QueryOptions<TAggregateRoot>.Empty();
			return await this.innerRepository.FindOneAsync(predicate, selector, queryOptions, cancellationToken).ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task<TResult> ICanFind<TAggregateRoot, TKey>.FindOneAsync<TResult>(ISpecification<TAggregateRoot> specification, Expression<Func<TAggregateRoot, TResult>> selector, IQueryOptions<TAggregateRoot>? queryOptions, CancellationToken cancellationToken)
		{
			Guard.Against.Disposed(this);
			Guard.Against.Null(specification, nameof(specification));
			Guard.Against.Null(selector, nameof(selector));

			queryOptions ??= QueryOptions<TAggregateRoot>.Empty();
			return await this.innerRepository.FindOneAsync(specification, selector, queryOptions, cancellationToken);
		}

		/// <inheritdoc />
		async Task<bool> ICanFind<TAggregateRoot, TKey>.ExistsAsync(Expression<Func<TAggregateRoot, bool>> predicate, CancellationToken cancellationToken)
		{
			Guard.Against.Disposed(this);
			Guard.Against.Null(predicate, nameof(predicate));

			return await this.innerRepository.ExistsAsync(predicate, cancellationToken).ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task<bool> ICanFind<TAggregateRoot, TKey>.ExistsAsync(ISpecification<TAggregateRoot> specification, CancellationToken cancellationToken)
		{
			Guard.Against.Disposed(this);
			Guard.Against.Null(specification, nameof(specification));

			return await this.innerRepository.ExistsAsync(specification, cancellationToken);
		}

		/// <inheritdoc />
		async Task<IReadOnlyCollection<TAggregateRoot>> ICanFind<TAggregateRoot, TKey>.FindManyAsync(Expression<Func<TAggregateRoot, bool>> predicate, IQueryOptions<TAggregateRoot>? queryOptions, CancellationToken cancellationToken)
		{
			Guard.Against.Disposed(this);
			Guard.Against.Null(predicate, nameof(predicate));

			queryOptions ??= QueryOptions<TAggregateRoot>.Empty();
			return await this.innerRepository.FindManyAsync(predicate, queryOptions, cancellationToken).ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task<IReadOnlyCollection<TAggregateRoot>> ICanFind<TAggregateRoot, TKey>.FindManyAsync(ISpecification<TAggregateRoot> specification, IQueryOptions<TAggregateRoot>? queryOptions, CancellationToken cancellationToken)
		{
			Guard.Against.Disposed(this);
			Guard.Against.Null(specification, nameof(specification));

			queryOptions ??= QueryOptions<TAggregateRoot>.Empty();
			return await this.innerRepository.FindManyAsync(specification, queryOptions, cancellationToken);
		}

		/// <inheritdoc />
		async Task<IReadOnlyCollection<TResult>> ICanFind<TAggregateRoot, TKey>.FindManyAsync<TResult>(Expression<Func<TAggregateRoot, bool>> predicate, Expression<Func<TAggregateRoot, TResult>> selector, IQueryOptions<TAggregateRoot>? queryOptions, CancellationToken cancellationToken)
		{
			Guard.Against.Disposed(this);
			Guard.Against.Null(predicate, nameof(predicate));
			Guard.Against.Null(selector, nameof(selector));

			queryOptions ??= QueryOptions<TAggregateRoot>.Empty();
			return await this.innerRepository.FindManyAsync(predicate, selector, queryOptions, cancellationToken).ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task<IReadOnlyCollection<TResult>> ICanFind<TAggregateRoot, TKey>.FindManyAsync<TResult>(ISpecification<TAggregateRoot> specification, Expression<Func<TAggregateRoot, TResult>> selector, IQueryOptions<TAggregateRoot>? queryOptions, CancellationToken cancellationToken)
		{
			return await this.innerRepository.FindManyAsync(specification, selector, queryOptions, cancellationToken);
		}

		/// <inheritdoc />
		async Task<long> ICanAggregate<TAggregateRoot, TKey>.CountAsync(CancellationToken cancellationToken)
		{
			Guard.Against.Disposed(this);

			return await this.innerRepository.CountAsync(cancellationToken).ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task<long> ICanAggregate<TAggregateRoot, TKey>.CountAsync(Expression<Func<TAggregateRoot, bool>> predicate, CancellationToken cancellationToken)
		{
			Guard.Against.Disposed(this);
			Guard.Against.Null(predicate, nameof(predicate));

			return await this.innerRepository.CountAsync(predicate, cancellationToken).ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task<long> ICanAggregate<TAggregateRoot, TKey>.CountAsync(ISpecification<TAggregateRoot> specification, CancellationToken cancellationToken)
		{
			Guard.Against.Disposed(this);
			Guard.Against.Null(specification, nameof(specification));

			return await this.innerRepository.CountAsync(specification, cancellationToken);
		}

		/// <inheritdoc />
		void IDisposable.Dispose()
		{
			if(!this.innerRepository.IsDisposed)
			{
				this.innerRepository.Dispose();
			}
		}

		/// <inheritdoc />
		bool IDisposableRepository.IsDisposed => this.innerRepository.IsDisposed;

		/// <inheritdoc />
		async ValueTask IAsyncDisposable.DisposeAsync()
		{
			if(!this.innerRepository.IsDisposed)
			{
				await this.innerRepository.DisposeAsync();
			}
		}

		/// <inheritdoc />
		async Task ICanRemove<TAggregateRoot, TKey>.RemoveAsync(TKey id, CancellationToken cancellationToken)
		{
			Guard.Against.Disposed(this);
			Guard.Against.Default(id, nameof(id));

			await this.innerRepository.RemoveAsync(id, cancellationToken).ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task<TAggregateRoot> ICanGet<TAggregateRoot, TKey>.GetAsync(TKey id, CancellationToken cancellationToken)
		{
			Guard.Against.Disposed(this);
			Guard.Against.Default(id, nameof(id));

			return await this.innerRepository.GetAsync(id, cancellationToken).ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task<TResult> ICanGet<TAggregateRoot, TKey>.GetAsync<TResult>(TKey id, Expression<Func<TAggregateRoot, TResult>> selector, CancellationToken cancellationToken)
		{
			Guard.Against.Disposed(this);
			Guard.Against.Default(id, nameof(id));
			Guard.Against.Null(selector, nameof(selector));

			return await this.innerRepository.GetAsync(id, selector, cancellationToken).ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task<bool> ICanGet<TAggregateRoot, TKey>.ExistsAsync(TKey id, CancellationToken cancellationToken)
		{
			Guard.Against.Disposed(this);
			Guard.Against.Default(id, nameof(id));

			return await this.innerRepository.ExistsAsync(id, cancellationToken).ConfigureAwait(false);
		}

		/// <inheritdoc />
		public override string ToString()
		{
			return this.innerRepository.ToString();
		}
	}
}
