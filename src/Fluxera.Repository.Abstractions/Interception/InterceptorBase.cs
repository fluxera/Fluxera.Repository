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
	///     A base class for a service that is used to intercept calls to the repository
	///     before they hit the underlying storage.
	/// </summary>
	/// <typeparam name="TEntity">The type of the aggregate root.</typeparam>
	/// <typeparam name="TKey">The type of the ID.</typeparam>
	[PublicAPI]
	public abstract class InterceptorBase<TEntity, TKey> : IInterceptor<TEntity, TKey>
		where TEntity : Entity<TEntity, TKey>
		where TKey : IComparable<TKey>, IEquatable<TKey>
	{
		/// <inheritdoc />
		public virtual int Order => 0;

		/// <inheritdoc />
		public virtual Task BeforeAddAsync(TEntity item, InterceptionEvent e)
		{
			return Task.CompletedTask;
		}

		/// <inheritdoc />
		public virtual Task BeforeUpdateAsync(TEntity item, InterceptionEvent e)
		{
			return Task.CompletedTask;
		}

		/// <inheritdoc />
		public virtual Task BeforeRemoveAsync(TEntity item, InterceptionEvent e)
		{
			return Task.CompletedTask;
		}

		/// <inheritdoc />
		public virtual Task<Expression<Func<TEntity, bool>>> BeforeRemoveRangeAsync(Expression<Func<TEntity, bool>> predicate, InterceptionEvent e)
		{
			return Task.FromResult(predicate);
		}

		/// <inheritdoc />
		public virtual Task<ISpecification<TEntity>> BeforeRemoveRangeAsync(ISpecification<TEntity> specification, InterceptionEvent e)
		{
			return Task.FromResult(specification);
		}

		/// <inheritdoc />
		public virtual Task<Expression<Func<TEntity, bool>>> BeforeFindAsync(Expression<Func<TEntity, bool>> predicate, IQueryOptions<TEntity> queryOptions)
		{
			return Task.FromResult(predicate);
		}

		/// <inheritdoc />
		public virtual Task<ISpecification<TEntity>> BeforeFindAsync(ISpecification<TEntity> specification, IQueryOptions<TEntity> queryOptions)
		{
			return Task.FromResult(specification);
		}
	}
}
