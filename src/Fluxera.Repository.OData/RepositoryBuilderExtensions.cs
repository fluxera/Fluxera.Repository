namespace Fluxera.Repository.OData
{
	using System;
	using JetBrains.Annotations;

	[PublicAPI]
	public static class RepositoryBuilderExtensions
	{
		public static IRepositoryBuilder AddODataRepository(this IRepositoryBuilder builder, string repositoryName, Action<IRepositoryOptionsBuilder> configure)
		{
			return builder.AddRepository(repositoryName, typeof(ODataRepository<>), configure);
		}
	}
}
