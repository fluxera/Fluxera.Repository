//namespace Fluxera.Repository.Interception
//{
//	using System;
//	using System.Collections.Generic;
//	using System.Linq.Expressions;
//	using System.Threading.Tasks;
//	using Fluxera.Entity;
//	using Fluxera.Repository.Query;

//	internal sealed class NoopInterceptor<TAggregateRoot> : IRepositoryInterceptor<TAggregateRoot>
//		where TAggregateRoot : AggregateRoot<TAggregateRoot>
//	{
//		public Task BeforeAddAsync(TAggregateRoot item, InterceptionEvent e)
//		{
//			return Task.CompletedTask;
//		}

//		/// <inheritdoc />
//		public Task BeforeAddAsync(IEnumerable<TAggregateRoot> items, InterceptionEvent e)
//		{
//			return Task.CompletedTask;
//		}

//		public Task AfterAddAsync(TAggregateRoot item)
//		{
//			return Task.CompletedTask;
//		}

//		/// <inheritdoc />
//		public Task AfterAddAsync(IEnumerable<TAggregateRoot> items)
//		{
//			return Task.CompletedTask;
//		}

//		public Task BeforeUpdateAsync(TAggregateRoot item, InterceptionEvent e)
//		{
//			return Task.CompletedTask;
//		}

//		/// <inheritdoc />
//		public Task BeforeUpdateAsync(IEnumerable<TAggregateRoot> items, InterceptionEvent e)
//		{
//			return Task.CompletedTask;
//		}

//		public Task AfterUpdateAsync(TAggregateRoot item)
//		{
//			return Task.CompletedTask;
//		}

//		/// <inheritdoc />
//		public Task AfterUpdateAsync(IEnumerable<TAggregateRoot> items)
//		{
//			return Task.CompletedTask;
//		}

//		public Task BeforeDeleteAsync(TAggregateRoot item, InterceptionEvent e)
//		{
//			return Task.CompletedTask;
//		}

//		/// <inheritdoc />
//		public Task BeforeDeleteAsync(string id, InterceptionEvent e)
//		{
//			return Task.CompletedTask;
//		}

//		/// <inheritdoc />
//		public Task BeforeDeleteAsync(Expression<Func<TAggregateRoot, bool>> predicate, InterceptionEvent e)
//		{
//			return Task.CompletedTask;
//		}

//		/// <inheritdoc />
//		public Task BeforeDeleteAsync(IEnumerable<TAggregateRoot> items, InterceptionEvent e)
//		{
//			return Task.CompletedTask;
//		}

//		public Task AfterDeleteAsync(TAggregateRoot item)
//		{
//			return Task.CompletedTask;
//		}

//		/// <inheritdoc />
//		public Task AfterDeleteAsync(string id)
//		{
//			return Task.CompletedTask;
//		}

//		/// <inheritdoc />
//		public Task AfterDeleteAsync(Expression<Func<TAggregateRoot, bool>> predicate)
//		{
//			return Task.CompletedTask;
//		}

//		/// <inheritdoc />
//		public Task AfterDeleteAsync(IEnumerable<TAggregateRoot> items)
//		{
//			return Task.CompletedTask;
//		}

//		public Task<Expression<Func<TAggregateRoot, bool>>> BeforeCountAsync(Expression<Func<TAggregateRoot, bool>> predicate)
//		{
//			return Task.FromResult(predicate);
//		}

//		public Task AfterCountAsync(long count)
//		{
//			return Task.CompletedTask;
//		}

//		public Task<Expression<Func<TAggregateRoot, bool>>> BeforeFindAsync(Expression<Func<TAggregateRoot, bool>> predicate, IQueryOptions<TAggregateRoot> queryOptions)
//		{
//			return Task.FromResult(predicate);
//		}

//		public Task AfterFindAsync(IEnumerable<TAggregateRoot> items)
//		{
//			return Task.CompletedTask;
//		}

//		public Task AfterFindAsync(TAggregateRoot item)
//		{
//			return Task.CompletedTask;
//		}

//		public Task AfterFindAsync<TResult>(IEnumerable<TResult> results)
//		{
//			return Task.CompletedTask;
//		}

//		public Task AfterFindAsync<TResult>(TResult result)
//		{
//			return Task.CompletedTask;
//		}

//		/// <inheritdoc />
//		public Task BeforeGetAsync(string id)
//		{
//			return Task.CompletedTask;
//		}

//		/// <inheritdoc />
//		public Task BeforeGetAsync<TResult>(string id, Expression<Func<TAggregateRoot, TResult>> selector)
//		{
//			return Task.CompletedTask;
//		}

//		/// <inheritdoc />
//		public Task AfterGetAsync<TResult>(TResult result)
//		{
//			return Task.CompletedTask;
//		}

//		/// <inheritdoc />
//		public Task AfterGetAsync(TAggregateRoot result)
//		{
//			return Task.CompletedTask;
//		}

//		/// <inheritdoc />
//		public Task BeforeExistsAsync(string id)
//		{
//			return Task.CompletedTask;
//		}

//		/// <inheritdoc />
//		public Task AfterExistsAsync(bool exists)
//		{
//			return Task.CompletedTask;
//		}
//	}
//}
