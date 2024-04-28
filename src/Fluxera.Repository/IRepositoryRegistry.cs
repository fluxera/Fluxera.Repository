namespace Fluxera.Repository
{
	using System;
	using Fluxera.Repository.Options;
	using JetBrains.Annotations;

	/// <summary>
	///     A contract for a service that provides access to the repository options.
	/// </summary>
	[PublicAPI]
	public interface IRepositoryRegistry
	{
		/// <summary>
		///     Gets the name of the repository for the aggregate type.
		/// </summary>
		/// <typeparam name="TAggregateRoot">The aggregate type.</typeparam>
		/// <returns>The name.</returns>
		RepositoryName GetRepositoryNameFor<TAggregateRoot>();

		/// <summary>
		///     Gets the name of the repository for the aggregate type.
		/// </summary>
		/// <returns>The name.</returns>
		RepositoryName GetRepositoryNameFor(Type aggregateType);

		/// <summary>
		///     Gets the repository options for the given repository name;
		/// </summary>
		/// <param name="repositoryName">The name.</param>
		/// <returns>The options.</returns>
		RepositoryOptions GetRepositoryOptionsFor(RepositoryName repositoryName);

		/// <summary>
		///     Gets the repository options for the given repository name;
		/// </summary>
		/// <param name="repositoryName">The name.</param>
		/// <returns>The options.</returns>
		RepositoryOptions GetRepositoryOptionsFor(string repositoryName);
	}
}
