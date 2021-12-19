namespace Fluxera.Repository
{
	using System;
	using System.Collections.Generic;
	using System.Reflection;
	using Fluxera.Entity.DomainEvents;
	using Fluxera.Extensions.Common;
	using Fluxera.Extensions.DependencyInjection;
	using Fluxera.Guards;
	using Fluxera.Repository.Caching;
	using Fluxera.Repository.Decorators;
	using Fluxera.Repository.Interception;
	using Fluxera.Repository.Options;
	using Fluxera.Repository.Validation;
	using Fluxera.Utilities.Extensions;
	using JetBrains.Annotations;
	using Microsoft.Extensions.DependencyInjection;

	[PublicAPI]
	public static class ServiceCollectionExtensions
	{
		public static IServiceCollection AddRepository(this IServiceCollection services, Action<IRepositoryBuilder> configure)
		{
			Guard.Against.Null(services, nameof(services));
			Guard.Against.Null(configure, nameof(configure));

			// Add the repository registry singleton.
			services.AddSingleton<IRepositoryRegistry, RepositoryRegistry>();

			// Build the options of the repositories.
			IRepositoryBuilder repositoryBuilder = new RepositoryBuilder(services);
			configure.Invoke(repositoryBuilder);

			// Register the decorators.
			services.DecorateRepository(typeof(IRepository<,>));
			services.DecorateRepository(typeof(IReadOnlyRepository<,>));

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
					RepositoryOptionsList repositoryOptionsList = services.GetSingletonInstance<RepositoryOptionsList>();
					IList<Assembly> domainEventHandlerAssemblies = new List<Assembly>();
					repositoryOptionsList.ForEach(x => domainEventHandlerAssemblies.AddRange(x.DomainEventsOptions.DomainEventHandlersAssemblies));
					return domainEventHandlerAssemblies.AsReadOnly();
				});
			});

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

			// Register the validation strategy factory.
			services.AddTransient<IValidationStrategyFactory, ValidationStrategyFactory>();

			// Register the interceptor factory.
			services.AddTransient(typeof(IDecoratingInterceptorFactory<,>), typeof(DecoratingInterceptorFactory<,>));

			return services;
		}

		private static IServiceCollection DecorateRepository(this IServiceCollection services, Type repositoryType, bool isInterceptionEnabled = false)
		{
			Guard.Against.Null(services, nameof(services));
			Guard.Against.Null(repositoryType, nameof(repositoryType));

			services
				.Decorate(repositoryType)
				.With(typeof(CachingRepositoryDecorator<,>))
				.With(typeof(DomainEventsRepositoryDecorator<,>))
				.With(typeof(ValidationRepositoryDecorator<,>))
				.With(typeof(InterceptionRepositoryDecorator<,>))
				.With(typeof(GuardRepositoryDecorator<,>))
				.With(typeof(ExceptionLoggingRepositoryDecorator<,>));

			return services;
		}
	}
}
