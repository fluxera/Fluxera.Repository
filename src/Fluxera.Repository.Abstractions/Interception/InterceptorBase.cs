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
		public abstract Task BeforeAddAsync(TAggregateRoot item, InterceptionEvent e);

		/// <inheritdoc />
		public abstract Task AfterAddAsync(TAggregateRoot item);

		/// <inheritdoc />
		public abstract Task BeforeUpdateAsync(TAggregateRoot item, InterceptionEvent e);

		/// <inheritdoc />
		public abstract Task AfterUpdateAsync(TAggregateRoot item);

		/// <inheritdoc />
		public abstract Task BeforeRemoveAsync(TAggregateRoot item, InterceptionEvent e);

		/// <inheritdoc />
		public abstract Task AfterRemoveAsync(TAggregateRoot item);

		/// <inheritdoc />
		public abstract Task<Expression<Func<TAggregateRoot, bool>>> BeforeRemoveRangeAsync(Expression<Func<TAggregateRoot, bool>> predicate, InterceptionEvent e);

		/// <inheritdoc />
		public abstract Task<ISpecification<TAggregateRoot>> BeforeRemoveRangeAsync(ISpecification<TAggregateRoot> specification, InterceptionEvent e);

		/// <inheritdoc />
		public abstract Task<Expression<Func<TAggregateRoot, bool>>> BeforeFindAsync(Expression<Func<TAggregateRoot, bool>> predicate, IQueryOptions<TAggregateRoot> queryOptions);

		/// <inheritdoc />
		public abstract Task<ISpecification<TAggregateRoot>> BeforeFindAsync(ISpecification<TAggregateRoot> specification, IQueryOptions<TAggregateRoot> queryOptions);

		/// <inheritdoc />
		public abstract Task AfterFindAsync(TAggregateRoot item);

		/// <inheritdoc />
		public abstract Task AfterFindAsync(IReadOnlyCollection<TAggregateRoot> items);

		/// <inheritdoc />
		public abstract Task AfterFindAsync<TResult>(TResult item);

		/// <inheritdoc />
		public abstract Task AfterFindAsync<TResult>(IReadOnlyCollection<TResult> items);
	}
}
