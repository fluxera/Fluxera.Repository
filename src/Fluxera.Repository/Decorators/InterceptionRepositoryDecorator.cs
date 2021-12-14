//namespace Fluxera.Repository.Decorators
//{
//	using System;
//	using System.Collections.Generic;
//	using System.Linq.Expressions;
//	using System.Threading;
//	using System.Threading.Tasks;
//	using Fluxera.Entity;
//	using Fluxera.Guards;
//	using Fluxera.Repository.Interception;
//	using Fluxera.Repository.Query;
//	using Fluxera.Repository.Traits;
//	using Fluxera.Utilities.Extensions;
//	using Microsoft.Extensions.DependencyInjection;
//	using Microsoft.Extensions.Logging;

//	public sealed class InterceptionRepositoryDecorator<TAggregateRoot> : IDisposableRepository<TAggregateRoot>
//		where TAggregateRoot : AggregateRoot<TAggregateRoot>
//	{
//		private readonly IRepository<TAggregateRoot> innerRepository;
//		private readonly IRepositoryInterceptor<TAggregateRoot> interceptor;

//		public InterceptionRepositoryDecorator(
//			IRepository<TAggregateRoot> innerRepository, 
//			IRepositoryRegistry repositoryRegistry, 
//			IServiceProvider serviceProvider,
//			ILoggerFactory loggerFactory)
//		{
//			Guard.Against.Null(innerRepository, nameof(innerRepository));
//			Guard.Against.Null(repositoryRegistry, nameof(repositoryRegistry));
//			Guard.Against.Null(serviceProvider, nameof(serviceProvider));

//			string repositoryName = repositoryRegistry.GetRepositoryNameFor<TAggregateRoot>();
//			if(string.IsNullOrWhiteSpace(repositoryName))
//			{
//				throw Errors.NoRepositoryNameAvailable(typeof(TAggregateRoot));
//			}

//			// Initialize the interceptor(s).
//			IEnumerable<IRepositoryInterceptor<TAggregateRoot>> innerInterceptors = serviceProvider.GetServices<IRepositoryInterceptor<TAggregateRoot>>();
//			this.interceptor = new DecoratingInterceptor<TAggregateRoot>(innerInterceptors.AsReadOnly(), loggerFactory);

//			this.innerRepository = innerRepository;
//		}
		
//		/// <inheritdoc />
//		async Task ICanAdd<TAggregateRoot>.AddAsync(TAggregateRoot item, CancellationToken cancellationToken)
//		{
//			InterceptionEvent e = new InterceptionEvent();
//			await this.interceptor.BeforeAddAsync(item, e).ConfigureAwait(false);
//			if (e.CancelOperation && e.ThrowOnCancellation)
//			{
//				throw new InvalidOperationException(e.CancellationMessage);
//			}

//			if (!e.CancelOperation)
//			{
//				await this.innerRepository.AddAsync(item, cancellationToken);

//				await this.interceptor.AfterAddAsync(item).ConfigureAwait(false);
//			}
//		}

//		/// <inheritdoc />
//		async Task ICanAdd<TAggregateRoot>.AddAsync(IEnumerable<TAggregateRoot> items, CancellationToken cancellationToken)
//		{
//			InterceptionEvent e = new InterceptionEvent();
//			foreach(TAggregateRoot item in items)
//			{
//				await this.interceptor.BeforeAddAsync(item, e).ConfigureAwait(false);	
//			}
//			if (e.CancelOperation && e.ThrowOnCancellation)
//			{
//				throw new InvalidOperationException(e.CancellationMessage);
//			}

//			if (!e.CancelOperation)
//			{
//				await this.innerRepository.AddAsync(items, cancellationToken);

//				foreach(TAggregateRoot item in items)
//				{
//					await this.interceptor.AfterAddAsync(item).ConfigureAwait(false);	
//				}
//			}
//		}

//		/// <inheritdoc />
//		async Task ICanUpdate<TAggregateRoot>.UpdateAsync(TAggregateRoot item, CancellationToken cancellationToken)
//		{
//			InterceptionEvent e = new InterceptionEvent();
//			await this.interceptor.BeforeUpdateAsync(item, e).ConfigureAwait(false);
//			if (e.CancelOperation && e.ThrowOnCancellation)
//			{
//				throw new InvalidOperationException(e.CancellationMessage);
//			}

//			if(!e.CancelOperation)
//			{
//				await this.innerRepository.UpdateAsync(item, cancellationToken);

//				await this.interceptor.AfterUpdateAsync(item).ConfigureAwait(false);
//			}
//		}

