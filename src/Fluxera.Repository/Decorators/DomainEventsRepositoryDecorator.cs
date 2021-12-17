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
	using Fluxera.Repository.Traits;
	using Microsoft.Extensions.Logging;

	public sealed class DomainEventsRepositoryDecorator<TAggregateRoot> : IRepository<TAggregateRoot>
		where TAggregateRoot : AggregateRoot<TAggregateRoot>
	{
		private readonly IDomainEventDispatcher domainEventDispatcher;
		private readonly IRepository<TAggregateRoot> innerRepository;
		private readonly ILogger logger;

		public DomainEventsRepositoryDecorator(
			IRepository<TAggregateRoot> innerRepository,
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
		async Task ICanAdd<TAggregateRoot>.AddAsync(TAggregateRoot item, CancellationToken cancellationToken)
		{
			await this.DispatchAsync(item).ConfigureAwait(false);

			await this.innerRepository.AddAsync(item, cancellationToken).ConfigureAwait(false);

			// Add event to dispatch 'item added' event only to committed event handlers.
			item.DomainEvents.Add(new ItemAdded<TAggregateRoot>(item));

			await this.DispatchCommittedAsync(item).ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task ICanAdd<TAggregateRoot>.AddAsync(IEnumerable<TAggregateRoot> items, CancellationToken cancellationToken)
		{
			await this.DispatchAsync(items).ConfigureAwait(false);

			await this.innerRepository.AddAsync(items, cancellationToken).ConfigureAwait(false);

			// Add event to dispatch 'item added' event only to committed event handlers.
			foreach(TAggregateRoot item in items)
			{
				item.DomainEvents.Add(new ItemAdded<TAggregateRoot>(item));
			}

			await this.DispatchCommittedAsync(items).ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task ICanUpdate<TAggregateRoot>.UpdateAsync(TAggregateRoot item, CancellationToken cancellationToken)
		{
			await this.DispatchAsync(item).ConfigureAwait(false);

			await this.innerRepository.UpdateAsync(item, cancellationToken).ConfigureAwait(false);

			// Add event to dispatch 'item updated' event only to committed event handlers.
			item.DomainEvents.Add(new ItemUpdated<TAggregateRoot>(item));

			await this.DispatchCommittedAsync(item).ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task ICanUpdate<TAggregateRoot>.UpdateAsync(IEnumerable<TAggregateRoot> items, CancellationToken cancellationToken)
		{
			await this.DispatchAsync(items).ConfigureAwait(false);

			await this.innerRepository.UpdateAsync(items, cancellationToken).ConfigureAwait(false);

			// Add event to dispatch 'item updated' event only to committed event handlers.
			foreach(TAggregateRoot item in items)
			{
				item.DomainEvents.Add(new ItemAdded<TAggregateRoot>(item));
			}

			await this.DispatchCommittedAsync(items).ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task ICanRemove<TAggregateRoot>.RemoveAsync(TAggregateRoot item, CancellationToken cancellationToken)
		{
			string id = item.ID;

			await this.DispatchAsync(item).ConfigureAwait(false);

			await this.innerRepository.RemoveAsync(item, cancellationToken).ConfigureAwait(false);

			// Add event to dispatch 'item removed' event only to committed event handlers.
			item.DomainEvents.Add(new ItemRemoved<TAggregateRoot>(item, id));

			await this.DispatchCommittedAsync(item).ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task ICanRemove<TAggregateRoot>.RemoveAsync(string id, CancellationToken cancellationToken)
		{
			TAggregateRoot item = await this.innerRepository.GetAsync(id, cancellationToken).ConfigureAwait(false);

			await this.DispatchAsync(item).ConfigureAwait(false);

			await this.innerRepository.RemoveAsync(id, cancellationToken).ConfigureAwait(false);

			// Add event to dispatch 'item removed' event only to committed event handlers.
			item.DomainEvents.Add(new ItemRemoved<TAggregateRoot>(item, id));

			await this.DispatchCommittedAsync(item).ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task ICanRemove<TAggregateRoot>.RemoveAsync(Expression<Func<TAggregateRoot, bool>> predicate, CancellationToken cancellationToken)
		{
			IReadOnlyCollection<TAggregateRoot> items = await this.innerRepository.FindManyAsync(predicate, cancellationToken: cancellationToken).ConfigureAwait(false);
			IDictionary<string?, TAggregateRoot> itemsDict = items.ToDictionary(x => x.ID, x => x);

			await this.DispatchAsync(items).ConfigureAwait(false);

			await this.innerRepository.RemoveAsync(predicate, cancellationToken).ConfigureAwait(false);

			// Add event to dispatch 'item removed' event only to committed event handlers.
			foreach((string? key, TAggregateRoot? value) in itemsDict)
			{
				value.DomainEvents.Add(new ItemRemoved<TAggregateRoot>(value, key));
			}

			await this.DispatchCommittedAsync(items).ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task ICanRemove<TAggregateRoot>.RemoveAsync(IEnumerable<TAggregateRoot> items, CancellationToken cancellationToken)
		{
			IDictionary<string?, TAggregateRoot> itemsDict = items.ToDictionary(x => x.ID, x => x);

			await this.DispatchAsync(items).ConfigureAwait(false);

			await this.innerRepository.RemoveAsync(items, cancellationToken).ConfigureAwait(false);

			// Add event to dispatch 'item removed' event only to committed event handlers.
			foreach((string? key, TAggregateRoot? value) in itemsDict)
			{
				value.DomainEvents.Add(new ItemRemoved<TAggregateRoot>(value, key));
			}

			await this.DispatchCommittedAsync(items).ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task<TAggregateRoot> ICanGet<TAggregateRoot>.GetAsync(string id, CancellationToken cancellationToken)
		{
			return await this.innerRepository.GetAsync(id, cancellationToken).ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task<TResult> ICanGet<TAggregateRoot>.GetAsync<TResult>(string id, Expression<Func<TAggregateRoot, TResult>> selector, CancellationToken cancellationToken)
		{
			return await this.innerRepository.GetAsync(id, selector, cancellationToken).ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task<bool> ICanGet<TAggregateRoot>.ExistsAsync(string id, CancellationToken cancellationToken)
		{
			return await this.innerRepository.ExistsAsync(id, cancellationToken).ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task<TAggregateRoot> ICanFind<TAggregateRoot>.FindOneAsync(Expression<Func<TAggregateRoot, bool>> predicate, IQueryOptions<TAggregateRoot>? queryOptions, CancellationToken cancellationToken)
		{
			return await this.innerRepository.FindOneAsync(predicate, queryOptions, cancellationToken).ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task<TResult> ICanFind<TAggregateRoot>.FindOneAsync<TResult>(Expression<Func<TAggregateRoot, bool>> predicate, Expression<Func<TAggregateRoot, TResult>> selector, IQueryOptions<TAggregateRoot>? queryOptions, CancellationToken cancellationToken)
		{
			return await this.innerRepository.FindOneAsync(predicate, selector, queryOptions, cancellationToken).ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task<bool> ICanFind<TAggregateRoot>.ExistsAsync(Expression<Func<TAggregateRoot, bool>> predicate, CancellationToken cancellationToken)
		{
			return await this.innerRepository.ExistsAsync(predicate, cancellationToken).ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task<IReadOnlyCollection<TAggregateRoot>> ICanFind<TAggregateRoot>.FindManyAsync(Expression<Func<TAggregateRoot, bool>> predicate, IQueryOptions<TAggregateRoot>? queryOptions, CancellationToken cancellationToken)
		{
			return await this.innerRepository.FindManyAsync(predicate, queryOptions, cancellationToken).ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task<IReadOnlyCollection<TResult>> ICanFind<TAggregateRoot>.FindManyAsync<TResult>(Expression<Func<TAggregateRoot, bool>> predicate, Expression<Func<TAggregateRoot, TResult>> selector, IQueryOptions<TAggregateRoot>? queryOptions, CancellationToken cancellationToken)
		{
			return await this.innerRepository.FindManyAsync(predicate, selector, queryOptions, cancellationToken).ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task<long> ICanAggregate<TAggregateRoot>.CountAsync(CancellationToken cancellationToken)
		{
			return await this.innerRepository.CountAsync(cancellationToken).ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task<long> ICanAggregate<TAggregateRoot>.CountAsync(Expression<Func<TAggregateRoot, bool>> predicate, CancellationToken cancellationToken)
		{
			return await this.innerRepository.CountAsync(predicate, cancellationToken).ConfigureAwait(false);
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
		bool IReadOnlyRepository<TAggregateRoot>.IsDisposed => this.innerRepository.IsDisposed;

		/// <inheritdoc />
		async ValueTask IAsyncDisposable.DisposeAsync()
		{
			if(!this.innerRepository.IsDisposed)
			{
				await this.innerRepository.DisposeAsync();
			}
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
