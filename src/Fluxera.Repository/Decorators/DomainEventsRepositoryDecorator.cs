namespace Fluxera.Repository.Decorators
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Linq.Expressions;
	using System.Threading;
	using System.Threading.Tasks;
	using Fluxera.Entity;
	using Fluxera.Entity.DomainEvents;
	using Fluxera.Guards;
	using Fluxera.Repository.Query;
	using Fluxera.Repository.Specifications;
	using Fluxera.Repository.Traits;
	using Microsoft.Extensions.Logging;

	/// <summary>
	///     A repository decorator that controls the domain events feature.
	/// </summary>
	/// <typeparam name="TAggregateRoot"></typeparam>
	/// <typeparam name="TKey"></typeparam>
	public sealed class DomainEventsRepositoryDecorator<TAggregateRoot, TKey> : IRepository<TAggregateRoot, TKey>
		where TAggregateRoot : AggregateRoot<TAggregateRoot, TKey>
	{
		private readonly IDomainEventDispatcher domainEventDispatcher;
		private readonly IRepository<TAggregateRoot, TKey> innerRepository;
		private readonly ILogger logger;

		/// <summary>
		///     Creates a new instance of the <see cref="DomainEventsRepositoryDecorator{TAggregateRoot,TKey}" /> type.
		/// </summary>
		/// <param name="innerRepository"></param>
		/// <param name="domainEventDispatcher"></param>
		/// <param name="loggerFactory"></param>
		public DomainEventsRepositoryDecorator(
			IRepository<TAggregateRoot, TKey> innerRepository,
			IDomainEventDispatcher domainEventDispatcher,
			ILoggerFactory loggerFactory)
		{
			Guard.Against.Null(innerRepository, nameof(innerRepository));
			Guard.Against.Null(domainEventDispatcher, nameof(domainEventDispatcher));
			Guard.Against.Null(loggerFactory, nameof(loggerFactory));

			this.innerRepository = innerRepository;
			this.domainEventDispatcher = domainEventDispatcher;
			this.logger = loggerFactory.CreateLogger(LoggerNames.EventsDispatcher);
		}

		/// <inheritdoc />
		async Task ICanAdd<TAggregateRoot, TKey>.AddAsync(TAggregateRoot item, CancellationToken cancellationToken)
		{
			await this.DispatchAsync(item).ConfigureAwait(false);

			await this.innerRepository.AddAsync(item, cancellationToken).ConfigureAwait(false);

			// Add event to dispatch 'item added' event only to committed event handlers.
			item.DomainEvents.Add(new ItemAdded<TAggregateRoot, TKey>(item));

			await this.DispatchCommittedAsync(item).ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task ICanAdd<TAggregateRoot, TKey>.AddRangeAsync(IEnumerable<TAggregateRoot> items, CancellationToken cancellationToken)
		{
			IEnumerable<TAggregateRoot> itemsList = items.ToList();

			await this.DispatchAsync(itemsList).ConfigureAwait(false);

			await this.innerRepository.AddRangeAsync(itemsList, cancellationToken).ConfigureAwait(false);

			// Add event to dispatch 'item added' event only to committed event handlers.
			foreach(TAggregateRoot item in itemsList)
			{
				item.DomainEvents.Add(new ItemAdded<TAggregateRoot, TKey>(item));
			}

			await this.DispatchCommittedAsync(itemsList).ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task ICanUpdate<TAggregateRoot, TKey>.UpdateAsync(TAggregateRoot item, CancellationToken cancellationToken)
		{
			await this.DispatchAsync(item).ConfigureAwait(false);

			await this.innerRepository.UpdateAsync(item, cancellationToken).ConfigureAwait(false);

			// Add event to dispatch 'item updated' event only to committed event handlers.
			item.DomainEvents.Add(new ItemUpdated<TAggregateRoot, TKey>(item));

			await this.DispatchCommittedAsync(item).ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task ICanUpdate<TAggregateRoot, TKey>.UpdateRangeAsync(IEnumerable<TAggregateRoot> items, CancellationToken cancellationToken)
		{
			IEnumerable<TAggregateRoot> itemsList = items.ToList();

			await this.DispatchAsync(itemsList).ConfigureAwait(false);

			await this.innerRepository.UpdateRangeAsync(itemsList, cancellationToken).ConfigureAwait(false);

			// Add event to dispatch 'item updated' event only to committed event handlers.
			foreach(TAggregateRoot item in itemsList)
			{
				item.DomainEvents.Add(new ItemAdded<TAggregateRoot, TKey>(item));
			}

			await this.DispatchCommittedAsync(itemsList).ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task ICanRemove<TAggregateRoot, TKey>.RemoveAsync(TAggregateRoot item, CancellationToken cancellationToken)
		{
			TKey id = item.ID!;

			await this.DispatchAsync(item).ConfigureAwait(false);

			await this.innerRepository.RemoveAsync(item, cancellationToken).ConfigureAwait(false);

			// Add event to dispatch 'item removed' event only to committed event handlers.
			item.DomainEvents.Add(new ItemRemoved<TAggregateRoot, TKey>(item, id));

			await this.DispatchCommittedAsync(item).ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task ICanRemove<TAggregateRoot, TKey>.RemoveRangeAsync(Expression<Func<TAggregateRoot, bool>> predicate, CancellationToken cancellationToken)
		{
			IReadOnlyCollection<TAggregateRoot> items = await this.innerRepository.FindManyAsync(predicate, cancellationToken: cancellationToken).ConfigureAwait(false);
			IDictionary<TKey, TAggregateRoot> itemsDict = items.ToDictionary(x => x.ID, x => x)!;

			await this.DispatchAsync(items).ConfigureAwait(false);

			await this.innerRepository.RemoveRangeAsync(predicate, cancellationToken).ConfigureAwait(false);

			// Add event to dispatch 'item removed' event only to committed event handlers.
			foreach((TKey key, TAggregateRoot value) in itemsDict)
			{
				value.DomainEvents.Add(new ItemRemoved<TAggregateRoot, TKey>(value, key));
			}

			await this.DispatchCommittedAsync(items).ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task ICanRemove<TAggregateRoot, TKey>.RemoveRangeAsync(ISpecification<TAggregateRoot> specification, CancellationToken cancellationToken)
		{
			IReadOnlyCollection<TAggregateRoot> items = await this.innerRepository.FindManyAsync(specification, cancellationToken: cancellationToken).ConfigureAwait(false);
			IDictionary<TKey, TAggregateRoot> itemsDict = items.ToDictionary(x => x.ID, x => x)!;

			await this.DispatchAsync(items).ConfigureAwait(false);

			await this.innerRepository.RemoveRangeAsync(specification, cancellationToken).ConfigureAwait(false);

			// Add event to dispatch 'item removed' event only to committed event handlers.
			foreach((TKey key, TAggregateRoot value) in itemsDict)
			{
				value.DomainEvents.Add(new ItemRemoved<TAggregateRoot, TKey>(value, key));
			}

			await this.DispatchCommittedAsync(items).ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task ICanRemove<TAggregateRoot, TKey>.RemoveRangeAsync(IEnumerable<TAggregateRoot> items, CancellationToken cancellationToken)
		{
			IEnumerable<TAggregateRoot> itemsList = items.ToList();
			Dictionary<TKey, TAggregateRoot> itemsDict = itemsList.ToDictionary(x => x.ID, x => x)!;

			await this.DispatchAsync(itemsList).ConfigureAwait(false);

			await this.innerRepository.RemoveRangeAsync(itemsList, cancellationToken).ConfigureAwait(false);

			// Add event to dispatch 'item removed' event only to committed event handlers.
			foreach((TKey key, TAggregateRoot value) in itemsDict)
			{
				value.DomainEvents.Add(new ItemRemoved<TAggregateRoot, TKey>(value, key));
			}

			await this.DispatchCommittedAsync(itemsList).ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task<TAggregateRoot> ICanFind<TAggregateRoot, TKey>.FindOneAsync(Expression<Func<TAggregateRoot, bool>> predicate, IQueryOptions<TAggregateRoot> queryOptions, CancellationToken cancellationToken)
		{
			return await this.innerRepository.FindOneAsync(predicate, queryOptions, cancellationToken).ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task<TAggregateRoot> ICanFind<TAggregateRoot, TKey>.FindOneAsync(ISpecification<TAggregateRoot> specification, IQueryOptions<TAggregateRoot> queryOptions, CancellationToken cancellationToken)
		{
			return await this.innerRepository.FindOneAsync(specification, queryOptions, cancellationToken);
		}

		/// <inheritdoc />
		async Task<TResult> ICanFind<TAggregateRoot, TKey>.FindOneAsync<TResult>(Expression<Func<TAggregateRoot, bool>> predicate, Expression<Func<TAggregateRoot, TResult>> selector, IQueryOptions<TAggregateRoot> queryOptions, CancellationToken cancellationToken)
		{
			return await this.innerRepository.FindOneAsync(predicate, selector, queryOptions, cancellationToken).ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task<TResult> ICanFind<TAggregateRoot, TKey>.FindOneAsync<TResult>(ISpecification<TAggregateRoot> specification, Expression<Func<TAggregateRoot, TResult>> selector, IQueryOptions<TAggregateRoot> queryOptions, CancellationToken cancellationToken)
		{
			return await this.innerRepository.FindOneAsync(specification, selector, queryOptions, cancellationToken);
		}

		/// <inheritdoc />
		async Task<bool> ICanFind<TAggregateRoot, TKey>.ExistsAsync(Expression<Func<TAggregateRoot, bool>> predicate, CancellationToken cancellationToken)
		{
			return await this.innerRepository.ExistsAsync(predicate, cancellationToken).ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task<bool> ICanFind<TAggregateRoot, TKey>.ExistsAsync(ISpecification<TAggregateRoot> specification, CancellationToken cancellationToken)
		{
			return await this.innerRepository.ExistsAsync(specification, cancellationToken);
		}

		/// <inheritdoc />
		async Task<IReadOnlyCollection<TAggregateRoot>> ICanFind<TAggregateRoot, TKey>.FindManyAsync(Expression<Func<TAggregateRoot, bool>> predicate, IQueryOptions<TAggregateRoot> queryOptions, CancellationToken cancellationToken)
		{
			return await this.innerRepository.FindManyAsync(predicate, queryOptions, cancellationToken).ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task<IReadOnlyCollection<TAggregateRoot>> ICanFind<TAggregateRoot, TKey>.FindManyAsync(ISpecification<TAggregateRoot> specification, IQueryOptions<TAggregateRoot> queryOptions, CancellationToken cancellationToken)
		{
			return await this.innerRepository.FindManyAsync(specification, queryOptions, cancellationToken);
		}

		/// <inheritdoc />
		async Task<IReadOnlyCollection<TResult>> ICanFind<TAggregateRoot, TKey>.FindManyAsync<TResult>(Expression<Func<TAggregateRoot, bool>> predicate, Expression<Func<TAggregateRoot, TResult>> selector, IQueryOptions<TAggregateRoot> queryOptions, CancellationToken cancellationToken)
		{
			return await this.innerRepository.FindManyAsync(predicate, selector, queryOptions, cancellationToken).ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task<IReadOnlyCollection<TResult>> ICanFind<TAggregateRoot, TKey>.FindManyAsync<TResult>(ISpecification<TAggregateRoot> specification, Expression<Func<TAggregateRoot, TResult>> selector, IQueryOptions<TAggregateRoot> queryOptions, CancellationToken cancellationToken)
		{
			return await this.innerRepository.FindManyAsync(specification, selector, queryOptions, cancellationToken);
		}

		/// <inheritdoc />
		async Task<long> ICanAggregate<TAggregateRoot, TKey>.CountAsync(CancellationToken cancellationToken)
		{
			return await this.innerRepository.CountAsync(cancellationToken).ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task<long> ICanAggregate<TAggregateRoot, TKey>.CountAsync(Expression<Func<TAggregateRoot, bool>> predicate, CancellationToken cancellationToken)
		{
			return await this.innerRepository.CountAsync(predicate, cancellationToken).ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task<long> ICanAggregate<TAggregateRoot, TKey>.CountAsync(ISpecification<TAggregateRoot> specification, CancellationToken cancellationToken)
		{
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
			TAggregateRoot item = await this.innerRepository.GetAsync(id, cancellationToken).ConfigureAwait(false);

			await this.DispatchAsync(item).ConfigureAwait(false);

			await this.innerRepository.RemoveAsync(id, cancellationToken).ConfigureAwait(false);

			// Add event to dispatch 'item removed' event only to committed event handlers.
			item.DomainEvents.Add(new ItemRemoved<TAggregateRoot, TKey>(item, id));

			await this.DispatchCommittedAsync(item).ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task<TAggregateRoot> ICanGet<TAggregateRoot, TKey>.GetAsync(TKey id, CancellationToken cancellationToken)
		{
			return await this.innerRepository.GetAsync(id, cancellationToken).ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task<TResult> ICanGet<TAggregateRoot, TKey>.GetAsync<TResult>(TKey id, Expression<Func<TAggregateRoot, TResult>> selector, CancellationToken cancellationToken)
		{
			return await this.innerRepository.GetAsync(id, selector, cancellationToken).ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task<bool> ICanGet<TAggregateRoot, TKey>.ExistsAsync(TKey id, CancellationToken cancellationToken)
		{
			return await this.innerRepository.ExistsAsync(id, cancellationToken).ConfigureAwait(false);
		}

		private async Task DispatchAsync(TAggregateRoot item)
		{
			this.logger.LogTrace($"Dispatching domain events (Before commit): Type = {typeof(TAggregateRoot).Name}, Count = {item.DomainEvents.Count}");

			foreach(IDomainEvent domainEvent in item.DomainEvents)
			{
				await this.domainEventDispatcher.DispatchAsync(domainEvent).ConfigureAwait(false);
			}
		}

		private async Task DispatchAsync(IEnumerable<TAggregateRoot> items)
		{
			foreach(TAggregateRoot item in items)
			{
				await this.DispatchAsync(item).ConfigureAwait(false);
			}
		}

		private async Task DispatchCommittedAsync(TAggregateRoot item)
		{
			this.logger.LogTrace($"Dispatching domain events (After commit): Type = {typeof(TAggregateRoot).Name}, Count = {item.DomainEvents.Count}");

			foreach(IDomainEvent domainEvent in item.DomainEvents)
			{
				await this.domainEventDispatcher.DispatchCommittedAsync(domainEvent).ConfigureAwait(false);
			}

			item.DomainEvents.Clear();
		}

		private async Task DispatchCommittedAsync(IEnumerable<TAggregateRoot> items)
		{
			foreach(TAggregateRoot item in items)
			{
				await this.DispatchCommittedAsync(item).ConfigureAwait(false);
			}
		}

		/// <inheritdoc />
		public override string ToString()
		{
			return this.innerRepository.ToString();
		}
	}
}
