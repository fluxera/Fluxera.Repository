namespace Fluxera.Repository
{
	using System;
	using System.Collections.Concurrent;
	using System.Collections.Generic;
	using System.Linq;
	using Fluxera.Repository.Options;
	using Fluxera.Utilities.Extensions;
	using JetBrains.Annotations;

	[UsedImplicitly]
	internal sealed class RepositoryRegistry : IRepositoryRegistry, IDisposable
	{
		private readonly IDictionary<RepositoryName, RepositoryOptions> repositories = new ConcurrentDictionary<RepositoryName, RepositoryOptions>();

		private readonly IDictionary<Type, RepositoryName> repositoryMappings = new ConcurrentDictionary<Type, RepositoryName>();
		private readonly RepositoryOptionsList repositoryOptionsList;

		public RepositoryRegistry(RepositoryOptionsList repositoryOptionsList)
		{
			this.repositoryOptionsList = repositoryOptionsList;
		}

		/// <inheritdoc />
		void IDisposable.Dispose()
		{
			this.repositories.Clear();
			this.repositoryMappings.Clear();
		}

		public RepositoryName GetRepositoryNameFor(Type aggregateType)
		{
			this.EnsureInitialized();

			RepositoryName repositoryName = this.repositoryMappings.GetOrDefault(aggregateType);
			if(repositoryName is null)
			{
				throw Errors.NoRepositoryNameAvailable(aggregateType);
			}

			return repositoryName;
		}

		public RepositoryName GetRepositoryNameFor<TAggregateRoot>()
		{
			this.EnsureInitialized();

			return this.GetRepositoryNameFor(typeof(TAggregateRoot));
		}

		public RepositoryOptions GetRepositoryOptionsFor(RepositoryName repositoryName)
		{
			this.EnsureInitialized();

			RepositoryOptions repositoryOptions = this.repositories.GetOrDefault(repositoryName);
			if(repositoryOptions is null)
			{
				throw Errors.NoRepositoryOptionsAvailable(repositoryName);
			}

			return repositoryOptions;
		}

		public IReadOnlyCollection<RepositoryOptions> GetRepositoryOptions()
		{
			this.EnsureInitialized();

			return this.repositories.Values.AsReadOnly();
		}

		private void EnsureInitialized()
		{
			if(this.repositories.Any() && this.repositoryMappings.Any())
			{
				return;
			}

			foreach(RepositoryOptions repositoryOptions in this.repositoryOptionsList)
			{
				this.Register(repositoryOptions);
			}
		}

		private void Register(RepositoryOptions repositoryOptions)
		{
			RepositoryName repositoryName = repositoryOptions.RepositoryName;

			if(!this.repositories.ContainsKey(repositoryName))
			{
				this.repositories.Add(repositoryName, repositoryOptions);
				foreach(Type aggregateRootType in repositoryOptions.AggregateRootTypes)
				{
					if(!this.repositoryMappings.ContainsKey(aggregateRootType))
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
	}
}
