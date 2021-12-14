namespace Fluxera.Repository.Options
{
	using System;
	using Fluxera.Extensions.DependencyInjection;
	using Fluxera.Guards;
	using JetBrains.Annotations;
	using Microsoft.Extensions.DependencyInjection;

	[PublicAPI]
	public sealed class RepositoryBuilder : IRepositoryBuilder
	{
		private readonly IServiceCollection services;

		public RepositoryBuilder(IServiceCollection services)
		{
			Guard.Against.Null(services, nameof(services));

			this.services = services;
		}

		public IRepositoryBuilder AddRepository(string repositoryName, Type repositoryType, Action<IRepositoryOptionsBuilder> configure)
		{
			Guard.Against.NullOrWhiteSpace(repositoryName, nameof(repositoryName));
			Guard.Against.Null(repositoryType, nameof(repositoryType));
			Guard.Against.Null(configure, nameof(configure));

			RepositoryOptionsBuilder repositoryOptionsBuilder = new RepositoryOptionsBuilder(this.services, repositoryName, repositoryType);
			configure.Invoke(repositoryOptionsBuilder);
			RepositoryOptions repositoryOptions = repositoryOptionsBuilder.Build();

			// Add the repositories for the registered aggregates.
			Type repositoryServiceTemplateType = typeof(IRepository<>);
			Type repositoryImplementationTemplateType = repositoryType;
			foreach(Type aggregateRootType in repositoryOptions.AggregateRootTypes)
			{
				Type serviceType = repositoryServiceTemplateType.MakeGenericType(aggregateRootType);
				Type implementationType = repositoryImplementationTemplateType.MakeGenericType(aggregateRootType);
				this.services.AddTransient(serviceType, implementationType);
			}

			IRepositoryRegistry repositoryRegistry = this.services.GetSingletonInstance<IRepositoryRegistry>();
			repositoryRegistry.Register((RepositoryName)repositoryName, repositoryOptions);

			return this;
		}
	}
}
