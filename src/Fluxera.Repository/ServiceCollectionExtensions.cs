namespace Fluxera.Repository
{
	using System;
	using Fluxera.Entity.DomainEvents;
	using Fluxera.Extensions.Common;
	using Fluxera.Extensions.DependencyInjection;
	using Fluxera.Extensions.Validation;
	using Fluxera.Guards;
	using Fluxera.Repository.Caching;
	using Fluxera.Repository.Decorators;
	using Fluxera.Repository.DomainEvents;
	using Fluxera.Repository.Interception;
	using Fluxera.Repository.Options;
	using Fluxera.Repository.Query;
	using Fluxera.Repository.Query.Impl;
	using Fluxera.Repository.Validation;
	using JetBrains.Annotations;
	using Microsoft.Extensions.DependencyInjection;

	/// <summary>
	///     The configuration extensions methods for the repository.
	/// </summary>
	[PublicAPI]
	public static class ServiceCollectionExtensions
	{
		/// <summary>
		///     Adds the repository services and configures the repository by executing the given configure function.
		/// </summary>
		/// <param name="services"></param>
		/// <param name="configure"></param>
		/// <returns></returns>
		public static IServiceCollection AddRepository(this IServiceCollection services, Action<IRepositoryBuilder> configure)
		{
			Guard.Against.Null(services);
			Guard.Against.Null(configure);

			// Add the repository registry singleton.
			services.AddSingleton<IRepositoryRegistry, RepositoryRegistry>();

			// Add the unit-of-work factory.
			services.AddTransient<IUnitOfWorkFactory, UnitOfWorkFactory>();

			// Add the query options builder services.
			services.AddTransient(typeof(IQueryOptionsBuilder<>), typeof(QueryOptionsBuilder<>));

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

			// Add the validation service.
			services.AddValidation();

			// Add the domain infrastructure with domain event handlers.
			services.AddDomainEvents(builder =>
			{
				builder.AddDomainEventDispatcher<OutboxDomainEventDispatcher>();
			});

			// Add the outbox domain event dispatcher service.
			services.AddScoped(serviceProvider =>
			{
				IDomainEventDispatcher domainEventDispatcher = serviceProvider.GetRequiredService<IDomainEventDispatcher>();
				return (IOutboxDomainEventDispatcher)domainEventDispatcher;
			});

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

		private static IServiceCollection DecorateRepository(this IServiceCollection services, Type repositoryType)
		{
			Guard.Against.Null(services);
			Guard.Against.Null(repositoryType);

			services
				.Decorate(repositoryType)
				.With(typeof(CachingRepositoryDecorator<,>))
				.With(typeof(DomainEventsRepositoryDecorator<,>))
				.With(typeof(ValidationRepositoryDecorator<,>))
				.With(typeof(InterceptionRepositoryDecorator<,>))
				.With(typeof(GuardRepositoryDecorator<,>))
				.With(typeof(ExceptionLoggingRepositoryDecorator<,>))
				.With(typeof(DiagnosticRepositoryDecorator<,>));

			return services;
		}
	}
}
