namespace Fluxera.Repository.Interception
{
	using System;
	using Fluxera.Entity;

	/// <summary>
	///     A contract for creating a factory service that creates the decorating interceptor.
	/// </summary>
	/// <typeparam name="TEntity"></typeparam>
	/// <typeparam name="TKey"></typeparam>
	public interface IDecoratingInterceptorFactory<TEntity, TKey>
		where TEntity : Entity<TEntity, TKey>
		where TKey : notnull, IComparable<TKey>, IEquatable<TKey>
	{
		/// <summary>
		///     Creates a new instance of the <see cref="DecoratingInterceptor{TAggregateRoot,TKey}" /> type.
		/// </summary>
		/// <returns></returns>
		IInterceptor<TEntity, TKey> CreateDecoratingInterceptor();
	}
}