//		/// <inheritdoc />
//		async Task ICanUpdate<TAggregateRoot>.UpdateAsync(IEnumerable<TAggregateRoot> items, CancellationToken cancellationToken)
//		{
//			InterceptionEvent e = new InterceptionEvent();
//			foreach(TAggregateRoot item in items)
//			{
//				await this.interceptor.BeforeUpdateAsync(item, e).ConfigureAwait(false);	
//			}
//			if (e.CancelOperation && e.ThrowOnCancellation)
//			{
//				throw new InvalidOperationException(e.CancellationMessage);
//			}

//			if(!e.CancelOperation)
//			{
//				await this.innerRepository.UpdateAsync(items, cancellationToken);

//				foreach(TAggregateRoot item in items)
//				{
//					await this.interceptor.AfterUpdateAsync(items).ConfigureAwait(false);	
//				}
//			}
//		}

//		/// <inheritdoc />
//		async Task ICanRemove<TAggregateRoot>.RemoveAsync(TAggregateRoot item, CancellationToken cancellationToken)
//		{
//			InterceptionEvent e = new InterceptionEvent();
//			await this.interceptor.BeforeDeleteAsync(item, e).ConfigureAwait(false);
//			if (e.CancelOperation && e.ThrowOnCancellation)
//			{
//				throw new InvalidOperationException(e.CancellationMessage);
//			}

//			if (!e.CancelOperation)
//			{
//				await this.innerRepository.RemoveAsync(item, cancellationToken);

//				await this.interceptor.AfterDeleteAsync(item).ConfigureAwait(false);
//			}
//		}

//		/// <inheritdoc />
//		async Task ICanRemove<TAggregateRoot>.RemoveAsync(string id, CancellationToken cancellationToken)
//		{
//			InterceptionEvent e = new InterceptionEvent();
//			await this.interceptor.BeforeDeleteAsync(id, e).ConfigureAwait(false);
//			if (e.CancelOperation && e.ThrowOnCancellation)
//			{
//				throw new InvalidOperationException(e.CancellationMessage);
//			}

//			if (!e.CancelOperation)
//			{
//				TAggregateRoot item = await this.innerRepository.FindOneAsync(x => x.ID == id, cancellationToken: cancellationToken);

//				await this.innerRepository.RemoveAsync(id, cancellationToken);

//				await this.interceptor.AfterDeleteAsync(item).ConfigureAwait(false);
//			}
//		}

//		/// <inheritdoc />
//		async Task ICanRemove<TAggregateRoot>.RemoveAsync(Expression<Func<TAggregateRoot, bool>> predicate, CancellationToken cancellationToken)
//		{
//			InterceptionEvent e = new InterceptionEvent();
//			await this.interceptor.BeforeDeleteAsync(predicate, e).ConfigureAwait(false);
//			if (e.CancelOperation && e.ThrowOnCancellation)
//			{
//				throw new InvalidOperationException(e.CancellationMessage);
//			}

//			if (!e.CancelOperation)
//			{
//				IReadOnlyCollection<TAggregateRoot> items = await this.innerRepository.FindManyAsync(predicate);	

//				await this.innerRepository.RemoveAsync(predicate, cancellationToken);

//				foreach(TAggregateRoot item in items)
//				{
//					await this.interceptor.AfterDeleteAsync(item).ConfigureAwait(false);	
//				}
//			}
//		}

//		/// <inheritdoc />
//		async Task ICanRemove<TAggregateRoot>.RemoveAsync(IEnumerable<TAggregateRoot> items, CancellationToken cancellationToken)
//		{
//			InterceptionEvent e = new InterceptionEvent();
//			await this.interceptor.BeforeDeleteAsync(items, e).ConfigureAwait(false);
//			if (e.CancelOperation && e.ThrowOnCancellation)
//			{
//				throw new InvalidOperationException(e.CancellationMessage);
//			}

//			if (!e.CancelOperation)
//			{
//				await this.innerRepository.RemoveAsync(items, cancellationToken);

//				await this.interceptor.AfterDeleteAsync(items).ConfigureAwait(false);
//			}
//		}

//		/// <inheritdoc />
//		async Task<TAggregateRoot> ICanGet<TAggregateRoot>.GetAsync(string id, CancellationToken cancellationToken)
//		{
//			await this.interceptor.BeforeGetAsync(id).ConfigureAwait(false);

//			TAggregateRoot result = await this.innerRepository.GetAsync(id, cancellationToken);

//			await this.interceptor.AfterGetAsync(result).ConfigureAwait(false);

//			return result;
//		}

//		/// <inheritdoc />
//		async Task<TResult> ICanGet<TAggregateRoot>.GetAsync<TResult>(string id, Expression<Func<TAggregateRoot, TResult>> selector, CancellationToken cancellationToken)
//		{
//			await this.interceptor.BeforeGetAsync(id, selector).ConfigureAwait(false);

