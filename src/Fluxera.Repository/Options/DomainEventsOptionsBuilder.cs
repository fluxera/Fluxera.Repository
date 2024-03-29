﻿namespace Fluxera.Repository.Options
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Reflection;
	using Fluxera.Entity.DomainEvents;
	using Fluxera.Repository.DomainEvents;
	using Fluxera.Utilities.Extensions;
	using JetBrains.Annotations;
	using Microsoft.Extensions.DependencyInjection;
	using Microsoft.Extensions.DependencyInjection.Extensions;

	[PublicAPI]
	internal sealed class DomainEventsOptionsBuilder : IDomainEventsOptionsBuilder
	{
		private readonly IServiceCollection services;

		public DomainEventsOptionsBuilder(RepositoryOptions repositoryOptions, IServiceCollection services)
		{
			this.DomainEventsOptions = repositoryOptions.DomainEventsOptions;
			this.services = services;
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

		/// <inheritdoc />
		public IDomainEventsOptionsBuilder AddCrudDomainEventsFactory<T>() where T : class, ICrudDomainEventsFactory
		{
			this.services.RemoveAll<ICrudDomainEventsFactory>();
			this.services.TryAddSingleton<ICrudDomainEventsFactory, T>();

			return this;
		}

		/// <inheritdoc />
		public IDomainEventsOptionsBuilder EnableAutomaticCrudDomainEvents()
		{
			this.DomainEventsOptions.IsAutomaticCrudDomainEventsEnabled = true;

			return this;
		}

		/// <inheritdoc />
		public IDomainEventsOptionsBuilder AddDomainEventsReducer<T>() where T : class, IDomainEventsReducer
		{
			this.services.AddTransient<IDomainEventsReducer, T>();

			return this;
		}
	}
}
