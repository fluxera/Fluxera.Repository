namespace Fluxera.Repository.Decorators
{
	using System;
	using System.Collections.Generic;
	using System.Linq.Expressions;
	using System.Threading;
	using System.Threading.Tasks;
	using Fluxera.Entity;
	using Fluxera.Guards;
	using Fluxera.Repository.Extensions;
	using Fluxera.Repository.Query;
	using Fluxera.Repository.Specifications;
	using Fluxera.Repository.Traits;
	using Microsoft.Extensions.Logging;

	/// <summary>
	///     A repository decorator that handles unhandled exceptions thrown and logs the errors.
	/// </summary>
	/// <typeparam name="TEntity"></typeparam>
	/// <typeparam name="TKey"></typeparam>
	public sealed class ExceptionLoggingRepositoryDecorator<TEntity, TKey> : IRepository<TEntity, TKey>
		where TEntity : Entity<TEntity, TKey>
		where TKey : notnull, IComparable<TKey>, IEquatable<TKey>
	{
		private readonly IRepository<TEntity, TKey> innerRepository;
		private readonly ILogger logger;

		/// <summary>
		///     Creates a new instance of the <see cref="ExceptionLoggingRepositoryDecorator{TAggregateRoot,TKey}" /> type.
		/// </summary>
		/// <param name="innerRepository"></param>
		/// <param name="loggerFactory"></param>
		public ExceptionLoggingRepositoryDecorator(
			IRepository<TEntity, TKey> innerRepository,
			ILoggerFactory loggerFactory)
		{
			this.innerRepository = Guard.Against.Null(innerRepository);
			Guard.Against.Null(loggerFactory);

			this.logger = loggerFactory.CreateLogger(LoggerNames.Repository);
		}

		/// <inheritdoc />
		async Task ICanAdd<TEntity, TKey>.AddAsync(TEntity item, CancellationToken cancellationToken)
		{
			try
			{
				await this.innerRepository.AddAsync(item, cancellationToken);
			}
			catch(Exception ex)
			{
				this.logger.LogErrorOccurredForOperation("add", typeof(TEntity).Name, ex);
				throw;
			}
		}

		/// <inheritdoc />
		async Task ICanAdd<TEntity, TKey>.AddRangeAsync(IEnumerable<TEntity> items, CancellationToken cancellationToken)
		{
			try
			{
				await this.innerRepository.AddRangeAsync(items, cancellationToken);
			}
			catch(Exception ex)
			{
				this.logger.LogErrorOccurredForOperation("add", typeof(TEntity).Name, ex);
				throw;
			}
		}

		/// <inheritdoc />
		async Task ICanUpdate<TEntity, TKey>.UpdateAsync(TEntity item, CancellationToken cancellationToken)
		{
			try
			{
				await this.innerRepository.UpdateAsync(item, cancellationToken);
			}
			catch(Exception ex)
			{
				this.logger.LogErrorOccurredForOperation("update", typeof(TEntity).Name, ex);
				throw;
			}
		}

		/// <inheritdoc />
		async Task ICanUpdate<TEntity, TKey>.UpdateRangeAsync(IEnumerable<TEntity> items, CancellationToken cancellationToken)
		{
			try
			{
				await this.innerRepository.UpdateRangeAsync(items, cancellationToken);
			}
			catch(Exception ex)
			{
				this.logger.LogErrorOccurredForOperation("update", typeof(TEntity).Name, ex);
				throw;
			}
		}

		/// <inheritdoc />
		async Task ICanRemove<TEntity, TKey>.RemoveAsync(TEntity item, CancellationToken cancellationToken)
		{
			try
			{
				await this.innerRepository.RemoveAsync(item, cancellationToken);
			}
			catch(Exception ex)
			{
				this.logger.LogErrorOccurredForOperation("remove", typeof(TEntity).Name, ex);
				throw;
			}
		}

		/// <inheritdoc />
		async Task ICanRemove<TEntity, TKey>.RemoveAsync(TKey id, CancellationToken cancellationToken)
		{
			try
			{
				await this.innerRepository.RemoveAsync(id, cancellationToken);
			}
			catch(Exception ex)
			{
				this.logger.LogErrorOccurredForOperation("remove", typeof(TEntity).Name, ex);
				throw;
			}
		}

		/// <inheritdoc />
		async Task ICanRemove<TEntity, TKey>.RemoveRangeAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken)
		{
			try
			{
				await this.innerRepository.RemoveRangeAsync(predicate, cancellationToken);
			}
			catch(Exception ex)
			{
				this.logger.LogErrorOccurredForOperation("remove", typeof(TEntity).Name, ex);
				throw;
			}
		}

		/// <inheritdoc />
		async Task ICanRemove<TEntity, TKey>.RemoveRangeAsync(ISpecification<TEntity> specification, CancellationToken cancellationToken)
		{
			try
			{
				await this.innerRepository.RemoveRangeAsync(specification, cancellationToken);
			}
			catch(Exception ex)
			{
				this.logger.LogErrorOccurredForOperation("remove", typeof(TEntity).Name, ex);
				throw;
			}
		}

		/// <inheritdoc />
		async Task ICanRemove<TEntity, TKey>.RemoveRangeAsync(IEnumerable<TEntity> items, CancellationToken cancellationToken)
		{
			try
			{
				await this.innerRepository.RemoveRangeAsync(items, cancellationToken);
			}
			catch(Exception ex)
			{
				this.logger.LogErrorOccurredForOperation("remove", typeof(TEntity).Name, ex);
				throw;
			}
		}

		/// <inheritdoc />
		async Task<TEntity> ICanGet<TEntity, TKey>.GetAsync(TKey id, CancellationToken cancellationToken)
		{
			try
			{
				return await this.innerRepository.GetAsync(id, cancellationToken);
			}
			catch(Exception ex)
			{
				this.logger.LogErrorOccurredForOperation("get", typeof(TEntity).Name, ex);
				throw;
			}
		}

		/// <inheritdoc />
		async Task<TResult> ICanGet<TEntity, TKey>.GetAsync<TResult>(TKey id, Expression<Func<TEntity, TResult>> selector,
			CancellationToken cancellationToken)
		{
			try
			{
				return await this.innerRepository.GetAsync(id, selector, cancellationToken);
			}
			catch(Exception ex)
			{
				this.logger.LogErrorOccurredForOperation("get", typeof(TEntity).Name, ex);
				throw;
			}
		}

		/// <inheritdoc />
		async Task<bool> ICanGet<TEntity, TKey>.ExistsAsync(TKey id, CancellationToken cancellationToken)
		{
			try
			{
				return await this.innerRepository.ExistsAsync(id, cancellationToken);
			}
			catch(Exception ex)
			{
				this.logger.LogErrorOccurredForOperation("exists", typeof(TEntity).Name, ex);
				throw;
			}
		}

		/// <inheritdoc />
		async Task<TEntity> ICanFind<TEntity, TKey>.FindOneAsync(Expression<Func<TEntity, bool>> predicate,
			IQueryOptions<TEntity> queryOptions, CancellationToken cancellationToken)
		{
			try
			{
				return await this.innerRepository.FindOneAsync(predicate, queryOptions, cancellationToken);
			}
			catch(Exception ex)
			{
				this.logger.LogErrorOccurredForOperation("find one", typeof(TEntity).Name, ex);
				throw;
			}
		}

		/// <inheritdoc />
		async Task<TEntity> ICanFind<TEntity, TKey>.FindOneAsync(ISpecification<TEntity> specification,
			IQueryOptions<TEntity> queryOptions, CancellationToken cancellationToken)
		{
			try
			{
				return await this.innerRepository.FindOneAsync(specification, queryOptions, cancellationToken);
			}
			catch(Exception ex)
			{
				this.logger.LogErrorOccurredForOperation("find one", typeof(TEntity).Name, ex);
				throw;
			}
		}

		/// <inheritdoc />
		async Task<TResult> ICanFind<TEntity, TKey>.FindOneAsync<TResult>(Expression<Func<TEntity, bool>> predicate,
			Expression<Func<TEntity, TResult>> selector, IQueryOptions<TEntity> queryOptions, CancellationToken cancellationToken)
		{
			try
			{
				return await this.innerRepository.FindOneAsync(predicate, selector, queryOptions, cancellationToken);
			}
			catch(Exception ex)
			{
				this.logger.LogErrorOccurredForOperation("find one", typeof(TEntity).Name, ex);
				throw;
			}
		}

		/// <inheritdoc />
		async Task<TResult> ICanFind<TEntity, TKey>.FindOneAsync<TResult>(ISpecification<TEntity> specification,
			Expression<Func<TEntity, TResult>> selector, IQueryOptions<TEntity> queryOptions, CancellationToken cancellationToken)
		{
			try
			{
				return await this.innerRepository.FindOneAsync(specification, selector, queryOptions, cancellationToken);
			}
			catch(Exception ex)
			{
				this.logger.LogErrorOccurredForOperation("find one", typeof(TEntity).Name, ex);
				throw;
			}
		}

		/// <inheritdoc />
		async Task<bool> ICanFind<TEntity, TKey>.ExistsAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken)
		{
			try
			{
				return await this.innerRepository.ExistsAsync(predicate, cancellationToken);
			}
			catch(Exception ex)
			{
				this.logger.LogErrorOccurredForOperation("exists", typeof(TEntity).Name, ex);
				throw;
			}
		}

		/// <inheritdoc />
		async Task<bool> ICanFind<TEntity, TKey>.ExistsAsync(ISpecification<TEntity> specification, CancellationToken cancellationToken)
		{
			try
			{
				return await this.innerRepository.ExistsAsync(specification, cancellationToken);
			}
			catch(Exception ex)
			{
				this.logger.LogErrorOccurredForOperation("exists", typeof(TEntity).Name, ex);
				throw;
			}
		}

		/// <inheritdoc />
		async Task<IReadOnlyCollection<TEntity>> ICanFind<TEntity, TKey>.FindManyAsync(Expression<Func<TEntity, bool>> predicate,
			IQueryOptions<TEntity> queryOptions, CancellationToken cancellationToken)
		{
			try
			{
				return await this.innerRepository.FindManyAsync(predicate, queryOptions, cancellationToken);
			}
			catch(Exception ex)
			{
				this.logger.LogErrorOccurredForOperation("find many", typeof(TEntity).Name, ex);
				throw;
			}
		}

		/// <inheritdoc />
		async Task<IReadOnlyCollection<TEntity>> ICanFind<TEntity, TKey>.FindManyAsync(ISpecification<TEntity> specification,
			IQueryOptions<TEntity> queryOptions, CancellationToken cancellationToken)
		{
			try
			{
				return await this.innerRepository.FindManyAsync(specification, queryOptions, cancellationToken);
			}
			catch(Exception ex)
			{
				this.logger.LogErrorOccurredForOperation("find many", typeof(TEntity).Name, ex);
				throw;
			}
		}

		/// <inheritdoc />
		async Task<IReadOnlyCollection<TResult>> ICanFind<TEntity, TKey>.FindManyAsync<TResult>(Expression<Func<TEntity, bool>> predicate,
			Expression<Func<TEntity, TResult>> selector, IQueryOptions<TEntity> queryOptions, CancellationToken cancellationToken)
		{
			try
			{
				return await this.innerRepository.FindManyAsync(predicate, selector, queryOptions, cancellationToken);
			}
			catch(Exception ex)
			{
				this.logger.LogErrorOccurredForOperation("find many", typeof(TEntity).Name, ex);
				throw;
			}
		}

		/// <inheritdoc />
		async Task<IReadOnlyCollection<TResult>> ICanFind<TEntity, TKey>.FindManyAsync<TResult>(ISpecification<TEntity> specification,
			Expression<Func<TEntity, TResult>> selector, IQueryOptions<TEntity> queryOptions, CancellationToken cancellationToken)
		{
			try
			{
				return await this.innerRepository.FindManyAsync(specification, selector, queryOptions, cancellationToken);
			}
			catch(Exception ex)
			{
				this.logger.LogErrorOccurredForOperation("find many", typeof(TEntity).Name, ex);
				throw;
			}
		}

		/// <inheritdoc />
		async Task<long> ICanGet<TEntity, TKey>.CountAsync(CancellationToken cancellationToken)
		{
			try
			{
				return await this.innerRepository.CountAsync(cancellationToken);
			}
			catch(Exception ex)
			{
				this.logger.LogErrorOccurredForOperation("count", typeof(TEntity).Name, ex);
				throw;
			}
		}

		/// <inheritdoc />
		async Task<long> ICanGet<TEntity, TKey>.CountAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken)
		{
			try
			{
				return await this.innerRepository.CountAsync(predicate, cancellationToken);
			}
			catch(Exception ex)
			{
				this.logger.LogErrorOccurredForOperation("count", typeof(TEntity).Name, ex);
				throw;
			}
		}

		/// <inheritdoc />
		async Task<long> ICanGet<TEntity, TKey>.CountAsync(ISpecification<TEntity> specification, CancellationToken cancellationToken)
		{
			try
			{
				return await this.innerRepository.CountAsync(specification, cancellationToken);
			}
			catch(Exception ex)
			{
				this.logger.LogErrorOccurredForOperation("count", typeof(TEntity).Name, ex);
				throw;
			}
		}

		/// <inheritdoc />
		async Task<int> ICanAggregate<TEntity, TKey>.SumAsync(Expression<Func<TEntity, int>> selector, CancellationToken cancellationToken)
		{
			try
			{
				return await this.innerRepository.SumAsync(selector, cancellationToken);
			}
			catch(Exception ex)
			{
				this.logger.LogErrorOccurredForOperation("sum", typeof(TEntity).Name, ex);
				throw;
			}
		}

		/// <inheritdoc />
		async Task<int> ICanAggregate<TEntity, TKey>.SumAsync(Expression<Func<TEntity, int?>> selector, CancellationToken cancellationToken)
		{
			try
			{
				return await this.innerRepository.SumAsync(selector, cancellationToken);
			}
			catch(Exception ex)
			{
				this.logger.LogErrorOccurredForOperation("sum", typeof(TEntity).Name, ex);
				throw;
			}
		}

		/// <inheritdoc />
		async Task<long> ICanAggregate<TEntity, TKey>.SumAsync(Expression<Func<TEntity, long>> selector, CancellationToken cancellationToken)
		{
			try
			{
				return await this.innerRepository.SumAsync(selector, cancellationToken);
			}
			catch(Exception ex)
			{
				this.logger.LogErrorOccurredForOperation("sum", typeof(TEntity).Name, ex);
				throw;
			}
		}

		/// <inheritdoc />
		async Task<long> ICanAggregate<TEntity, TKey>.SumAsync(Expression<Func<TEntity, long?>> selector, CancellationToken cancellationToken)
		{
			try
			{
				return await this.innerRepository.SumAsync(selector, cancellationToken);
			}
			catch(Exception ex)
			{
				this.logger.LogErrorOccurredForOperation("sum", typeof(TEntity).Name, ex);
				throw;
			}
		}

		/// <inheritdoc />
		async Task<decimal> ICanAggregate<TEntity, TKey>.SumAsync(Expression<Func<TEntity, decimal>> selector,
			CancellationToken cancellationToken)
		{
			try
			{
				return await this.innerRepository.SumAsync(selector, cancellationToken);
			}
			catch(Exception ex)
			{
				this.logger.LogErrorOccurredForOperation("sum", typeof(TEntity).Name, ex);
				throw;
			}
		}

		/// <inheritdoc />
		async Task<decimal> ICanAggregate<TEntity, TKey>.SumAsync(Expression<Func<TEntity, decimal?>> selector,
			CancellationToken cancellationToken)
		{
			try
			{
				return await this.innerRepository.SumAsync(selector, cancellationToken);
			}
			catch(Exception ex)
			{
				this.logger.LogErrorOccurredForOperation("sum", typeof(TEntity).Name, ex);
				throw;
			}
		}

		/// <inheritdoc />
		async Task<float> ICanAggregate<TEntity, TKey>.SumAsync(Expression<Func<TEntity, float>> selector, CancellationToken cancellationToken)
		{
			try
			{
				return await this.innerRepository.SumAsync(selector, cancellationToken);
			}
			catch(Exception ex)
			{
				this.logger.LogErrorOccurredForOperation("sum", typeof(TEntity).Name, ex);
				throw;
			}
		}

		/// <inheritdoc />
		async Task<float> ICanAggregate<TEntity, TKey>.SumAsync(Expression<Func<TEntity, float?>> selector, CancellationToken cancellationToken)
		{
			try
			{
				return await this.innerRepository.SumAsync(selector, cancellationToken);
			}
			catch(Exception ex)
			{
				this.logger.LogErrorOccurredForOperation("sum", typeof(TEntity).Name, ex);
				throw;
			}
		}

		/// <inheritdoc />
		async Task<double> ICanAggregate<TEntity, TKey>.SumAsync(Expression<Func<TEntity, double>> selector, CancellationToken cancellationToken)
		{
			try
			{
				return await this.innerRepository.SumAsync(selector, cancellationToken);
			}
			catch(Exception ex)
			{
				this.logger.LogErrorOccurredForOperation("sum", typeof(TEntity).Name, ex);
				throw;
			}
		}

		/// <inheritdoc />
		async Task<double> ICanAggregate<TEntity, TKey>.SumAsync(Expression<Func<TEntity, double?>> selector, CancellationToken cancellationToken)
		{
			try
			{
				return await this.innerRepository.SumAsync(selector, cancellationToken);
			}
			catch(Exception ex)
			{
				this.logger.LogErrorOccurredForOperation("sum", typeof(TEntity).Name, ex);
				throw;
			}
		}

		/// <inheritdoc />
		async Task<int> ICanAggregate<TEntity, TKey>.SumAsync(Expression<Func<TEntity, bool>> predicate,
			Expression<Func<TEntity, int>> selector, CancellationToken cancellationToken)
		{
			try
			{
				return await this.innerRepository.SumAsync(predicate, selector, cancellationToken);
			}
			catch(Exception ex)
			{
				this.logger.LogErrorOccurredForOperation("sum", typeof(TEntity).Name, ex);
				throw;
			}
		}

		/// <inheritdoc />
		async Task<int> ICanAggregate<TEntity, TKey>.SumAsync(Expression<Func<TEntity, bool>> predicate,
			Expression<Func<TEntity, int?>> selector, CancellationToken cancellationToken)
		{
			try
			{
				return await this.innerRepository.SumAsync(predicate, selector, cancellationToken);
			}
			catch(Exception ex)
			{
				this.logger.LogErrorOccurredForOperation("sum", typeof(TEntity).Name, ex);
				throw;
			}
		}

		/// <inheritdoc />
		async Task<long> ICanAggregate<TEntity, TKey>.SumAsync(Expression<Func<TEntity, bool>> predicate,
			Expression<Func<TEntity, long>> selector, CancellationToken cancellationToken)
		{
			try
			{
				return await this.innerRepository.SumAsync(predicate, selector, cancellationToken);
			}
			catch(Exception ex)
			{
				this.logger.LogErrorOccurredForOperation("sum", typeof(TEntity).Name, ex);
				throw;
			}
		}

		/// <inheritdoc />
		async Task<long> ICanAggregate<TEntity, TKey>.SumAsync(Expression<Func<TEntity, bool>> predicate,
			Expression<Func<TEntity, long?>> selector, CancellationToken cancellationToken)
		{
			try
			{
				return await this.innerRepository.SumAsync(predicate, selector, cancellationToken);
			}
			catch(Exception ex)
			{
				this.logger.LogErrorOccurredForOperation("sum", typeof(TEntity).Name, ex);
				throw;
			}
		}

		/// <inheritdoc />
		async Task<decimal> ICanAggregate<TEntity, TKey>.SumAsync(Expression<Func<TEntity, bool>> predicate,
			Expression<Func<TEntity, decimal>> selector, CancellationToken cancellationToken)
		{
			try
			{
				return await this.innerRepository.SumAsync(predicate, selector, cancellationToken);
			}
			catch(Exception ex)
			{
				this.logger.LogErrorOccurredForOperation("sum", typeof(TEntity).Name, ex);
				throw;
			}
		}

		/// <inheritdoc />
		async Task<decimal> ICanAggregate<TEntity, TKey>.SumAsync(Expression<Func<TEntity, bool>> predicate,
			Expression<Func<TEntity, decimal?>> selector, CancellationToken cancellationToken)
		{
			try
			{
				return await this.innerRepository.SumAsync(predicate, selector, cancellationToken);
			}
			catch(Exception ex)
			{
				this.logger.LogErrorOccurredForOperation("sum", typeof(TEntity).Name, ex);
				throw;
			}
		}

		/// <inheritdoc />
		async Task<float> ICanAggregate<TEntity, TKey>.SumAsync(Expression<Func<TEntity, bool>> predicate,
			Expression<Func<TEntity, float>> selector, CancellationToken cancellationToken)
		{
			try
			{
				return await this.innerRepository.SumAsync(predicate, selector, cancellationToken);
			}
			catch(Exception ex)
			{
				this.logger.LogErrorOccurredForOperation("sum", typeof(TEntity).Name, ex);
				throw;
			}
		}

		/// <inheritdoc />
		async Task<float> ICanAggregate<TEntity, TKey>.SumAsync(Expression<Func<TEntity, bool>> predicate,
			Expression<Func<TEntity, float?>> selector, CancellationToken cancellationToken)
		{
			try
			{
				return await this.innerRepository.SumAsync(predicate, selector, cancellationToken);
			}
			catch(Exception ex)
			{
				this.logger.LogErrorOccurredForOperation("sum", typeof(TEntity).Name, ex);
				throw;
			}
		}

		/// <inheritdoc />
		async Task<double> ICanAggregate<TEntity, TKey>.SumAsync(Expression<Func<TEntity, bool>> predicate,
			Expression<Func<TEntity, double>> selector, CancellationToken cancellationToken)
		{
			try
			{
				return await this.innerRepository.SumAsync(predicate, selector, cancellationToken);
			}
			catch(Exception ex)
			{
				this.logger.LogErrorOccurredForOperation("sum", typeof(TEntity).Name, ex);
				throw;
			}
		}

		/// <inheritdoc />
		async Task<double> ICanAggregate<TEntity, TKey>.SumAsync(Expression<Func<TEntity, bool>> predicate,
			Expression<Func<TEntity, double?>> selector, CancellationToken cancellationToken)
		{
			try
			{
				return await this.innerRepository.SumAsync(predicate, selector, cancellationToken);
			}
			catch(Exception ex)
			{
				this.logger.LogErrorOccurredForOperation("sum", typeof(TEntity).Name, ex);
				throw;
			}
		}

		/// <inheritdoc />
		async Task<int> ICanAggregate<TEntity, TKey>.SumAsync(ISpecification<TEntity> specification,
			Expression<Func<TEntity, int>> selector, CancellationToken cancellationToken)
		{
			try
			{
				return await this.innerRepository.SumAsync(specification, selector, cancellationToken);
			}
			catch(Exception ex)
			{
				this.logger.LogErrorOccurredForOperation("sum", typeof(TEntity).Name, ex);
				throw;
			}
		}

		/// <inheritdoc />
		async Task<int> ICanAggregate<TEntity, TKey>.SumAsync(ISpecification<TEntity> specification,
			Expression<Func<TEntity, int?>> selector, CancellationToken cancellationToken)
		{
			try
			{
				return await this.innerRepository.SumAsync(specification, selector, cancellationToken);
			}
			catch(Exception ex)
			{
				this.logger.LogErrorOccurredForOperation("sum", typeof(TEntity).Name, ex);
				throw;
			}
		}

		/// <inheritdoc />
		async Task<long> ICanAggregate<TEntity, TKey>.SumAsync(ISpecification<TEntity> specification,
			Expression<Func<TEntity, long>> selector, CancellationToken cancellationToken)
		{
			try
			{
				return await this.innerRepository.SumAsync(specification, selector, cancellationToken);
			}
			catch(Exception ex)
			{
				this.logger.LogErrorOccurredForOperation("sum", typeof(TEntity).Name, ex);
				throw;
			}
		}

		/// <inheritdoc />
		async Task<long> ICanAggregate<TEntity, TKey>.SumAsync(ISpecification<TEntity> specification,
			Expression<Func<TEntity, long?>> selector, CancellationToken cancellationToken)
		{
			try
			{
				return await this.innerRepository.SumAsync(specification, selector, cancellationToken);
			}
			catch(Exception ex)
			{
				this.logger.LogErrorOccurredForOperation("sum", typeof(TEntity).Name, ex);
				throw;
			}
		}

		/// <inheritdoc />
		async Task<decimal> ICanAggregate<TEntity, TKey>.SumAsync(ISpecification<TEntity> specification,
			Expression<Func<TEntity, decimal>> selector, CancellationToken cancellationToken)
		{
			try
			{
				return await this.innerRepository.SumAsync(specification, selector, cancellationToken);
			}
			catch(Exception ex)
			{
				this.logger.LogErrorOccurredForOperation("sum", typeof(TEntity).Name, ex);
				throw;
			}
		}

		/// <inheritdoc />
		async Task<decimal> ICanAggregate<TEntity, TKey>.SumAsync(ISpecification<TEntity> specification,
			Expression<Func<TEntity, decimal?>> selector, CancellationToken cancellationToken)
		{
			try
			{
				return await this.innerRepository.SumAsync(specification, selector, cancellationToken);
			}
			catch(Exception ex)
			{
				this.logger.LogErrorOccurredForOperation("sum", typeof(TEntity).Name, ex);
				throw;
			}
		}

		/// <inheritdoc />
		async Task<float> ICanAggregate<TEntity, TKey>.SumAsync(ISpecification<TEntity> specification,
			Expression<Func<TEntity, float>> selector, CancellationToken cancellationToken)
		{
			try
			{
				return await this.innerRepository.SumAsync(specification, selector, cancellationToken);
			}
			catch(Exception ex)
			{
				this.logger.LogErrorOccurredForOperation("sum", typeof(TEntity).Name, ex);
				throw;
			}
		}

		/// <inheritdoc />
		async Task<float> ICanAggregate<TEntity, TKey>.SumAsync(ISpecification<TEntity> specification,
			Expression<Func<TEntity, float?>> selector, CancellationToken cancellationToken)
		{
			try
			{
				return await this.innerRepository.SumAsync(specification, selector, cancellationToken);
			}
			catch(Exception ex)
			{
				this.logger.LogErrorOccurredForOperation("sum", typeof(TEntity).Name, ex);
				throw;
			}
		}

		/// <inheritdoc />
		async Task<double> ICanAggregate<TEntity, TKey>.SumAsync(ISpecification<TEntity> specification,
			Expression<Func<TEntity, double>> selector, CancellationToken cancellationToken)
		{
			try
			{
				return await this.innerRepository.SumAsync(specification, selector, cancellationToken);
			}
			catch(Exception ex)
			{
				this.logger.LogErrorOccurredForOperation("sum", typeof(TEntity).Name, ex);
				throw;
			}
		}

		/// <inheritdoc />
		async Task<double> ICanAggregate<TEntity, TKey>.SumAsync(ISpecification<TEntity> specification,
			Expression<Func<TEntity, double?>> selector, CancellationToken cancellationToken)
		{
			try
			{
				return await this.innerRepository.SumAsync(specification, selector, cancellationToken);
			}
			catch(Exception ex)
			{
				this.logger.LogErrorOccurredForOperation("sum", typeof(TEntity).Name, ex);
				throw;
			}
		}

		/// <inheritdoc />
		async Task<double> ICanAggregate<TEntity, TKey>.AverageAsync(Expression<Func<TEntity, int>> selector, CancellationToken cancellationToken)
		{
			try
			{
				return await this.innerRepository.AverageAsync(selector, cancellationToken);
			}
			catch(Exception ex)
			{
				this.logger.LogErrorOccurredForOperation("average", typeof(TEntity).Name, ex);
				throw;
			}
		}

		/// <inheritdoc />
		async Task<double> ICanAggregate<TEntity, TKey>.AverageAsync(Expression<Func<TEntity, int?>> selector,
			CancellationToken cancellationToken)
		{
			try
			{
				return await this.innerRepository.AverageAsync(selector, cancellationToken);
			}
			catch(Exception ex)
			{
				this.logger.LogErrorOccurredForOperation("average", typeof(TEntity).Name, ex);
				throw;
			}
		}

		/// <inheritdoc />
		async Task<double> ICanAggregate<TEntity, TKey>.AverageAsync(Expression<Func<TEntity, long>> selector,
			CancellationToken cancellationToken)
		{
			try
			{
				return await this.innerRepository.AverageAsync(selector, cancellationToken);
			}
			catch(Exception ex)
			{
				this.logger.LogErrorOccurredForOperation("average", typeof(TEntity).Name, ex);
				throw;
			}
		}

		/// <inheritdoc />
		async Task<double> ICanAggregate<TEntity, TKey>.AverageAsync(Expression<Func<TEntity, long?>> selector,
			CancellationToken cancellationToken)
		{
			try
			{
				return await this.innerRepository.AverageAsync(selector, cancellationToken);
			}
			catch(Exception ex)
			{
				this.logger.LogErrorOccurredForOperation("average", typeof(TEntity).Name, ex);
				throw;
			}
		}

		/// <inheritdoc />
		async Task<decimal> ICanAggregate<TEntity, TKey>.AverageAsync(Expression<Func<TEntity, decimal>> selector,
			CancellationToken cancellationToken)
		{
			try
			{
				return await this.innerRepository.AverageAsync(selector, cancellationToken);
			}
			catch(Exception ex)
			{
				this.logger.LogErrorOccurredForOperation("average", typeof(TEntity).Name, ex);
				throw;
			}
		}

		/// <inheritdoc />
		async Task<decimal> ICanAggregate<TEntity, TKey>.AverageAsync(Expression<Func<TEntity, decimal?>> selector,
			CancellationToken cancellationToken)
		{
			try
			{
				return await this.innerRepository.AverageAsync(selector, cancellationToken);
			}
			catch(Exception ex)
			{
				this.logger.LogErrorOccurredForOperation("average", typeof(TEntity).Name, ex);
				throw;
			}
		}

		/// <inheritdoc />
		async Task<float> ICanAggregate<TEntity, TKey>.AverageAsync(Expression<Func<TEntity, float>> selector,
			CancellationToken cancellationToken)
		{
			try
			{
				return await this.innerRepository.AverageAsync(selector, cancellationToken);
			}
			catch(Exception ex)
			{
				this.logger.LogErrorOccurredForOperation("average", typeof(TEntity).Name, ex);
				throw;
			}
		}

		/// <inheritdoc />
		async Task<float> ICanAggregate<TEntity, TKey>.AverageAsync(Expression<Func<TEntity, float?>> selector,
			CancellationToken cancellationToken)
		{
			try
			{
				return await this.innerRepository.AverageAsync(selector, cancellationToken);
			}
			catch(Exception ex)
			{
				this.logger.LogErrorOccurredForOperation("average", typeof(TEntity).Name, ex);
				throw;
			}
		}

		/// <inheritdoc />
		async Task<double> ICanAggregate<TEntity, TKey>.AverageAsync(Expression<Func<TEntity, double>> selector,
			CancellationToken cancellationToken)
		{
			try
			{
				return await this.innerRepository.AverageAsync(selector, cancellationToken);
			}
			catch(Exception ex)
			{
				this.logger.LogErrorOccurredForOperation("average", typeof(TEntity).Name, ex);
				throw;
			}
		}

		/// <inheritdoc />
		async Task<double> ICanAggregate<TEntity, TKey>.AverageAsync(Expression<Func<TEntity, double?>> selector,
			CancellationToken cancellationToken)
		{
			try
			{
				return await this.innerRepository.AverageAsync(selector, cancellationToken);
			}
			catch(Exception ex)
			{
				this.logger.LogErrorOccurredForOperation("average", typeof(TEntity).Name, ex);
				throw;
			}
		}

		/// <inheritdoc />
		async Task<double> ICanAggregate<TEntity, TKey>.AverageAsync(Expression<Func<TEntity, bool>> predicate,
			Expression<Func<TEntity, int>> selector, CancellationToken cancellationToken)
		{
			try
			{
				return await this.innerRepository.AverageAsync(predicate, selector, cancellationToken);
			}
			catch(Exception ex)
			{
				this.logger.LogErrorOccurredForOperation("average", typeof(TEntity).Name, ex);
				throw;
			}
		}

		/// <inheritdoc />
		async Task<double> ICanAggregate<TEntity, TKey>.AverageAsync(Expression<Func<TEntity, bool>> predicate,
			Expression<Func<TEntity, int?>> selector, CancellationToken cancellationToken)
		{
			try
			{
				return await this.innerRepository.AverageAsync(predicate, selector, cancellationToken);
			}
			catch(Exception ex)
			{
				this.logger.LogErrorOccurredForOperation("average", typeof(TEntity).Name, ex);
				throw;
			}
		}

		/// <inheritdoc />
		async Task<double> ICanAggregate<TEntity, TKey>.AverageAsync(Expression<Func<TEntity, bool>> predicate,
			Expression<Func<TEntity, long>> selector, CancellationToken cancellationToken)
		{
			try
			{
				return await this.innerRepository.AverageAsync(predicate, selector, cancellationToken);
			}
			catch(Exception ex)
			{
				this.logger.LogErrorOccurredForOperation("average", typeof(TEntity).Name, ex);
				throw;
			}
		}

		/// <inheritdoc />
		async Task<double> ICanAggregate<TEntity, TKey>.AverageAsync(Expression<Func<TEntity, bool>> predicate,
			Expression<Func<TEntity, long?>> selector, CancellationToken cancellationToken)
		{
			try
			{
				return await this.innerRepository.AverageAsync(predicate, selector, cancellationToken);
			}
			catch(Exception ex)
			{
				this.logger.LogErrorOccurredForOperation("average", typeof(TEntity).Name, ex);
				throw;
			}
		}

		/// <inheritdoc />
		async Task<decimal> ICanAggregate<TEntity, TKey>.AverageAsync(Expression<Func<TEntity, bool>> predicate,
			Expression<Func<TEntity, decimal>> selector, CancellationToken cancellationToken)
		{
			try
			{
				return await this.innerRepository.AverageAsync(predicate, selector, cancellationToken);
			}
			catch(Exception ex)
			{
				this.logger.LogErrorOccurredForOperation("average", typeof(TEntity).Name, ex);
				throw;
			}
		}

		/// <inheritdoc />
		async Task<decimal> ICanAggregate<TEntity, TKey>.AverageAsync(Expression<Func<TEntity, bool>> predicate,
			Expression<Func<TEntity, decimal?>> selector, CancellationToken cancellationToken)
		{
			try
			{
				return await this.innerRepository.AverageAsync(predicate, selector, cancellationToken);
			}
			catch(Exception ex)
			{
				this.logger.LogErrorOccurredForOperation("average", typeof(TEntity).Name, ex);
				throw;
			}
		}

		/// <inheritdoc />
		async Task<float> ICanAggregate<TEntity, TKey>.AverageAsync(Expression<Func<TEntity, bool>> predicate,
			Expression<Func<TEntity, float>> selector, CancellationToken cancellationToken)
		{
			try
			{
				return await this.innerRepository.AverageAsync(predicate, selector, cancellationToken);
			}
			catch(Exception ex)
			{
				this.logger.LogErrorOccurredForOperation("average", typeof(TEntity).Name, ex);
				throw;
			}
		}

		/// <inheritdoc />
		async Task<float> ICanAggregate<TEntity, TKey>.AverageAsync(Expression<Func<TEntity, bool>> predicate,
			Expression<Func<TEntity, float?>> selector, CancellationToken cancellationToken)
		{
			try
			{
				return await this.innerRepository.AverageAsync(predicate, selector, cancellationToken);
			}
			catch(Exception ex)
			{
				this.logger.LogErrorOccurredForOperation("average", typeof(TEntity).Name, ex);
				throw;
			}
		}

		/// <inheritdoc />
		async Task<double> ICanAggregate<TEntity, TKey>.AverageAsync(Expression<Func<TEntity, bool>> predicate,
			Expression<Func<TEntity, double>> selector, CancellationToken cancellationToken)
		{
			try
			{
				return await this.innerRepository.AverageAsync(predicate, selector, cancellationToken);
			}
			catch(Exception ex)
			{
				this.logger.LogErrorOccurredForOperation("average", typeof(TEntity).Name, ex);
				throw;
			}
		}

		/// <inheritdoc />
		async Task<double> ICanAggregate<TEntity, TKey>.AverageAsync(Expression<Func<TEntity, bool>> predicate,
			Expression<Func<TEntity, double?>> selector, CancellationToken cancellationToken)
		{
			try
			{
				return await this.innerRepository.AverageAsync(predicate, selector, cancellationToken);
			}
			catch(Exception ex)
			{
				this.logger.LogErrorOccurredForOperation("average", typeof(TEntity).Name, ex);
				throw;
			}
		}

		/// <inheritdoc />
		async Task<double> ICanAggregate<TEntity, TKey>.AverageAsync(ISpecification<TEntity> specification,
			Expression<Func<TEntity, int>> selector, CancellationToken cancellationToken)
		{
			try
			{
				return await this.innerRepository.AverageAsync(specification, selector, cancellationToken);
			}
			catch(Exception ex)
			{
				this.logger.LogErrorOccurredForOperation("average", typeof(TEntity).Name, ex);
				throw;
			}
		}

		/// <inheritdoc />
		async Task<double> ICanAggregate<TEntity, TKey>.AverageAsync(ISpecification<TEntity> specification,
			Expression<Func<TEntity, int?>> selector, CancellationToken cancellationToken)
		{
			try
			{
				return await this.innerRepository.AverageAsync(specification, selector, cancellationToken);
			}
			catch(Exception ex)
			{
				this.logger.LogErrorOccurredForOperation("average", typeof(TEntity).Name, ex);
				throw;
			}
		}

		/// <inheritdoc />
		async Task<double> ICanAggregate<TEntity, TKey>.AverageAsync(ISpecification<TEntity> specification,
			Expression<Func<TEntity, long>> selector, CancellationToken cancellationToken)
		{
			try
			{
				return await this.innerRepository.AverageAsync(specification, selector, cancellationToken);
			}
			catch(Exception ex)
			{
				this.logger.LogErrorOccurredForOperation("average", typeof(TEntity).Name, ex);
				throw;
			}
		}

		/// <inheritdoc />
		async Task<double> ICanAggregate<TEntity, TKey>.AverageAsync(ISpecification<TEntity> specification,
			Expression<Func<TEntity, long?>> selector, CancellationToken cancellationToken)
		{
			try
			{
				return await this.innerRepository.AverageAsync(specification, selector, cancellationToken);
			}
			catch(Exception ex)
			{
				this.logger.LogErrorOccurredForOperation("average", typeof(TEntity).Name, ex);
				throw;
			}
		}

		/// <inheritdoc />
		async Task<decimal> ICanAggregate<TEntity, TKey>.AverageAsync(ISpecification<TEntity> specification,
			Expression<Func<TEntity, decimal>> selector, CancellationToken cancellationToken)
		{
			try
			{
				return await this.innerRepository.AverageAsync(specification, selector, cancellationToken);
			}
			catch(Exception ex)
			{
				this.logger.LogErrorOccurredForOperation("average", typeof(TEntity).Name, ex);
				throw;
			}
		}

		/// <inheritdoc />
		async Task<decimal> ICanAggregate<TEntity, TKey>.AverageAsync(ISpecification<TEntity> specification,
			Expression<Func<TEntity, decimal?>> selector, CancellationToken cancellationToken)
		{
			try
			{
				return await this.innerRepository.AverageAsync(specification, selector, cancellationToken);
			}
			catch(Exception ex)
			{
				this.logger.LogErrorOccurredForOperation("average", typeof(TEntity).Name, ex);
				throw;
			}
		}

		/// <inheritdoc />
		async Task<float> ICanAggregate<TEntity, TKey>.AverageAsync(ISpecification<TEntity> specification,
			Expression<Func<TEntity, float>> selector, CancellationToken cancellationToken)
		{
			try
			{
				return await this.innerRepository.AverageAsync(specification, selector, cancellationToken);
			}
			catch(Exception ex)
			{
				this.logger.LogErrorOccurredForOperation("average", typeof(TEntity).Name, ex);
				throw;
			}
		}

		/// <inheritdoc />
		async Task<float> ICanAggregate<TEntity, TKey>.AverageAsync(ISpecification<TEntity> specification,
			Expression<Func<TEntity, float?>> selector, CancellationToken cancellationToken)
		{
			try
			{
				return await this.innerRepository.AverageAsync(specification, selector, cancellationToken);
			}
			catch(Exception ex)
			{
				this.logger.LogErrorOccurredForOperation("average", typeof(TEntity).Name, ex);
				throw;
			}
		}

		/// <inheritdoc />
		async Task<double> ICanAggregate<TEntity, TKey>.AverageAsync(ISpecification<TEntity> specification,
			Expression<Func<TEntity, double>> selector, CancellationToken cancellationToken)
		{
			try
			{
				return await this.innerRepository.AverageAsync(specification, selector, cancellationToken);
			}
			catch(Exception ex)
			{
				this.logger.LogErrorOccurredForOperation("average", typeof(TEntity).Name, ex);
				throw;
			}
		}

		/// <inheritdoc />
		async Task<double> ICanAggregate<TEntity, TKey>.AverageAsync(ISpecification<TEntity> specification,
			Expression<Func<TEntity, double?>> selector, CancellationToken cancellationToken)
		{
			try
			{
				return await this.innerRepository.AverageAsync(specification, selector, cancellationToken);
			}
			catch(Exception ex)
			{
				this.logger.LogErrorOccurredForOperation("average", typeof(TEntity).Name, ex);
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
		public RepositoryName RepositoryName => this.innerRepository.RepositoryName;

		/// <inheritdoc />
		public override string ToString()
		{
			return this.innerRepository.ToString();
		}
	}
}
