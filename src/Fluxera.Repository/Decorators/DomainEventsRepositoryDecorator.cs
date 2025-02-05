namespace Fluxera.Repository.Decorators
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Linq.Expressions;
	using System.Threading;
	using System.Threading.Tasks;
	using Fluxera.Entity;
	using Fluxera.DomainEvents.Abstractions;
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
	/// <typeparam name="TEntity"></typeparam>
	/// <typeparam name="TKey"></typeparam>
	public sealed class DomainEventsRepositoryDecorator<TEntity, TKey> : IRepository<TEntity, TKey>
		where TEntity : Entity<TEntity, TKey>
		where TKey : notnull, IComparable<TKey>, IEquatable<TKey>
	{
		private readonly ILogger logger;

		private readonly IRepository<TEntity, TKey> innerRepository;
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
			IRepository<TEntity, TKey> innerRepository,
			IOutboxDomainEventDispatcher domainEventDispatcher,
			IRepositoryRegistry repositoryRegistry,
			ILoggerFactory loggerFactory)
		{
			this.innerRepository = Guard.Against.Null(innerRepository);
			this.domainEventDispatcher = Guard.Against.Null(domainEventDispatcher);
			Guard.Against.Null(loggerFactory);
			Guard.Against.Null(repositoryRegistry);

			this.logger = loggerFactory.CreateLogger(LoggerNames.EventsDispatcher);

			RepositoryName repositoryName = repositoryRegistry.GetRepositoryNameFor<TEntity>();
			this.repositoryOptions = repositoryRegistry.GetRepositoryOptionsFor(repositoryName);
			this.domainEventsOptions = this.repositoryOptions.DomainEventsOptions;
		}

		/// <inheritdoc />
		async Task ICanAdd<TEntity, TKey>.AddAsync(TEntity item, CancellationToken cancellationToken)
		{
			await this.innerRepository.AddAsync(item, cancellationToken).ConfigureAwait(false);

			await this.DispatchDomainEventsAsync(item).ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task ICanAdd<TEntity, TKey>.AddRangeAsync(IEnumerable<TEntity> items, CancellationToken cancellationToken)
		{
			IEnumerable<TEntity> itemsList = items.ToList();

			await this.innerRepository.AddRangeAsync(itemsList, cancellationToken).ConfigureAwait(false);

			await this.DispatchDomainEventsAsync(itemsList).ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task ICanUpdate<TEntity, TKey>.UpdateAsync(TEntity item, CancellationToken cancellationToken)
		{
			await this.innerRepository.UpdateAsync(item, cancellationToken).ConfigureAwait(false);

			await this.DispatchDomainEventsAsync(item).ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task ICanUpdate<TEntity, TKey>.UpdateRangeAsync(IEnumerable<TEntity> items, CancellationToken cancellationToken)
		{
			IEnumerable<TEntity> itemsList = items.ToList();

			await this.innerRepository.UpdateRangeAsync(itemsList, cancellationToken).ConfigureAwait(false);

			await this.DispatchDomainEventsAsync(itemsList).ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task ICanRemove<TEntity, TKey>.RemoveAsync(TEntity item, CancellationToken cancellationToken)
		{
			await this.innerRepository.RemoveAsync(item, cancellationToken).ConfigureAwait(false);

			await this.DispatchDomainEventsAsync(item).ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task ICanRemove<TEntity, TKey>.RemoveRangeAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken)
		{
			IReadOnlyCollection<TEntity> items = await this.innerRepository.FindManyAsync(predicate, cancellationToken: cancellationToken).ConfigureAwait(false);

			await this.innerRepository.RemoveRangeAsync(predicate, cancellationToken).ConfigureAwait(false);

			await this.DispatchDomainEventsAsync(items).ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task ICanRemove<TEntity, TKey>.RemoveRangeAsync(ISpecification<TEntity> specification, CancellationToken cancellationToken)
		{
			IReadOnlyCollection<TEntity> items = await this.innerRepository.FindManyAsync(specification, cancellationToken: cancellationToken).ConfigureAwait(false);

			await this.innerRepository.RemoveRangeAsync(specification, cancellationToken).ConfigureAwait(false);

			await this.DispatchDomainEventsAsync(items).ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task ICanRemove<TEntity, TKey>.RemoveRangeAsync(IEnumerable<TEntity> items, CancellationToken cancellationToken)
		{
			IEnumerable<TEntity> itemsList = items.ToList();

			await this.innerRepository.RemoveRangeAsync(itemsList, cancellationToken).ConfigureAwait(false);

			await this.DispatchDomainEventsAsync(itemsList).ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task ICanRemove<TEntity, TKey>.RemoveAsync(TKey id, CancellationToken cancellationToken)
		{
			TEntity item = await this.innerRepository.GetAsync(id, cancellationToken).ConfigureAwait(false);

			await this.innerRepository.RemoveAsync(id, cancellationToken).ConfigureAwait(false);

			await this.DispatchDomainEventsAsync(item).ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task<TEntity> ICanGet<TEntity, TKey>.GetAsync(TKey id, CancellationToken cancellationToken)
		{
			return await this.innerRepository.GetAsync(id, cancellationToken).ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task<TResult> ICanGet<TEntity, TKey>.GetAsync<TResult>(TKey id, Expression<Func<TEntity, TResult>> selector, 
			CancellationToken cancellationToken)
		{
			return await this.innerRepository.GetAsync(id, selector, cancellationToken).ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task<bool> ICanGet<TEntity, TKey>.ExistsAsync(TKey id, CancellationToken cancellationToken)
		{
			return await this.innerRepository.ExistsAsync(id, cancellationToken).ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task<TEntity> ICanFind<TEntity, TKey>.FindOneAsync(Expression<Func<TEntity, bool>> predicate, 
			IQueryOptions<TEntity> queryOptions, CancellationToken cancellationToken)
		{
			return await this.innerRepository.FindOneAsync(predicate, queryOptions, cancellationToken).ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task<TEntity> ICanFind<TEntity, TKey>.FindOneAsync(ISpecification<TEntity> specification,
			IQueryOptions<TEntity> queryOptions, CancellationToken cancellationToken)
		{
			return await this.innerRepository.FindOneAsync(specification, queryOptions, cancellationToken);
		}

		/// <inheritdoc />
		async Task<TResult> ICanFind<TEntity, TKey>.FindOneAsync<TResult>(Expression<Func<TEntity, bool>> predicate,
			Expression<Func<TEntity, TResult>> selector, IQueryOptions<TEntity> queryOptions, CancellationToken cancellationToken)
		{
			return await this.innerRepository.FindOneAsync(predicate, selector, queryOptions, cancellationToken).ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task<TResult> ICanFind<TEntity, TKey>.FindOneAsync<TResult>(ISpecification<TEntity> specification,
			Expression<Func<TEntity, TResult>> selector, IQueryOptions<TEntity> queryOptions, CancellationToken cancellationToken)
		{
			return await this.innerRepository.FindOneAsync(specification, selector, queryOptions, cancellationToken);
		}

		/// <inheritdoc />
		async Task<bool> ICanFind<TEntity, TKey>.ExistsAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken)
		{
			return await this.innerRepository.ExistsAsync(predicate, cancellationToken).ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task<bool> ICanFind<TEntity, TKey>.ExistsAsync(ISpecification<TEntity> specification, CancellationToken cancellationToken)
		{
			return await this.innerRepository.ExistsAsync(specification, cancellationToken);
		}

		/// <inheritdoc />
		async Task<IReadOnlyCollection<TEntity>> ICanFind<TEntity, TKey>.FindManyAsync(Expression<Func<TEntity, bool>> predicate,
			IQueryOptions<TEntity> queryOptions, CancellationToken cancellationToken)
		{
			return await this.innerRepository.FindManyAsync(predicate, queryOptions, cancellationToken).ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task<IReadOnlyCollection<TEntity>> ICanFind<TEntity, TKey>.FindManyAsync(ISpecification<TEntity> specification,
			IQueryOptions<TEntity> queryOptions, CancellationToken cancellationToken)
		{
			return await this.innerRepository.FindManyAsync(specification, queryOptions, cancellationToken);
		}

		/// <inheritdoc />
		async Task<IReadOnlyCollection<TResult>> ICanFind<TEntity, TKey>.FindManyAsync<TResult>(Expression<Func<TEntity, bool>> predicate,
			Expression<Func<TEntity, TResult>> selector, IQueryOptions<TEntity> queryOptions, CancellationToken cancellationToken)
		{
			return await this.innerRepository.FindManyAsync(predicate, selector, queryOptions, cancellationToken).ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task<IReadOnlyCollection<TResult>> ICanFind<TEntity, TKey>.FindManyAsync<TResult>(ISpecification<TEntity> specification,
			Expression<Func<TEntity, TResult>> selector, IQueryOptions<TEntity> queryOptions, CancellationToken cancellationToken)
		{
			return await this.innerRepository.FindManyAsync(specification, selector, queryOptions, cancellationToken);
		}

		/// <inheritdoc />
		async Task<long> ICanGet<TEntity, TKey>.CountAsync(CancellationToken cancellationToken)
		{
			return await this.innerRepository.CountAsync(cancellationToken).ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task<long> ICanGet<TEntity, TKey>.CountAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken)
		{
			return await this.innerRepository.CountAsync(predicate, cancellationToken).ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task<long> ICanGet<TEntity, TKey>.CountAsync(ISpecification<TEntity> specification, CancellationToken cancellationToken)
		{
			return await this.innerRepository.CountAsync(specification, cancellationToken);
		}

		/// <inheritdoc />
		async Task<int> ICanAggregate<TEntity, TKey>.SumAsync(Expression<Func<TEntity, int>> selector, CancellationToken cancellationToken)
		{
			return await this.innerRepository.SumAsync(selector, cancellationToken);
		}

		/// <inheritdoc />
		async Task<int> ICanAggregate<TEntity, TKey>.SumAsync(Expression<Func<TEntity, int?>> selector, CancellationToken cancellationToken)
		{
			return await this.innerRepository.SumAsync(selector, cancellationToken);
		}

		/// <inheritdoc />
		async Task<long> ICanAggregate<TEntity, TKey>.SumAsync(Expression<Func<TEntity, long>> selector, CancellationToken cancellationToken)
		{
			return await this.innerRepository.SumAsync(selector, cancellationToken);
		}

		/// <inheritdoc />
		async Task<long> ICanAggregate<TEntity, TKey>.SumAsync(Expression<Func<TEntity, long?>> selector, CancellationToken cancellationToken)
		{
			return await this.innerRepository.SumAsync(selector, cancellationToken);
		}

		/// <inheritdoc />
		async Task<decimal> ICanAggregate<TEntity, TKey>.SumAsync(Expression<Func<TEntity, decimal>> selector,
			CancellationToken cancellationToken)
		{
			return await this.innerRepository.SumAsync(selector, cancellationToken);
		}

		/// <inheritdoc />
		async Task<decimal> ICanAggregate<TEntity, TKey>.SumAsync(Expression<Func<TEntity, decimal?>> selector,
			CancellationToken cancellationToken)
		{
			return await this.innerRepository.SumAsync(selector, cancellationToken);
		}

		/// <inheritdoc />
		async Task<float> ICanAggregate<TEntity, TKey>.SumAsync(Expression<Func<TEntity, float>> selector, CancellationToken cancellationToken)
		{
			return await this.innerRepository.SumAsync(selector, cancellationToken);
		}

		/// <inheritdoc />
		async Task<float> ICanAggregate<TEntity, TKey>.SumAsync(Expression<Func<TEntity, float?>> selector, CancellationToken cancellationToken)
		{
			return await this.innerRepository.SumAsync(selector, cancellationToken);
		}

		/// <inheritdoc />
		async Task<double> ICanAggregate<TEntity, TKey>.SumAsync(Expression<Func<TEntity, double>> selector, CancellationToken cancellationToken)
		{
			return await this.innerRepository.SumAsync(selector, cancellationToken);
		}

		/// <inheritdoc />
		async Task<double> ICanAggregate<TEntity, TKey>.SumAsync(Expression<Func<TEntity, double?>> selector, CancellationToken cancellationToken)
		{
			return await this.innerRepository.SumAsync(selector, cancellationToken);
		}

		/// <inheritdoc />
		async Task<int> ICanAggregate<TEntity, TKey>.SumAsync(Expression<Func<TEntity, bool>> predicate,
			Expression<Func<TEntity, int>> selector, CancellationToken cancellationToken)
		{
			return await this.innerRepository.SumAsync(predicate, selector, cancellationToken);
		}

		/// <inheritdoc />
		async Task<int> ICanAggregate<TEntity, TKey>.SumAsync(Expression<Func<TEntity, bool>> predicate,
			Expression<Func<TEntity, int?>> selector, CancellationToken cancellationToken)
		{
			return await this.innerRepository.SumAsync(predicate, selector, cancellationToken);
		}

		/// <inheritdoc />
		async Task<long> ICanAggregate<TEntity, TKey>.SumAsync(Expression<Func<TEntity, bool>> predicate,
			Expression<Func<TEntity, long>> selector, CancellationToken cancellationToken)
		{
			return await this.innerRepository.SumAsync(predicate, selector, cancellationToken);
		}

		/// <inheritdoc />
		async Task<long> ICanAggregate<TEntity, TKey>.SumAsync(Expression<Func<TEntity, bool>> predicate,
			Expression<Func<TEntity, long?>> selector, CancellationToken cancellationToken)
		{
			return await this.innerRepository.SumAsync(predicate, selector, cancellationToken);
		}

		/// <inheritdoc />
		async Task<decimal> ICanAggregate<TEntity, TKey>.SumAsync(Expression<Func<TEntity, bool>> predicate,
			Expression<Func<TEntity, decimal>> selector, CancellationToken cancellationToken)
		{
			return await this.innerRepository.SumAsync(predicate, selector, cancellationToken);
		}

		/// <inheritdoc />
		async Task<decimal> ICanAggregate<TEntity, TKey>.SumAsync(Expression<Func<TEntity, bool>> predicate,
			Expression<Func<TEntity, decimal?>> selector, CancellationToken cancellationToken)
		{
			return await this.innerRepository.SumAsync(predicate, selector, cancellationToken);
		}

		/// <inheritdoc />
		async Task<float> ICanAggregate<TEntity, TKey>.SumAsync(Expression<Func<TEntity, bool>> predicate,
			Expression<Func<TEntity, float>> selector, CancellationToken cancellationToken)
		{
			return await this.innerRepository.SumAsync(predicate, selector, cancellationToken);
		}

		/// <inheritdoc />
		async Task<float> ICanAggregate<TEntity, TKey>.SumAsync(Expression<Func<TEntity, bool>> predicate,
			Expression<Func<TEntity, float?>> selector, CancellationToken cancellationToken)
		{
			return await this.innerRepository.SumAsync(predicate, selector, cancellationToken);
		}

		/// <inheritdoc />
		async Task<double> ICanAggregate<TEntity, TKey>.SumAsync(Expression<Func<TEntity, bool>> predicate,
			Expression<Func<TEntity, double>> selector, CancellationToken cancellationToken)
		{
			return await this.innerRepository.SumAsync(predicate, selector, cancellationToken);
		}

		/// <inheritdoc />
		async Task<double> ICanAggregate<TEntity, TKey>.SumAsync(Expression<Func<TEntity, bool>> predicate,
			Expression<Func<TEntity, double?>> selector, CancellationToken cancellationToken)
		{
			return await this.innerRepository.SumAsync(predicate, selector, cancellationToken);
		}

		/// <inheritdoc />
		async Task<int> ICanAggregate<TEntity, TKey>.SumAsync(ISpecification<TEntity> specification,
			Expression<Func<TEntity, int>> selector, CancellationToken cancellationToken)
		{
			return await this.innerRepository.SumAsync(specification, selector, cancellationToken);
		}

		/// <inheritdoc />
		async Task<int> ICanAggregate<TEntity, TKey>.SumAsync(ISpecification<TEntity> specification,
			Expression<Func<TEntity, int?>> selector, CancellationToken cancellationToken)
		{
			return await this.innerRepository.SumAsync(specification, selector, cancellationToken);
		}

		/// <inheritdoc />
		async Task<long> ICanAggregate<TEntity, TKey>.SumAsync(ISpecification<TEntity> specification,
			Expression<Func<TEntity, long>> selector, CancellationToken cancellationToken)
		{
			return await this.innerRepository.SumAsync(specification, selector, cancellationToken);
		}

		/// <inheritdoc />
		async Task<long> ICanAggregate<TEntity, TKey>.SumAsync(ISpecification<TEntity> specification,
			Expression<Func<TEntity, long?>> selector, CancellationToken cancellationToken)
		{
			return await this.innerRepository.SumAsync(specification, selector, cancellationToken);
		}

		/// <inheritdoc />
		async Task<decimal> ICanAggregate<TEntity, TKey>.SumAsync(ISpecification<TEntity> specification,
			Expression<Func<TEntity, decimal>> selector, CancellationToken cancellationToken)
		{
			return await this.innerRepository.SumAsync(specification, selector, cancellationToken);
		}

		/// <inheritdoc />
		async Task<decimal> ICanAggregate<TEntity, TKey>.SumAsync(ISpecification<TEntity> specification,
			Expression<Func<TEntity, decimal?>> selector, CancellationToken cancellationToken)
		{
			return await this.innerRepository.SumAsync(specification, selector, cancellationToken);
		}

		/// <inheritdoc />
		async Task<float> ICanAggregate<TEntity, TKey>.SumAsync(ISpecification<TEntity> specification,
			Expression<Func<TEntity, float>> selector, CancellationToken cancellationToken)
		{
			return await this.innerRepository.SumAsync(specification, selector, cancellationToken);
		}

		/// <inheritdoc />
		async Task<float> ICanAggregate<TEntity, TKey>.SumAsync(ISpecification<TEntity> specification,
			Expression<Func<TEntity, float?>> selector, CancellationToken cancellationToken)
		{
			return await this.innerRepository.SumAsync(specification, selector, cancellationToken);
		}

		/// <inheritdoc />
		async Task<double> ICanAggregate<TEntity, TKey>.SumAsync(ISpecification<TEntity> specification,
			Expression<Func<TEntity, double>> selector, CancellationToken cancellationToken)
		{
			return await this.innerRepository.SumAsync(specification, selector, cancellationToken);
		}

		/// <inheritdoc />
		async Task<double> ICanAggregate<TEntity, TKey>.SumAsync(ISpecification<TEntity> specification,
			Expression<Func<TEntity, double?>> selector, CancellationToken cancellationToken)
		{
			return await this.innerRepository.SumAsync(specification, selector, cancellationToken);
		}

		/// <inheritdoc />
		async Task<double> ICanAggregate<TEntity, TKey>.AverageAsync(Expression<Func<TEntity, int>> selector, CancellationToken cancellationToken)
		{
			return await this.innerRepository.AverageAsync(selector, cancellationToken);
		}

		/// <inheritdoc />
		async Task<double> ICanAggregate<TEntity, TKey>.AverageAsync(Expression<Func<TEntity, int?>> selector,
			CancellationToken cancellationToken)
		{
			return await this.innerRepository.AverageAsync(selector, cancellationToken);
		}

		/// <inheritdoc />
		async Task<double> ICanAggregate<TEntity, TKey>.AverageAsync(Expression<Func<TEntity, long>> selector,
			CancellationToken cancellationToken)
		{
			return await this.innerRepository.AverageAsync(selector, cancellationToken);
		}

		/// <inheritdoc />
		async Task<double> ICanAggregate<TEntity, TKey>.AverageAsync(Expression<Func<TEntity, long?>> selector,
			CancellationToken cancellationToken)
		{
			return await this.innerRepository.AverageAsync(selector, cancellationToken);
		}

		/// <inheritdoc />
		async Task<decimal> ICanAggregate<TEntity, TKey>.AverageAsync(Expression<Func<TEntity, decimal>> selector,
			CancellationToken cancellationToken)
		{
			return await this.innerRepository.AverageAsync(selector, cancellationToken);
		}

		/// <inheritdoc />
		async Task<decimal> ICanAggregate<TEntity, TKey>.AverageAsync(Expression<Func<TEntity, decimal?>> selector,
			CancellationToken cancellationToken)
		{
			return await this.innerRepository.AverageAsync(selector, cancellationToken);
		}

		/// <inheritdoc />
		async Task<float> ICanAggregate<TEntity, TKey>.AverageAsync(Expression<Func<TEntity, float>> selector,
			CancellationToken cancellationToken)
		{
			return await this.innerRepository.AverageAsync(selector, cancellationToken);
		}

		/// <inheritdoc />
		async Task<float> ICanAggregate<TEntity, TKey>.AverageAsync(Expression<Func<TEntity, float?>> selector,
			CancellationToken cancellationToken)
		{
			return await this.innerRepository.AverageAsync(selector, cancellationToken);
		}

		/// <inheritdoc />
		async Task<double> ICanAggregate<TEntity, TKey>.AverageAsync(Expression<Func<TEntity, double>> selector,
			CancellationToken cancellationToken)
		{
			return await this.innerRepository.AverageAsync(selector, cancellationToken);
		}

		/// <inheritdoc />
		async Task<double> ICanAggregate<TEntity, TKey>.AverageAsync(Expression<Func<TEntity, double?>> selector,
			CancellationToken cancellationToken)
		{
			return await this.innerRepository.AverageAsync(selector, cancellationToken);
		}

		/// <inheritdoc />
		async Task<double> ICanAggregate<TEntity, TKey>.AverageAsync(Expression<Func<TEntity, bool>> predicate,
			Expression<Func<TEntity, int>> selector, CancellationToken cancellationToken)
		{
			return await this.innerRepository.AverageAsync(predicate, selector, cancellationToken);
		}

		/// <inheritdoc />
		async Task<double> ICanAggregate<TEntity, TKey>.AverageAsync(Expression<Func<TEntity, bool>> predicate,
			Expression<Func<TEntity, int?>> selector, CancellationToken cancellationToken)
		{
			return await this.innerRepository.AverageAsync(predicate, selector, cancellationToken);
		}

		/// <inheritdoc />
		async Task<double> ICanAggregate<TEntity, TKey>.AverageAsync(Expression<Func<TEntity, bool>> predicate,
			Expression<Func<TEntity, long>> selector, CancellationToken cancellationToken)
		{
			return await this.innerRepository.AverageAsync(predicate, selector, cancellationToken);
		}

		/// <inheritdoc />
		async Task<double> ICanAggregate<TEntity, TKey>.AverageAsync(Expression<Func<TEntity, bool>> predicate,
			Expression<Func<TEntity, long?>> selector, CancellationToken cancellationToken)
		{
			return await this.innerRepository.AverageAsync(predicate, selector, cancellationToken);
		}

		/// <inheritdoc />
		async Task<decimal> ICanAggregate<TEntity, TKey>.AverageAsync(Expression<Func<TEntity, bool>> predicate,
			Expression<Func<TEntity, decimal>> selector, CancellationToken cancellationToken)
		{
			return await this.innerRepository.AverageAsync(predicate, selector, cancellationToken);
		}

		/// <inheritdoc />
		async Task<decimal> ICanAggregate<TEntity, TKey>.AverageAsync(Expression<Func<TEntity, bool>> predicate,
			Expression<Func<TEntity, decimal?>> selector, CancellationToken cancellationToken)
		{
			return await this.innerRepository.AverageAsync(predicate, selector, cancellationToken);
		}

		/// <inheritdoc />
		async Task<float> ICanAggregate<TEntity, TKey>.AverageAsync(Expression<Func<TEntity, bool>> predicate,
			Expression<Func<TEntity, float>> selector, CancellationToken cancellationToken)
		{
			return await this.innerRepository.AverageAsync(predicate, selector, cancellationToken);
		}

		/// <inheritdoc />
		async Task<float> ICanAggregate<TEntity, TKey>.AverageAsync(Expression<Func<TEntity, bool>> predicate,
			Expression<Func<TEntity, float?>> selector, CancellationToken cancellationToken)
		{
			return await this.innerRepository.AverageAsync(predicate, selector, cancellationToken);
		}

		/// <inheritdoc />
		async Task<double> ICanAggregate<TEntity, TKey>.AverageAsync(Expression<Func<TEntity, bool>> predicate,
			Expression<Func<TEntity, double>> selector, CancellationToken cancellationToken)
		{
			return await this.innerRepository.AverageAsync(predicate, selector, cancellationToken);
		}

		/// <inheritdoc />
		async Task<double> ICanAggregate<TEntity, TKey>.AverageAsync(Expression<Func<TEntity, bool>> predicate,
			Expression<Func<TEntity, double?>> selector, CancellationToken cancellationToken)
		{
			return await this.innerRepository.AverageAsync(predicate, selector, cancellationToken);
		}

		/// <inheritdoc />
		async Task<double> ICanAggregate<TEntity, TKey>.AverageAsync(ISpecification<TEntity> specification,
			Expression<Func<TEntity, int>> selector, CancellationToken cancellationToken)
		{
			return await this.innerRepository.AverageAsync(specification, selector, cancellationToken);
		}

		/// <inheritdoc />
		async Task<double> ICanAggregate<TEntity, TKey>.AverageAsync(ISpecification<TEntity> specification,
			Expression<Func<TEntity, int?>> selector, CancellationToken cancellationToken)
		{
			return await this.innerRepository.AverageAsync(specification, selector, cancellationToken);
		}

		/// <inheritdoc />
		async Task<double> ICanAggregate<TEntity, TKey>.AverageAsync(ISpecification<TEntity> specification,
			Expression<Func<TEntity, long>> selector, CancellationToken cancellationToken)
		{
			return await this.innerRepository.AverageAsync(specification, selector, cancellationToken);
		}

		/// <inheritdoc />
		async Task<double> ICanAggregate<TEntity, TKey>.AverageAsync(ISpecification<TEntity> specification,
			Expression<Func<TEntity, long?>> selector, CancellationToken cancellationToken)
		{
			return await this.innerRepository.AverageAsync(specification, selector, cancellationToken);
		}

		/// <inheritdoc />
		async Task<decimal> ICanAggregate<TEntity, TKey>.AverageAsync(ISpecification<TEntity> specification,
			Expression<Func<TEntity, decimal>> selector, CancellationToken cancellationToken)
		{
			return await this.innerRepository.AverageAsync(specification, selector, cancellationToken);
		}

		/// <inheritdoc />
		async Task<decimal> ICanAggregate<TEntity, TKey>.AverageAsync(ISpecification<TEntity> specification,
			Expression<Func<TEntity, decimal?>> selector, CancellationToken cancellationToken)
		{
			return await this.innerRepository.AverageAsync(specification, selector, cancellationToken);
		}

		/// <inheritdoc />
		async Task<float> ICanAggregate<TEntity, TKey>.AverageAsync(ISpecification<TEntity> specification,
			Expression<Func<TEntity, float>> selector, CancellationToken cancellationToken)
		{
			return await this.innerRepository.AverageAsync(specification, selector, cancellationToken);
		}

		/// <inheritdoc />
		async Task<float> ICanAggregate<TEntity, TKey>.AverageAsync(ISpecification<TEntity> specification,
			Expression<Func<TEntity, float?>> selector, CancellationToken cancellationToken)
		{
			return await this.innerRepository.AverageAsync(specification, selector, cancellationToken);
		}

		/// <inheritdoc />
		async Task<double> ICanAggregate<TEntity, TKey>.AverageAsync(ISpecification<TEntity> specification,
			Expression<Func<TEntity, double>> selector, CancellationToken cancellationToken)
		{
			return await this.innerRepository.AverageAsync(specification, selector, cancellationToken);
		}

		/// <inheritdoc />
		async Task<double> ICanAggregate<TEntity, TKey>.AverageAsync(ISpecification<TEntity> specification,
			Expression<Func<TEntity, double?>> selector, CancellationToken cancellationToken)
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

		private async Task DispatchDomainEventsAsync(TEntity item)
		{
			if(this.domainEventsOptions.IsEnabled)
			{
				this.logger.LogDispatchedDomainEvents(typeof(TEntity).Name, item.DomainEvents.Count);

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

		private async Task DispatchDomainEventsAsync(IEnumerable<TEntity> items)
		{
			if(this.domainEventsOptions.IsEnabled)
			{
				foreach(TEntity item in items)
				{
					this.logger.LogDispatchedDomainEvents(typeof(TEntity).Name, item.DomainEvents.Count);

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
