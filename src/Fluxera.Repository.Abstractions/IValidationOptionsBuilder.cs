namespace Fluxera.Repository
{
	using JetBrains.Annotations;
	using System.Collections.Generic;
	using System.Reflection;

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
		///		Adds the validators available in the given assemblies.
		/// </summary>
		/// <param name="assemblies"></param>
		/// <returns></returns>
		IValidationOptionsBuilder AddValidators(IEnumerable<Assembly> assemblies);

		/// <summary>
		///		Adds the validators available in the given assembly.
		/// </summary>
		/// <param name="assembly"></param>
		/// <returns></returns>
		IValidationOptionsBuilder AddValidators(Assembly assembly);
	}
}
