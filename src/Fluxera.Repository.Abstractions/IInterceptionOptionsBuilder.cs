namespace Fluxera.Repository
{
	using System;
	using System.Collections.Generic;
	using System.Reflection;
	using Fluxera.Repository.Interception;
	using JetBrains.Annotations;

	/// <summary>
	///     A contract for the interception options builder.
	/// </summary>
	[PublicAPI]
	public interface IInterceptionOptionsBuilder
	{
		/// <summary>
		///     Adds the interceptors available in the given assemblies to the service collection.
		/// </summary>
		/// <param name="assemblies"></param>
		/// <returns>The interception options builder.</returns>
		IInterceptionOptionsBuilder AddInterceptors(IEnumerable<Assembly> assemblies);

		/// <summary>
		///     Adds the interceptors available in the given assembly to the service collection.
		/// </summary>
		/// <param name="assembly"></param>
		/// <returns>The interception options builder.</returns>
		IInterceptionOptionsBuilder AddInterceptors(Assembly assembly);

		/// <summary>
		///     Adds the given interceptors to the service collection.
		/// </summary>
		/// <param name="types"></param>
		/// <returns>The interception options builder.</returns>
		IInterceptionOptionsBuilder AddInterceptors(IEnumerable<Type> types);

		/// <summary>
		///     Adds the given interceptor to the service collection.
		/// </summary>
		/// <param name="interceptorType"></param>
		/// <returns>The interception options builder.</returns>
		IInterceptionOptionsBuilder AddInterceptor(Type interceptorType);

		/// <summary>
		/// </summary>
		/// <typeparam name="TInterceptor"></typeparam>
		/// <returns>The interception options builder.</returns>
		IInterceptionOptionsBuilder AddInterceptor<TInterceptor>()
			where TInterceptor : IInterceptor;
	}
}
