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
	using Fluxera.Repository.Traits;
	using Fluxera.Utilities.Extensions;

	public sealed class CachingRepositoryDecorator<TAggregateRoot> : IRepository<TAggregateRoot>
		where TAggregateRoot : AggregateRoot<TAggregateRoot>
	{
		private readonly IRepository<TAggregateRoot> innerRepository;
		private readonly ICachingStrategy<TAggregateRoot> cachingStrategy;

		public CachingRepositoryDecorator(IRepository<TAggregateRoot> innerRepository, IRepositoryRegistry repositoryRegistry, ICachingStrategyFactory cachingStrategyFactory)
		{
			Guard.Against.Null(innerRepository, nameof(innerRepository));

			this.innerRepository = innerRepository;

			// Get the repository options.
			RepositoryName repositoryName = repositoryRegistry.GetRepositoryNameFor<TAggregateRoot>();

			// Initialize caching strategy.
			this.cachingStrategy = cachingStrategyFactory.CreateStrategy<TAggregateRoot>(repositoryName);
		}

		/// <inheritdoc />
		async Task ICanAdd<TAggregateRoot>.AddAsync(TAggregateRoot item, CancellationToken cancellationToken)
		{
			await this.innerRepository.AddAsync(item, cancellationToken).ConfigureAwait(false);

			await this.cachingStrategy.AddAsync(item).ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task ICanAdd<TAggregateRoot>.AddAsync(IEnumerable<TAggregateRoot> items, CancellationToken cancellationToken)
		{
			await this.innerRepository.AddAsync(items, cancellationToken).ConfigureAwait(false);

			await this.cachingStrategy.AddAsync(items).ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task ICanUpdate<TAggregateRoot>.UpdateAsync(TAggregateRoot item, CancellationToken cancellationToken)
		{
			await this.innerRepository.UpdateAsync(item, cancellationToken).ConfigureAwait(false);

			await this.cachingStrategy.UpdateAsync(item).ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task ICanUpdate<TAggregateRoot>.UpdateAsync(IEnumerable<TAggregateRoot> items, CancellationToken cancellationToken)
		{
			await this.innerRepository.UpdateAsync(items, cancellationToken).ConfigureAwait(false);

			await this.cachingStrategy.UpdateAsync(items).ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task ICanRemove<TAggregateRoot>.RemoveAsync(TAggregateRoot item, CancellationToken cancellationToken)
		{
			string id = item.ID;

			await this.innerRepository.RemoveAsync(item, cancellationToken).ConfigureAwait(false);

			await this.cachingStrategy.DeleteAsync(id).ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task ICanRemove<TAggregateRoot>.RemoveAsync(string id, CancellationToken cancellationToken)
		{
			await this.innerRepository.RemoveAsync(id, cancellationToken).ConfigureAwait(false);

			await this.cachingStrategy.DeleteAsync(id).ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task ICanRemove<TAggregateRoot>.RemoveAsync(Expression<Func<TAggregateRoot, bool>> predicate, CancellationToken cancellationToken)
		{
			IReadOnlyCollection<TAggregateRoot> items = await this.innerRepository.FindManyAsync(predicate, cancellationToken: cancellationToken).ConfigureAwait(false);
			IReadOnlyCollection<string> ids = items.Select(x => x.ID).AsReadOnly();

			await this.innerRepository.RemoveAsync(predicate, cancellationToken).ConfigureAwait(false);

			await this.cachingStrategy.DeleteAsync(ids).ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task ICanRemove<TAggregateRoot>.RemoveAsync(IEnumerable<TAggregateRoot> items, CancellationToken cancellationToken)
		{
			IReadOnlyCollection<string> ids = items.Select(x => x.ID).AsReadOnly();

			await this.innerRepository.RemoveAsync(items, cancellationToken).ConfigureAwait(false);

			await this.cachingStrategy.DeleteAsync(ids).ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task<TAggregateRoot> ICanGet<TAggregateRoot>.GetAsync(string id, CancellationToken cancellationToken)
		{
			return await this.cachingStrategy.GetAsync(id, 
				async () => await this.innerRepository.GetAsync(id, cancellationToken)
					.ConfigureAwait(false))
				.ConfigureAwait(true);
		}

		/// <inheritdoc />
		async Task<TResult> ICanGet<TAggregateRoot>.GetAsync<TResult>(string id, Expression<Func<TAggregateRoot, TResult>> selector, CancellationToken cancellationToken)
		{
			return await this.cachingStrategy.GetAsync(id, selector, 
				async () => await this.innerRepository.GetAsync(id, selector, cancellationToken)
					.ConfigureAwait(false))
				.ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task<bool> ICanGet<TAggregateRoot>.ExistsAsync(string id, CancellationToken cancellationToken)
		{
			return await this.cachingStrategy.ExistsAsync(id, 
					async () => await this.innerRepository.ExistsAsync(id, cancellationToken)
						.ConfigureAwait(false))
				.ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task<TAggregateRoot> ICanFind<TAggregateRoot>.FindOneAsync(Expression<Func<TAggregateRoot, bool>> predicate, IQueryOptions<TAggregateRoot>? queryOptions, CancellationToken cancellationToken)
		{
			return await this.cachingStrategy.FindOneAsync(predicate, queryOptions, 
					async () => await this.innerRepository.FindOneAsync(predicate, queryOptions, cancellationToken)
						.ConfigureAwait(false))
				.ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task<TResult> ICanFind<TAggregateRoot>.FindOneAsync<TResult>(Expression<Func<TAggregateRoot, bool>> predicate, Expression<Func<TAggregateRoot, TResult>> selector, IQueryOptions<TAggregateRoot>? queryOptions, CancellationToken cancellationToken)
		{
			return await this.cachingStrategy.FindOneAsync(predicate, selector, queryOptions, 
					async () => await this.innerRepository.FindOneAsync(predicate, selector, queryOptions, cancellationToken)
						.ConfigureAwait(false))
				.ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task<bool> ICanFind<TAggregateRoot>.ExistsAsync(Expression<Func<TAggregateRoot, bool>> predicate, CancellationToken cancellationToken)
		{
			return await this.cachingStrategy.ExistsAsync(predicate,
					async () => await this.innerRepository.ExistsAsync(predicate, cancellationToken)
						.ConfigureAwait(false))
				.ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task<IReadOnlyCollection<TAggregateRoot>> ICanFind<TAggregateRoot>.FindManyAsync(Expression<Func<TAggregateRoot, bool>> predicate, IQueryOptions<TAggregateRoot>? queryOptions, CancellationToken cancellationToken)
		{
			return await this.cachingStrategy.FindManyAsync(predicate, queryOptions,
					async () => await this.innerRepository.FindManyAsync(predicate, queryOptions, cancellationToken)
						.ConfigureAwait(false))
				.ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task<IReadOnlyCollection<TResult>> ICanFind<TAggregateRoot>.FindManyAsync<TResult>(Expression<Func<TAggregateRoot, bool>> predicate, Expression<Func<TAggregateRoot, TResult>> selector, IQueryOptions<TAggregateRoot>? queryOptions, CancellationToken cancellationToken)
		{
			return await this.cachingStrategy.FindManyAsync(predicate, selector, queryOptions,
					async () => await this.innerRepository.FindManyAsync(predicate, selector, queryOptions, cancellationToken)
						.ConfigureAwait(false))
				.ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task<long> ICanAggregate<TAggregateRoot>.CountAsync(CancellationToken cancellationToken)
		{
			return await this.cachingStrategy.CountAsync(
					async () => await this.innerRepository.CountAsync(cancellationToken)
						.ConfigureAwait(false))
				.ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task<long> ICanAggregate<TAggregateRoot>.CountAsync(Expression<Func<TAggregateRoot, bool>> predicate, CancellationToken cancellationToken)
		{
			return await this.cachingStrategy.CountAsync(predicate, 
					async () => await this.innerRepository.CountAsync(cancellationToken)
						.ConfigureAwait(false))
				.ConfigureAwait(false);
		}

		/// <inheritdoc />
		public override string ToString()
		{
			return this.innerRepository.ToString();
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
	}
}
