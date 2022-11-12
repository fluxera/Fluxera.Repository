// ReSharper disable AssignNullToNotNullAttribute

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
			this.Services = Guard.Against.Null(services);
		}

		/// <inheritdoc />
		public IRepositoryBuilder AddRepository(string repositoryName, Type repositoryType, Action<IRepositoryOptionsBuilder> configure)
		{
			Guard.Against.NullOrWhiteSpace(repositoryName);
			Guard.Against.Null(repositoryType);
			Guard.Against.Null(configure);

			RepositoryOptionsBuilder repositoryOptionsBuilder = new RepositoryOptionsBuilder(this.Services, repositoryName, repositoryType);
			configure.Invoke(repositoryOptionsBuilder);
			RepositoryOptions repositoryOptions = repositoryOptionsBuilder.Build();

			// Add the repositories for the registered aggregates.
			Type repositoryServiceTemplateType = typeof(IRepository<,>);
			Type readOnlyRepositoryServiceTemplateType = typeof(IReadOnlyRepository<,>);

			foreach(Type aggregateRootType in repositoryOptions.AggregateRootTypes)
			{
				Type keyType = aggregateRootType.BaseType?.GenericTypeArguments[1];

				Type repositoryServiceType = repositoryServiceTemplateType.MakeGenericType(aggregateRootType, keyType);
				Type readOnlyRepositoryServiceType = readOnlyRepositoryServiceTemplateType.MakeGenericType(aggregateRootType, keyType);
				Type implementationType = repositoryType.MakeGenericType(aggregateRootType, keyType);

				this.Services.AddTransient(repositoryServiceType, implementationType);
				this.Services.AddTransient(readOnlyRepositoryServiceType, implementationType);
			}

			RepositoryOptionsList repositoryOptionsList = this.Services.GetSingletonInstanceOrDefault<RepositoryOptionsList>();
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
