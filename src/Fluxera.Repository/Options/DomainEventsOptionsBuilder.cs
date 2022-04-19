namespace Fluxera.Repository.Options
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using JetBrains.Annotations;

	[PublicAPI]
	internal sealed class DomainEventsOptionsBuilder : IDomainEventsOptionsBuilder
	{
		public DomainEventsOptionsBuilder(RepositoryOptions repositoryOptions)
		{
			this.DomainEventsOptions = repositoryOptions.DomainEventsOptions;
		}

		private DomainEventsOptions DomainEventsOptions { get; }

		//public IDomainEventsOptionsBuilder AddEventHandlers(IEnumerable<Assembly> assemblies)
		//{
		//	assemblies ??= Enumerable.Empty<Assembly>();

		//	foreach(Assembly assembly in assemblies)
		//	{
		//		this.AddEventHandlers(assembly);
		//	}

		//	return this;
		//}

		//public IDomainEventsOptionsBuilder AddEventHandlers(Assembly assembly)
		//{
		//	assembly.GetTypes().Where(x => x.IsAssignableTo<I>())

		//	this.DomainEventsOptions.DomainEventHandlerTypes.Add(assembly);

		//	return this;
		//}

		/// <inheritdoc />
		public IDomainEventsOptionsBuilder AddEventHandlers(IEnumerable<Type> types)
		{
			types ??= Enumerable.Empty<Type>();
			foreach(Type type in types)
			{
				this.AddEventHandler(type);
			}

			return this;
		}

		/// <inheritdoc />
		public IDomainEventsOptionsBuilder AddEventHandler<T>()
		{
			return this.AddEventHandler(typeof(T));
		}

		/// <inheritdoc />
		public IDomainEventsOptionsBuilder AddEventHandler(Type type)
		{
			this.DomainEventsOptions.DomainEventHandlerTypes.Add(type);
			return this;
		}
	}
}
