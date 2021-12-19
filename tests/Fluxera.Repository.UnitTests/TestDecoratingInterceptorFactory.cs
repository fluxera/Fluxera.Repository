namespace Fluxera.Repository.UnitTests
{
	using Fluxera.Entity;
	using Fluxera.Repository.Interception;
	using Fluxera.Repository.UnitTests.Decorators;

	public class TestDecoratingInterceptorFactory<T, TKey> : IDecoratingInterceptorFactory<T, TKey>
		where T : AggregateRoot<T, TKey>
	{
		/// <inheritdoc />
		public IInterceptor<T, TKey> CreateDecoratingInterceptor()
		{
			return new TestDecoratingInterceptor<T, TKey>();
		}
	}
}
