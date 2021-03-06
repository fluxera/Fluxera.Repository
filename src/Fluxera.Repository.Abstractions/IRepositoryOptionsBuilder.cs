namespace Fluxera.Repository
{
	using System;
	using System.Collections.Generic;
	using System.Reflection;
	using JetBrains.Annotations;

	/// <summary>
	///     A contract for a repository options builder service.
	/// </summary>
	[PublicAPI]
	public interface IRepositoryOptionsBuilder
	{
		/// <summary>
		///     Configures that the repository that is currently configured will be used
		///     for the available aggregate root types in the given assemblies.
		/// </summary>
		/// <param name="assemblies"></param>
		/// <returns></returns>
		IRepositoryOptionsBuilder UseFor(IEnumerable<Assembly> assemblies);

		/// <summary>
		///     Configures that the repository that is currently configured will be used
		///     for the available aggregate root types in the given assembly.
		/// </summary>
		/// <param name="assembly"></param>
		/// <returns></returns>
		IRepositoryOptionsBuilder UseFor(Assembly assembly);

		/// <summary>
		///     Configures that the repository that is currently configured will be used
		///     for the given aggregate root types.
		/// </summary>
		/// <param name="types"></param>
		/// <returns></returns>
		IRepositoryOptionsBuilder UseFor(IEnumerable<Type> types);

		/// <summary>
		///     Configures that the repository that is currently configured will be used
		///     for the given aggregate root type.
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		IRepositoryOptionsBuilder UseFor(Type type);

		/// <summary>
		///     Configures that the repository that is currently configured will be used
		///     for the given aggregate root type.
		/// </summary>
		/// <typeparam name="TAggregateRoot">The aggregate root type.</typeparam>
		/// <returns></returns>
		IRepositoryOptionsBuilder UseFor<TAggregateRoot>();

		/// <summary>
		///     Adds a key/value setting for the storage to use. This values will be
		///     available in the repository settings of the storage implementation.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="key"></param>
		/// <param name="value"></param>
		/// <returns></returns>
		IRepositoryOptionsBuilder AddSetting<T>(string key, T value);

		/// <summary>
		///     Add the validation feature to the repository.
		/// </summary>
		/// <param name="configure"></param>
		/// <returns></returns>
		IRepositoryOptionsBuilder AddValidation(Action<IValidationOptionsBuilder> configure);

		/// <summary>
		///     Adds the domain events handling feature to the repository.
		/// </summary>
		/// <param name="configure"></param>
		/// <returns></returns>
		IRepositoryOptionsBuilder AddDomainEventHandling(Action<IDomainEventsOptionsBuilder> configure);

		/// <summary>
		///     Adds the caching feature to the repository.
		/// </summary>
		/// <param name="configure"></param>
		/// <returns></returns>
		IRepositoryOptionsBuilder AddCaching(Action<ICachingOptionsBuilder> configure = null);

		/// <summary>
		///     Adds the interception feature to the repository.
		/// </summary>
		/// <param name="configure"></param>
		/// <returns></returns>
		IRepositoryOptionsBuilder AddInterception(Action<IInterceptionOptionsBuilder> configure);
	}
}
