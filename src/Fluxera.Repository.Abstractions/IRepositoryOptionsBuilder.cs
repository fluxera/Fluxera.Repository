namespace Fluxera.Repository
{
	using System;
	using System.Collections.Generic;
	using System.Reflection;
	using Fluxera.Entity;
	using JetBrains.Annotations;

	[PublicAPI]
	public interface IRepositoryOptionsBuilder
	{
		IRepositoryOptionsBuilder UseFor(IEnumerable<Assembly> assemblies);

		IRepositoryOptionsBuilder UseFor(Assembly assembly);

		IRepositoryOptionsBuilder UseFor(IEnumerable<Type> types);

		IRepositoryOptionsBuilder UseFor(Type type);

		IRepositoryOptionsBuilder UseFor<TAggregateRoot>() where TAggregateRoot : AggregateRoot<TAggregateRoot>;

		IRepositoryOptionsBuilder AddSetting<T>(string key, T value);

		IRepositoryOptionsBuilder AddValidation(Action<IValidationOptionsBuilder> configure);

		//IRepositoryOptionsBuilder AddInterception(Action<IInterceptionOptionsBuilder> configure);

		IRepositoryOptionsBuilder AddDomainEventHandling(Action<IDomainEventsOptionsBuilder> configure);

		IRepositoryOptionsBuilder AddCaching(Action<ICachingOptionsBuilder>? configure = null);
	}
}
