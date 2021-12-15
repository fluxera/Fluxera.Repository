namespace Fluxera.Repository.Options
{
	using System;
	using Fluxera.Extensions.DependencyInjection;
	using Fluxera.Guards;
	using JetBrains.Annotations;
	using Microsoft.Extensions.DependencyInjection;

	[PublicAPI]
	internal sealed class RepositoryBuilder : IRepositoryBuilder
	{
		public RepositoryBuilder(IServiceCollection services)
		{
			Guard.Against.Null(services, nameof(services));

			this.Services = services;
		}

		/// <inheritdoc />
		public IRepositoryBuilder AddRepository(string repositoryName, Type repositoryType, Action<IRepositoryOptionsBuilder> configure)
		{
			Guard.Against.NullOrWhiteSpace(repositoryName, nameof(repositoryName));
			Guard.Against.Null(repositoryType, nameof(repositoryType));
			Guard.Against.Null(configure, nameof(configure));

			RepositoryOptionsBuilder repositoryOptionsBuilder = new RepositoryOptionsBuilder(this.Services, repositoryName, repositoryType);
			configure.Invoke(repositoryOptionsBuilder);
			RepositoryOptions repositoryOptions = repositoryOptionsBuilder.Build();

			// Add the repositories for the registered aggregates.
			Type repositoryServiceTemplateType = typeof(IRepository<>);
			Type readOnlyRepositoryServiceTemplateType = typeof(IReadOnlyRepository<>);
			Type repositoryImplementationTemplateType = repositoryType;
			foreach(Type aggregateRootType in repositoryOptions.AggregateRootTypes)
			{
				Type serviceType = repositoryServiceTemplateType.MakeGenericType(aggregateRootType);
				Type implementationType = repositoryImplementationTemplateType.MakeGenericType(aggregateRootType);
				this.Services.AddTransient(serviceType, implementationType);
			}

			foreach(Type aggregateRootType in repositoryOptions.AggregateRootTypes)
			{
				Type serviceType = readOnlyRepositoryServiceTemplateType.MakeGenericType(aggregateRootType);
				Type implementationType = repositoryImplementationTemplateType.MakeGenericType(aggregateRootType);
				this.Services.AddTransient(serviceType, implementationType);
			}

			RepositoryOptionsList? repositoryOptionsList = this.Services.GetSingletonInstanceOrDefault<RepositoryOptionsList>();
			if(repositoryOptionsList is null)
			{
				repositoryOptionsList = new RepositoryOptionsList();
				this.Services.AddSingleton(repositoryOptionsList);
			}

			repositoryOptionsList.Add(repositoryOptions);

			return this;
		}

		/// <inheritdoc />
		public IServiceCollection Services { get; }
	}
}
