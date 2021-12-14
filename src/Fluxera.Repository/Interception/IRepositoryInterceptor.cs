//namespace Fluxera.Repository.Interception
//{
//	using System;
//	using System.Collections.Generic;
//	using System.Linq.Expressions;
//	using System.Threading.Tasks;
//	using Fluxera.Entity;
//	using Fluxera.Repository.Query;
//	using JetBrains.Annotations;

//	[PublicAPI]
//	public interface IRepositoryInterceptor<TAggregateRoot>
//		where TAggregateRoot : AggregateRoot<TAggregateRoot>
//	{
//		/// <summary>
//		///     This method is called before the execution of the repository add operation.
//		/// </summary>
//		/// <param name="item">The item that will be added.</param>
//		/// <param name="e">The event data.</param>
//		/// <returns></returns>
//		Task BeforeAddAsync(TAggregateRoot item, InterceptionEvent e);

//		/// <summary>
//		///     This method is called after the execution of the repository add operation.
//		/// </summary>
//		/// <param name="item">The item that was added.</param>
//		/// <returns></returns>
//		Task AfterAddAsync(TAggregateRoot item);

//		/// <summary>
//		///     This method is called before the execution of the repository update operation.
//		/// </summary>
//		/// <param name="item">The item that will be updated.</param>
//		/// <param name="e">The event data.</param>
//		/// <returns></returns>
//		Task BeforeUpdateAsync(TAggregateRoot item, InterceptionEvent e);

//		/// <summary>
//		///     This method is called after the execution of the repository update operation.
//		/// </summary>
//		/// <param name="item">The item that was updated.</param>
//		/// <returns></returns>
//		Task AfterUpdateAsync(TAggregateRoot item);

//		/// <summary>
//		///     This method is called before the execution of the repository delete operation.
//		/// </summary>
//		/// <param name="item">The item that will be deleted.</param>
//		/// <param name="e">The event data.</param>
//		/// <returns></returns>
//		Task BeforeDeleteAsync(TAggregateRoot item, InterceptionEvent e);

//		/// <summary>
//		///     This method is called before the execution of the repository delete operation.
//		/// </summary>
//		/// <param name="predicate">The item(s) that will be deleted.</param>
//		/// <param name="e">The event data.</param>
//		/// <returns></returns>
//		Task BeforeDeleteAsync(Expression<Func<TAggregateRoot, bool>> predicate, InterceptionEvent e);

//		/// <summary>
//		///     This method is called after the execution of the repository delete operation.
//		/// </summary>
//		/// <param name="item">The item that was deleted.</param>
//		/// <returns></returns>
//		Task AfterDeleteAsync(TAggregateRoot item);

//		///  <summary>
//		/// 		This method is called before the execution of the repository find operation.
//		///  </summary>
//		///  <param name="predicate">The predicate.</param>
//		///  <param name="queryOptions">The query options.</param>
//		///  <returns></returns>
//		Task<Expression<Func<TAggregateRoot, bool>>> BeforeFindAsync(Expression<Func<TAggregateRoot, bool>> predicate, IQueryOptions<TAggregateRoot> queryOptions);

//		/// <summary>
//		///		This method is called after the execution of the repository find operation.
//		/// </summary>
//		/// <param name="items">The result items.</param>
//		/// <returns></returns>
//		Task AfterFindAsync(IEnumerable<TAggregateRoot> items);
//	}
//}
