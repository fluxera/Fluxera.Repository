namespace Fluxera.Repository.Interception
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using Fluxera.Entity;
	using Microsoft.Extensions.Logging;

	internal sealed class DecoratingInterceptorFactory<TEntity, TKey> : IDecoratingInterceptorFactory<TEntity, TKey>
		where TEntity : Entity<TEntity, TKey>
		where TKey : IComparable<TKey>, IEquatable<TKey>
	{
		private readonly IEnumerable<IInterceptor<TEntity, TKey>> interceptors;
		private readonly ILoggerFactory loggerFactory;

		public DecoratingInterceptorFactory(ILoggerFactory loggerFactory, IEnumerable<IInterceptor<TEntity, TKey>> interceptors)
		{
			this.loggerFactory = loggerFactory;
			this.interceptors = interceptors;
		}

		/// <inheritdoc />
		public IInterceptor<TEntity, TKey> CreateDecoratingInterceptor()
		{
			return new DecoratingInterceptor<TEntity, TKey>(this.loggerFactory, this.interceptors.OrderBy(x => x.Order));
		}
	}
}
