namespace Fluxera.Repository.Decorators
{
	using System;
	using System.Collections.Generic;
	using System.Linq.Expressions;
	using System.Threading;
	using System.Threading.Tasks;
	using Fluxera.Entity;
	using Fluxera.Repository.Query;
	using Fluxera.Repository.Specifications;
	using Fluxera.Repository.Traits;
	using Microsoft.Extensions.Logging;

	/// <summary>
	///     A repository decorator that handles unhandled exceptions of the pipeline and logs the errors.
	/// </summary>
	/// <typeparam name="TAggregateRoot"></typeparam>
	/// <typeparam name="TKey"></typeparam>
	public sealed class ExceptionLoggingRepositoryDecorator<TAggregateRoot, TKey> : IRepository<TAggregateRoot, TKey>
		where TAggregateRoot : AggregateRoot<TAggregateRoot, TKey>
		where TKey : notnull, IComparable<TKey>, IEquatable<TKey>
	{
		private readonly IRepository<TAggregateRoot, TKey> innerRepository;
		private readonly ILogger logger;

		/// <summary>
		///     Creates a new instance of the <see cref="ExceptionLoggingRepositoryDecorator{TAggregateRoot,TKey}" /> type.
		/// </summary>
		/// <param name="innerRepository"></param>
		/// <param name="loggerFactory"></param>
		public ExceptionLoggingRepositoryDecorator(IRepository<TAggregateRoot, TKey> innerRepository, ILoggerFactory loggerFactory)
		{
			this.innerRepository = innerRepository;
			this.logger = loggerFactory.CreateLogger(LoggerNames.Repository);
		}

		/// <inheritdoc />
		async Task ICanAdd<TAggregateRoot, TKey>.AddAsync(TAggregateRoot item, CancellationToken cancellationToken)
		{
			try
			{
				await this.innerRepository.AddAsync(item, cancellationToken);
			}
			catch(Exception ex)
			{
				this.logger.LogCritical(ex, "A critical error occurred trying to perform add: {AggregateRoot}", typeof(TAggregateRoot).Name);
				throw;
			}
		}

		/// <inheritdoc />
		async Task ICanAdd<TAggregateRoot, TKey>.AddRangeAsync(IEnumerable<TAggregateRoot> items, CancellationToken cancellationToken)
		{
			try
			{
				await this.innerRepository.AddRangeAsync(items, cancellationToken);
			}
			catch(Exception ex)
			{
				this.logger.LogCritical(ex, "A critical error occurred trying to perform add: {AggregateRoot}", typeof(TAggregateRoot).Name);
				throw;
			}
		}

		/// <inheritdoc />
		async Task ICanUpdate<TAggregateRoot, TKey>.UpdateAsync(TAggregateRoot item, CancellationToken cancellationToken)
		{
			try
			{
				await this.innerRepository.UpdateAsync(item, cancellationToken);
			}
			catch(Exception ex)
			{
				this.logger.LogCritical(ex, "A critical error occurred trying to perform update: {AggregateRoot}", typeof(TAggregateRoot).Name);
				throw;
			}
		}

		/// <inheritdoc />
		async Task ICanUpdate<TAggregateRoot, TKey>.UpdateRangeAsync(IEnumerable<TAggregateRoot> items, CancellationToken cancellationToken)
		{
			try
			{
				await this.innerRepository.UpdateRangeAsync(items, cancellationToken);
			}
			catch(Exception ex)
			{
				this.logger.LogCritical(ex, "A critical error occurred trying to perform update: {AggregateRoot}", typeof(TAggregateRoot).Name);
				throw;
			}
		}

		/// <inheritdoc />
		async Task ICanRemove<TAggregateRoot, TKey>.RemoveAsync(TAggregateRoot item, CancellationToken cancellationToken)
		{
			try
			{
				await this.innerRepository.RemoveAsync(item, cancellationToken);
			}
			catch(Exception ex)
			{
				this.logger.LogCritical(ex, "A critical error occurred trying to perform remove: {AggregateRoot}", typeof(TAggregateRoot).Name);
				throw;
			}
		}

		/// <inheritdoc />
		async Task ICanRemove<TAggregateRoot, TKey>.RemoveAsync(TKey id, CancellationToken cancellationToken)
		{
			try
			{
				await this.innerRepository.RemoveAsync(id, cancellationToken);
			}
			catch(Exception ex)
			{
				this.logger.LogCritical(ex, "A critical error occurred trying to perform remove: {AggregateRoot}", typeof(TAggregateRoot).Name);
				throw;
			}
		}

		/// <inheritdoc />
		async Task ICanRemove<TAggregateRoot, TKey>.RemoveRangeAsync(Expression<Func<TAggregateRoot, bool>> predicate, CancellationToken cancellationToken)
		{
			try
			{
				await this.innerRepository.RemoveRangeAsync(predicate, cancellationToken);
			}
			catch(Exception ex)
			{
				this.logger.LogCritical(ex, "A critical error occurred trying to perform remove: {AggregateRoot}", typeof(TAggregateRoot).Name);
				throw;
			}
		}

		/// <inheritdoc />
		async Task ICanRemove<TAggregateRoot, TKey>.RemoveRangeAsync(ISpecification<TAggregateRoot> specification, CancellationToken cancellationToken)
		{
			try
			{
				await this.innerRepository.RemoveRangeAsync(specification, cancellationToken);
			}
			catch(Exception ex)
			{
				this.logger.LogCritical(ex, "A critical error occurred trying to perform remove: {AggregateRoot}", typeof(TAggregateRoot).Name);
				throw;
			}
		}

		/// <inheritdoc />
		async Task ICanRemove<TAggregateRoot, TKey>.RemoveRangeAsync(IEnumerable<TAggregateRoot> items, CancellationToken cancellationToken)
		{
			try
			{
				await this.innerRepository.RemoveRangeAsync(items, cancellationToken);
			}
			catch(Exception ex)
			{
				this.logger.LogCritical(ex, "A critical error occurred trying to perform remove: {AggregateRoot}", typeof(TAggregateRoot).Name);
				throw;
			}
		}

		/// <inheritdoc />
		async Task<TAggregateRoot> ICanGet<TAggregateRoot, TKey>.GetAsync(TKey id, CancellationToken cancellationToken)
		{
			try
			{
				return await this.innerRepository.GetAsync(id, cancellationToken);
			}
			catch(Exception ex)
			{
				this.logger.LogCritical(ex, "A critical error occurred trying to perform get: {AggregateRoot}", typeof(TAggregateRoot).Name);
				throw;
			}
		}

		/// <inheritdoc />
		async Task<TResult> ICanGet<TAggregateRoot, TKey>.GetAsync<TResult>(TKey id, Expression<Func<TAggregateRoot, TResult>> selector, CancellationToken cancellationToken)
		{
			try
			{
				return await this.innerRepository.GetAsync(id, selector, cancellationToken);
			}
			catch(Exception ex)
			{
				this.logger.LogCritical(ex, "A critical error occurred trying to perform get: {AggregateRoot}", typeof(TAggregateRoot).Name);
				throw;
			}
		}

		/// <inheritdoc />
		async Task<bool> ICanGet<TAggregateRoot, TKey>.ExistsAsync(TKey id, CancellationToken cancellationToken)
		{
			try
			{
				return await this.innerRepository.ExistsAsync(id, cancellationToken);
			}
			catch(Exception ex)
			{
				this.logger.LogCritical(ex, "A critical error occurred trying to perform exists: {AggregateRoot}", typeof(TAggregateRoot).Name);
				throw;
			}
		}

		/// <inheritdoc />
		async Task<TAggregateRoot> ICanFind<TAggregateRoot, TKey>.FindOneAsync(Expression<Func<TAggregateRoot, bool>> predicate, IQueryOptions<TAggregateRoot> queryOptions, CancellationToken cancellationToken)
		{
			try
			{
				return await this.innerRepository.FindOneAsync(predicate, queryOptions, cancellationToken);
			}
			catch(Exception ex)
			{
				this.logger.LogCritical(ex, "A critical error occurred trying to perform find one: {AggregateRoot}", typeof(TAggregateRoot).Name);
				throw;
			}
		}

		/// <inheritdoc />
		async Task<TAggregateRoot> ICanFind<TAggregateRoot, TKey>.FindOneAsync(ISpecification<TAggregateRoot> specification, IQueryOptions<TAggregateRoot> queryOptions, CancellationToken cancellationToken)
		{
			try
			{
				return await this.innerRepository.FindOneAsync(specification, queryOptions, cancellationToken);
			}
			catch(Exception ex)
			{
				this.logger.LogCritical(ex, "A critical error occurred trying to perform find one: {AggregateRoot}", typeof(TAggregateRoot).Name);
				throw;
			}
		}

		/// <inheritdoc />
		async Task<TResult> ICanFind<TAggregateRoot, TKey>.FindOneAsync<TResult>(Expression<Func<TAggregateRoot, bool>> predicate, Expression<Func<TAggregateRoot, TResult>> selector, IQueryOptions<TAggregateRoot> queryOptions, CancellationToken cancellationToken)
		{
			try
			{
				return await this.innerRepository.FindOneAsync(predicate, selector, queryOptions, cancellationToken);
			}
			catch(Exception ex)
			{
				this.logger.LogCritical(ex, "A critical error occurred trying to perform find one: {AggregateRoot}", typeof(TAggregateRoot).Name);
				throw;
			}
		}

		/// <inheritdoc />
		async Task<TResult> ICanFind<TAggregateRoot, TKey>.FindOneAsync<TResult>(ISpecification<TAggregateRoot> specification, Expression<Func<TAggregateRoot, TResult>> selector, IQueryOptions<TAggregateRoot> queryOptions, CancellationToken cancellationToken)
		{
			try
			{
				return await this.innerRepository.FindOneAsync(specification, selector, queryOptions, cancellationToken);
			}
			catch(Exception ex)
			{
				this.logger.LogCritical(ex, "A critical error occurred trying to perform find one: {AggregateRoot}", typeof(TAggregateRoot).Name);
				throw;
			}
		}

		/// <inheritdoc />
		async Task<bool> ICanFind<TAggregateRoot, TKey>.ExistsAsync(Expression<Func<TAggregateRoot, bool>> predicate, CancellationToken cancellationToken)
		{
			try
			{
				return await this.innerRepository.ExistsAsync(predicate, cancellationToken);
			}
			catch(Exception ex)
			{
				this.logger.LogCritical(ex, "A critical error occurred trying to perform exists: {AggregateRoot}", typeof(TAggregateRoot).Name);
				throw;
			}
		}

		/// <inheritdoc />
		async Task<bool> ICanFind<TAggregateRoot, TKey>.ExistsAsync(ISpecification<TAggregateRoot> specification, CancellationToken cancellationToken)
		{
			try
			{
				return await this.innerRepository.ExistsAsync(specification, cancellationToken);
			}
			catch(Exception ex)
			{
				this.logger.LogCritical(ex, "A critical error occurred trying to perform exists: {AggregateRoot}", typeof(TAggregateRoot).Name);
				throw;
			}
		}

		/// <inheritdoc />
		async Task<IReadOnlyCollection<TAggregateRoot>> ICanFind<TAggregateRoot, TKey>.FindManyAsync(Expression<Func<TAggregateRoot, bool>> predicate, IQueryOptions<TAggregateRoot> queryOptions, CancellationToken cancellationToken)
		{
			try
			{
				return await this.innerRepository.FindManyAsync(predicate, queryOptions, cancellationToken);
			}
			catch(Exception ex)
			{
				this.logger.LogCritical(ex, "A critical error occurred trying to perform find many: {AggregateRoot}", typeof(TAggregateRoot).Name);
				throw;
			}
		}

		/// <inheritdoc />
		async Task<IReadOnlyCollection<TAggregateRoot>> ICanFind<TAggregateRoot, TKey>.FindManyAsync(ISpecification<TAggregateRoot> specification, IQueryOptions<TAggregateRoot> queryOptions, CancellationToken cancellationToken)
		{
			try
			{
				return await this.innerRepository.FindManyAsync(specification, queryOptions, cancellationToken);
			}
			catch(Exception ex)
			{
				this.logger.LogCritical(ex, "A critical error occurred trying to perform find many: {AggregateRoot}", typeof(TAggregateRoot).Name);
				throw;
			}
		}

		/// <inheritdoc />
		async Task<IReadOnlyCollection<TResult>> ICanFind<TAggregateRoot, TKey>.FindManyAsync<TResult>(Expression<Func<TAggregateRoot, bool>> predicate, Expression<Func<TAggregateRoot, TResult>> selector, IQueryOptions<TAggregateRoot> queryOptions, CancellationToken cancellationToken)
		{
			try
			{
				return await this.innerRepository.FindManyAsync(predicate, selector, queryOptions, cancellationToken);
			}
			catch(Exception ex)
			{
				this.logger.LogCritical(ex, "A critical error occurred trying to perform find many: {AggregateRoot}", typeof(TAggregateRoot).Name);
				throw;
			}
		}

		/// <inheritdoc />
		async Task<IReadOnlyCollection<TResult>> ICanFind<TAggregateRoot, TKey>.FindManyAsync<TResult>(ISpecification<TAggregateRoot> specification, Expression<Func<TAggregateRoot, TResult>> selector, IQueryOptions<TAggregateRoot> queryOptions, CancellationToken cancellationToken)
		{
			try
			{
				return await this.innerRepository.FindManyAsync(specification, selector, queryOptions, cancellationToken);
			}
			catch(Exception ex)
			{
				this.logger.LogCritical(ex, "A critical error occurred trying to perform find many: {AggregateRoot}", typeof(TAggregateRoot).Name);
				throw;
			}
		}

		/// <inheritdoc />
		async Task<long> ICanAggregate<TAggregateRoot, TKey>.CountAsync(CancellationToken cancellationToken)
		{
			try
			{
				return await this.innerRepository.CountAsync(cancellationToken);
			}
			catch(Exception ex)
			{
				this.logger.LogCritical(ex, "A critical error occurred trying to perform count: {AggregateRoot}", typeof(TAggregateRoot).Name);
				throw;
			}
		}

		/// <inheritdoc />
		async Task<long> ICanAggregate<TAggregateRoot, TKey>.CountAsync(Expression<Func<TAggregateRoot, bool>> predicate, CancellationToken cancellationToken)
		{
			try
			{
				return await this.innerRepository.CountAsync(predicate, cancellationToken);
			}
			catch(Exception ex)
			{
				this.logger.LogCritical(ex, "A critical error occurred trying to perform count: {AggregateRoot}", typeof(TAggregateRoot).Name);
				throw;
			}
		}

		/// <inheritdoc />
		public async Task<long> CountAsync(ISpecification<TAggregateRoot> specification, CancellationToken cancellationToken = default)
		{
			try
			{
				return await this.innerRepository.CountAsync(specification, cancellationToken);
			}
			catch(Exception ex)
			{
				this.logger.LogCritical(ex, "A critical error occurred trying to perform count: {AggregateRoot}", typeof(TAggregateRoot).Name);
				throw;
			}
		}

		/// <inheritdoc />
		async Task<int> ICanAggregate<TAggregateRoot, TKey>.SumAsync(Expression<Func<TAggregateRoot, int>> selector, CancellationToken cancellationToken)
		{
			try
			{
				return await this.innerRepository.SumAsync(selector, cancellationToken);
			}
			catch(Exception ex)
			{
				this.logger.LogCritical(ex, "A critical error occurred trying to perform sum: {AggregateRoot}", typeof(TAggregateRoot).Name);
				throw;
			}
		}

		/// <inheritdoc />
		async Task<long> ICanAggregate<TAggregateRoot, TKey>.SumAsync(Expression<Func<TAggregateRoot, long>> selector, CancellationToken cancellationToken)
		{
			try
			{
				return await this.innerRepository.SumAsync(selector, cancellationToken);
			}
			catch(Exception ex)
			{
				this.logger.LogCritical(ex, "A critical error occurred trying to perform sum: {AggregateRoot}", typeof(TAggregateRoot).Name);
				throw;
			}
		}

		/// <inheritdoc />
		async Task<decimal> ICanAggregate<TAggregateRoot, TKey>.SumAsync(Expression<Func<TAggregateRoot, decimal>> selector, CancellationToken cancellationToken)
		{
			try
			{
				return await this.innerRepository.SumAsync(selector, cancellationToken);
			}
			catch(Exception ex)
			{
				this.logger.LogCritical(ex, "A critical error occurred trying to perform sum: {AggregateRoot}", typeof(TAggregateRoot).Name);
				throw;
			}
		}

		/// <inheritdoc />
		async Task<float> ICanAggregate<TAggregateRoot, TKey>.SumAsync(Expression<Func<TAggregateRoot, float>> selector, CancellationToken cancellationToken)
		{
			try
			{
				return await this.innerRepository.SumAsync(selector, cancellationToken);
			}
			catch(Exception ex)
			{
				this.logger.LogCritical(ex, "A critical error occurred trying to perform sum: {AggregateRoot}", typeof(TAggregateRoot).Name);
				throw;
			}
		}

		/// <inheritdoc />
		async Task<double> ICanAggregate<TAggregateRoot, TKey>.SumAsync(Expression<Func<TAggregateRoot, double>> selector, CancellationToken cancellationToken)
		{
			try
			{
				return await this.innerRepository.SumAsync(selector, cancellationToken);
			}
			catch(Exception ex)
			{
				this.logger.LogCritical(ex, "A critical error occurred trying to perform sum: {AggregateRoot}", typeof(TAggregateRoot).Name);
				throw;
			}
		}

		/// <inheritdoc />
		async Task<int> ICanAggregate<TAggregateRoot, TKey>.SumAsync(Expression<Func<TAggregateRoot, bool>> predicate, Expression<Func<TAggregateRoot, int>> selector, CancellationToken cancellationToken)
		{
			try
			{
				return await this.innerRepository.SumAsync(predicate, selector, cancellationToken);
			}
			catch(Exception ex)
			{
				this.logger.LogCritical(ex, "A critical error occurred trying to perform sum: {AggregateRoot}", typeof(TAggregateRoot).Name);
				throw;
			}
		}

		/// <inheritdoc />
		async Task<long> ICanAggregate<TAggregateRoot, TKey>.SumAsync(Expression<Func<TAggregateRoot, bool>> predicate, Expression<Func<TAggregateRoot, long>> selector, CancellationToken cancellationToken)
		{
			try
			{
				return await this.innerRepository.SumAsync(predicate, selector, cancellationToken);
			}
			catch(Exception ex)
			{
				this.logger.LogCritical(ex, "A critical error occurred trying to perform sum: {AggregateRoot}", typeof(TAggregateRoot).Name);
				throw;
			}
		}

		/// <inheritdoc />
		async Task<decimal> ICanAggregate<TAggregateRoot, TKey>.SumAsync(Expression<Func<TAggregateRoot, bool>> predicate, Expression<Func<TAggregateRoot, decimal>> selector, CancellationToken cancellationToken)
		{
			try
			{
				return await this.innerRepository.SumAsync(predicate, selector, cancellationToken);
			}
			catch(Exception ex)
			{
				this.logger.LogCritical(ex, "A critical error occurred trying to perform sum: {AggregateRoot}", typeof(TAggregateRoot).Name);
				throw;
			}
		}

		/// <inheritdoc />
		async Task<float> ICanAggregate<TAggregateRoot, TKey>.SumAsync(Expression<Func<TAggregateRoot, bool>> predicate, Expression<Func<TAggregateRoot, float>> selector, CancellationToken cancellationToken)
		{
			try
			{
				return await this.innerRepository.SumAsync(predicate, selector, cancellationToken);
			}
			catch(Exception ex)
			{
				this.logger.LogCritical(ex, "A critical error occurred trying to perform sum: {AggregateRoot}", typeof(TAggregateRoot).Name);
				throw;
			}
		}

		/// <inheritdoc />
		async Task<double> ICanAggregate<TAggregateRoot, TKey>.SumAsync(Expression<Func<TAggregateRoot, bool>> predicate, Expression<Func<TAggregateRoot, double>> selector, CancellationToken cancellationToken)
		{
			try
			{
				return await this.innerRepository.SumAsync(predicate, selector, cancellationToken);
			}
			catch(Exception ex)
			{
				this.logger.LogCritical(ex, "A critical error occurred trying to perform sum: {AggregateRoot}", typeof(TAggregateRoot).Name);
				throw;
			}
		}

		/// <inheritdoc />
		async Task<int> ICanAggregate<TAggregateRoot, TKey>.SumAsync(ISpecification<TAggregateRoot> specification, Expression<Func<TAggregateRoot, int>> selector, CancellationToken cancellationToken)
		{
			try
			{
				return await this.innerRepository.SumAsync(specification, selector, cancellationToken);
			}
			catch(Exception ex)
			{
				this.logger.LogCritical(ex, "A critical error occurred trying to perform sum: {AggregateRoot}", typeof(TAggregateRoot).Name);
				throw;
			}
		}

		/// <inheritdoc />
		async Task<long> ICanAggregate<TAggregateRoot, TKey>.SumAsync(ISpecification<TAggregateRoot> specification, Expression<Func<TAggregateRoot, long>> selector, CancellationToken cancellationToken)
		{
			try
			{
				return await this.innerRepository.SumAsync(specification, selector, cancellationToken);
			}
			catch(Exception ex)
			{
				this.logger.LogCritical(ex, "A critical error occurred trying to perform sum: {AggregateRoot}", typeof(TAggregateRoot).Name);
				throw;
			}
		}

		/// <inheritdoc />
		async Task<decimal> ICanAggregate<TAggregateRoot, TKey>.SumAsync(ISpecification<TAggregateRoot> specification, Expression<Func<TAggregateRoot, decimal>> selector, CancellationToken cancellationToken)
		{
			try
			{
				return await this.innerRepository.SumAsync(specification, selector, cancellationToken);
			}
			catch(Exception ex)
			{
				this.logger.LogCritical(ex, "A critical error occurred trying to perform sum: {AggregateRoot}", typeof(TAggregateRoot).Name);
				throw;
			}
		}

		/// <inheritdoc />
		async Task<float> ICanAggregate<TAggregateRoot, TKey>.SumAsync(ISpecification<TAggregateRoot> specification, Expression<Func<TAggregateRoot, float>> selector, CancellationToken cancellationToken)
		{
			try
			{
				return await this.innerRepository.SumAsync(specification, selector, cancellationToken);
			}
			catch(Exception ex)
			{
				this.logger.LogCritical(ex, "A critical error occurred trying to perform sum: {AggregateRoot}", typeof(TAggregateRoot).Name);
				throw;
			}
		}

		/// <inheritdoc />
		async Task<double> ICanAggregate<TAggregateRoot, TKey>.SumAsync(ISpecification<TAggregateRoot> specification, Expression<Func<TAggregateRoot, double>> selector, CancellationToken cancellationToken)
		{
			try
			{
				return await this.innerRepository.SumAsync(specification, selector, cancellationToken);
			}
			catch(Exception ex)
			{
				this.logger.LogCritical(ex, "A critical error occurred trying to perform sum: {AggregateRoot}", typeof(TAggregateRoot).Name);
				throw;
			}
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
		public override string ToString()
		{
			return this.innerRepository.ToString();
		}
	}
}
