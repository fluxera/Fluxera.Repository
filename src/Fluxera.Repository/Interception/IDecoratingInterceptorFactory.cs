namespace Fluxera.Repository.Interception
{
	using Fluxera.Entity;

	public interface IDecoratingInterceptorFactory<TAggregateRoot, TKey>
		where TAggregateRoot : AggregateRoot<TAggregateRoot, TKey>
	{
		IInterceptor<TAggregateRoot, TKey> CreateDecoratingInterceptor();
	}
}
