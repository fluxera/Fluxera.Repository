namespace Fluxera.Repository.Decorators
{
	using System;
	using System.Collections.Generic;
	using System.Diagnostics;
	using System.Linq;
	using System.Linq.Expressions;
	using System.Threading;
	using System.Threading.Tasks;
	using Fluxera.Entity;
	using Fluxera.Guards;
	using Fluxera.Linq.Expressions;
	using Fluxera.Repository.Extensions;
	using Fluxera.Repository.Interception;
	using Fluxera.Repository.Options;
	using Fluxera.Repository.Query;
	using Fluxera.Repository.Query.Impl;
	using Fluxera.Repository.Specifications;
	using Fluxera.Repository.Traits;
	using Microsoft.Extensions.Logging;

	/// <summary>
	///     A repository decorator that controls the interceptor feature.
	/// </summary>
	/// <typeparam name="TEntity"></typeparam>
	/// <typeparam name="TKey"></typeparam>
	public sealed class InterceptionRepositoryDecorator<TEntity, TKey> : IRepository<TEntity, TKey>
		where TEntity : Entity<TEntity, TKey>
		where TKey : notnull, IComparable<TKey>, IEquatable<TKey>
	{
		private readonly IRepository<TEntity, TKey> innerRepository;
		private readonly IInterceptor<TEntity, TKey> interceptor;
		private readonly ILogger logger;
		private readonly RepositoryOptions repositoryOptions;

		/// <summary>
		///     Creates a new instance of the <see cref="InterceptionRepositoryDecorator{TAggregateRoot,TKey}" /> type.
		/// </summary>
		/// <param name="loggerFactory"></param>
		/// <param name="innerRepository"></param>
		/// <param name="repositoryRegistry"></param>
		/// <param name="decoratingInterceptorFactory"></param>
		public InterceptionRepositoryDecorator(
			IRepository<TEntity, TKey> innerRepository,
			IRepositoryRegistry repositoryRegistry,
			IDecoratingInterceptorFactory<TEntity, TKey> decoratingInterceptorFactory,
			ILoggerFactory loggerFactory)
		{
			this.innerRepository = Guard.Against.Null(innerRepository);
			Guard.Against.Null(repositoryRegistry);
			Guard.Against.Null(decoratingInterceptorFactory);
			Guard.Against.Null(loggerFactory);

			RepositoryName repositoryName = repositoryRegistry.GetRepositoryNameFor<TEntity>();
			this.repositoryOptions = repositoryRegistry.GetRepositoryOptionsFor(repositoryName);

			this.interceptor = decoratingInterceptorFactory.CreateDecoratingInterceptor();

			this.logger = loggerFactory.CreateLogger(LoggerNames.Interception);
		}

		/// <inheritdoc />
		async Task ICanAdd<TEntity, TKey>.AddAsync(TEntity item, CancellationToken cancellationToken)
		{
			if(this.repositoryOptions.InterceptionOptions.IsEnabled)
			{
				InterceptionEvent e = new InterceptionEvent();
				await this.interceptor.BeforeAddAsync(item, e).ConfigureAwait(false);
				if(e.CancelOperation && e.ThrowOnCancellation)
				{
					throw new InvalidOperationException(e.CancellationMessage);
				}

				if(!e.CancelOperation)
				{
					await this.innerRepository.AddAsync(item, cancellationToken).ConfigureAwait(false);
				}
				else
				{
					RecordCancellation(e.CancellationMessage);

					this.logger.LogCancellationMessage(e.CancellationMessage);
				}
			}
			else
			{
				await this.innerRepository.AddAsync(item, cancellationToken).ConfigureAwait(false);
			}
		}

		/// <inheritdoc />
		async Task ICanAdd<TEntity, TKey>.AddRangeAsync(IEnumerable<TEntity> items, CancellationToken cancellationToken)
		{
			if(this.repositoryOptions.InterceptionOptions.IsEnabled)
			{
				IEnumerable<TEntity> itemsList = items.ToList();

				InterceptionEvent e = new InterceptionEvent();
				foreach(TEntity item in itemsList)
				{
					await this.interceptor.BeforeAddAsync(item, e).ConfigureAwait(false);
					if(e.CancelOperation && e.ThrowOnCancellation)
					{
						throw new InvalidOperationException(e.CancellationMessage);
					}
				}

				if(!e.CancelOperation)
				{
					await this.innerRepository.AddRangeAsync(itemsList, cancellationToken).ConfigureAwait(false);
				}
				else
				{
					RecordCancellation(e.CancellationMessage);

					this.logger.LogCancellationMessage(e.CancellationMessage);
				}
			}
			else
			{
				await this.innerRepository.AddRangeAsync(items, cancellationToken).ConfigureAwait(false);
			}
		}

		/// <inheritdoc />
		async Task ICanUpdate<TEntity, TKey>.UpdateAsync(TEntity item, CancellationToken cancellationToken)
		{
			if(this.repositoryOptions.InterceptionOptions.IsEnabled)
			{
				InterceptionEvent e = new InterceptionEvent();
				await this.interceptor.BeforeUpdateAsync(item, e).ConfigureAwait(false);
				if(e.CancelOperation && e.ThrowOnCancellation)
				{
					throw new InvalidOperationException(e.CancellationMessage);
				}

				if(!e.CancelOperation)
				{
					await this.innerRepository.UpdateAsync(item, cancellationToken).ConfigureAwait(false);
				}
				else
				{
					RecordCancellation(e.CancellationMessage);

					this.logger.LogCancellationMessage(e.CancellationMessage);
				}
			}
			else
			{
				await this.innerRepository.UpdateAsync(item, cancellationToken).ConfigureAwait(false);
			}
		}

		/// <inheritdoc />
		async Task ICanUpdate<TEntity, TKey>.UpdateRangeAsync(IEnumerable<TEntity> items, CancellationToken cancellationToken)
		{
			if(this.repositoryOptions.InterceptionOptions.IsEnabled)
			{
				IEnumerable<TEntity> itemsList = items.ToList();

				InterceptionEvent e = new InterceptionEvent();
				foreach(TEntity item in itemsList)
				{
					await this.interceptor.BeforeUpdateAsync(item, e).ConfigureAwait(false);
					if(e.CancelOperation && e.ThrowOnCancellation)
					{
						throw new InvalidOperationException(e.CancellationMessage);
					}
				}

				if(!e.CancelOperation)
				{
					await this.innerRepository.UpdateRangeAsync(itemsList, cancellationToken).ConfigureAwait(false);
				}
				else
				{
					RecordCancellation(e.CancellationMessage);

					this.logger.LogCancellationMessage(e.CancellationMessage);
				}
			}
			else
			{
				await this.innerRepository.UpdateRangeAsync(items, cancellationToken).ConfigureAwait(false);
			}
		}

		/// <inheritdoc />
		async Task ICanRemove<TEntity, TKey>.RemoveAsync(TKey id, CancellationToken cancellationToken)
		{
			if(this.repositoryOptions.InterceptionOptions.IsEnabled)
			{
				InterceptionEvent e = new InterceptionEvent();
				Expression<Func<TEntity, bool>> predicate = CreatePrimaryKeyPredicate(id);
				predicate = await this.interceptor.BeforeRemoveRangeAsync(predicate, e).ConfigureAwait(false);
				if(e.CancelOperation && e.ThrowOnCancellation)
				{
					throw new InvalidOperationException(e.CancellationMessage);
				}

				if(!e.CancelOperation)
				{
					RecordStatement(predicate);

					await this.innerRepository.RemoveRangeAsync(predicate, cancellationToken).ConfigureAwait(false);
				}
				else
				{
					RecordCancellation(e.CancellationMessage);

					this.logger.LogCancellationMessage(e.CancellationMessage);
				}
			}
			else
			{
				RecordStatement(CreatePrimaryKeyPredicate(id));

				await this.innerRepository.RemoveAsync(id, cancellationToken).ConfigureAwait(false);
			}
		}

		/// <inheritdoc />
		async Task ICanRemove<TEntity, TKey>.RemoveAsync(TEntity item, CancellationToken cancellationToken)
		{
			if(this.repositoryOptions.InterceptionOptions.IsEnabled)
			{
				InterceptionEvent e = new InterceptionEvent();
				await this.interceptor.BeforeRemoveAsync(item, e).ConfigureAwait(false);
				if(e.CancelOperation && e.ThrowOnCancellation)
				{
					throw new InvalidOperationException(e.CancellationMessage);
				}

				if(!e.CancelOperation)
				{
					await this.innerRepository.RemoveAsync(item, cancellationToken).ConfigureAwait(false);
				}
				else
				{
					RecordCancellation(e.CancellationMessage);

					this.logger.LogCancellationMessage(e.CancellationMessage);
				}
			}
			else
			{
				await this.innerRepository.RemoveAsync(item, cancellationToken).ConfigureAwait(false);
			}
		}

		/// <inheritdoc />
		async Task ICanRemove<TEntity, TKey>.RemoveRangeAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken)
		{
			if(this.repositoryOptions.InterceptionOptions.IsEnabled)
			{
				InterceptionEvent e = new InterceptionEvent();
				predicate = await this.interceptor.BeforeRemoveRangeAsync(predicate, e).ConfigureAwait(false);
				if(e.CancelOperation && e.ThrowOnCancellation)
				{
					throw new InvalidOperationException(e.CancellationMessage);
				}

				if(!e.CancelOperation)
				{
					RecordStatement(predicate);

					await this.innerRepository.RemoveRangeAsync(predicate, cancellationToken).ConfigureAwait(false);
				}
				else
				{
					RecordCancellation(e.CancellationMessage);

					this.logger.LogCancellationMessage(e.CancellationMessage);
				}
			}
			else
			{
				RecordStatement(predicate);

				await this.innerRepository.RemoveRangeAsync(predicate, cancellationToken).ConfigureAwait(false);
			}
		}

		/// <inheritdoc />
		async Task ICanRemove<TEntity, TKey>.RemoveRangeAsync(ISpecification<TEntity> specification, CancellationToken cancellationToken)
		{
			if(this.repositoryOptions.InterceptionOptions.IsEnabled)
			{
				InterceptionEvent e = new InterceptionEvent();
				specification = await this.interceptor.BeforeRemoveRangeAsync(specification, e).ConfigureAwait(false);
				if(e.CancelOperation && e.ThrowOnCancellation)
				{
					throw new InvalidOperationException(e.CancellationMessage);
				}

				if(!e.CancelOperation)
				{
					RecordStatement(specification);

					await this.innerRepository.RemoveRangeAsync(specification, cancellationToken).ConfigureAwait(false);
				}
				else
				{
					RecordCancellation(e.CancellationMessage);

					this.logger.LogCancellationMessage(e.CancellationMessage);
				}
			}
			else
			{
				RecordStatement(specification);

				await this.innerRepository.RemoveRangeAsync(specification, cancellationToken).ConfigureAwait(false);
			}
		}

		/// <inheritdoc />
		async Task ICanRemove<TEntity, TKey>.RemoveRangeAsync(IEnumerable<TEntity> items, CancellationToken cancellationToken)
		{
			if(this.repositoryOptions.InterceptionOptions.IsEnabled)
			{
				IEnumerable<TEntity> itemsList = items.ToList();

				InterceptionEvent e = new InterceptionEvent();
				foreach(TEntity item in itemsList)
				{
					await this.interceptor.BeforeRemoveAsync(item, e).ConfigureAwait(false);
					if(e.CancelOperation && e.ThrowOnCancellation)
					{
						throw new InvalidOperationException(e.CancellationMessage);
					}
				}

				if(!e.CancelOperation)
				{
					await this.innerRepository.RemoveRangeAsync(itemsList, cancellationToken).ConfigureAwait(false);
				}
				else
				{
					RecordCancellation(e.CancellationMessage);

					this.logger.LogCancellationMessage(e.CancellationMessage);
				}
			}
			else
			{
				await this.innerRepository.RemoveRangeAsync(items, cancellationToken).ConfigureAwait(false);
			}
		}

		/// <inheritdoc />
		async Task<TEntity> ICanFind<TEntity, TKey>.FindOneAsync(Expression<Func<TEntity, bool>> predicate,
			IQueryOptions<TEntity> queryOptions, CancellationToken cancellationToken)
		{
			if(this.repositoryOptions.InterceptionOptions.IsEnabled)
			{
				predicate = await this.interceptor.BeforeFindAsync(predicate, queryOptions).ConfigureAwait(false);
			}

			RecordStatement(predicate);

			TEntity result = await this.innerRepository.FindOneAsync(predicate, queryOptions, cancellationToken).ConfigureAwait(false);

			return result;
		}

		/// <inheritdoc />
		async Task<TEntity> ICanFind<TEntity, TKey>.FindOneAsync(ISpecification<TEntity> specification,
			IQueryOptions<TEntity> queryOptions, CancellationToken cancellationToken)
		{
			if(this.repositoryOptions.InterceptionOptions.IsEnabled)
			{
				specification = await this.interceptor.BeforeFindAsync(specification, queryOptions).ConfigureAwait(false);
			}

			RecordStatement(specification);

			TEntity result = await this.innerRepository.FindOneAsync(specification, queryOptions, cancellationToken).ConfigureAwait(false);

			return result;
		}

		/// <inheritdoc />
		async Task<TResult> ICanFind<TEntity, TKey>.FindOneAsync<TResult>(Expression<Func<TEntity, bool>> predicate,
			Expression<Func<TEntity, TResult>> selector, IQueryOptions<TEntity> queryOptions, CancellationToken cancellationToken)
		{
			if(this.repositoryOptions.InterceptionOptions.IsEnabled)
			{
				predicate = await this.interceptor.BeforeFindAsync(predicate, queryOptions).ConfigureAwait(false);
			}

			RecordStatement(predicate, selector);

			TResult result = await this.innerRepository.FindOneAsync(predicate, selector, queryOptions, cancellationToken).ConfigureAwait(false);

			return result;
		}

		/// <inheritdoc />
		async Task<TResult> ICanFind<TEntity, TKey>.FindOneAsync<TResult>(ISpecification<TEntity> specification,
			Expression<Func<TEntity, TResult>> selector, IQueryOptions<TEntity> queryOptions, CancellationToken cancellationToken)
		{
			if(this.repositoryOptions.InterceptionOptions.IsEnabled)
			{
				specification = await this.interceptor.BeforeFindAsync(specification, queryOptions).ConfigureAwait(false);
			}

			RecordStatement(specification, selector);

			TResult result = await this.innerRepository.FindOneAsync(specification, selector, queryOptions, cancellationToken).ConfigureAwait(false);

			return result;
		}

		/// <inheritdoc />
		async Task<bool> ICanGet<TEntity, TKey>.ExistsAsync(TKey id, CancellationToken cancellationToken)
		{
			bool result;

			if(this.repositoryOptions.InterceptionOptions.IsEnabled)
			{
				Expression<Func<TEntity, bool>> predicate = CreatePrimaryKeyPredicate(id);
				predicate = await this.interceptor.BeforeFindAsync(predicate, QueryOptions<TEntity>.Empty());

				RecordStatement(predicate);

				result = await this.innerRepository.ExistsAsync(predicate, cancellationToken).ConfigureAwait(false);
			}
			else
			{
				RecordStatement(CreatePrimaryKeyPredicate(id));

				result = await this.innerRepository.ExistsAsync(id, cancellationToken).ConfigureAwait(false);
			}

			return result;
		}

		/// <inheritdoc />
		async Task<bool> ICanFind<TEntity, TKey>.ExistsAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken)
		{
			if(this.repositoryOptions.InterceptionOptions.IsEnabled)
			{
				predicate = await this.interceptor.BeforeFindAsync(predicate, QueryOptions<TEntity>.Empty());
			}

			RecordStatement(predicate);

			bool result = await this.innerRepository.ExistsAsync(predicate, cancellationToken).ConfigureAwait(false);

			return result;
		}

		/// <inheritdoc />
		async Task<bool> ICanFind<TEntity, TKey>.ExistsAsync(ISpecification<TEntity> specification, CancellationToken cancellationToken)
		{
			if(this.repositoryOptions.InterceptionOptions.IsEnabled)
			{
				specification = await this.interceptor.BeforeFindAsync(specification, QueryOptions<TEntity>.Empty());
			}

			RecordStatement(specification);

			bool result = await this.innerRepository.ExistsAsync(specification, cancellationToken).ConfigureAwait(false);

			return result;
		}

		/// <inheritdoc />
		async Task<IReadOnlyCollection<TEntity>> ICanFind<TEntity, TKey>.FindManyAsync(Expression<Func<TEntity, bool>> predicate,
			IQueryOptions<TEntity> queryOptions, CancellationToken cancellationToken)
		{
			if(this.repositoryOptions.InterceptionOptions.IsEnabled)
			{
				predicate = await this.interceptor.BeforeFindAsync(predicate, queryOptions);
			}

			RecordStatement(predicate);

			IReadOnlyCollection<TEntity> result =
				await this.innerRepository.FindManyAsync(predicate, queryOptions, cancellationToken).ConfigureAwait(false);

			return result;
		}

		/// <inheritdoc />
		async Task<IReadOnlyCollection<TEntity>> ICanFind<TEntity, TKey>.FindManyAsync(ISpecification<TEntity> specification,
			IQueryOptions<TEntity> queryOptions, CancellationToken cancellationToken)
		{
			if(this.repositoryOptions.InterceptionOptions.IsEnabled)
			{
				specification = await this.interceptor.BeforeFindAsync(specification, queryOptions);
			}

			RecordStatement(specification);

			IReadOnlyCollection<TEntity> result =
				await this.innerRepository.FindManyAsync(specification, queryOptions, cancellationToken).ConfigureAwait(false);

			return result;
		}

		/// <inheritdoc />
		async Task<IReadOnlyCollection<TResult>> ICanFind<TEntity, TKey>.FindManyAsync<TResult>(Expression<Func<TEntity, bool>> predicate,
			Expression<Func<TEntity, TResult>> selector, IQueryOptions<TEntity> queryOptions, CancellationToken cancellationToken)
		{
			if(this.repositoryOptions.InterceptionOptions.IsEnabled)
			{
				predicate = await this.interceptor.BeforeFindAsync(predicate, queryOptions);
			}

			RecordStatement(predicate, selector);

			IReadOnlyCollection<TResult> result =
				await this.innerRepository.FindManyAsync(predicate, selector, queryOptions, cancellationToken).ConfigureAwait(false);

			return result;
		}

		/// <inheritdoc />
		async Task<IReadOnlyCollection<TResult>> ICanFind<TEntity, TKey>.FindManyAsync<TResult>(ISpecification<TEntity> specification,
			Expression<Func<TEntity, TResult>> selector, IQueryOptions<TEntity> queryOptions, CancellationToken cancellationToken)
		{
			if(this.repositoryOptions.InterceptionOptions.IsEnabled)
			{
				specification = await this.interceptor.BeforeFindAsync(specification, queryOptions);
			}

			RecordStatement(specification, selector);

			IReadOnlyCollection<TResult> result =
				await this.innerRepository.FindManyAsync(specification, selector, queryOptions, cancellationToken).ConfigureAwait(false);

			return result;
		}

		/// <inheritdoc />
		async Task<long> ICanGet<TEntity, TKey>.CountAsync(CancellationToken cancellationToken)
		{
			long result;

			if(this.repositoryOptions.InterceptionOptions.IsEnabled)
			{
				Expression<Func<TEntity, bool>> predicate = x => true;
				predicate = await this.interceptor.BeforeFindAsync(predicate, QueryOptions<TEntity>.Empty());

				RecordStatement(predicate);

				result = await this.innerRepository.CountAsync(predicate, cancellationToken).ConfigureAwait(false);
			}
			else
			{
				RecordStatement(x => true);

				result = await this.innerRepository.CountAsync(cancellationToken).ConfigureAwait(false);
			}

			return result;
		}

		/// <inheritdoc />
		async Task<long> ICanGet<TEntity, TKey>.CountAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken)
		{
			if(this.repositoryOptions.InterceptionOptions.IsEnabled)
			{
				predicate = await this.interceptor.BeforeFindAsync(predicate, QueryOptions<TEntity>.Empty());
			}

			RecordStatement(predicate);

			long result = await this.innerRepository.CountAsync(predicate, cancellationToken).ConfigureAwait(false);

			return result;
		}

		/// <inheritdoc />
		async Task<long> ICanGet<TEntity, TKey>.CountAsync(ISpecification<TEntity> specification, CancellationToken cancellationToken)
		{
			if(this.repositoryOptions.InterceptionOptions.IsEnabled)
			{
				specification = await this.interceptor.BeforeFindAsync(specification, QueryOptions<TEntity>.Empty());
			}

			RecordStatement(specification);

			long result = await this.innerRepository.CountAsync(specification, cancellationToken).ConfigureAwait(false);

			return result;
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
		async Task<TEntity> ICanGet<TEntity, TKey>.GetAsync(TKey id, CancellationToken cancellationToken)
		{
			TEntity result;

			if(this.repositoryOptions.InterceptionOptions.IsEnabled)
			{
				Expression<Func<TEntity, bool>> predicate = CreatePrimaryKeyPredicate(id);
				predicate = await this.interceptor.BeforeFindAsync(predicate, QueryOptions<TEntity>.Empty());

				RecordStatement(predicate);

				result = await this.innerRepository.FindOneAsync(predicate, QueryOptions<TEntity>.Empty(), cancellationToken).ConfigureAwait(false);
			}
			else
			{
				RecordStatement(CreatePrimaryKeyPredicate(id));

				result = await this.innerRepository.GetAsync(id, cancellationToken).ConfigureAwait(false);
			}

			return result;
		}

		/// <inheritdoc />
		async Task<TResult> ICanGet<TEntity, TKey>.GetAsync<TResult>(TKey id, Expression<Func<TEntity, TResult>> selector,
			CancellationToken cancellationToken)
		{
			TResult result;

			if(this.repositoryOptions.InterceptionOptions.IsEnabled)
			{
				Expression<Func<TEntity, bool>> predicate = CreatePrimaryKeyPredicate(id);
				predicate = await this.interceptor.BeforeFindAsync(predicate, QueryOptions<TEntity>.Empty());

				RecordStatement(predicate, selector);

				result = await this.innerRepository.FindOneAsync(predicate, selector, QueryOptions<TEntity>.Empty(), cancellationToken)
					.ConfigureAwait(false);
			}
			else
			{
				RecordStatement(id?.ToString(), selector);

				result = await this.innerRepository.GetAsync(id, selector, cancellationToken).ConfigureAwait(false);
			}

			return result;
		}


		/// <inheritdoc />
		bool IDisposableRepository.IsDisposed => this.innerRepository.IsDisposed;

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
		public RepositoryName RepositoryName => this.innerRepository.RepositoryName;

		/// <inheritdoc />
		public override string ToString()
		{
			return this.innerRepository.ToString();
		}

		private static void RecordCancellation(string cancellationMessage)
		{
			Activity activity = Activity.Current;

			if(activity?.IsAllDataRequested == true && !string.IsNullOrWhiteSpace(cancellationMessage))
			{
				activity.AddTag("db.repository.cancellation", cancellationMessage);
			}
		}

		private static void RecordStatement(ISpecification<TEntity> specification)
		{
			if(Activity.Current != null)
			{
				RecordStatement(specification.Predicate);
			}
		}

		private static void RecordStatement<TResult>(ISpecification<TEntity> specification, Expression<Func<TEntity, TResult>> selector)
		{
			if(Activity.Current != null)
			{
				RecordStatement(specification.Predicate, selector);
			}
		}

		private static void RecordStatement(Expression<Func<TEntity, bool>> predicate)
		{
			if(Activity.Current != null)
			{
				RecordStatement(predicate.ToExpressionString());
			}
		}

		private static void RecordStatement<TResult>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TResult>> selector)
		{
			if(Activity.Current != null)
			{
				RecordStatement(predicate.ToExpressionString(), selector.ToExpressionString());
			}
		}

		private static void RecordStatement<TResult>(string predicate, Expression<Func<TEntity, TResult>> selector)
		{
			if(Activity.Current != null)
			{
				RecordStatement(predicate, selector.ToExpressionString());
			}
		}

		private static void RecordStatement(string predicate, string selector = "" /*, string? queryOptions = ""*/)
		{
			Activity activity = Activity.Current;

			if(activity != null && activity.IsAllDataRequested && !string.IsNullOrWhiteSpace(predicate))
			{
				string statement = predicate;

				if(!string.IsNullOrWhiteSpace(predicate))
				{
					activity.AddTag("db.repository.predicate", predicate);
				}

				if(!string.IsNullOrWhiteSpace(selector))
				{
					statement = string.Concat(statement, " | ", selector);
					activity.AddTag("db.repository.selector", selector);
				}

				//if(!string.IsNullOrWhiteSpace(queryOptions))
				//{
				//	statement = string.Concat(statement, " | ", queryOptions);
				//	activity.AddTag("db.repository.queryOptions", queryOptions);
				//}

				activity.AddTag("db.statement", statement);
			}
		}

		private static Expression<Func<TEntity, bool>> CreatePrimaryKeyPredicate(TKey id)
		{
			return id.CreatePrimaryKeyPredicate<TEntity, TKey>();
		}
	}
}
