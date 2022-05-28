namespace Fluxera.Repository.UnitTests
{
	using System;
	using Fluxera.Entity;
	using Fluxera.Repository.Interception;

	public class TestDecoratingInterceptorFactory<T, TKey> : IDecoratingInterceptorFactory<T, TKey>
		where T : AggregateRoot<T, TKey>
		where TKey : IComparable<TKey>, IEquatable<TKey>
	{
		/// <inheritdoc />
		public IInterceptor<T, TKey> CreateDecoratingInterceptor()
		{
			return new TestDecoratingInterceptor<T, TKey>();
		}
	}
}
