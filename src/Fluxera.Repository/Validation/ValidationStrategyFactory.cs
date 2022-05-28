namespace Fluxera.Repository.Validation
{
	using System;
	using System.Collections.Generic;
	using Fluxera.Entity;
	using Fluxera.Extensions.DependencyInjection;
	using Fluxera.Extensions.Validation;
	using Fluxera.Guards;
	using Fluxera.Repository.Options;
	using Fluxera.Utilities.Extensions;
	using JetBrains.Annotations;

	[UsedImplicitly]
	internal sealed class ValidationStrategyFactory : IValidationStrategyFactory
	{
		private readonly IRepositoryRegistry repositoryRegistry;
		private readonly IServiceProvider serviceProvider;

		public ValidationStrategyFactory(IRepositoryRegistry repositoryRegistry, IServiceProvider serviceProvider)
		{
			Guard.Against.Null(repositoryRegistry, nameof(repositoryRegistry));
			Guard.Against.Null(serviceProvider, nameof(serviceProvider));

			this.repositoryRegistry = repositoryRegistry;
			this.serviceProvider = serviceProvider;
		}

		/// <inheritdoc />
		public IValidationStrategy<TAggregateRoot, TKey> CreateStrategy<TAggregateRoot, TKey>()
			where TAggregateRoot : AggregateRoot<TAggregateRoot, TKey>
			where TKey : IComparable<TKey>, IEquatable<TKey>
		{
			RepositoryName repositoryName = this.repositoryRegistry.GetRepositoryNameFor<TAggregateRoot>();
			RepositoryOptions repositoryOptions = this.repositoryRegistry.GetRepositoryOptionsFor(repositoryName);

			if(repositoryOptions.ValidationOptions.IsEnabled)
			{
				return new StandardValidationStrategy<TAggregateRoot, TKey>(this.GetValidators(repositoryName));
			}

			return new NoValidationStrategy<TAggregateRoot, TKey>();
		}

		private IReadOnlyCollection<IValidator> GetValidators(RepositoryName repositoryName)
		{
			IList<IValidator> validators = new List<IValidator>();

			IEnumerable<IValidatorFactory> validatorFactories = this.serviceProvider.GetNamedServices<IValidatorFactory>((string)repositoryName);
			foreach(IValidatorFactory validatorFactory in validatorFactories)
			{
				IValidator validator = validatorFactory.CreateValidator();
				validators.Add(validator);
			}

			return validators.AsReadOnly();
		}
	}
}
