namespace Fluxera.Repository.Options
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Reflection;
	using Fluxera.Entity.DomainEvents;
	using Fluxera.Utilities.Extensions;
	using JetBrains.Annotations;

	[PublicAPI]
	internal sealed class DomainEventsOptionsBuilder : IDomainEventsOptionsBuilder
	{
		public DomainEventsOptionsBuilder(RepositoryOptions repositoryOptions)
		{
			this.DomainEventsOptions = repositoryOptions.DomainEventsOptions;
		}

		private DomainEventsOptions DomainEventsOptions { get; }

		public IDomainEventsOptionsBuilder AddDomainEventHandlers(IEnumerable<Assembly> assemblies)
		{
			assemblies ??= Enumerable.Empty<Assembly>();

			foreach(Assembly assembly in assemblies)
			{
				this.AddDomainEventHandlers(assembly);
			}

			return this;
		}

		public IDomainEventsOptionsBuilder AddDomainEventHandlers(Assembly assembly)
		{
			IEnumerable<Type> types = assembly.GetTypes().Where(x => x.Implements<IDomainEventHandler>());

			this.AddDomainEventHandlers(types);

			return this;
		}

		/// <inheritdoc />
		public IDomainEventsOptionsBuilder AddDomainEventHandlers(IEnumerable<Type> types)
		{
			types ??= Enumerable.Empty<Type>();
			foreach(Type type in types)
			{
				this.AddDomainEventHandler(type);
			}

			return this;
		}

		/// <inheritdoc />
		public IDomainEventsOptionsBuilder AddDomainEventHandler<T>() where T : IDomainEventHandler
		{
			return this.AddDomainEventHandler(typeof(T));
		}

		/// <inheritdoc />
		public IDomainEventsOptionsBuilder AddDomainEventHandler(Type type)
		{
			if(!type.Implements<IDomainEventHandler>())
			{
				throw new ArgumentException("The given type was no domain event handler.", nameof(type));
			}

			this.DomainEventsOptions.DomainEventHandlerTypes.Add(type);
			return this;
		}
	}
}
