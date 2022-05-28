namespace Fluxera.Repository.Decorators
{
	using System;
	using System.Collections.Generic;
	using System.Diagnostics;
	using System.Linq;
	using System.Linq.Expressions;
	using System.Reflection;
	using System.Threading;
	using System.Threading.Tasks;
	using Fluxera.Entity;
	using Fluxera.Guards;
	using Fluxera.Linq.Expressions;
	using Fluxera.Repository.Interception;
	using Fluxera.Repository.Options;
	using Fluxera.Repository.Query;
	using Fluxera.Repository.Specifications;
	using Fluxera.Repository.Traits;
	using Microsoft.Extensions.Logging;

	/// <summary>
	///     A repository decorator that controls the interceptor feature.
	/// </summary>
	/// <typeparam name="TAggregateRoot"></typeparam>
	/// <typeparam name="TKey"></typeparam>
	public sealed class InterceptionRepositoryDecorator<TAggregateRoot, TKey> : IRepository<TAggregateRoot, TKey>
		where TAggregateRoot : AggregateRoot<TAggregateRoot, TKey>
		where TKey : IComparable<TKey>, IEquatable<TKey>
	{
		private readonly IRepository<TAggregateRoot, TKey> innerRepository;
		private readonly IInterceptor<TAggregateRoot, TKey> interceptor;
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
			ILoggerFactory loggerFactory,
			IRepository<TAggregateRoot, TKey> innerRepository,
			IRepositoryRegistry repositoryRegistry,
			IDecoratingInterceptorFactory<TAggregateRoot, TKey> decoratingInterceptorFactory)
		{
			Guard.Against.Null(loggerFactory, nameof(loggerFactory));
			Guard.Against.Null(innerRepository, nameof(innerRepository));
			Guard.Against.Null(repositoryRegistry, nameof(repositoryRegistry));
			Guard.Against.Null(decoratingInterceptorFactory, nameof(decoratingInterceptorFactory));

			this.logger = loggerFactory.CreateLogger(LoggerNames.Interception);

			this.innerRepository = innerRepository;

			RepositoryName repositoryName = repositoryRegistry.GetRepositoryNameFor<TAggregateRoot>();
			this.repositoryOptions = repositoryRegistry.GetRepositoryOptionsFor(repositoryName);

			this.interceptor = decoratingInterceptorFactory.CreateDecoratingInterceptor();
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
		async Task ICanAdd<TAggregateRoot, TKey>.AddAsync(TAggregateRoot item, CancellationToken cancellationToken)
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
					//await this.interceptor.AfterAddAsync(item).ConfigureAwait(false);
				}
				else
				{
					RecordCancellation(e.CancellationMessage);

					this.logger.LogInformation(e.CancellationMessage);
				}
			}
			else
			{
				await this.innerRepository.AddAsync(item, cancellationToken).ConfigureAwait(false);
			}
		}

		/// <inheritdoc />
		async Task ICanAdd<TAggregateRoot, TKey>.AddRangeAsync(IEnumerable<TAggregateRoot> items, CancellationToken cancellationToken)
		{
			if(this.repositoryOptions.InterceptionOptions.IsEnabled)
			{
				IEnumerable<TAggregateRoot> itemsList = items.ToList();

				InterceptionEvent e = new InterceptionEvent();
				foreach(TAggregateRoot item in itemsList)
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

					//foreach(TAggregateRoot item in itemsList)
					//{
					//	await this.interceptor.AfterAddAsync(item).ConfigureAwait(false);
					//}
				}
				else
				{
					RecordCancellation(e.CancellationMessage);

					this.logger.LogInformation(e.CancellationMessage);
				}
			}
			else
			{
				await this.innerRepository.AddRangeAsync(items, cancellationToken).ConfigureAwait(false);
			}
		}

		/// <inheritdoc />
		async Task ICanUpdate<TAggregateRoot, TKey>.UpdateAsync(TAggregateRoot item, CancellationToken cancellationToken)
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
					//await this.interceptor.AfterUpdateAsync(item).ConfigureAwait(false);
				}
				else
				{
					RecordCancellation(e.CancellationMessage);

					this.logger.LogInformation(e.CancellationMessage);
				}
			}
			else
			{
				await this.innerRepository.UpdateAsync(item, cancellationToken).ConfigureAwait(false);
			}
		}

		/// <inheritdoc />
		async Task ICanUpdate<TAggregateRoot, TKey>.UpdateRangeAsync(IEnumerable<TAggregateRoot> items, CancellationToken cancellationToken)
		{
			if(this.repositoryOptions.InterceptionOptions.IsEnabled)
			{
				IEnumerable<TAggregateRoot> itemsList = items.ToList();

				InterceptionEvent e = new InterceptionEvent();
				foreach(TAggregateRoot item in itemsList)
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

					//foreach(TAggregateRoot item in itemsList)
					//{
					//	await this.interceptor.AfterUpdateAsync(item).ConfigureAwait(false);
					//}
				}
				else
				{
					RecordCancellation(e.CancellationMessage);

					this.logger.LogInformation(e.CancellationMessage);
				}
			}
			else
			{
				await this.innerRepository.UpdateRangeAsync(items, cancellationToken).ConfigureAwait(false);
			}
		}

		/// <inheritdoc />
		async Task ICanRemove<TAggregateRoot, TKey>.RemoveAsync(TKey id, CancellationToken cancellationToken)
		{
			if(this.repositoryOptions.InterceptionOptions.IsEnabled)
			{
				InterceptionEvent e = new InterceptionEvent();
				Expression<Func<TAggregateRoot, bool>> predicate = CreatePrimaryKeyPredicate(id);
				predicate = await this.interceptor.BeforeRemoveRangeAsync(predicate, e).ConfigureAwait(false);
				if(e.CancelOperation && e.ThrowOnCancellation)
				{
					throw new InvalidOperationException(e.CancellationMessage);
				}

				if(!e.CancelOperation)
				{
					RecordStatement(predicate);

					await this.innerRepository.RemoveRangeAsync(predicate, cancellationToken).ConfigureAwait(false);
					//await this.interceptor.AfterRemoveRangeAsync().ConfigureAwait(false);
				}
				else
				{
					RecordCancellation(e.CancellationMessage);

					this.logger.LogInformation(e.CancellationMessage);
				}
			}
			else
			{
				RecordStatement(CreatePrimaryKeyPredicate(id));

				await this.innerRepository.RemoveAsync(id, cancellationToken).ConfigureAwait(false);
			}
		}

		/// <inheritdoc />
		async Task ICanRemove<TAggregateRoot, TKey>.RemoveAsync(TAggregateRoot item, CancellationToken cancellationToken)
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
					//await this.interceptor.AfterRemoveAsync(item).ConfigureAwait(false);
				}
				else
				{
					RecordCancellation(e.CancellationMessage);

					this.logger.LogInformation(e.CancellationMessage);
				}
			}
			else
			{
				await this.innerRepository.RemoveAsync(item, cancellationToken).ConfigureAwait(false);
			}
		}

		/// <inheritdoc />
		async Task ICanRemove<TAggregateRoot, TKey>.RemoveRangeAsync(Expression<Func<TAggregateRoot, bool>> predicate, CancellationToken cancellationToken)
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
					//await this.interceptor.AfterRemoveRangeAsync().ConfigureAwait(false);
				}
				else
				{
					RecordCancellation(e.CancellationMessage);

					this.logger.LogInformation(e.CancellationMessage);
				}
			}
			else
			{
				RecordStatement(predicate);

				await this.innerRepository.RemoveRangeAsync(predicate, cancellationToken).ConfigureAwait(false);
			}
		}

		/// <inheritdoc />
		async Task ICanRemove<TAggregateRoot, TKey>.RemoveRangeAsync(ISpecification<TAggregateRoot> specification, CancellationToken cancellationToken)
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
					//await this.interceptor.AfterRemoveRangeAsync().ConfigureAwait(false);
				}
				else
				{
					RecordCancellation(e.CancellationMessage);

					this.logger.LogInformation(e.CancellationMessage);
				}
			}
			else
			{
				RecordStatement(specification);

				await this.innerRepository.RemoveRangeAsync(specification, cancellationToken).ConfigureAwait(false);
			}
		}

		/// <inheritdoc />
		async Task ICanRemove<TAggregateRoot, TKey>.RemoveRangeAsync(IEnumerable<TAggregateRoot> items, CancellationToken cancellationToken)
		{
			if(this.repositoryOptions.InterceptionOptions.IsEnabled)
			{
				IEnumerable<TAggregateRoot> itemsList = items.ToList();

				InterceptionEvent e = new InterceptionEvent();
				foreach(TAggregateRoot item in itemsList)
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

					//foreach(TAggregateRoot item in itemsList)
					//{
					//	await this.interceptor.AfterRemoveAsync(item).ConfigureAwait(false);
					//}
				}
				else
				{
					RecordCancellation(e.CancellationMessage);

					this.logger.LogInformation(e.CancellationMessage);
				}
			}
			else
			{
				await this.innerRepository.RemoveRangeAsync(items, cancellationToken).ConfigureAwait(false);
			}
		}

		/// <inheritdoc />
		async Task<TAggregateRoot> ICanFind<TAggregateRoot, TKey>.FindOneAsync(Expression<Func<TAggregateRoot, bool>> predicate, IQueryOptions<TAggregateRoot> queryOptions, CancellationToken cancellationToken)
		{
			if(this.repositoryOptions.InterceptionOptions.IsEnabled)
			{
				predicate = await this.interceptor.BeforeFindAsync(predicate, queryOptions!).ConfigureAwait(false);
			}

			RecordStatement(predicate);

			TAggregateRoot result = await this.innerRepository.FindOneAsync(predicate, queryOptions, cancellationToken).ConfigureAwait(false);

			//if(this.repositoryOptions.InterceptionOptions.IsEnabled)
			//{
			//	await this.interceptor.AfterFindAsync(result).ConfigureAwait(false);
			//}

			return result;
		}

		/// <inheritdoc />
		async Task<TAggregateRoot> ICanFind<TAggregateRoot, TKey>.FindOneAsync(ISpecification<TAggregateRoot> specification, IQueryOptions<TAggregateRoot> queryOptions, CancellationToken cancellationToken)
		{
			if(this.repositoryOptions.InterceptionOptions.IsEnabled)
			{
				specification = await this.interceptor.BeforeFindAsync(specification, queryOptions!).ConfigureAwait(false);
			}

			RecordStatement(specification);

			TAggregateRoot result = await this.innerRepository.FindOneAsync(specification, queryOptions, cancellationToken).ConfigureAwait(false);

			//if(this.repositoryOptions.InterceptionOptions.IsEnabled)
			//{
			//	await this.interceptor.AfterFindAsync(result).ConfigureAwait(false);
			//}

			return result;
		}

		/// <inheritdoc />
		async Task<TResult> ICanFind<TAggregateRoot, TKey>.FindOneAsync<TResult>(Expression<Func<TAggregateRoot, bool>> predicate, Expression<Func<TAggregateRoot, TResult>> selector, IQueryOptions<TAggregateRoot> queryOptions, CancellationToken cancellationToken)
		{
			if(this.repositoryOptions.InterceptionOptions.IsEnabled)
			{
				predicate = await this.interceptor.BeforeFindAsync(predicate, queryOptions!).ConfigureAwait(false);
			}

			RecordStatement(predicate, selector);

			TResult result = await this.innerRepository.FindOneAsync(predicate, selector, queryOptions, cancellationToken).ConfigureAwait(false);

			//if(this.repositoryOptions.InterceptionOptions.IsEnabled)
			//{
			//	await this.interceptor.AfterFindAsync(result).ConfigureAwait(false);
			//}

			return result;
		}

		/// <inheritdoc />
		async Task<TResult> ICanFind<TAggregateRoot, TKey>.FindOneAsync<TResult>(ISpecification<TAggregateRoot> specification, Expression<Func<TAggregateRoot, TResult>> selector, IQueryOptions<TAggregateRoot> queryOptions, CancellationToken cancellationToken)
		{
			if(this.repositoryOptions.InterceptionOptions.IsEnabled)
			{
				specification = await this.interceptor.BeforeFindAsync(specification, queryOptions!).ConfigureAwait(false);
			}

			RecordStatement(specification, selector);

			TResult result = await this.innerRepository.FindOneAsync(specification, selector, queryOptions, cancellationToken).ConfigureAwait(false);

			//if(this.repositoryOptions.InterceptionOptions.IsEnabled)
			//{
			//	await this.interceptor.AfterFindAsync(result).ConfigureAwait(false);
			//}

			return result;
		}

		/// <inheritdoc />
		async Task<bool> ICanGet<TAggregateRoot, TKey>.ExistsAsync(TKey id, CancellationToken cancellationToken)
		{
			bool result;

			if(this.repositoryOptions.InterceptionOptions.IsEnabled)
			{
				Expression<Func<TAggregateRoot, bool>> predicate = CreatePrimaryKeyPredicate(id);
				predicate = await this.interceptor.BeforeFindAsync(predicate, QueryOptions<TAggregateRoot>.Empty());

				RecordStatement(predicate);

				result = await this.innerRepository.ExistsAsync(predicate, cancellationToken).ConfigureAwait(false);

				//await this.interceptor.AfterFindAsync(result);
			}
			else
			{
				RecordStatement(CreatePrimaryKeyPredicate(id));

				result = await this.innerRepository.ExistsAsync(id, cancellationToken).ConfigureAwait(false);
			}

			return result;
		}

		/// <inheritdoc />
		async Task<bool> ICanFind<TAggregateRoot, TKey>.ExistsAsync(Expression<Func<TAggregateRoot, bool>> predicate, CancellationToken cancellationToken)
		{
			if(this.repositoryOptions.InterceptionOptions.IsEnabled)
			{
				predicate = await this.interceptor.BeforeFindAsync(predicate, QueryOptions<TAggregateRoot>.Empty());
			}

			RecordStatement(predicate);

			bool result = await this.innerRepository.ExistsAsync(predicate, cancellationToken).ConfigureAwait(false);

			//if(this.repositoryOptions.InterceptionOptions.IsEnabled)
			//{
			//	await this.interceptor.AfterFindAsync(result);
			//}

			return result;
		}

		/// <inheritdoc />
		async Task<bool> ICanFind<TAggregateRoot, TKey>.ExistsAsync(ISpecification<TAggregateRoot> specification, CancellationToken cancellationToken)
		{
			if(this.repositoryOptions.InterceptionOptions.IsEnabled)
			{
				specification = await this.interceptor.BeforeFindAsync(specification, QueryOptions<TAggregateRoot>.Empty());
			}

			RecordStatement(specification);

			bool result = await this.innerRepository.ExistsAsync(specification, cancellationToken).ConfigureAwait(false);

			//if(this.repositoryOptions.InterceptionOptions.IsEnabled)
			//{
			//	await this.interceptor.AfterFindAsync(result);
			//}

			return result;
		}

		/// <inheritdoc />
		async Task<IReadOnlyCollection<TAggregateRoot>> ICanFind<TAggregateRoot, TKey>.FindManyAsync(Expression<Func<TAggregateRoot, bool>> predicate, IQueryOptions<TAggregateRoot> queryOptions, CancellationToken cancellationToken)
		{
			if(this.repositoryOptions.InterceptionOptions.IsEnabled)
			{
				predicate = await this.interceptor.BeforeFindAsync(predicate, queryOptions!);
			}

			RecordStatement(predicate);

			IReadOnlyCollection<TAggregateRoot> result = await this.innerRepository.FindManyAsync(predicate, queryOptions, cancellationToken).ConfigureAwait(false);

			//if(this.repositoryOptions.InterceptionOptions.IsEnabled)
			//{
			//	await this.interceptor.AfterFindAsync(result);
			//}

			return result;
		}

		/// <inheritdoc />
		async Task<IReadOnlyCollection<TAggregateRoot>> ICanFind<TAggregateRoot, TKey>.FindManyAsync(ISpecification<TAggregateRoot> specification, IQueryOptions<TAggregateRoot> queryOptions, CancellationToken cancellationToken)
		{
			if(this.repositoryOptions.InterceptionOptions.IsEnabled)
			{
				specification = await this.interceptor.BeforeFindAsync(specification, queryOptions!);
			}

			RecordStatement(specification);

			IReadOnlyCollection<TAggregateRoot> result = await this.innerRepository.FindManyAsync(specification, queryOptions, cancellationToken).ConfigureAwait(false);

			//if(this.repositoryOptions.InterceptionOptions.IsEnabled)
			//{
			//	await this.interceptor.AfterFindAsync(result);
			//}

			return result;
		}

		/// <inheritdoc />
		async Task<IReadOnlyCollection<TResult>> ICanFind<TAggregateRoot, TKey>.FindManyAsync<TResult>(Expression<Func<TAggregateRoot, bool>> predicate, Expression<Func<TAggregateRoot, TResult>> selector, IQueryOptions<TAggregateRoot> queryOptions, CancellationToken cancellationToken)
		{
			if(this.repositoryOptions.InterceptionOptions.IsEnabled)
			{
				predicate = await this.interceptor.BeforeFindAsync(predicate, queryOptions!);
			}

			RecordStatement(predicate, selector);

			IReadOnlyCollection<TResult> result = await this.innerRepository.FindManyAsync(predicate, selector, queryOptions, cancellationToken).ConfigureAwait(false);

			//if(this.repositoryOptions.InterceptionOptions.IsEnabled)
			//{
			//	await this.interceptor.AfterFindAsync(result);
			//}

			return result;
		}

		/// <inheritdoc />
		async Task<IReadOnlyCollection<TResult>> ICanFind<TAggregateRoot, TKey>.FindManyAsync<TResult>(ISpecification<TAggregateRoot> specification, Expression<Func<TAggregateRoot, TResult>> selector, IQueryOptions<TAggregateRoot> queryOptions, CancellationToken cancellationToken)
		{
			if(this.repositoryOptions.InterceptionOptions.IsEnabled)
			{
				specification = await this.interceptor.BeforeFindAsync(specification, queryOptions!);
			}

			RecordStatement(specification, selector);

			IReadOnlyCollection<TResult> result = await this.innerRepository.FindManyAsync(specification, selector, queryOptions, cancellationToken).ConfigureAwait(false);

			//if(this.repositoryOptions.InterceptionOptions.IsEnabled)
			//{
			//	await this.interceptor.AfterFindAsync(result);
			//}

			return result;
		}

		/// <inheritdoc />
		async Task<long> ICanAggregate<TAggregateRoot, TKey>.CountAsync(CancellationToken cancellationToken)
		{
			long result;

			if(this.repositoryOptions.InterceptionOptions.IsEnabled)
			{
				Expression<Func<TAggregateRoot, bool>> predicate = x => true;
				predicate = await this.interceptor.BeforeFindAsync(predicate, QueryOptions<TAggregateRoot>.Empty());

				RecordStatement(predicate);

				result = await this.innerRepository.CountAsync(predicate, cancellationToken).ConfigureAwait(false);

				//await this.interceptor.AfterFindAsync(result);
			}
			else
			{
				RecordStatement(x => true);

				result = await this.innerRepository.CountAsync(cancellationToken).ConfigureAwait(false);
			}

			return result;
		}

		/// <inheritdoc />
		async Task<long> ICanAggregate<TAggregateRoot, TKey>.CountAsync(Expression<Func<TAggregateRoot, bool>> predicate, CancellationToken cancellationToken)
		{
			if(this.repositoryOptions.InterceptionOptions.IsEnabled)
			{
				predicate = await this.interceptor.BeforeFindAsync(predicate, QueryOptions<TAggregateRoot>.Empty());
			}

			RecordStatement(predicate);

			long result = await this.innerRepository.CountAsync(predicate, cancellationToken).ConfigureAwait(false);

			//if(this.repositoryOptions.InterceptionOptions.IsEnabled)
			//{
			//	await this.interceptor.AfterFindAsync(result);
			//}

			return result;
		}

		/// <inheritdoc />
		async Task<long> ICanAggregate<TAggregateRoot, TKey>.CountAsync(ISpecification<TAggregateRoot> specification, CancellationToken cancellationToken)
		{
			if(this.repositoryOptions.InterceptionOptions.IsEnabled)
			{
				specification = await this.interceptor.BeforeFindAsync(specification, QueryOptions<TAggregateRoot>.Empty());
			}

			RecordStatement(specification);

			long result = await this.innerRepository.CountAsync(specification, cancellationToken).ConfigureAwait(false);

			//if(this.repositoryOptions.InterceptionOptions.IsEnabled)
			//{
			//	await this.interceptor.AfterFindAsync(result);
			//}

			return result;
		}

		/// <inheritdoc />
		async Task<TAggregateRoot> ICanGet<TAggregateRoot, TKey>.GetAsync(TKey id, CancellationToken cancellationToken)
		{
			TAggregateRoot result;

			if(this.repositoryOptions.InterceptionOptions.IsEnabled)
			{
				Expression<Func<TAggregateRoot, bool>> predicate = CreatePrimaryKeyPredicate(id);
				predicate = await this.interceptor.BeforeFindAsync(predicate, QueryOptions<TAggregateRoot>.Empty());

				RecordStatement(predicate);

				result = await this.innerRepository.FindOneAsync(predicate, QueryOptions<TAggregateRoot>.Empty(), cancellationToken).ConfigureAwait(false);

				//await this.interceptor.AfterFindAsync(result);
			}
			else
			{
				RecordStatement(CreatePrimaryKeyPredicate(id));

				result = await this.innerRepository.GetAsync(id, cancellationToken).ConfigureAwait(false);
			}

			return result;
		}

		/// <inheritdoc />
		async Task<TResult> ICanGet<TAggregateRoot, TKey>.GetAsync<TResult>(TKey id, Expression<Func<TAggregateRoot, TResult>> selector, CancellationToken cancellationToken)
		{
			TResult result;

			if(this.repositoryOptions.InterceptionOptions.IsEnabled)
			{
				Expression<Func<TAggregateRoot, bool>> predicate = CreatePrimaryKeyPredicate(id);
				predicate = await this.interceptor.BeforeFindAsync(predicate, QueryOptions<TAggregateRoot>.Empty());

				RecordStatement(predicate, selector);

				result = await this.innerRepository.FindOneAsync(predicate, selector, QueryOptions<TAggregateRoot>.Empty(), cancellationToken).ConfigureAwait(false);

				//await this.interceptor.AfterFindAsync(result);
			}
			else
			{
				RecordStatement(id?.ToString(), selector);

				result = await this.innerRepository.GetAsync(id, selector, cancellationToken).ConfigureAwait(false);
			}

			return result;
		}

		private static void RecordCancellation(string cancellationMessage)
		{
			Activity activity = Activity.Current;

			if(activity?.IsAllDataRequested == true && !string.IsNullOrWhiteSpace(cancellationMessage))
			{
				activity.AddTag("db.repository.cancellation", cancellationMessage);
			}
		}

		private static void RecordStatement(ISpecification<TAggregateRoot> specification)
		{
			if(Activity.Current != null)
			{
				RecordStatement(specification.Predicate);
			}
		}

		private static void RecordStatement<TResult>(ISpecification<TAggregateRoot> specification, Expression<Func<TAggregateRoot, TResult>> selector)
		{
			if(Activity.Current != null)
			{
				RecordStatement(specification.Predicate, selector);
			}
		}

		private static void RecordStatement(Expression<Func<TAggregateRoot, bool>> predicate)
		{
			if(Activity.Current != null)
			{
				RecordStatement(predicate.ToExpressionString());
			}
		}

		private static void RecordStatement<TResult>(Expression<Func<TAggregateRoot, bool>> predicate, Expression<Func<TAggregateRoot, TResult>> selector)
		{
			if(Activity.Current != null)
			{
				RecordStatement(predicate.ToExpressionString(), selector.ToExpressionString());
			}
		}

		private static void RecordStatement<TResult>(string predicate, Expression<Func<TAggregateRoot, TResult>> selector)
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

		/// <inheritdoc />
		public override string ToString()
		{
			return this.innerRepository.ToString();
		}

		private static Expression<Func<TAggregateRoot, bool>> CreatePrimaryKeyPredicate(TKey id)
		{
			PropertyInfo primaryKeyProperty = GetPrimaryKeyProperty();

			ParameterExpression parameter = Expression.Parameter(typeof(TAggregateRoot), "x");
			Expression<Func<TAggregateRoot, bool>> predicate = Expression.Lambda<Func<TAggregateRoot, bool>>(
				Expression.Equal(
					Expression.PropertyOrField(parameter, primaryKeyProperty.Name),
					Expression.Constant(id)
				),
				parameter);

			return predicate;
		}

		//private ISpecification<TAggregateRoot> CreatePrimaryKeySpecification(TKey id)
		//{
		//	Expression<Func<TAggregateRoot, bool>> predicate = this.CreatePrimaryKeyPredicate(id);
		//	ISpecification<TAggregateRoot> specification = new Specification<TAggregateRoot>(predicate);
		//	return specification;
		//}

		private static PropertyInfo GetPrimaryKeyProperty()
		{
			Type type = typeof(TAggregateRoot);
			Type keyType = typeof(TKey);

			Tuple<Type, Type> key = Tuple.Create(type, keyType);

			// Check the cache for already existing property info instance.
			if(PropertyInfoCache.PrimaryKeyDict.ContainsKey(key))
			{
				return PropertyInfoCache.PrimaryKeyDict[key];
			}

			string keyPropertyName = nameof(AggregateRoot<TAggregateRoot, TKey>.ID);
			PropertyInfo propertyInfo = type.GetTypeInfo().GetDeclaredProperty(keyPropertyName);
			while(propertyInfo == null && type.GetTypeInfo().BaseType != null)
			{
				type = type.GetTypeInfo().BaseType;
				propertyInfo = type.GetTypeInfo().GetDeclaredProperty(keyPropertyName);
			}

			if(propertyInfo == null)
			{
				throw new InvalidOperationException($"No property '{keyPropertyName}' found for type '{typeof(TAggregateRoot)}'.");
			}

			if(propertyInfo.PropertyType != keyType)
			{
				throw new InvalidOperationException($"No property '{keyPropertyName}' found for type '{typeof(TAggregateRoot)}' that has the type {typeof(TKey)}.");
			}

			PropertyInfoCache.PrimaryKeyDict[key] = propertyInfo;
			return propertyInfo;
		}
	}
}
