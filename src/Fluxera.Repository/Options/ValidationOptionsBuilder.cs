namespace Fluxera.Repository.Options
{
	using System;
	using Fluxera.Extensions.Validation;
	using Fluxera.Guards;

	internal sealed class ValidationOptionsBuilder : IValidationOptionsBuilder
	{
		private readonly IValidationBuilder validationBuilder;

		public ValidationOptionsBuilder(IValidationBuilder validationBuilder, RepositoryOptions repositoryOptions)
		{
			this.validationBuilder = validationBuilder;
			this.RepositoryName = repositoryOptions.RepositoryName;
		}

		public string RepositoryName { get; }

		public IValidationOptionsBuilder AddValidatorFactory(Action<IValidationBuilder> configureBuilder)
		{
			Guard.Against.Null(configureBuilder, nameof(configureBuilder));

			configureBuilder.Invoke(this.validationBuilder);

			return this;
		}
	}
}
