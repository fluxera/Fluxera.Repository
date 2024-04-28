namespace Fluxera.Repository.Interception
{
	using System;
	using System.Linq.Expressions;
	using System.Threading.Tasks;
	using Fluxera.Entity;
	using Fluxera.Repository.Query;
	using Fluxera.Repository.Specifications;
	using JetBrains.Annotations;

	/// <summary>
	///     Marker interface for interceptors.
	/// </summary>
	[PublicAPI]
	public interface IInterceptor
	{
	}

	/// <summary>
	///     Contract for a service that is used to intercept calls to the repository
	///     before they hit the underlying storage.
	/// </summary>
	/// <typeparam name="TAggregateRoot">The type of the aggregate root.</typeparam>
	/// <typeparam name="TKey">The type of the ID.</typeparam>
	[PublicAPI]
	public interface IInterceptor<TAggregateRoot, TKey> : IInterceptor
		where TAggregateRoot : AggregateRoot<TAggregateRoot, TKey>
		where TKey : notnull, IComparable<TKey>, IEquatable<TKey>
	{
		/// <summary>
		///     Gets the order of execution for this interceptor. The higher, the later this interceptor executes.
		///     The default value is 0.
		/// </summary>
		int Order => 0;

		/// <summary>
		///     This method is called before the execution of the repository add operation.
		/// </summary>
		/// <param name="item">The item that will be added.</param>
		/// <param name="e">The event data.</param>
		/// <returns></returns>
		Task BeforeAddAsync(TAggregateRoot item, InterceptionEvent e);

		///// <summary>
		/////     This method is called after the execution of the repository add operation.
		///// </summary>
		///// <param name="item">The item that was added.</param>
		///// <returns></returns>
		//Task AfterAddAsync(TAggregateRoot item);

		/// <summary>
		///     This method is called before the execution of the repository update operation.
		/// </summary>
		/// <param name="item">The item that will be updated.</param>
		/// <param name="e">The event data.</param>
		/// <returns></returns>
		Task BeforeUpdateAsync(TAggregateRoot item, InterceptionEvent e);

		///// <summary>
		/////     This method is called after the execution of the repository update operation.
		///// </summary>
		///// <param name="item">The item that was updated.</param>
		///// <returns></returns>
		//Task AfterUpdateAsync(TAggregateRoot item);

		/// <summary>
		///     This method is called before the execution of the repository remove operation.
		/// </summary>
		/// <param name="item">The item that will be deleted.</param>
		/// <param name="e">The event data.</param>
		/// <returns></returns>
		Task BeforeRemoveAsync(TAggregateRoot item, InterceptionEvent e);

		///// <summary>
		/////     This method is called after the execution of the repository remove operation.
		///// </summary>
		///// <param name="item">The item that was deleted.</param>
		///// <returns></returns>
		//Task AfterRemoveAsync(TAggregateRoot item);

		/// <summary>
		///     This method is called before the execution of the repository remove operation.
		/// </summary>
		/// <param name="predicate">The predicate.</param>
		/// <param name="e">The event data.</param>
		/// <returns></returns>
		Task<Expression<Func<TAggregateRoot, bool>>> BeforeRemoveRangeAsync(Expression<Func<TAggregateRoot, bool>> predicate, InterceptionEvent e);

		/// <summary>
		///     This method is called before the execution of the repository remove operation.
		/// </summary>
		/// <param name="specification">The specification.</param>
		/// <param name="e">The event data.</param>
		/// <returns></returns>
		Task<ISpecification<TAggregateRoot>> BeforeRemoveRangeAsync(ISpecification<TAggregateRoot> specification, InterceptionEvent e);

		/// <summary>
		///     This method is called before the execution of the repository find operation.
		/// </summary>
		/// <param name="predicate">The predicate.</param>
		/// <param name="queryOptions">The query options.</param>
		/// <returns></returns>
		Task<Expression<Func<TAggregateRoot, bool>>> BeforeFindAsync(Expression<Func<TAggregateRoot, bool>> predicate, IQueryOptions<TAggregateRoot> queryOptions);

		/// <summary>
		///     This method is called before the execution of the repository find operation.
		/// </summary>
		/// <param name="specification">The specification.</param>
		/// <param name="queryOptions">The query options.</param>
		/// <returns></returns>
		Task<ISpecification<TAggregateRoot>> BeforeFindAsync(ISpecification<TAggregateRoot> specification, IQueryOptions<TAggregateRoot> queryOptions);

		///// <summary>
		/////     This method is called after the execution of the repository find operation.
		///// </summary>
		///// <param name="item">The result item.</param>
		///// <returns></returns>
		//Task AfterFindAsync(TAggregateRoot item);

		///// <summary>
		/////     This method is called after the execution of the repository find operation.
		///// </summary>
		///// <param name="items">The result items.</param>
		///// <returns></returns>
		//Task AfterFindAsync(IReadOnlyCollection<TAggregateRoot> items);

		///// <summary>
		/////     This method is called after the execution of the repository find operation.
		///// </summary>
		///// <param name="item">The result item.</param>
		///// <returns></returns>
		//Task AfterFindAsync<TResult>(TResult item);

		///// <summary>
		/////     This method is called after the execution of the repository find operation.
		///// </summary>
		///// <param name="items">The result items.</param>
		///// <returns></returns>
		//Task AfterFindAsync<TResult>(IReadOnlyCollection<TResult> items);
	}
}
