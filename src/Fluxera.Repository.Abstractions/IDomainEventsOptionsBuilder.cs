namespace Fluxera.Repository
{
	using System.Collections.Generic;
	using System.Reflection;
	using Fluxera.Repository.DomainEvents;
	using JetBrains.Annotations;

	/// <summary>
	///     A contract for a domain events options builder.
	/// </summary>
	[PublicAPI]
	public interface IDomainEventsOptionsBuilder
	{
		/// <summary>
		///     Adds the domain event handlers available in the given assemblies.
		/// </summary>
		/// <param name="assemblies"></param>
		/// <returns></returns>
		IDomainEventsOptionsBuilder AddDomainEventHandlers(IEnumerable<Assembly> assemblies);

		/// <summary>
		///     Adds the domain event handlers available in the given assembly.
		/// </summary>
		/// <param name="assembly"></param>
		/// <returns></returns>
		IDomainEventsOptionsBuilder AddDomainEventHandlers(Assembly assembly);

		/// <summary>
		///     Adds a domain events reducer.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		IDomainEventsOptionsBuilder AddDomainEventsReducer<T>() where T : class, IDomainEventsReducer;
	}
}
