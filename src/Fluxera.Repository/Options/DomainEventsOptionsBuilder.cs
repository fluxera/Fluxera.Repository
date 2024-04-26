namespace Fluxera.Repository.Options
{
	using System.Collections.Generic;
	using System.Linq;
	using System.Reflection;
	using Fluxera.Guards;
	using Fluxera.Repository.DomainEvents;
	using JetBrains.Annotations;
	using Microsoft.Extensions.DependencyInjection;

	[PublicAPI]
	internal sealed class DomainEventsOptionsBuilder : IDomainEventsOptionsBuilder
	{
		private readonly RepositoryOptions repositoryOptions;
		private readonly IServiceCollection services;

		public DomainEventsOptionsBuilder(RepositoryOptions repositoryOptions, IServiceCollection services)
		{
			this.repositoryOptions = repositoryOptions;
			this.services = services;
		}

		public IDomainEventsOptionsBuilder AddDomainEventHandlers(IEnumerable<Assembly> assemblies)
		{
			assemblies ??= Enumerable.Empty<Assembly>();

			this.services.AddMediatR(cfg =>
			{
				cfg.RegisterServicesFromAssemblies(assemblies.ToArray());
			});

			return this;
		}

		public IDomainEventsOptionsBuilder AddDomainEventHandlers(Assembly assembly)
		{
			assembly = Guard.Against.Null(assembly);

			this.services.AddMediatR(cfg =>
			{
				cfg.RegisterServicesFromAssembly(assembly);
			});

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
