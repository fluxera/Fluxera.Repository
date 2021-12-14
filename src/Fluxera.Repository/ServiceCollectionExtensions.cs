namespace Fluxera.Repository
{
	using System;
	using System.Collections.Generic;
	using System.Reflection;
	using Fluxera.Entity.DomainEvents;
	using Fluxera.Extensions.Common;
	using Fluxera.Extensions.DependencyInjection;
	using Fluxera.Extensions.Validation;
	using Fluxera.Guards;
	using Fluxera.Repository.Caching;
	using Fluxera.Repository.Decorators;
	using Fluxera.Repository.Options;
	using Fluxera.Utilities.Extensions;
	using JetBrains.Annotations;
	using Microsoft.Extensions.DependencyInjection;
	using Microsoft.Extensions.DependencyInjection.Extensions;

	[PublicAPI]
	public static class ServiceCollectionExtensions
	{
		public static IServiceCollection AddRepository(this IServiceCollection services, Action<IRepositoryBuilder> configure)
		{
			Guard.Against.Null(services, nameof(services));
			Guard.Against.Null(configure, nameof(configure));

			// Add the repository registry singleton instance.
			IRepositoryRegistry repositoryRegistry = new RepositoryRegistry();
			services.TryAddSingleton(repositoryRegistry);

			// Build the options of the repositories.
			IRepositoryBuilder repositoryBuilder = new RepositoryBuilder(services);
			configure.Invoke(repositoryBuilder);

			// Register the decorators.
			services
				.Decorate(typeof(IRepository<>))
				.With(typeof(CachingRepositoryDecorator<>))
				.With(typeof(DomainEventsRepositoryDecorator<>))
				.With(typeof(ValidationRepositoryDecorator<>))
				.With(typeof(GuardRepositoryDecorator<>));
			services
				.Decorate(typeof(IReadOnlyRepository<>))
				.With(typeof(CachingRepositoryDecorator<>))
				.With(typeof(DomainEventsRepositoryDecorator<>))
				.With(typeof(ValidationRepositoryDecorator<>))
				.With(typeof(GuardRepositoryDecorator<>));

			// Add logging infrastructure.
			services.AddLogging();

			// Add options infrastructure.
			services.AddOptions();

			// Add the basic distributed memory cache in-memory implementation.
			services.AddDistributedMemoryCache();

			// Add hash calculation service.
			services.AddHashCalculator();

			// Add the domain infrastructure with domain event handlers.
			services.AddDomainEvents(builder =>
			{
				builder.AddDomainEventHandlers(() =>
				{
					IReadOnlyCollection<RepositoryOptions> repositoryOptions = repositoryRegistry.GetRepositoryOptions();
					IList<Assembly> domainEventHandlerAssemblies = new List<Assembly>();
					repositoryOptions.ForEach(x => domainEventHandlerAssemblies.AddRange(x.DomainEventsOptions.DomainEventHandlersAssemblies));
					return domainEventHandlerAssemblies.AsReadOnly();
				});
			});

			// Register service that provides the registered repository name for a given aggregate root.
			services.AddTransient<IRepositoryNameProvider, RepositoryNameProvider>();

			// Register the repository configuration provider.
			services.AddTransient<IRepositoryOptionsProvider, RepositoryOptionsProvider>();

			// Register caching strategy factory.
			services.AddTransient<ICachingStrategyFactory, CachingStrategyFactory>();

			// Register caching provider factory.
			services.AddTransient<ICachingProviderFactory, CachingProviderFactory>();

			// Register default cache key provider.
			services.AddTransient<ICacheKeyProvider, DefaultCacheKeyProvider>();

			// Register cache prefix manager.
			services.AddTransient<ICachePrefixManager, CachePrefixManager>();

			// Register the jitter calculator.
			services.AddJitterCalculator();

			// Register validation service.
			services.AddValidation();

			// Register the validator provider service.
			services.AddTransient<IValidatorProvider, ValidatorProvider>();

			return services;
		}
	}
}
