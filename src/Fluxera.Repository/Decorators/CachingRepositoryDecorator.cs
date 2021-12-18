namespace Fluxera.Repository.Decorators
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Linq.Expressions;
	using System.Threading;
	using System.Threading.Tasks;
	using Fluxera.Entity;
	using Fluxera.Guards;
	using Fluxera.Repository.Caching;
	using Fluxera.Repository.Query;
	using Fluxera.Repository.Specifications;
	using Fluxera.Repository.Traits;
	using Fluxera.Utilities.Extensions;

	public sealed class CachingRepositoryDecorator<TAggregateRoot, TKey> : IRepository<TAggregateRoot, TKey>
		where TAggregateRoot : AggregateRoot<TAggregateRoot, TKey>
	{
		private readonly ICachingStrategy<TAggregateRoot, TKey> cachingStrategy;
		private readonly IRepository<TAggregateRoot, TKey> innerRepository;

		public CachingRepositoryDecorator(IRepository<TAggregateRoot, TKey> innerRepository, ICachingStrategyFactory cachingStrategyFactory)
		{
			Guard.Against.Null(innerRepository, nameof(innerRepository));

			this.innerRepository = innerRepository;

			this.cachingStrategy = cachingStrategyFactory.CreateStrategy<TAggregateRoot, TKey>();
		}

		/// <inheritdoc />
		bool IDisposableRepository.IsDisposed => this.innerRepository.IsDisposed;

		/// <inheritdoc />
		async Task ICanAdd<TAggregateRoot, TKey>.AddAsync(TAggregateRoot item, CancellationToken cancellationToken)
		{
			await this.innerRepository.AddAsync(item, cancellationToken).ConfigureAwait(false);

			await this.cachingStrategy.AddAsync(item).ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task ICanAdd<TAggregateRoot, TKey>.AddAsync(IEnumerable<TAggregateRoot> items, CancellationToken cancellationToken)
		{
			await this.innerRepository.AddAsync(items, cancellationToken).ConfigureAwait(false);

			await this.cachingStrategy.AddAsync(items).ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task ICanUpdate<TAggregateRoot, TKey>.UpdateAsync(TAggregateRoot item, CancellationToken cancellationToken)
		{
			await this.innerRepository.UpdateAsync(item, cancellationToken).ConfigureAwait(false);

			await this.cachingStrategy.UpdateAsync(item).ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task ICanUpdate<TAggregateRoot, TKey>.UpdateAsync(IEnumerable<TAggregateRoot> items, CancellationToken cancellationToken)
		{
			await this.innerRepository.UpdateAsync(items, cancellationToken).ConfigureAwait(false);

			await this.cachingStrategy.UpdateAsync(items).ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task ICanRemove<TAggregateRoot, TKey>.RemoveAsync(TAggregateRoot item, CancellationToken cancellationToken)
		{
			TKey id = item.ID;

			await this.innerRepository.RemoveAsync(item, cancellationToken).ConfigureAwait(false);

			await this.cachingStrategy.RemoveAsync(id).ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task ICanRemove<TAggregateRoot, TKey>.RemoveAsync(TKey id, CancellationToken cancellationToken)
		{
			await this.innerRepository.RemoveAsync(id, cancellationToken).ConfigureAwait(false);

			await this.cachingStrategy.RemoveAsync(id).ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task ICanRemove<TAggregateRoot, TKey>.RemoveAsync(Expression<Func<TAggregateRoot, bool>> predicate, CancellationToken cancellationToken)
		{
			IReadOnlyCollection<TAggregateRoot> items = await this.innerRepository.FindManyAsync(predicate, cancellationToken: cancellationToken).ConfigureAwait(false);
			IReadOnlyCollection<TKey> ids = items.Select(x => x.ID).AsReadOnly();

			await this.innerRepository.RemoveAsync(predicate, cancellationToken).ConfigureAwait(false);

			await this.cachingStrategy.RemoveAsync(ids).ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task ICanRemove<TAggregateRoot, TKey>.RemoveAsync(ISpecification<TAggregateRoot> specification, CancellationToken cancellationToken)
		{
			IReadOnlyCollection<TAggregateRoot> items = await this.innerRepository.FindManyAsync(specification, cancellationToken: cancellationToken).ConfigureAwait(false);
			IReadOnlyCollection<TKey> ids = items.Select(x => x.ID).AsReadOnly();

			await this.innerRepository.RemoveAsync(specification, cancellationToken).ConfigureAwait(false);

			await this.cachingStrategy.RemoveAsync(ids).ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task ICanRemove<TAggregateRoot, TKey>.RemoveAsync(IEnumerable<TAggregateRoot> items, CancellationToken cancellationToken)
		{
			IReadOnlyCollection<TKey> ids = items.Select(x => x.ID).AsReadOnly();

			await this.innerRepository.RemoveAsync(items, cancellationToken).ConfigureAwait(false);

			await this.cachingStrategy.RemoveAsync(ids).ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task<TAggregateRoot> ICanGet<TAggregateRoot, TKey>.GetAsync(TKey id, CancellationToken cancellationToken)
		{
			return await this.cachingStrategy.GetAsync(id,
					async () => await this.innerRepository.GetAsync(id, cancellationToken)
						.ConfigureAwait(false))
				.ConfigureAwait(true);
		}

		/// <inheritdoc />
		async Task<TResult> ICanGet<TAggregateRoot, TKey>.GetAsync<TResult>(TKey id, Expression<Func<TAggregateRoot, TResult>> selector, CancellationToken cancellationToken)
		{
			return await this.cachingStrategy.GetAsync(id, selector,
					async () => await this.innerRepository.GetAsync(id, selector, cancellationToken)
						.ConfigureAwait(false))
				.ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task<bool> ICanGet<TAggregateRoot, TKey>.ExistsAsync(TKey id, CancellationToken cancellationToken)
		{
			return await this.cachingStrategy.ExistsAsync(id,
					async () => await this.innerRepository.ExistsAsync(id, cancellationToken)
						.ConfigureAwait(false))
				.ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task<TAggregateRoot> ICanFind<TAggregateRoot, TKey>.FindOneAsync(Expression<Func<TAggregateRoot, bool>> predicate, IQueryOptions<TAggregateRoot>? queryOptions, CancellationToken cancellationToken)
		{
			return await this.cachingStrategy.FindOneAsync(predicate, queryOptions,
					async () => await this.innerRepository.FindOneAsync(predicate, queryOptions, cancellationToken)
						.ConfigureAwait(false))
				.ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task<TAggregateRoot> ICanFind<TAggregateRoot, TKey>.FindOneAsync(ISpecification<TAggregateRoot> specification, IQueryOptions<TAggregateRoot>? queryOptions, CancellationToken cancellationToken)
		{
			return await this.cachingStrategy.FindOneAsync(specification.Predicate, queryOptions,
					async () => await this.innerRepository.FindOneAsync(specification, queryOptions, cancellationToken)
						.ConfigureAwait(false))
				.ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task<TResult> ICanFind<TAggregateRoot, TKey>.FindOneAsync<TResult>(Expression<Func<TAggregateRoot, bool>> predicate, Expression<Func<TAggregateRoot, TResult>> selector, IQueryOptions<TAggregateRoot>? queryOptions, CancellationToken cancellationToken)
		{
			return await this.cachingStrategy.FindOneAsync(predicate, selector, queryOptions,
					async () => await this.innerRepository.FindOneAsync(predicate, selector, queryOptions, cancellationToken)
						.ConfigureAwait(false))
				.ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task<TResult> ICanFind<TAggregateRoot, TKey>.FindOneAsync<TResult>(ISpecification<TAggregateRoot> specification, Expression<Func<TAggregateRoot, TResult>> selector, IQueryOptions<TAggregateRoot>? queryOptions, CancellationToken cancellationToken)
		{
			return await this.cachingStrategy.FindOneAsync(specification.Predicate, selector, queryOptions,
					async () => await this.innerRepository.FindOneAsync(specification, selector, queryOptions, cancellationToken)
						.ConfigureAwait(false))
				.ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task<bool> ICanFind<TAggregateRoot, TKey>.ExistsAsync(Expression<Func<TAggregateRoot, bool>> predicate, CancellationToken cancellationToken)
		{
			return await this.cachingStrategy.ExistsAsync(predicate,
					async () => await this.innerRepository.ExistsAsync(predicate, cancellationToken)
						.ConfigureAwait(false))
				.ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task<bool> ICanFind<TAggregateRoot, TKey>.ExistsAsync(ISpecification<TAggregateRoot> specification, CancellationToken cancellationToken)
		{
			return await this.cachingStrategy.ExistsAsync(specification.Predicate,
					async () => await this.innerRepository.ExistsAsync(specification, cancellationToken)
						.ConfigureAwait(false))
				.ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task<IReadOnlyCollection<TAggregateRoot>> ICanFind<TAggregateRoot, TKey>.FindManyAsync(Expression<Func<TAggregateRoot, bool>> predicate, IQueryOptions<TAggregateRoot>? queryOptions, CancellationToken cancellationToken)
		{
			return await this.cachingStrategy.FindManyAsync(predicate, queryOptions,
					async () => await this.innerRepository.FindManyAsync(predicate, queryOptions, cancellationToken)
						.ConfigureAwait(false))
				.ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task<IReadOnlyCollection<TAggregateRoot>> ICanFind<TAggregateRoot, TKey>.FindManyAsync(ISpecification<TAggregateRoot> specification, IQueryOptions<TAggregateRoot>? queryOptions, CancellationToken cancellationToken)
		{
			return await this.cachingStrategy.FindManyAsync(specification.Predicate, queryOptions,
					async () => await this.innerRepository.FindManyAsync(specification, queryOptions, cancellationToken)
						.ConfigureAwait(false))
				.ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task<IReadOnlyCollection<TResult>> ICanFind<TAggregateRoot, TKey>.FindManyAsync<TResult>(Expression<Func<TAggregateRoot, bool>> predicate, Expression<Func<TAggregateRoot, TResult>> selector, IQueryOptions<TAggregateRoot>? queryOptions, CancellationToken cancellationToken)
		{
			return await this.cachingStrategy.FindManyAsync(predicate, selector, queryOptions,
					async () => await this.innerRepository.FindManyAsync(predicate, selector, queryOptions, cancellationToken)
						.ConfigureAwait(false))
				.ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task<IReadOnlyCollection<TResult>> ICanFind<TAggregateRoot, TKey>.FindManyAsync<TResult>(ISpecification<TAggregateRoot> specification, Expression<Func<TAggregateRoot, TResult>> selector, IQueryOptions<TAggregateRoot>? queryOptions, CancellationToken cancellationToken)
		{
			return await this.cachingStrategy.FindManyAsync(specification.Predicate, selector, queryOptions,
					async () => await this.innerRepository.FindManyAsync(specification, selector, queryOptions, cancellationToken)
						.ConfigureAwait(false))
				.ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task<long> ICanAggregate<TAggregateRoot, TKey>.CountAsync(CancellationToken cancellationToken)
		{
			return await this.cachingStrategy.CountAsync(
					async () => await this.innerRepository.CountAsync(cancellationToken)
						.ConfigureAwait(false))
				.ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task<long> ICanAggregate<TAggregateRoot, TKey>.CountAsync(Expression<Func<TAggregateRoot, bool>> predicate, CancellationToken cancellationToken)
		{
			return await this.cachingStrategy.CountAsync(predicate,
					async () => await this.innerRepository.CountAsync(predicate, cancellationToken)
						.ConfigureAwait(false))
				.ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task<long> ICanAggregate<TAggregateRoot, TKey>.CountAsync(ISpecification<TAggregateRoot> specification, CancellationToken cancellationToken)
		{
			return await this.cachingStrategy.CountAsync(specification.Predicate,
					async () => await this.innerRepository.CountAsync(specification, cancellationToken)
						.ConfigureAwait(false))
				.ConfigureAwait(false);
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
		async ValueTask IAsyncDisposable.DisposeAsync()
		{
			if(!this.innerRepository.IsDisposed)
			{
				await this.innerRepository.DisposeAsync();
			}
		}

		/// <inheritdoc />
		public override string ToString()
		{
			return this.innerRepository.ToString();
		}
	}
}
