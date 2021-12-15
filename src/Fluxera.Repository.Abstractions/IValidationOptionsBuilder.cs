namespace Fluxera.Repository
{
	using System;
	using Fluxera.Extensions.Validation;

	public interface IValidationOptionsBuilder
	{
		string RepositoryName { get; }

		IValidationOptionsBuilder AddValidatorFactory(Action<IValidationBuilder> configure);
	}
}
