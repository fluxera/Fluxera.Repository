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
		/// <inheritdoc />
		public Task BeforeAddAsync(T item, InterceptionEvent e)
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc />
		public Task BeforeUpdateAsync(T item, InterceptionEvent e)
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc />
		public Task BeforeRemoveAsync(T item, InterceptionEvent e)
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc />
		public Task<Expression<Func<T, bool>>> BeforeRemoveRangeAsync(Expression<Func<T, bool>> predicate, InterceptionEvent e)
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc />
		public Task<ISpecification<T>> BeforeRemoveRangeAsync(ISpecification<T> specification, InterceptionEvent e)
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc />
		public Task<Expression<Func<T, bool>>> BeforeFindAsync(Expression<Func<T, bool>> predicate, IQueryOptions<T> queryOptions)
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc />
		public Task<ISpecification<T>> BeforeFindAsync(ISpecification<T> specification, IQueryOptions<T> queryOptions)
		{
			throw new NotImplementedException();
		}
	}
}
