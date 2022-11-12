namespace Fluxera.Repository.InMemory
{
	using System;
	using JetBrains.Annotations;
	using Microsoft.Extensions.DependencyInjection;

	/// <summary>
	///     Extension methods for the <see cref="IServiceCollection" /> type.
	/// </summary>
	[PublicAPI]
	public static class ServiceCollectionExtensions
	{
		/// <summary>
		///     Adds a in-memory repository context.
		/// </summary>
		/// <typeparam name="TContext"></typeparam>
		/// <param name="services"></param>
		/// <param name="implementationFactory"></param>
		/// <returns></returns>
		public static IServiceCollection AddInMemoryContext<TContext>(this IServiceCollection services, Func<IServiceProvider, TContext> implementationFactory)
			where TContext : InMemoryContext
		{
			services.AddScoped(implementationFactory);

			return services;
		}
	}
}
