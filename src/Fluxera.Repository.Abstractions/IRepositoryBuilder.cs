namespace Fluxera.Repository
{
	using System;
	using JetBrains.Annotations;
	using Microsoft.Extensions.DependencyInjection;

	/// <summary>
	///     A contract for a repository builder service.
	/// </summary>
	[PublicAPI]
	public interface IRepositoryBuilder
	{
		/// <summary>
		///     Gets the service collection.
		/// </summary>
		IServiceCollection Services { get; }

		/// <summary>
		///     Adds a storage repository implementation for the "Default" repository name and type.
		///     The repository options are configured using the repository options builder action.
		/// </summary>
		/// <remarks>
		///     The repository name must be unique.
		/// </remarks>
		/// <param name="repositoryType"></param>
		/// <param name="contextType"></param>
		/// <param name="configure"></param>
		/// <returns></returns>
		IRepositoryBuilder AddRepository(Type repositoryType, Type contextType, Action<IRepositoryOptionsBuilder> configure)
		{
			return this.AddRepository("Default", repositoryType, contextType, configure);
		}

		/// <summary>
		///     Adds a storage repository implementation for the given repository name and type.
		///     The repository options are configured using the repository options builder action.
		/// </summary>
		/// <remarks>
		///     The repository name must be unique.
		/// </remarks>
		/// <param name="repositoryName"></param>
		/// <param name="repositoryType"></param>
		/// <param name="contextType"></param>
		/// <param name="configure"></param>
		/// <returns></returns>
		IRepositoryBuilder AddRepository(string repositoryName, Type repositoryType, Type contextType, Action<IRepositoryOptionsBuilder> configure);
	}
}
