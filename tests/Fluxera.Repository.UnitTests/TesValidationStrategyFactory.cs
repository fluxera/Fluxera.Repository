namespace Fluxera.Repository.UnitTests
{
	using System;
	using System.Collections.Generic;
	using Fluxera.Entity;
	using Fluxera.Extensions.Validation;
	using Fluxera.Repository.Validation;
	using Fluxera.Utilities.Extensions;
	using Microsoft.Extensions.DependencyInjection;

	public class TesValidationStrategyFactory : IValidationStrategyFactory
	{
		private readonly IServiceProvider serviceProvider;

		public TesValidationStrategyFactory(IServiceProvider serviceProvider)
		{
			this.serviceProvider = serviceProvider;
		}

		/// <inheritdoc />
		public IValidationStrategy<TAggregateRoot> CreateStrategy<TAggregateRoot>() where TAggregateRoot : AggregateRoot<TAggregateRoot>
		{
			return new TestValidationStrategy<TAggregateRoot>(this.GetValidators());
		}

		private IReadOnlyCollection<IValidator> GetValidators()
		{
			IList<IValidator> validators = new List<IValidator>();

			IEnumerable<IValidatorFactory> validatorFactories = this.serviceProvider.GetServices<IValidatorFactory>();
			foreach(IValidatorFactory validatorFactory in validatorFactories)
			{
				IValidator validator = validatorFactory.CreateValidator();
				validators.Add(validator);
			}

			return validators.AsReadOnly();
		}
	}
}
