namespace Fluxera.Repository
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
	internal sealed class ValidatorProvider : IValidatorProvider
	{
		private readonly IRepositoryRegistry repositoryRegistry;
		private readonly IServiceProvider serviceProvider;

		public ValidatorProvider(IRepositoryRegistry repositoryRegistry, IServiceProvider serviceProvider)
		{
			Guard.Against.Null(repositoryRegistry, nameof(repositoryRegistry));
			Guard.Against.Null(serviceProvider, nameof(serviceProvider));

			this.repositoryRegistry = repositoryRegistry;
			this.serviceProvider = serviceProvider;
		}

		/// <inheritdoc />
		public IEnumerable<IValidator> GetValidatorsFor<TAggregateRoot>()
			where TAggregateRoot : AggregateRoot<TAggregateRoot>
		{
			// Get the repository options.
			RepositoryName repositoryName = this.repositoryRegistry.GetRepositoryNameFor<TAggregateRoot>();
			RepositoryOptions repositoryOptions = this.repositoryRegistry.GetRepositoryOptionsFor(repositoryName);

			// Initialize validators.
			IList<IValidator> validators = new List<IValidator>();

			if(repositoryOptions.ValidationOptions.IsEnabled)
			{
				IEnumerable<IValidatorFactory> validatorFactories = this.serviceProvider.GetNamedServices<IValidatorFactory>((string)repositoryName);
				foreach(IValidatorFactory validatorFactory in validatorFactories)
				{
					IValidator validator = validatorFactory.CreateValidator();
					validators.Add(validator);
				}
			}

			return validators.AsReadOnly();
		}
	}
}
