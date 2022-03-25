namespace Fluxera.Repository.Interception
{
	using System;
	using System.Collections.Generic;
	using System.Linq.Expressions;
	using System.Threading.Tasks;
	using Fluxera.Entity;
	using Fluxera.Repository.Query;
	using Fluxera.Repository.Specifications;
	using JetBrains.Annotations;

	/// <summary>
	///     A base class for a service that is used to intercept calls tot he repository
	///     before they hit the underlying storage.
	/// </summary>
	/// <typeparam name="TAggregateRoot">The type of the aggregate root.</typeparam>
	/// <typeparam name="TKey">The type of the ID.</typeparam>
	[PublicAPI]
	public abstract class InterceptorBase<TAggregateRoot, TKey> : IInterceptor<TAggregateRoot, TKey>
		where TAggregateRoot : AggregateRoot<TAggregateRoot, TKey>
	{
		/// <inheritdoc />
		public virtual int Order => 0;

		/// <inheritdoc />
		public virtual Task BeforeAddAsync(TAggregateRoot item, InterceptionEvent e)
		{
			return Task.CompletedTask;
		}

		/// <inheritdoc />
		public virtual Task AfterAddAsync(TAggregateRoot item)
		{
			return Task.CompletedTask;
		}

		/// <inheritdoc />
		public virtual Task BeforeUpdateAsync(TAggregateRoot item, InterceptionEvent e)
		{
			return Task.CompletedTask;
		}

		/// <inheritdoc />
		public virtual Task AfterUpdateAsync(TAggregateRoot item)
		{
			return Task.CompletedTask;
		}

		/// <inheritdoc />
		public virtual Task BeforeRemoveAsync(TAggregateRoot item, InterceptionEvent e)
		{
			return Task.CompletedTask;
		}

		/// <inheritdoc />
		public virtual Task AfterRemoveAsync(TAggregateRoot item)
		{
			return Task.CompletedTask;
		}

		/// <inheritdoc />
		public virtual Task<Expression<Func<TAggregateRoot, bool>>> BeforeRemoveRangeAsync(Expression<Func<TAggregateRoot, bool>> predicate, InterceptionEvent e)
		{
			return Task.FromResult(predicate);
		}

		/// <inheritdoc />
		public virtual Task<ISpecification<TAggregateRoot>> BeforeRemoveRangeAsync(ISpecification<TAggregateRoot> specification, InterceptionEvent e)
		{
			return Task.FromResult(specification);
		}

		/// <inheritdoc />
		public virtual Task<Expression<Func<TAggregateRoot, bool>>> BeforeFindAsync(Expression<Func<TAggregateRoot, bool>> predicate, IQueryOptions<TAggregateRoot> queryOptions)
		{
			return Task.FromResult(predicate);
		}

		/// <inheritdoc />
		public virtual Task<ISpecification<TAggregateRoot>> BeforeFindAsync(ISpecification<TAggregateRoot> specification, IQueryOptions<TAggregateRoot> queryOptions)
		{
			return Task.FromResult(specification);
		}

		/// <inheritdoc />
		public virtual Task AfterFindAsync(TAggregateRoot item)
		{
			return Task.CompletedTask;
		}

		/// <inheritdoc />
		public virtual Task AfterFindAsync(IReadOnlyCollection<TAggregateRoot> items)
		{
			return Task.CompletedTask;
		}

		/// <inheritdoc />
		public virtual Task AfterFindAsync<TResult>(TResult item)
		{
			return Task.CompletedTask;
		}

		/// <inheritdoc />
		public virtual Task AfterFindAsync<TResult>(IReadOnlyCollection<TResult> items)
		{
			return Task.CompletedTask;
		}
	}
}
