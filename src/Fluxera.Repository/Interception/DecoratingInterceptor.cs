namespace Fluxera.Repository.Interception
{
	using System;
	using System.Collections.Generic;
	using System.Linq.Expressions;
	using System.Threading.Tasks;
	using Fluxera.Entity;
	using Fluxera.Repository.Extensions;
	using Fluxera.Repository.Query;
	using Fluxera.Repository.Specifications;
	using Microsoft.Extensions.Logging;

	internal sealed class DecoratingInterceptor<TEntity, TKey> : IInterceptor<TEntity, TKey>, IDecoratingInterceptor
		where TEntity : Entity<TEntity, TKey>
		where TKey : notnull, IComparable<TKey>, IEquatable<TKey>
	{
		private readonly IEnumerable<IInterceptor<TEntity, TKey>> innerInterceptors;
		private readonly ILogger logger;

		public DecoratingInterceptor(
			ILoggerFactory loggerFactory,
			IEnumerable<IInterceptor<TEntity, TKey>> interceptors)
		{
			this.innerInterceptors = interceptors;
			this.logger = loggerFactory.CreateLogger(LoggerNames.Interception);
		}

		/// <inheritdoc />
		public async Task BeforeAddAsync(TEntity item, InterceptionEvent e)
		{
			this.logger.LogInterceptingBeforeOperation("add", typeof(TEntity).Name);

			foreach(IInterceptor<TEntity, TKey> interceptor in this.innerInterceptors)
			{
				await interceptor.BeforeAddAsync(item, e);
			}
		}

		/// <inheritdoc />
		public async Task BeforeUpdateAsync(TEntity item, InterceptionEvent e)
		{
			this.logger.LogInterceptingBeforeOperation("update", typeof(TEntity).Name);

			foreach(IInterceptor<TEntity, TKey> interceptor in this.innerInterceptors)
			{
				await interceptor.BeforeUpdateAsync(item, e);
			}
		}

		/// <inheritdoc />
		public async Task BeforeRemoveAsync(TEntity item, InterceptionEvent e)
		{
			this.logger.LogInterceptingBeforeOperation("remove", typeof(TEntity).Name);

			foreach(IInterceptor<TEntity, TKey> interceptor in this.innerInterceptors)
			{
				await interceptor.BeforeRemoveAsync(item, e);
			}
		}

		/// <inheritdoc />
		public async Task<Expression<Func<TEntity, bool>>> BeforeRemoveRangeAsync(Expression<Func<TEntity, bool>> predicate, InterceptionEvent e)
		{
			this.logger.LogInterceptingBeforeOperation("remove", typeof(TEntity).Name);

			Expression<Func<TEntity, bool>> interceptorPredicate = predicate;

			foreach(IInterceptor<TEntity, TKey> interceptor in this.innerInterceptors)
			{
				interceptorPredicate = await interceptor.BeforeRemoveRangeAsync(interceptorPredicate, e);
			}

			return interceptorPredicate;
		}

		/// <inheritdoc />
		public async Task<ISpecification<TEntity>> BeforeRemoveRangeAsync(ISpecification<TEntity> specification, InterceptionEvent e)
		{
			this.logger.LogInterceptingBeforeOperation("remove", typeof(TEntity).Name);

			ISpecification<TEntity> interceptorSpecification = specification;

			foreach(IInterceptor<TEntity, TKey> interceptor in this.innerInterceptors)
			{
				interceptorSpecification = await interceptor.BeforeRemoveRangeAsync(interceptorSpecification, e);
			}

			return interceptorSpecification;
		}

		/// <inheritdoc />
		public async Task<Expression<Func<TEntity, bool>>> BeforeFindAsync(Expression<Func<TEntity, bool>> predicate, IQueryOptions<TEntity> queryOptions)
		{
			this.logger.LogInterceptingBeforeOperation("find", typeof(TEntity).Name);

			Expression<Func<TEntity, bool>> interceptorPredicate = predicate;

			foreach(IInterceptor<TEntity, TKey> interceptor in this.innerInterceptors)
			{
				interceptorPredicate = await interceptor.BeforeFindAsync(interceptorPredicate, queryOptions);
			}

			return interceptorPredicate;
		}

		/// <inheritdoc />
		public async Task<ISpecification<TEntity>> BeforeFindAsync(ISpecification<TEntity> specification, IQueryOptions<TEntity> queryOptions)
		{
			this.logger.LogInterceptingBeforeOperation("find", typeof(TEntity).Name);

			ISpecification<TEntity> interceptorSpecification = specification;

			foreach(IInterceptor<TEntity, TKey> interceptor in this.innerInterceptors)
			{
				interceptorSpecification = await interceptor.BeforeFindAsync(interceptorSpecification, queryOptions);
			}

			return interceptorSpecification;
		}
	}
}
