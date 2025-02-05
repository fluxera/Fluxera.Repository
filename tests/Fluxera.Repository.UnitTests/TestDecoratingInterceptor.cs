namespace Fluxera.Repository.UnitTests
{
	using System;
	using System.Linq.Expressions;
	using System.Threading.Tasks;
	using Fluxera.Entity;
	using Fluxera.Repository.Interception;
	using Fluxera.Repository.Query;
	using Fluxera.Repository.Specifications;

	public class TestDecoratingInterceptor<TEntity, TKey> : IInterceptor<TEntity, TKey>, IDecoratingInterceptor
		where TEntity : Entity<TEntity, TKey>
		where TKey : IComparable<TKey>, IEquatable<TKey>
	{
		public bool BeforeAddCalled;
		public bool BeforeUpdateCalled;
		public bool BeforeRemoveCalled;
		public bool BeforeRemoveRangeExpressionCalled;
		public bool BeforeRemoveRangeSpecCalled;
		public bool BeforeFindExpressionCalled;
		public bool BeforeFindSpecCalled;

		/// <inheritdoc />
		public Task BeforeAddAsync(TEntity item, InterceptionEvent e)
		{
			this.BeforeAddCalled = true;
			return Task.CompletedTask;
		}

		/// <inheritdoc />
		public Task BeforeUpdateAsync(TEntity item, InterceptionEvent e)
		{
			this.BeforeUpdateCalled = true;
			return Task.CompletedTask;
		}

		/// <inheritdoc />
		public Task BeforeRemoveAsync(TEntity item, InterceptionEvent e)
		{
			this.BeforeRemoveCalled = true;
			return Task.CompletedTask;
		}

		/// <inheritdoc />
		public Task<Expression<Func<TEntity, bool>>> BeforeRemoveRangeAsync(Expression<Func<TEntity, bool>> predicate, InterceptionEvent e)
		{
			this.BeforeRemoveRangeExpressionCalled = true;
			return Task.FromResult(predicate);
		}

		/// <inheritdoc />
		public Task<ISpecification<TEntity>> BeforeRemoveRangeAsync(ISpecification<TEntity> specification, InterceptionEvent e)
		{
			this.BeforeRemoveRangeSpecCalled = true;
			return Task.FromResult(specification);
		}

		/// <inheritdoc />
		public Task<Expression<Func<TEntity, bool>>> BeforeFindAsync(Expression<Func<TEntity, bool>> predicate, IQueryOptions<TEntity> queryOptions)
		{
			this.BeforeFindExpressionCalled = true;
			return Task.FromResult(predicate);
		}

		/// <inheritdoc />
		public Task<ISpecification<TEntity>> BeforeFindAsync(ISpecification<TEntity> specification, IQueryOptions<TEntity> queryOptions)
		{
			this.BeforeFindSpecCalled = true;
			return Task.FromResult(specification);
		}
	}
}
