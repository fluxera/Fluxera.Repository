namespace Fluxera.Repository
{
	using System;
	using System.Collections.Generic;
	using JetBrains.Annotations;

	/// <summary>
	///     A contract for a domain events options builder.
	/// </summary>
	[PublicAPI]
	public interface IDomainEventsOptionsBuilder
	{
		///// <summary>
		/////     Adds the domain event handlers available in the given assemblies.
		///// </summary>
		///// <param name="assemblies"></param>
		///// <returns></returns>
		//IDomainEventsOptionsBuilder AddEventHandlers(IEnumerable<Assembly> assemblies);

		///// <summary>
		/////     Adds the domain event handlers available in the given assembly.
		///// </summary>
		///// <param name="assembly"></param>
		///// <returns></returns>
		//IDomainEventsOptionsBuilder AddEventHandlers(Assembly assembly);

		/// <summary>
		///     Adds the given domain event handlers.
		/// </summary>
		/// <param name="types"></param>
		/// <returns></returns>
		IDomainEventsOptionsBuilder AddEventHandlers(IEnumerable<Type> types);

		/// <summary>
		///     Adds the given domain event handler.
		/// </summary>
		/// <returns></returns>
		IDomainEventsOptionsBuilder AddEventHandler<T>();

		/// <summary>
		///     Adds the given domain event handler.
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		IDomainEventsOptionsBuilder AddEventHandler(Type type);
	}
}
