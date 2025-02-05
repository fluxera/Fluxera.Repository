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
	/// <typeparam name="TEntity">The type of the aggregate root.</typeparam>
	/// <typeparam name="TKey">The type of the ID.</typeparam>
	[PublicAPI]
	public interface IInterceptor<TEntity, TKey> : IInterceptor
		where TEntity : Entity<TEntity, TKey>
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
		Task BeforeAddAsync(TEntity item, InterceptionEvent e);

		///// <summary>
		/////     This method is called after the execution of the repository add operation.
		///// </summary>
		///// <param name="item">The item that was added.</param>
		///// <returns></returns>
		//Task AfterAddAsync(TEntity item);

		/// <summary>
		///     This method is called before the execution of the repository update operation.
		/// </summary>
		/// <param name="item">The item that will be updated.</param>
		/// <param name="e">The event data.</param>
		/// <returns></returns>
		Task BeforeUpdateAsync(TEntity item, InterceptionEvent e);

		///// <summary>
		/////     This method is called after the execution of the repository update operation.
		///// </summary>
		///// <param name="item">The item that was updated.</param>
		///// <returns></returns>
		//Task AfterUpdateAsync(TEntity item);

		/// <summary>
		///     This method is called before the execution of the repository remove operation.
		/// </summary>
		/// <param name="item">The item that will be deleted.</param>
		/// <param name="e">The event data.</param>
		/// <returns></returns>
		Task BeforeRemoveAsync(TEntity item, InterceptionEvent e);

		///// <summary>
		/////     This method is called after the execution of the repository remove operation.
		///// </summary>
		///// <param name="item">The item that was deleted.</param>
		///// <returns></returns>
		//Task AfterRemoveAsync(TEntity item);

		/// <summary>
		///     This method is called before the execution of the repository remove operation.
		/// </summary>
		/// <param name="predicate">The predicate.</param>
		/// <param name="e">The event data.</param>
		/// <returns></returns>
		Task<Expression<Func<TEntity, bool>>> BeforeRemoveRangeAsync(Expression<Func<TEntity, bool>> predicate, InterceptionEvent e);

		/// <summary>
		///     This method is called before the execution of the repository remove operation.
		/// </summary>
		/// <param name="specification">The specification.</param>
		/// <param name="e">The event data.</param>
		/// <returns></returns>
		Task<ISpecification<TEntity>> BeforeRemoveRangeAsync(ISpecification<TEntity> specification, InterceptionEvent e);

		/// <summary>
		///     This method is called before the execution of the repository find operation.
		/// </summary>
		/// <param name="predicate">The predicate.</param>
		/// <param name="queryOptions">The query options.</param>
		/// <returns></returns>
		Task<Expression<Func<TEntity, bool>>> BeforeFindAsync(Expression<Func<TEntity, bool>> predicate, IQueryOptions<TEntity> queryOptions);

		/// <summary>
		///     This method is called before the execution of the repository find operation.
		/// </summary>
		/// <param name="specification">The specification.</param>
		/// <param name="queryOptions">The query options.</param>
		/// <returns></returns>
		Task<ISpecification<TEntity>> BeforeFindAsync(ISpecification<TEntity> specification, IQueryOptions<TEntity> queryOptions);

		///// <summary>
		/////     This method is called after the execution of the repository find operation.
		///// </summary>
		///// <param name="item">The result item.</param>
		///// <returns></returns>
		//Task AfterFindAsync(TEntity item);

		///// <summary>
		/////     This method is called after the execution of the repository find operation.
		///// </summary>
		///// <param name="items">The result items.</param>
		///// <returns></returns>
		//Task AfterFindAsync(IReadOnlyCollection<TEntity> items);

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
