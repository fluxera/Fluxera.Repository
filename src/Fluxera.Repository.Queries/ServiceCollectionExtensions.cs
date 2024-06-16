namespace Fluxera.Repository.Queries
{
	using Fluxera.Queries;
	using JetBrains.Annotations;
	using Microsoft.Extensions.DependencyInjection;
	using Microsoft.Extensions.DependencyInjection.Extensions;

	/// <summary>
	///		Extensions methods for the <see cref="IServiceCollection"/> type.
	/// </summary>
	[PublicAPI]
	public static class ServiceCollectionExtensions
	{
		/// <summary>
		///		Adds the repository query executor service.
		/// </summary>
		/// <param name="services"></param>
		/// <returns></returns>
		public static IServiceCollection AddRepositoryQueryExecutor(this IServiceCollection services)
		{
			services.TryAddTransient(typeof(IQueryExecutor<,>), typeof(RepositoryQueryExecutor<,>));

			return services;
		}
	}
}
