namespace Fluxera.Repository.Options
{
	using System;
	using System.Collections.Generic;
	using System.Reflection;
	using Fluxera.Entity;
	using Fluxera.Guards;
	using JetBrains.Annotations;
	using Microsoft.Extensions.DependencyInjection;

	[PublicAPI]
	internal sealed class RepositoryOptionsBuilder : IRepositoryOptionsBuilder
	{
		private readonly RepositoryOptions repositoryOptions;
		private readonly IServiceCollection services;

		public RepositoryOptionsBuilder(IServiceCollection services, string repositoryName, Type repositoryType)
		{
			Guard.Against.Null(services);
			Guard.Against.NullOrWhiteSpace(repositoryName);
			Guard.Against.Null(repositoryType);

			this.services = services;
			this.repositoryOptions = new RepositoryOptions((RepositoryName)repositoryName);
		}

		public IRepositoryOptionsBuilder UseFor(IEnumerable<Assembly> assemblies)
		{
			assemblies ??= [];

			foreach(Assembly assembly in assemblies)
			{
				this.UseFor(assembly);
			}

			return this;
		}

		public IRepositoryOptionsBuilder UseFor(Assembly assembly)
		{
			Guard.Against.Null(assembly);

			foreach(Type type in assembly.GetTypes())
			{
				if(type.IsEntity())
				{
					this.UseFor(type);
				}
			}

			return this;
		}

		public IRepositoryOptionsBuilder UseFor(IEnumerable<Type> types)
		{
			types ??= [];

			foreach(Type type in types)
			{
				if(type.IsEntity())
				{
					this.UseFor(type);
				}
			}

			return this;
		}

		public IRepositoryOptionsBuilder UseFor(Type type)
		{
			Guard.Against.Null(type);
			Guard.Against.False(type.IsEntity(), message: $"The repository can only use entity types: '{type.Name}'");

			if(!this.repositoryOptions.AggregateRootTypes.Contains(type))
			{
				this.repositoryOptions.AggregateRootTypes.Add(type);
			}
			else
			{
				throw new InvalidOperationException(
					$"The entity type '{type.FullName}' was already added for the repository '{this.repositoryOptions.RepositoryName}'.");
			}

			return this;
		}

		public IRepositoryOptionsBuilder UseFor<TAggregateRoot>()
		{
			return this.UseFor(typeof(TAggregateRoot));
		}

		public IRepositoryOptionsBuilder EnableValidation(Action<IValidationOptionsBuilder> configure = null)
		{
			if(this.repositoryOptions.ValidationOptions.IsEnabled)
			{
				throw new InvalidOperationException(
					$"The validation was already enabled for repository '{this.repositoryOptions.RepositoryName}'.");
			}

			ValidationOptionsBuilder builder = new ValidationOptionsBuilder(this.repositoryOptions, this.services);
			configure?.Invoke(builder);

			this.repositoryOptions.ValidationOptions.IsEnabled = true;

			return this;
		}

		public IRepositoryOptionsBuilder EnableDomainEventHandling(Action<IDomainEventsOptionsBuilder> configure = null)
		{
			if(this.repositoryOptions.DomainEventsOptions.IsEnabled)
			{
				throw new InvalidOperationException(
					$"The domain event handling was already enabled for repository '{this.repositoryOptions.RepositoryName}'.");
			}

			DomainEventsOptionsBuilder builder = new DomainEventsOptionsBuilder(this.repositoryOptions, this.services);
			configure?.Invoke(builder);

			this.repositoryOptions.DomainEventsOptions.IsEnabled = true;

			return this;
		}

		public IRepositoryOptionsBuilder EnableCaching(Action<ICachingOptionsBuilder> configure)
		{
			Guard.Against.Null(configure);

			if(this.repositoryOptions.CachingOptions.IsEnabled)
			{
				throw new InvalidOperationException(
					$"The caching was already enabled for repository '{this.repositoryOptions.RepositoryName}'.");
			}

			CachingOptionsBuilder builder = new CachingOptionsBuilder(this.repositoryOptions);
			configure.Invoke(builder);

			this.repositoryOptions.CachingOptions.IsEnabled = true;

			return this;
		}

		/// <inheritdoc />
		public IRepositoryOptionsBuilder EnableInterception(Action<IInterceptionOptionsBuilder> configure)
		{
			Guard.Against.Null(configure);

			if(this.repositoryOptions.InterceptionOptions.IsEnabled)
			{
				throw new InvalidOperationException(
					$"The interception was already enabled for repository '{this.repositoryOptions.RepositoryName}'.");
			}

			IInterceptionOptionsBuilder builder = new InterceptionOptionsBuilder(this.services);
			configure.Invoke(builder);

			this.repositoryOptions.InterceptionOptions.IsEnabled = true;

			return this;
		}

		/// <inheritdoc />
		public IRepositoryOptionsBuilder EnableUnitOfWork()
		{
			this.repositoryOptions.IsUnitOfWorkEnabled = true;

			return this;
		}

		internal void SetRepositoryContextType(Type contextType)
		{
			Guard.Against.Null(contextType);

			this.repositoryOptions.RepositoryContextType = contextType;
		}

		internal RepositoryOptions Build()
		{
			return this.repositoryOptions;
		}
	}
}
