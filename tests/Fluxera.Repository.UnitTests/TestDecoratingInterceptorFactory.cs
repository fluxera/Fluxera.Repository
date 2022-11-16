namespace Fluxera.Repository.UnitTests
{
	using System;
	using Fluxera.Entity;
	using Fluxera.Repository.Interception;

	public class TestDecoratingInterceptorFactory<T, TKey> : IDecoratingInterceptorFactory<T, TKey>
		where T : AggregateRoot<T, TKey>
		where TKey : IComparable<TKey>, IEquatable<TKey>
	{
		private IInterceptor<T, TKey> interceptor;

		/// <inheritdoc />
		public IInterceptor<T, TKey> CreateDecoratingInterceptor()
		{
			this.interceptor ??= new TestDecoratingInterceptor<T, TKey>();
			return this.interceptor;
		}
	}
}
