namespace Fluxera.Repository.Options
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Reflection;
	using Fluxera.Entity;
	using Fluxera.Extensions.Validation;
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
			Guard.Against.Null(services, nameof(services));
			Guard.Against.NullOrWhiteSpace(repositoryName, nameof(repositoryName));
			Guard.Against.Null(repositoryType, nameof(repositoryType));

			this.services = services;
			this.repositoryOptions = new RepositoryOptions((RepositoryName)repositoryName, repositoryType);
		}

		public IRepositoryOptionsBuilder UseFor(IEnumerable<Assembly>? assemblies)
		{
			assemblies ??= Enumerable.Empty<Assembly>();

			foreach(Assembly assembly in assemblies)
			{
				this.UseFor(assembly);
			}

			return this;
		}

		public IRepositoryOptionsBuilder UseFor(Assembly assembly)
		{
			Guard.Against.Null(assembly, nameof(assembly));

			foreach(Type type in assembly.GetTypes())
			{
				if(type.IsAggregateRoot())
				{
					this.UseFor(type);
				}
			}

			return this;
		}

		public IRepositoryOptionsBuilder UseFor(IEnumerable<Type>? types)
		{
			types ??= Enumerable.Empty<Type>();

			foreach(Type type in types)
			{
				if(type.IsAggregateRoot())
				{
					this.UseFor(type);
				}
			}

			return this;
		}

		public IRepositoryOptionsBuilder UseFor(Type type)
		{
			Guard.Against.Null(type, nameof(type));
			Guard.Against.False(type.IsAggregateRoot(), nameof(type), $"The repository can only use aggregate root types: '{type.Name}'");

			if(!this.repositoryOptions.AggregateRootTypes.Contains(type))
			{
				this.repositoryOptions.AggregateRootTypes.Add(type);
			}
			else
			{
				throw new InvalidOperationException(
					$"The aggregate root type '{type.FullName}' was already added for the repository '{this.repositoryOptions.RepositoryName}'.");
			}

			return this;
		}

		public IRepositoryOptionsBuilder UseFor<TAggregateRoot>()
		{
			return this.UseFor(typeof(TAggregateRoot));
		}

		public IRepositoryOptionsBuilder AddSetting<T>(string key, T value)
		{
			Guard.Against.NullOrWhiteSpace(key, nameof(key));
			Guard.Against.Null(value, nameof(value));

			if(!this.repositoryOptions.SettingsValues.ContainsKey(key))
			{
				this.repositoryOptions.SettingsValues.Add(key, value!);
			}
			else
			{
				throw new InvalidOperationException(
					$"The setting '{key}' was already added for the repository '{this.repositoryOptions.RepositoryName}'.");
			}

			return this;
		}

		public IRepositoryOptionsBuilder AddValidation(Action<IValidationOptionsBuilder> configure)
		{
			Guard.Against.Null(configure, nameof(configure));

			if(this.repositoryOptions.ValidationOptions.IsEnabled)
			{
				throw new InvalidOperationException(
					$"The validation was already enabled for repository '{this.repositoryOptions.RepositoryName}'.");
			}

			this.services.AddValidation(builder =>
			{
				IValidationOptionsBuilder validationOptionsBuilder = new ValidationOptionsBuilder(builder, this.repositoryOptions);
				configure.Invoke(validationOptionsBuilder);
			});

			this.repositoryOptions.ValidationOptions.IsEnabled = true;

			return this;
		}

		public IRepositoryOptionsBuilder AddDomainEventHandling(Action<IDomainEventsOptionsBuilder> configure)
		{
			Guard.Against.Null(configure, nameof(configure));

			if(this.repositoryOptions.DomainEventsOptions.IsEnabled)
			{
				throw new InvalidOperationException(
					$"The domain event handling was already enabled for repository '{this.repositoryOptions.RepositoryName}'.");
			}

			DomainEventsOptionsBuilder builder = new DomainEventsOptionsBuilder(this.repositoryOptions);
			configure.Invoke(builder);

			this.repositoryOptions.DomainEventsOptions.IsEnabled = true;

			return this;
		}

		public IRepositoryOptionsBuilder AddCaching(Action<ICachingOptionsBuilder>? configure = null)
		{
			if(this.repositoryOptions.CachingOptions.IsEnabled)
			{
				throw new InvalidOperationException(
					$"The caching was already enabled for repository '{this.repositoryOptions.RepositoryName}'.");
			}

			CachingOptionsBuilder builder = new CachingOptionsBuilder(this.repositoryOptions);
			configure?.Invoke(builder);

			this.repositoryOptions.CachingOptions.IsEnabled = true;

			return this;
		}


		/// <inheritdoc />
		public IRepositoryOptionsBuilder AddInterception(Action<IInterceptionOptionsBuilder> configure)
		{
			Guard.Against.Null(configure, nameof(configure));

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

		internal RepositoryOptions Build()
		{
			return this.repositoryOptions;
		}
	}
}
