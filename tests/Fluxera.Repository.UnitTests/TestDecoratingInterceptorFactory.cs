namespace Fluxera.Repository.UnitTests
{
	using System;
	using Fluxera.Entity;
	using Fluxera.Repository.Interception;

	public class TestDecoratingInterceptorFactory<TEntity, TKey> : IDecoratingInterceptorFactory<TEntity, TKey>
		where TEntity : Entity<TEntity, TKey>
		where TKey : IComparable<TKey>, IEquatable<TKey>
	{
		private IInterceptor<TEntity, TKey> interceptor;

		/// <inheritdoc />
		public IInterceptor<TEntity, TKey> CreateDecoratingInterceptor()
		{
			this.interceptor ??= new TestDecoratingInterceptor<TEntity, TKey>();
			return this.interceptor;
		}
	}
}
