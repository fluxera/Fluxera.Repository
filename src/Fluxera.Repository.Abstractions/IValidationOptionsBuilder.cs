namespace Fluxera.Repository
{
	using System;
	using Fluxera.Extensions.Validation;
	using JetBrains.Annotations;

	/// <summary>
	///     A contract for a validation options builder service.
	/// </summary>
	[PublicAPI]
	public interface IValidationOptionsBuilder
	{
		/// <summary>
		///     Gets the repository name.
		/// </summary>
		string RepositoryName { get; }

		/// <summary>
		///     Adds a validator factory. <see cref="IValidationBuilder" /> for infos.
		/// </summary>
		/// <param name="configure"></param>
		/// <returns></returns>
		IValidationOptionsBuilder AddValidatorFactory(Action<IValidationBuilder> configure);
	}
}
