namespace Fluxera.Repository.Interception
{
	using System.Collections.Generic;
	using Fluxera.Entity;
	using Microsoft.Extensions.Logging;

	internal sealed class DecoratingInterceptorFactory<TAggregateRoot, TKey> : IDecoratingInterceptorFactory<TAggregateRoot, TKey>
		where TAggregateRoot : AggregateRoot<TAggregateRoot, TKey>
	{
		private readonly IEnumerable<IInterceptor<TAggregateRoot, TKey>> interceptors;
		private readonly ILoggerFactory loggerFactory;

		public DecoratingInterceptorFactory(ILoggerFactory loggerFactory, IEnumerable<IInterceptor<TAggregateRoot, TKey>> interceptors)
		{
			this.loggerFactory = loggerFactory;
			this.interceptors = interceptors;
		}

		/// <inheritdoc />
		public IInterceptor<TAggregateRoot, TKey> CreateDecoratingInterceptor()
		{
			return new DecoratingInterceptor<TAggregateRoot, TKey>(this.loggerFactory, this.interceptors);
		}
	}
}
