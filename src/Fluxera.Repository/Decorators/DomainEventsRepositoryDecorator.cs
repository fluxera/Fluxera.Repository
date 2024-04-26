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
	using Fluxera.Repository.DomainEvents;
	using Fluxera.Repository.Extensions;
	using Fluxera.Repository.Options;
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
		where TKey : notnull, IComparable<TKey>, IEquatable<TKey>
	{
		private readonly ILogger logger;

		private readonly IRepository<TAggregateRoot, TKey> innerRepository;
		private readonly IOutboxDomainEventDispatcher domainEventDispatcher;
		private readonly RepositoryOptions repositoryOptions;
		private readonly DomainEventsOptions domainEventsOptions;

		/// <summary>
		///     Creates a new instance of the <see cref="DomainEventsRepositoryDecorator{TAggregateRoot,TKey}" /> type.
		/// </summary>
		/// <param name="innerRepository"></param>
		/// <param name="domainEventDispatcher"></param>
		/// <param name="repositoryRegistry"></param>
		/// <param name="loggerFactory"></param>
		public DomainEventsRepositoryDecorator(
			IRepository<TAggregateRoot, TKey> innerRepository,
			IOutboxDomainEventDispatcher domainEventDispatcher,
			IRepositoryRegistry repositoryRegistry,
			ILoggerFactory loggerFactory)
		{
			this.innerRepository = Guard.Against.Null(innerRepository);
			this.domainEventDispatcher = Guard.Against.Null(domainEventDispatcher);
			Guard.Against.Null(loggerFactory);
			Guard.Against.Null(repositoryRegistry);

			this.logger = loggerFactory.CreateLogger(LoggerNames.EventsDispatcher);

			RepositoryName repositoryName = repositoryRegistry.GetRepositoryNameFor<TAggregateRoot>();
			this.repositoryOptions = repositoryRegistry.GetRepositoryOptionsFor(repositoryName);
			this.domainEventsOptions = this.repositoryOptions.DomainEventsOptions;
		}

		/// <inheritdoc />
		async Task ICanAdd<TAggregateRoot, TKey>.AddAsync(TAggregateRoot item, CancellationToken cancellationToken)
		{
			await this.innerRepository.AddAsync(item, cancellationToken).ConfigureAwait(false);

			await this.DispatchDomainEventsAsync(item).ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task ICanAdd<TAggregateRoot, TKey>.AddRangeAsync(IEnumerable<TAggregateRoot> items, CancellationToken cancellationToken)
		{
			IEnumerable<TAggregateRoot> itemsList = items.ToList();

			await this.innerRepository.AddRangeAsync(itemsList, cancellationToken).ConfigureAwait(false);

			await this.DispatchDomainEventsAsync(itemsList).ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task ICanUpdate<TAggregateRoot, TKey>.UpdateAsync(TAggregateRoot item, CancellationToken cancellationToken)
		{
			await this.innerRepository.UpdateAsync(item, cancellationToken).ConfigureAwait(false);

			await this.DispatchDomainEventsAsync(item).ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task ICanUpdate<TAggregateRoot, TKey>.UpdateRangeAsync(IEnumerable<TAggregateRoot> items, CancellationToken cancellationToken)
		{
			IEnumerable<TAggregateRoot> itemsList = items.ToList();

			await this.innerRepository.UpdateRangeAsync(itemsList, cancellationToken).ConfigureAwait(false);

			await this.DispatchDomainEventsAsync(itemsList).ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task ICanRemove<TAggregateRoot, TKey>.RemoveAsync(TAggregateRoot item, CancellationToken cancellationToken)
		{
			await this.innerRepository.RemoveAsync(item, cancellationToken).ConfigureAwait(false);

			await this.DispatchDomainEventsAsync(item).ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task ICanRemove<TAggregateRoot, TKey>.RemoveRangeAsync(Expression<Func<TAggregateRoot, bool>> predicate, CancellationToken cancellationToken)
		{
			IReadOnlyCollection<TAggregateRoot> items = await this.innerRepository.FindManyAsync(predicate, cancellationToken: cancellationToken).ConfigureAwait(false);

			await this.innerRepository.RemoveRangeAsync(predicate, cancellationToken).ConfigureAwait(false);

			await this.DispatchDomainEventsAsync(items).ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task ICanRemove<TAggregateRoot, TKey>.RemoveRangeAsync(ISpecification<TAggregateRoot> specification, CancellationToken cancellationToken)
		{
			IReadOnlyCollection<TAggregateRoot> items = await this.innerRepository.FindManyAsync(specification, cancellationToken: cancellationToken).ConfigureAwait(false);

			await this.innerRepository.RemoveRangeAsync(specification, cancellationToken).ConfigureAwait(false);

			await this.DispatchDomainEventsAsync(items).ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task ICanRemove<TAggregateRoot, TKey>.RemoveRangeAsync(IEnumerable<TAggregateRoot> items, CancellationToken cancellationToken)
		{
			IEnumerable<TAggregateRoot> itemsList = items.ToList();

			await this.innerRepository.RemoveRangeAsync(itemsList, cancellationToken).ConfigureAwait(false);

			await this.DispatchDomainEventsAsync(itemsList).ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task ICanRemove<TAggregateRoot, TKey>.RemoveAsync(TKey id, CancellationToken cancellationToken)
		{
			TAggregateRoot item = await this.innerRepository.GetAsync(id, cancellationToken).ConfigureAwait(false);

			await this.innerRepository.RemoveAsync(id, cancellationToken).ConfigureAwait(false);

			await this.DispatchDomainEventsAsync(item).ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task<TAggregateRoot> ICanGet<TAggregateRoot, TKey>.GetAsync(TKey id, CancellationToken cancellationToken)
		{
			return await this.innerRepository.GetAsync(id, cancellationToken).ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task<TResult> ICanGet<TAggregateRoot, TKey>.GetAsync<TResult>(TKey id, Expression<Func<TAggregateRoot, TResult>> selector, 
			CancellationToken cancellationToken)
		{
			return await this.innerRepository.GetAsync(id, selector, cancellationToken).ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task<bool> ICanGet<TAggregateRoot, TKey>.ExistsAsync(TKey id, CancellationToken cancellationToken)
		{
			return await this.innerRepository.ExistsAsync(id, cancellationToken).ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task<TAggregateRoot> ICanFind<TAggregateRoot, TKey>.FindOneAsync(Expression<Func<TAggregateRoot, bool>> predicate, 
			IQueryOptions<TAggregateRoot> queryOptions, CancellationToken cancellationToken)
		{
			return await this.innerRepository.FindOneAsync(predicate, queryOptions, cancellationToken).ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task<TAggregateRoot> ICanFind<TAggregateRoot, TKey>.FindOneAsync(ISpecification<TAggregateRoot> specification,
			IQueryOptions<TAggregateRoot> queryOptions, CancellationToken cancellationToken)
		{
			return await this.innerRepository.FindOneAsync(specification, queryOptions, cancellationToken);
		}

		/// <inheritdoc />
		async Task<TResult> ICanFind<TAggregateRoot, TKey>.FindOneAsync<TResult>(Expression<Func<TAggregateRoot, bool>> predicate,
			Expression<Func<TAggregateRoot, TResult>> selector, IQueryOptions<TAggregateRoot> queryOptions, CancellationToken cancellationToken)
		{
			return await this.innerRepository.FindOneAsync(predicate, selector, queryOptions, cancellationToken).ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task<TResult> ICanFind<TAggregateRoot, TKey>.FindOneAsync<TResult>(ISpecification<TAggregateRoot> specification,
			Expression<Func<TAggregateRoot, TResult>> selector, IQueryOptions<TAggregateRoot> queryOptions, CancellationToken cancellationToken)
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
		async Task<IReadOnlyCollection<TAggregateRoot>> ICanFind<TAggregateRoot, TKey>.FindManyAsync(Expression<Func<TAggregateRoot, bool>> predicate,
			IQueryOptions<TAggregateRoot> queryOptions, CancellationToken cancellationToken)
		{
			return await this.innerRepository.FindManyAsync(predicate, queryOptions, cancellationToken).ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task<IReadOnlyCollection<TAggregateRoot>> ICanFind<TAggregateRoot, TKey>.FindManyAsync(ISpecification<TAggregateRoot> specification,
			IQueryOptions<TAggregateRoot> queryOptions, CancellationToken cancellationToken)
		{
			return await this.innerRepository.FindManyAsync(specification, queryOptions, cancellationToken);
		}

		/// <inheritdoc />
		async Task<IReadOnlyCollection<TResult>> ICanFind<TAggregateRoot, TKey>.FindManyAsync<TResult>(Expression<Func<TAggregateRoot, bool>> predicate,
			Expression<Func<TAggregateRoot, TResult>> selector, IQueryOptions<TAggregateRoot> queryOptions, CancellationToken cancellationToken)
		{
			return await this.innerRepository.FindManyAsync(predicate, selector, queryOptions, cancellationToken).ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task<IReadOnlyCollection<TResult>> ICanFind<TAggregateRoot, TKey>.FindManyAsync<TResult>(ISpecification<TAggregateRoot> specification,
			Expression<Func<TAggregateRoot, TResult>> selector, IQueryOptions<TAggregateRoot> queryOptions, CancellationToken cancellationToken)
		{
			return await this.innerRepository.FindManyAsync(specification, selector, queryOptions, cancellationToken);
		}

		/// <inheritdoc />
		async Task<long> ICanGet<TAggregateRoot, TKey>.CountAsync(CancellationToken cancellationToken)
		{
			return await this.innerRepository.CountAsync(cancellationToken).ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task<long> ICanGet<TAggregateRoot, TKey>.CountAsync(Expression<Func<TAggregateRoot, bool>> predicate, CancellationToken cancellationToken)
		{
			return await this.innerRepository.CountAsync(predicate, cancellationToken).ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task<long> ICanGet<TAggregateRoot, TKey>.CountAsync(ISpecification<TAggregateRoot> specification, CancellationToken cancellationToken)
		{
			return await this.innerRepository.CountAsync(specification, cancellationToken);
		}

		/// <inheritdoc />
		async Task<int> ICanAggregate<TAggregateRoot, TKey>.SumAsync(Expression<Func<TAggregateRoot, int>> selector, CancellationToken cancellationToken)
		{
			return await this.innerRepository.SumAsync(selector, cancellationToken);
		}

		/// <inheritdoc />
		async Task<int> ICanAggregate<TAggregateRoot, TKey>.SumAsync(Expression<Func<TAggregateRoot, int?>> selector, CancellationToken cancellationToken)
		{
			return await this.innerRepository.SumAsync(selector, cancellationToken);
		}

		/// <inheritdoc />
		async Task<long> ICanAggregate<TAggregateRoot, TKey>.SumAsync(Expression<Func<TAggregateRoot, long>> selector, CancellationToken cancellationToken)
		{
			return await this.innerRepository.SumAsync(selector, cancellationToken);
		}

		/// <inheritdoc />
		async Task<long> ICanAggregate<TAggregateRoot, TKey>.SumAsync(Expression<Func<TAggregateRoot, long?>> selector, CancellationToken cancellationToken)
		{
			return await this.innerRepository.SumAsync(selector, cancellationToken);
		}

		/// <inheritdoc />
		async Task<decimal> ICanAggregate<TAggregateRoot, TKey>.SumAsync(Expression<Func<TAggregateRoot, decimal>> selector,
			CancellationToken cancellationToken)
		{
			return await this.innerRepository.SumAsync(selector, cancellationToken);
		}

		/// <inheritdoc />
		async Task<decimal> ICanAggregate<TAggregateRoot, TKey>.SumAsync(Expression<Func<TAggregateRoot, decimal?>> selector,
			CancellationToken cancellationToken)
		{
			return await this.innerRepository.SumAsync(selector, cancellationToken);
		}

		/// <inheritdoc />
		async Task<float> ICanAggregate<TAggregateRoot, TKey>.SumAsync(Expression<Func<TAggregateRoot, float>> selector, CancellationToken cancellationToken)
		{
			return await this.innerRepository.SumAsync(selector, cancellationToken);
		}

		/// <inheritdoc />
		async Task<float> ICanAggregate<TAggregateRoot, TKey>.SumAsync(Expression<Func<TAggregateRoot, float?>> selector, CancellationToken cancellationToken)
		{
			return await this.innerRepository.SumAsync(selector, cancellationToken);
		}

		/// <inheritdoc />
		async Task<double> ICanAggregate<TAggregateRoot, TKey>.SumAsync(Expression<Func<TAggregateRoot, double>> selector, CancellationToken cancellationToken)
		{
			return await this.innerRepository.SumAsync(selector, cancellationToken);
		}

		/// <inheritdoc />
		async Task<double> ICanAggregate<TAggregateRoot, TKey>.SumAsync(Expression<Func<TAggregateRoot, double?>> selector, CancellationToken cancellationToken)
		{
			return await this.innerRepository.SumAsync(selector, cancellationToken);
		}

		/// <inheritdoc />
		async Task<int> ICanAggregate<TAggregateRoot, TKey>.SumAsync(Expression<Func<TAggregateRoot, bool>> predicate,
			Expression<Func<TAggregateRoot, int>> selector, CancellationToken cancellationToken)
		{
			return await this.innerRepository.SumAsync(predicate, selector, cancellationToken);
		}

		/// <inheritdoc />
		async Task<int> ICanAggregate<TAggregateRoot, TKey>.SumAsync(Expression<Func<TAggregateRoot, bool>> predicate,
			Expression<Func<TAggregateRoot, int?>> selector, CancellationToken cancellationToken)
		{
			return await this.innerRepository.SumAsync(predicate, selector, cancellationToken);
		}

		/// <inheritdoc />
		async Task<long> ICanAggregate<TAggregateRoot, TKey>.SumAsync(Expression<Func<TAggregateRoot, bool>> predicate,
			Expression<Func<TAggregateRoot, long>> selector, CancellationToken cancellationToken)
		{
			return await this.innerRepository.SumAsync(predicate, selector, cancellationToken);
		}

		/// <inheritdoc />
		async Task<long> ICanAggregate<TAggregateRoot, TKey>.SumAsync(Expression<Func<TAggregateRoot, bool>> predicate,
			Expression<Func<TAggregateRoot, long?>> selector, CancellationToken cancellationToken)
		{
			return await this.innerRepository.SumAsync(predicate, selector, cancellationToken);
		}

		/// <inheritdoc />
		async Task<decimal> ICanAggregate<TAggregateRoot, TKey>.SumAsync(Expression<Func<TAggregateRoot, bool>> predicate,
			Expression<Func<TAggregateRoot, decimal>> selector, CancellationToken cancellationToken)
		{
			return await this.innerRepository.SumAsync(predicate, selector, cancellationToken);
		}

		/// <inheritdoc />
		async Task<decimal> ICanAggregate<TAggregateRoot, TKey>.SumAsync(Expression<Func<TAggregateRoot, bool>> predicate,
			Expression<Func<TAggregateRoot, decimal?>> selector, CancellationToken cancellationToken)
		{
			return await this.innerRepository.SumAsync(predicate, selector, cancellationToken);
		}

		/// <inheritdoc />
		async Task<float> ICanAggregate<TAggregateRoot, TKey>.SumAsync(Expression<Func<TAggregateRoot, bool>> predicate,
			Expression<Func<TAggregateRoot, float>> selector, CancellationToken cancellationToken)
		{
			return await this.innerRepository.SumAsync(predicate, selector, cancellationToken);
		}

		/// <inheritdoc />
		async Task<float> ICanAggregate<TAggregateRoot, TKey>.SumAsync(Expression<Func<TAggregateRoot, bool>> predicate,
			Expression<Func<TAggregateRoot, float?>> selector, CancellationToken cancellationToken)
		{
			return await this.innerRepository.SumAsync(predicate, selector, cancellationToken);
		}

		/// <inheritdoc />
		async Task<double> ICanAggregate<TAggregateRoot, TKey>.SumAsync(Expression<Func<TAggregateRoot, bool>> predicate,
			Expression<Func<TAggregateRoot, double>> selector, CancellationToken cancellationToken)
		{
			return await this.innerRepository.SumAsync(predicate, selector, cancellationToken);
		}

		/// <inheritdoc />
		async Task<double> ICanAggregate<TAggregateRoot, TKey>.SumAsync(Expression<Func<TAggregateRoot, bool>> predicate,
			Expression<Func<TAggregateRoot, double?>> selector, CancellationToken cancellationToken)
		{
			return await this.innerRepository.SumAsync(predicate, selector, cancellationToken);
		}

		/// <inheritdoc />
		async Task<int> ICanAggregate<TAggregateRoot, TKey>.SumAsync(ISpecification<TAggregateRoot> specification,
			Expression<Func<TAggregateRoot, int>> selector, CancellationToken cancellationToken)
		{
			return await this.innerRepository.SumAsync(specification, selector, cancellationToken);
		}

		/// <inheritdoc />
		async Task<int> ICanAggregate<TAggregateRoot, TKey>.SumAsync(ISpecification<TAggregateRoot> specification,
			Expression<Func<TAggregateRoot, int?>> selector, CancellationToken cancellationToken)
		{
			return await this.innerRepository.SumAsync(specification, selector, cancellationToken);
		}

		/// <inheritdoc />
		async Task<long> ICanAggregate<TAggregateRoot, TKey>.SumAsync(ISpecification<TAggregateRoot> specification,
			Expression<Func<TAggregateRoot, long>> selector, CancellationToken cancellationToken)
		{
			return await this.innerRepository.SumAsync(specification, selector, cancellationToken);
		}

		/// <inheritdoc />
		async Task<long> ICanAggregate<TAggregateRoot, TKey>.SumAsync(ISpecification<TAggregateRoot> specification,
			Expression<Func<TAggregateRoot, long?>> selector, CancellationToken cancellationToken)
		{
			return await this.innerRepository.SumAsync(specification, selector, cancellationToken);
		}

		/// <inheritdoc />
		async Task<decimal> ICanAggregate<TAggregateRoot, TKey>.SumAsync(ISpecification<TAggregateRoot> specification,
			Expression<Func<TAggregateRoot, decimal>> selector, CancellationToken cancellationToken)
		{
			return await this.innerRepository.SumAsync(specification, selector, cancellationToken);
		}

		/// <inheritdoc />
		async Task<decimal> ICanAggregate<TAggregateRoot, TKey>.SumAsync(ISpecification<TAggregateRoot> specification,
			Expression<Func<TAggregateRoot, decimal?>> selector, CancellationToken cancellationToken)
		{
			return await this.innerRepository.SumAsync(specification, selector, cancellationToken);
		}

		/// <inheritdoc />
		async Task<float> ICanAggregate<TAggregateRoot, TKey>.SumAsync(ISpecification<TAggregateRoot> specification,
			Expression<Func<TAggregateRoot, float>> selector, CancellationToken cancellationToken)
		{
			return await this.innerRepository.SumAsync(specification, selector, cancellationToken);
		}

		/// <inheritdoc />
		async Task<float> ICanAggregate<TAggregateRoot, TKey>.SumAsync(ISpecification<TAggregateRoot> specification,
			Expression<Func<TAggregateRoot, float?>> selector, CancellationToken cancellationToken)
		{
			return await this.innerRepository.SumAsync(specification, selector, cancellationToken);
		}

		/// <inheritdoc />
		async Task<double> ICanAggregate<TAggregateRoot, TKey>.SumAsync(ISpecification<TAggregateRoot> specification,
			Expression<Func<TAggregateRoot, double>> selector, CancellationToken cancellationToken)
		{
			return await this.innerRepository.SumAsync(specification, selector, cancellationToken);
		}

		/// <inheritdoc />
		async Task<double> ICanAggregate<TAggregateRoot, TKey>.SumAsync(ISpecification<TAggregateRoot> specification,
			Expression<Func<TAggregateRoot, double?>> selector, CancellationToken cancellationToken)
		{
			return await this.innerRepository.SumAsync(specification, selector, cancellationToken);
		}

		/// <inheritdoc />
		async Task<double> ICanAggregate<TAggregateRoot, TKey>.AverageAsync(Expression<Func<TAggregateRoot, int>> selector, CancellationToken cancellationToken)
		{
			return await this.innerRepository.AverageAsync(selector, cancellationToken);
		}

		/// <inheritdoc />
		async Task<double> ICanAggregate<TAggregateRoot, TKey>.AverageAsync(Expression<Func<TAggregateRoot, int?>> selector,
			CancellationToken cancellationToken)
		{
			return await this.innerRepository.AverageAsync(selector, cancellationToken);
		}

		/// <inheritdoc />
		async Task<double> ICanAggregate<TAggregateRoot, TKey>.AverageAsync(Expression<Func<TAggregateRoot, long>> selector,
			CancellationToken cancellationToken)
		{
			return await this.innerRepository.AverageAsync(selector, cancellationToken);
		}

		/// <inheritdoc />
		async Task<double> ICanAggregate<TAggregateRoot, TKey>.AverageAsync(Expression<Func<TAggregateRoot, long?>> selector,
			CancellationToken cancellationToken)
		{
			return await this.innerRepository.AverageAsync(selector, cancellationToken);
		}

		/// <inheritdoc />
		async Task<decimal> ICanAggregate<TAggregateRoot, TKey>.AverageAsync(Expression<Func<TAggregateRoot, decimal>> selector,
			CancellationToken cancellationToken)
		{
			return await this.innerRepository.AverageAsync(selector, cancellationToken);
		}

		/// <inheritdoc />
		async Task<decimal> ICanAggregate<TAggregateRoot, TKey>.AverageAsync(Expression<Func<TAggregateRoot, decimal?>> selector,
			CancellationToken cancellationToken)
		{
			return await this.innerRepository.AverageAsync(selector, cancellationToken);
		}

		/// <inheritdoc />
		async Task<float> ICanAggregate<TAggregateRoot, TKey>.AverageAsync(Expression<Func<TAggregateRoot, float>> selector,
			CancellationToken cancellationToken)
		{
			return await this.innerRepository.AverageAsync(selector, cancellationToken);
		}

		/// <inheritdoc />
		async Task<float> ICanAggregate<TAggregateRoot, TKey>.AverageAsync(Expression<Func<TAggregateRoot, float?>> selector,
			CancellationToken cancellationToken)
		{
			return await this.innerRepository.AverageAsync(selector, cancellationToken);
		}

		/// <inheritdoc />
		async Task<double> ICanAggregate<TAggregateRoot, TKey>.AverageAsync(Expression<Func<TAggregateRoot, double>> selector,
			CancellationToken cancellationToken)
		{
			return await this.innerRepository.AverageAsync(selector, cancellationToken);
		}

		/// <inheritdoc />
		async Task<double> ICanAggregate<TAggregateRoot, TKey>.AverageAsync(Expression<Func<TAggregateRoot, double?>> selector,
			CancellationToken cancellationToken)
		{
			return await this.innerRepository.AverageAsync(selector, cancellationToken);
		}

		/// <inheritdoc />
		async Task<double> ICanAggregate<TAggregateRoot, TKey>.AverageAsync(Expression<Func<TAggregateRoot, bool>> predicate,
			Expression<Func<TAggregateRoot, int>> selector, CancellationToken cancellationToken)
		{
			return await this.innerRepository.AverageAsync(predicate, selector, cancellationToken);
		}

		/// <inheritdoc />
		async Task<double> ICanAggregate<TAggregateRoot, TKey>.AverageAsync(Expression<Func<TAggregateRoot, bool>> predicate,
			Expression<Func<TAggregateRoot, int?>> selector, CancellationToken cancellationToken)
		{
			return await this.innerRepository.AverageAsync(predicate, selector, cancellationToken);
		}

		/// <inheritdoc />
		async Task<double> ICanAggregate<TAggregateRoot, TKey>.AverageAsync(Expression<Func<TAggregateRoot, bool>> predicate,
			Expression<Func<TAggregateRoot, long>> selector, CancellationToken cancellationToken)
		{
			return await this.innerRepository.AverageAsync(predicate, selector, cancellationToken);
		}

		/// <inheritdoc />
		async Task<double> ICanAggregate<TAggregateRoot, TKey>.AverageAsync(Expression<Func<TAggregateRoot, bool>> predicate,
			Expression<Func<TAggregateRoot, long?>> selector, CancellationToken cancellationToken)
		{
			return await this.innerRepository.AverageAsync(predicate, selector, cancellationToken);
		}

		/// <inheritdoc />
		async Task<decimal> ICanAggregate<TAggregateRoot, TKey>.AverageAsync(Expression<Func<TAggregateRoot, bool>> predicate,
			Expression<Func<TAggregateRoot, decimal>> selector, CancellationToken cancellationToken)
		{
			return await this.innerRepository.AverageAsync(predicate, selector, cancellationToken);
		}

		/// <inheritdoc />
		async Task<decimal> ICanAggregate<TAggregateRoot, TKey>.AverageAsync(Expression<Func<TAggregateRoot, bool>> predicate,
			Expression<Func<TAggregateRoot, decimal?>> selector, CancellationToken cancellationToken)
		{
			return await this.innerRepository.AverageAsync(predicate, selector, cancellationToken);
		}

		/// <inheritdoc />
		async Task<float> ICanAggregate<TAggregateRoot, TKey>.AverageAsync(Expression<Func<TAggregateRoot, bool>> predicate,
			Expression<Func<TAggregateRoot, float>> selector, CancellationToken cancellationToken)
		{
			return await this.innerRepository.AverageAsync(predicate, selector, cancellationToken);
		}

		/// <inheritdoc />
		async Task<float> ICanAggregate<TAggregateRoot, TKey>.AverageAsync(Expression<Func<TAggregateRoot, bool>> predicate,
			Expression<Func<TAggregateRoot, float?>> selector, CancellationToken cancellationToken)
		{
			return await this.innerRepository.AverageAsync(predicate, selector, cancellationToken);
		}

		/// <inheritdoc />
		async Task<double> ICanAggregate<TAggregateRoot, TKey>.AverageAsync(Expression<Func<TAggregateRoot, bool>> predicate,
			Expression<Func<TAggregateRoot, double>> selector, CancellationToken cancellationToken)
		{
			return await this.innerRepository.AverageAsync(predicate, selector, cancellationToken);
		}

		/// <inheritdoc />
		async Task<double> ICanAggregate<TAggregateRoot, TKey>.AverageAsync(Expression<Func<TAggregateRoot, bool>> predicate,
			Expression<Func<TAggregateRoot, double?>> selector, CancellationToken cancellationToken)
		{
			return await this.innerRepository.AverageAsync(predicate, selector, cancellationToken);
		}

		/// <inheritdoc />
		async Task<double> ICanAggregate<TAggregateRoot, TKey>.AverageAsync(ISpecification<TAggregateRoot> specification,
			Expression<Func<TAggregateRoot, int>> selector, CancellationToken cancellationToken)
		{
			return await this.innerRepository.AverageAsync(specification, selector, cancellationToken);
		}

		/// <inheritdoc />
		async Task<double> ICanAggregate<TAggregateRoot, TKey>.AverageAsync(ISpecification<TAggregateRoot> specification,
			Expression<Func<TAggregateRoot, int?>> selector, CancellationToken cancellationToken)
		{
			return await this.innerRepository.AverageAsync(specification, selector, cancellationToken);
		}

		/// <inheritdoc />
		async Task<double> ICanAggregate<TAggregateRoot, TKey>.AverageAsync(ISpecification<TAggregateRoot> specification,
			Expression<Func<TAggregateRoot, long>> selector, CancellationToken cancellationToken)
		{
			return await this.innerRepository.AverageAsync(specification, selector, cancellationToken);
		}

		/// <inheritdoc />
		async Task<double> ICanAggregate<TAggregateRoot, TKey>.AverageAsync(ISpecification<TAggregateRoot> specification,
			Expression<Func<TAggregateRoot, long?>> selector, CancellationToken cancellationToken)
		{
			return await this.innerRepository.AverageAsync(specification, selector, cancellationToken);
		}

		/// <inheritdoc />
		async Task<decimal> ICanAggregate<TAggregateRoot, TKey>.AverageAsync(ISpecification<TAggregateRoot> specification,
			Expression<Func<TAggregateRoot, decimal>> selector, CancellationToken cancellationToken)
		{
			return await this.innerRepository.AverageAsync(specification, selector, cancellationToken);
		}

		/// <inheritdoc />
		async Task<decimal> ICanAggregate<TAggregateRoot, TKey>.AverageAsync(ISpecification<TAggregateRoot> specification,
			Expression<Func<TAggregateRoot, decimal?>> selector, CancellationToken cancellationToken)
		{
			return await this.innerRepository.AverageAsync(specification, selector, cancellationToken);
		}

		/// <inheritdoc />
		async Task<float> ICanAggregate<TAggregateRoot, TKey>.AverageAsync(ISpecification<TAggregateRoot> specification,
			Expression<Func<TAggregateRoot, float>> selector, CancellationToken cancellationToken)
		{
			return await this.innerRepository.AverageAsync(specification, selector, cancellationToken);
		}

		/// <inheritdoc />
		async Task<float> ICanAggregate<TAggregateRoot, TKey>.AverageAsync(ISpecification<TAggregateRoot> specification,
			Expression<Func<TAggregateRoot, float?>> selector, CancellationToken cancellationToken)
		{
			return await this.innerRepository.AverageAsync(specification, selector, cancellationToken);
		}

		/// <inheritdoc />
		async Task<double> ICanAggregate<TAggregateRoot, TKey>.AverageAsync(ISpecification<TAggregateRoot> specification,
			Expression<Func<TAggregateRoot, double>> selector, CancellationToken cancellationToken)
		{
			return await this.innerRepository.AverageAsync(specification, selector, cancellationToken);
		}

		/// <inheritdoc />
		async Task<double> ICanAggregate<TAggregateRoot, TKey>.AverageAsync(ISpecification<TAggregateRoot> specification,
			Expression<Func<TAggregateRoot, double?>> selector, CancellationToken cancellationToken)
		{
			return await this.innerRepository.AverageAsync(specification, selector, cancellationToken);
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
		public RepositoryName RepositoryName => this.innerRepository.RepositoryName;

		/// <inheritdoc />
		public override string ToString()
		{
			return this.innerRepository.ToString();
		}

		private async Task DispatchDomainEventsAsync(TAggregateRoot item)
		{
			if(this.domainEventsOptions.IsEnabled)
			{
				this.logger.LogDispatchedDomainEvents(typeof(TAggregateRoot).Name, item.DomainEvents.Count);

				foreach(IDomainEvent domainEvent in item.DomainEvents)
				{
					await this.domainEventDispatcher.DispatchAsync(domainEvent).ConfigureAwait(false);
				}

				item.ClearDomainEvents();
			}

			// We need to flush the domain event outbox immediately if the UoW is disabled.
			if(!this.repositoryOptions.IsUnitOfWorkEnabled)
			{
				await this.FlushDomainEventsOutboxAsync();
			}
		}

		private async Task DispatchDomainEventsAsync(IEnumerable<TAggregateRoot> items)
		{
			if(this.domainEventsOptions.IsEnabled)
			{
				foreach(TAggregateRoot item in items)
				{
					this.logger.LogDispatchedDomainEvents(typeof(TAggregateRoot).Name, item.DomainEvents.Count);

					foreach(IDomainEvent domainEvent in item.DomainEvents)
					{
						await this.domainEventDispatcher.DispatchAsync(domainEvent).ConfigureAwait(false);
					}

					item.ClearDomainEvents();
				}
			}

			// We need to flush the domain event outbox immediately if the UoW is disabled.
			if(!this.repositoryOptions.IsUnitOfWorkEnabled)
			{
				await this.FlushDomainEventsOutboxAsync();
			}
		}

		private async Task FlushDomainEventsOutboxAsync()
		{
			try
			{
				await this.domainEventDispatcher.FlushAsync();
			}
			finally
			{
				this.domainEventDispatcher.Clear();
			}
		}
	}
}
