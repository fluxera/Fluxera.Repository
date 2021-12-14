namespace Fluxera.Repository
{
	using System;
	using System.Collections.Concurrent;
	using System.Collections.Generic;
	using Fluxera.Entity;
	using Fluxera.Repository.Options;
	using Fluxera.Utilities.Extensions;
	using JetBrains.Annotations;

	[UsedImplicitly]
	internal sealed class RepositoryRegistry : IRepositoryRegistry, IDisposable
	{
		private readonly IDictionary<RepositoryName, RepositoryOptions> repositories = new ConcurrentDictionary<RepositoryName, RepositoryOptions>();

		private readonly IDictionary<Type, RepositoryName> repositoryMappings = new ConcurrentDictionary<Type, RepositoryName>();

		void IRepositoryRegistry.Register(RepositoryName repositoryName, RepositoryOptions repositoryOptions)
		{
			if (!this.repositories.ContainsKey(repositoryName))
			{
				this.repositories.Add(repositoryName, repositoryOptions);
				foreach (Type aggregateRootType in repositoryOptions.AggregateRootTypes)
				{
					if (!this.repositoryMappings.ContainsKey(aggregateRootType))
					{
						this.repositoryMappings.Add(aggregateRootType, repositoryName);
					}
					else
					{
						RepositoryName name = this.repositoryMappings[aggregateRootType];
						throw new InvalidOperationException($"The aggregate type '{aggregateRootType.Name}' is already used by the repository '{name}'.");
					}
				}
			}
			else
			{
				throw new InvalidOperationException($"A repository with the name '{repositoryName}' was already added.");
			}
		}

		public RepositoryName GetRepositoryNameFor(Type repositoryType)
		{
			RepositoryName? repositoryName = this.repositoryMappings.GetOrDefault(repositoryType);
			if(repositoryName is null)
			{
				throw Errors.NoRepositoryNameAvailable(repositoryType);
			}

			return repositoryName;
		}

		public RepositoryName GetRepositoryNameFor<TAggregateRoot>() where TAggregateRoot : AggregateRoot<TAggregateRoot>
		{
			return this.GetRepositoryNameFor(typeof(TAggregateRoot));
		}

		public RepositoryOptions GetRepositoryOptionsFor(RepositoryName repositoryName)
		{
			RepositoryOptions? repositoryOptions = this.repositories.GetOrDefault(repositoryName);
			if(repositoryOptions is null)
			{
				throw Errors.NoRepositoryOptionsAvailable(repositoryName);
			}

			return repositoryOptions;
		}

		public IReadOnlyCollection<RepositoryOptions> GetRepositoryOptions()
		{
			return this.repositories.Values.AsReadOnly();
		}

		/// <inheritdoc />
		void IDisposable.Dispose()
		{
			this.repositories.Clear();
			this.repositoryMappings.Clear();
		}
	}
}
