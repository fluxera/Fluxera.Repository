﻿namespace Fluxera.Repository
{
	using System.Collections.Generic;
	using System.Reflection;
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
		IInterceptionOptionsBuilder AddInterceptorsFromAssemblies(IEnumerable<Assembly> assemblies);

		/// <summary>
		///     Adds the interceptors available in the given assembly to the service collection.
		/// </summary>
		/// <param name="assembly"></param>
		/// <returns>The interception options builder.</returns>
		IInterceptionOptionsBuilder AddInterceptorsFromAssembly(Assembly assembly);
	}
}
