namespace Fluxera.Repository.UnitTests
{
	using System;
	using System.Linq.Expressions;
	using System.Threading.Tasks;
	using Fluxera.Entity;
	using Fluxera.Repository.Interception;
	using Fluxera.Repository.Query;
	using Fluxera.Repository.Specifications;

	public class TestDecoratingInterceptor<T, TKey> : IInterceptor<T, TKey>
		where T : AggregateRoot<T, TKey>
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
		public Task BeforeAddAsync(T item, InterceptionEvent e)
		{
			this.BeforeAddCalled = true;
			return Task.CompletedTask;
		}

		/// <inheritdoc />
		public Task BeforeUpdateAsync(T item, InterceptionEvent e)
		{
			this.BeforeUpdateCalled = true;
			return Task.CompletedTask;
		}

		/// <inheritdoc />
		public Task BeforeRemoveAsync(T item, InterceptionEvent e)
		{
			this.BeforeRemoveCalled = true;
			return Task.CompletedTask;
		}

		/// <inheritdoc />
		public Task<Expression<Func<T, bool>>> BeforeRemoveRangeAsync(Expression<Func<T, bool>> predicate, InterceptionEvent e)
		{
			this.BeforeRemoveRangeExpressionCalled = true;
			return Task.FromResult(predicate);
		}

		/// <inheritdoc />
		public Task<ISpecification<T>> BeforeRemoveRangeAsync(ISpecification<T> specification, InterceptionEvent e)
		{
			this.BeforeRemoveRangeSpecCalled = true;
			return Task.FromResult(specification);
		}

		/// <inheritdoc />
		public Task<Expression<Func<T, bool>>> BeforeFindAsync(Expression<Func<T, bool>> predicate, IQueryOptions<T> queryOptions)
		{
			this.BeforeFindExpressionCalled = true;
			return Task.FromResult(predicate);
		}

		/// <inheritdoc />
		public Task<ISpecification<T>> BeforeFindAsync(ISpecification<T> specification, IQueryOptions<T> queryOptions)
		{
			this.BeforeFindSpecCalled = true;
			return Task.FromResult(specification);
		}
	}
}