//			TResult result = await this.innerRepository.GetAsync(id, selector, cancellationToken);

//			await this.interceptor.AfterGetAsync(result).ConfigureAwait(false);

//			return result;
//		}

//		/// <inheritdoc />
//		async Task<bool> ICanGet<TAggregateRoot>.ExistsAsync(string id, CancellationToken cancellationToken)
//		{
//			await this.interceptor.BeforeExistsAsync(id).ConfigureAwait(false);

//			bool result = await this.innerRepository.ExistsAsync(id, cancellationToken);

//			await this.interceptor.AfterExistsAsync(result).ConfigureAwait(false);

//			return result;
//		}

//		/// <inheritdoc />
//		async Task<TAggregateRoot> ICanFind<TAggregateRoot>.FindOneAsync(Expression<Func<TAggregateRoot, bool>> predicate, IQueryOptions<TAggregateRoot>? queryOptions, CancellationToken cancellationToken)
//		{
//			queryOptions ??= QueryOptionsEx<TAggregateRoot>.None;
//			predicate = await this.interceptor.BeforeFindAsync(predicate, queryOptions).ConfigureAwait(false);

//			TAggregateRoot result = await this.innerRepository.FindOneAsync(predicate, queryOptions, cancellationToken);

//			await this.interceptor.AfterFindAsync(result).ConfigureAwait(false);

//			return result;
//		}

//		/// <inheritdoc />
//		async Task<TResult> ICanFind<TAggregateRoot>.FindOneAsync<TResult>(Expression<Func<TAggregateRoot, bool>> predicate, Expression<Func<TAggregateRoot, TResult>> selector, IQueryOptions<TAggregateRoot>? queryOptions, CancellationToken cancellationToken)
//		{
//			queryOptions ??= QueryOptionsEx<TAggregateRoot>.None;
//			predicate = await this.interceptor.BeforeFindAsync(predicate, queryOptions).ConfigureAwait(false);

//			TResult result = await this.innerRepository.FindOneAsync(predicate, selector, queryOptions, cancellationToken);
			
//			await this.interceptor.AfterFindAsync(result).ConfigureAwait(false);

//			return result;
//		}

//		/// <inheritdoc />
//		async Task<bool> ICanFind<TAggregateRoot>.ExistsAsync(Expression<Func<TAggregateRoot, bool>> predicate, CancellationToken cancellationToken)
//		{
//			predicate = await this.interceptor.BeforeExistsAsync(predicate).ConfigureAwait(false);

//			bool result = await this.innerRepository.ExistsAsync(predicate, cancellationToken);

//			await this.interceptor.AfterExistsAsync(result).ConfigureAwait(false);

//			return result;
//		}

//		/// <inheritdoc />
//		async Task<IReadOnlyCollection<TAggregateRoot>> ICanFind<TAggregateRoot>.FindManyAsync(Expression<Func<TAggregateRoot, bool>> predicate, IQueryOptions<TAggregateRoot>? queryOptions, CancellationToken cancellationToken)
//		{
//			return await this.innerRepository.FindManyAsync(predicate, queryOptions, cancellationToken);
//		}

//		/// <inheritdoc />
//		async Task<IReadOnlyCollection<TResult>> ICanFind<TAggregateRoot>.FindManyAsync<TResult>(Expression<Func<TAggregateRoot, bool>> predicate, Expression<Func<TAggregateRoot, TResult>> selector, IQueryOptions<TAggregateRoot>? queryOptions, CancellationToken cancellationToken)
//		{
//			return await this.innerRepository.FindManyAsync(predicate, selector, queryOptions, cancellationToken);
//		}

//		/// <inheritdoc />
//		async Task<long> ICanAggregate<TAggregateRoot>.CountAsync(Expression<Func<TAggregateRoot, bool>> predicate, CancellationToken cancellationToken)
//		{
//			await this.interceptor.BeforeCountAsync(predicate).ConfigureAwait(false);

//			long count = await this.innerRepository.CountAsync(predicate, cancellationToken);

//			await this.interceptor.AfterCountAsync(count).ConfigureAwait(false);

//			return count;
//		}

///// <inheritdoc />
//public override string ToString()
//{
//	return this.innerRepository.ToString();
//}

///// <inheritdoc />
//void IDisposable.Dispose()
//{
//	if(!this.innerRepository.IsDisposed)
//	{
//		Console.Write("Validation -> ");
//		this.innerRepository.Dispose();
//	}
//}

///// <inheritdoc />
//bool IDisposableRepository.IsDisposed => this.innerRepository.IsDisposed;
//	}
//}
