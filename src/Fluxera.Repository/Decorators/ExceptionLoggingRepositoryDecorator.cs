namespace Fluxera.Repository.Decorators
{
	using System;
	using System.Collections.Generic;
	using System.Linq.Expressions;
	using System.Threading;
	using System.Threading.Tasks;
	using Fluxera.Entity;
	using Fluxera.Repository.Query;
	using Fluxera.Repository.Traits;
	using Microsoft.Extensions.Logging;

	public sealed class ExceptionLoggingRepositoryDecorator<TAggregateRoot> : IRepository<TAggregateRoot>
		where TAggregateRoot : AggregateRoot<TAggregateRoot>
	{
		private readonly IRepository<TAggregateRoot> innerRepository;
		private readonly ILogger logger;

		/// <summary>
		///     Creates a new instance of the <see cref="ExceptionLoggingRepositoryDecorator{TAggregateRoot}" /> type.
		/// </summary>
		/// <param name="innerRepository"></param>
		/// <param name="loggerFactory"></param>
		public ExceptionLoggingRepositoryDecorator(IRepository<TAggregateRoot> innerRepository, ILoggerFactory loggerFactory)
		{
			this.innerRepository = innerRepository;
			this.logger = loggerFactory.CreateLogger(LoggerNames.RepositoryLogger);
		}

		/// <inheritdoc />
		async Task ICanAdd<TAggregateRoot>.AddAsync(TAggregateRoot item, CancellationToken cancellationToken)
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
		async Task ICanAdd<TAggregateRoot>.AddAsync(IEnumerable<TAggregateRoot> items, CancellationToken cancellationToken)
		{
			try
			{
				await this.innerRepository.AddAsync(items, cancellationToken);
			}
			catch(Exception ex)
			{
				this.logger.LogCritical(ex, "A critical error occurred trying to perform add: {AggregateRoot}", typeof(TAggregateRoot).Name);
				throw;
			}
		}

		/// <inheritdoc />
		async Task ICanUpdate<TAggregateRoot>.UpdateAsync(TAggregateRoot item, CancellationToken cancellationToken)
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
		async Task ICanUpdate<TAggregateRoot>.UpdateAsync(IEnumerable<TAggregateRoot> items, CancellationToken cancellationToken)
		{
			try
			{
				await this.innerRepository.UpdateAsync(items, cancellationToken);
			}
			catch(Exception ex)
			{
				this.logger.LogCritical(ex, "A critical error occurred trying to perform update: {AggregateRoot}", typeof(TAggregateRoot).Name);
				throw;
			}
		}

		/// <inheritdoc />
		async Task ICanRemove<TAggregateRoot>.RemoveAsync(TAggregateRoot item, CancellationToken cancellationToken)
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
		async Task ICanRemove<TAggregateRoot>.RemoveAsync(string id, CancellationToken cancellationToken)
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
		async Task ICanRemove<TAggregateRoot>.RemoveAsync(Expression<Func<TAggregateRoot, bool>> predicate, CancellationToken cancellationToken)
		{
			try
			{
				await this.innerRepository.RemoveAsync(predicate, cancellationToken);
			}
			catch(Exception ex)
			{
				this.logger.LogCritical(ex, "A critical error occurred trying to perform remove: {AggregateRoot}", typeof(TAggregateRoot).Name);
				throw;
			}
		}

		/// <inheritdoc />
		async Task ICanRemove<TAggregateRoot>.RemoveAsync(IEnumerable<TAggregateRoot> items, CancellationToken cancellationToken)
		{
			try
			{
				await this.innerRepository.RemoveAsync(items, cancellationToken);
			}
			catch(Exception ex)
			{
				this.logger.LogCritical(ex, "A critical error occurred trying to perform remove: {AggregateRoot}", typeof(TAggregateRoot).Name);
				throw;
			}
		}

		/// <inheritdoc />
		async Task<TAggregateRoot> ICanGet<TAggregateRoot>.GetAsync(string id, CancellationToken cancellationToken)
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
		async Task<TResult> ICanGet<TAggregateRoot>.GetAsync<TResult>(string id, Expression<Func<TAggregateRoot, TResult>> selector, CancellationToken cancellationToken)
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
		async Task<bool> ICanGet<TAggregateRoot>.ExistsAsync(string id, CancellationToken cancellationToken)
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
		async Task<TAggregateRoot> ICanFind<TAggregateRoot>.FindOneAsync(Expression<Func<TAggregateRoot, bool>> predicate, IQueryOptions<TAggregateRoot>? queryOptions, CancellationToken cancellationToken)
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
		async Task<TResult> ICanFind<TAggregateRoot>.FindOneAsync<TResult>(Expression<Func<TAggregateRoot, bool>> predicate, Expression<Func<TAggregateRoot, TResult>> selector, IQueryOptions<TAggregateRoot>? queryOptions, CancellationToken cancellationToken)
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
		async Task<bool> ICanFind<TAggregateRoot>.ExistsAsync(Expression<Func<TAggregateRoot, bool>> predicate, CancellationToken cancellationToken)
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
		async Task<IReadOnlyCollection<TAggregateRoot>> ICanFind<TAggregateRoot>.FindManyAsync(Expression<Func<TAggregateRoot, bool>> predicate, IQueryOptions<TAggregateRoot>? queryOptions, CancellationToken cancellationToken)
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
		async Task<IReadOnlyCollection<TResult>> ICanFind<TAggregateRoot>.FindManyAsync<TResult>(Expression<Func<TAggregateRoot, bool>> predicate, Expression<Func<TAggregateRoot, TResult>> selector, IQueryOptions<TAggregateRoot>? queryOptions, CancellationToken cancellationToken)
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
		async Task<long> ICanAggregate<TAggregateRoot>.CountAsync(CancellationToken cancellationToken)
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
		async Task<long> ICanAggregate<TAggregateRoot>.CountAsync(Expression<Func<TAggregateRoot, bool>> predicate, CancellationToken cancellationToken)
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

		/// <inheritdoc />
		public override string ToString()
		{
			return this.innerRepository.ToString();
		}
	}
}
