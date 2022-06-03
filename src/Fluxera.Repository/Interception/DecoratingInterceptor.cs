namespace Fluxera.Repository.Interception
{
	using System;
	using System.Collections.Generic;
	using System.Linq.Expressions;
	using System.Threading.Tasks;
	using Fluxera.Entity;
	using Fluxera.Repository.Query;
	using Fluxera.Repository.Specifications;
	using Microsoft.Extensions.Logging;

	internal sealed class DecoratingInterceptor<TAggregateRoot, TKey> : IInterceptor<TAggregateRoot, TKey>
		where TAggregateRoot : AggregateRoot<TAggregateRoot, TKey>
		where TKey : notnull, IComparable<TKey>, IEquatable<TKey>
	{
		private readonly IEnumerable<IInterceptor<TAggregateRoot, TKey>> innerInterceptors;
		private readonly ILogger logger;

		public DecoratingInterceptor(
			ILoggerFactory loggerFactory,
			IEnumerable<IInterceptor<TAggregateRoot, TKey>> interceptors)
		{
			this.innerInterceptors = interceptors;
			this.logger = loggerFactory.CreateLogger(LoggerNames.Interception);
		}

		/// <inheritdoc />
		public async Task BeforeAddAsync(TAggregateRoot item, InterceptionEvent e)
		{
			this.LogTrace($"Intercepting before add: Type = {typeof(TAggregateRoot)}");

			foreach(IInterceptor<TAggregateRoot, TKey> interceptor in this.innerInterceptors)
			{
				await interceptor.BeforeAddAsync(item, e);
			}
		}

		/// <inheritdoc />
		public async Task BeforeUpdateAsync(TAggregateRoot item, InterceptionEvent e)
		{
			this.LogTrace($"Intercepting before update: Type = {typeof(TAggregateRoot)}");

			foreach(IInterceptor<TAggregateRoot, TKey> interceptor in this.innerInterceptors)
			{
				await interceptor.BeforeUpdateAsync(item, e);
			}
		}

		/// <inheritdoc />
		public async Task BeforeRemoveAsync(TAggregateRoot item, InterceptionEvent e)
		{
			this.LogTrace($"Intercepting before remove: Type = {typeof(TAggregateRoot)}");

			foreach(IInterceptor<TAggregateRoot, TKey> interceptor in this.innerInterceptors)
			{
				await interceptor.BeforeRemoveAsync(item, e);
			}
		}

		/// <inheritdoc />
		public async Task<Expression<Func<TAggregateRoot, bool>>> BeforeRemoveRangeAsync(Expression<Func<TAggregateRoot, bool>> predicate, InterceptionEvent e)
		{
			this.LogTrace($"Intercepting before remove: Type = {typeof(TAggregateRoot)}");

			Expression<Func<TAggregateRoot, bool>> interceptorPredicate = predicate;

			foreach(IInterceptor<TAggregateRoot, TKey> interceptor in this.innerInterceptors)
			{
				interceptorPredicate = await interceptor.BeforeRemoveRangeAsync(interceptorPredicate, e);
			}

			return interceptorPredicate;
		}

		/// <inheritdoc />
		public async Task<ISpecification<TAggregateRoot>> BeforeRemoveRangeAsync(ISpecification<TAggregateRoot> specification, InterceptionEvent e)
		{
			this.LogTrace($"Intercepting before remove: Type = {typeof(TAggregateRoot)}");

			ISpecification<TAggregateRoot> interceptorSpecification = specification;

			foreach(IInterceptor<TAggregateRoot, TKey> interceptor in this.innerInterceptors)
			{
				interceptorSpecification = await interceptor.BeforeRemoveRangeAsync(interceptorSpecification, e);
			}

			return interceptorSpecification;
		}

		/// <inheritdoc />
		public async Task<Expression<Func<TAggregateRoot, bool>>> BeforeFindAsync(Expression<Func<TAggregateRoot, bool>> predicate, IQueryOptions<TAggregateRoot> queryOptions)
		{
			this.LogTrace($"Intercepting before find: Type = {typeof(TAggregateRoot)}");

			Expression<Func<TAggregateRoot, bool>> interceptorPredicate = predicate;

			foreach(IInterceptor<TAggregateRoot, TKey> interceptor in this.innerInterceptors)
			{
				interceptorPredicate = await interceptor.BeforeFindAsync(interceptorPredicate, queryOptions);
			}

			return interceptorPredicate;
		}

		/// <inheritdoc />
		public async Task<ISpecification<TAggregateRoot>> BeforeFindAsync(ISpecification<TAggregateRoot> specification, IQueryOptions<TAggregateRoot> queryOptions)
		{
			this.LogTrace($"Intercepting before find: Type = {typeof(TAggregateRoot)}");

			ISpecification<TAggregateRoot> interceptorSpecification = specification;

			foreach(IInterceptor<TAggregateRoot, TKey> interceptor in this.innerInterceptors)
			{
				interceptorSpecification = await interceptor.BeforeFindAsync(interceptorSpecification, queryOptions);
			}

			return interceptorSpecification;
		}

		private void LogTrace(string message)
		{
			this.logger.LogTrace(message);
		}
	}
}
