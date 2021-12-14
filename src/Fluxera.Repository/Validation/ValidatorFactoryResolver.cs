namespace Fluxera.Repository.Validation
{
	using System;
	using System.Collections.Generic;
	using Fluxera.Extensions.DependencyInjection;
	using Fluxera.Extensions.Validation;
	using Fluxera.Repository;

	internal sealed class ValidatorFactoryResolver
	{
		private readonly IServiceProvider serviceProvider;

		public ValidatorFactoryResolver(IServiceProvider serviceProvider)
		{
			this.serviceProvider = serviceProvider;
		}

		public IEnumerable<IValidatorFactory> ResolveValidatorFactories(RepositoryName repositoryName)
		{
			IEnumerable<IValidatorFactory> services = this.serviceProvider.GetNamedServices<IValidatorFactory>((string)repositoryName);
			return services;
		}
	}
}
