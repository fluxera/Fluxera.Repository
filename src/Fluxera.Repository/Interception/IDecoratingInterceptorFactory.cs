﻿namespace Fluxera.Repository.Interception
{
	using Fluxera.Entity;

	/// <summary>
	///     A contract for creating a factory service that creates the decorating interceptor.
	/// </summary>
	/// <typeparam name="TAggregateRoot"></typeparam>
	/// <typeparam name="TKey"></typeparam>
	public interface IDecoratingInterceptorFactory<TAggregateRoot, TKey>
		where TAggregateRoot : AggregateRoot<TAggregateRoot, TKey>
	{
		/// <summary>
		///     Creates a new instance of the <see cref="DecoratingInterceptor{TAggregateRoot,TKey}" /> type.
		/// </summary>
		/// <returns></returns>
		IInterceptor<TAggregateRoot, TKey> CreateDecoratingInterceptor();
	}
}
