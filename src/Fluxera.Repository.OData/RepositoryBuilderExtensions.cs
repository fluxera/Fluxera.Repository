namespace Fluxera.Repository.OData
{
	using System;
	using Fluxera.Extensions.DependencyInjection;
	using JetBrains.Annotations;

	/// <summary>
	///     The extensions methods to configure an OData repository.
	/// </summary>
	[PublicAPI]
	public static class RepositoryBuilderExtensions
	{
		/// <summary>
		///     Add an OData repository for the "Default" repository name. The repository options
		///     are configured using the given configure action.
		/// </summary>
		/// <param name="builder"></param>
		/// <param name="configure"></param>
		/// <returns></returns>
		public static IRepositoryBuilder AddODataRepository(this IRepositoryBuilder builder,
			Action<IRepositoryOptionsBuilder> configure)
		{
			return builder.AddODataRepository("Default", configure);
		}

		/// <summary>
		///     Adds an OData repository for the given repository name. The repository options
		///     are configured using the options builder configure action.
		/// </summary>
		/// <param name="builder"></param>
		/// <param name="repositoryName"></param>
		/// <param name="configure"></param>
		/// <returns></returns>
		public static IRepositoryBuilder AddODataRepository(this IRepositoryBuilder builder, string repositoryName,
			Action<IRepositoryOptionsBuilder> configure)
		{
			builder.Services.AddNamedTransient<IUnitOfWork>(serviceBuilder =>
			{
				serviceBuilder.AddNameFor<ODataUnitOfWork>(repositoryName);
			});

			return builder.AddRepository(repositoryName, typeof(ODataRepository<,>), configure);
		}
	}
}
