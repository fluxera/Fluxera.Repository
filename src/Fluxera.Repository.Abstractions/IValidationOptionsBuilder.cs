namespace Fluxera.Repository
{
	using System;
	using Fluxera.Extensions.Validation;

	public interface IValidationOptionsBuilder
	{
		IValidationOptionsBuilder AddValidatorFactory(Action<IValidationBuilder> configure);

		string RepositoryName { get; }
	}
}
