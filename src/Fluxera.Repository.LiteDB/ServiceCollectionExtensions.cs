namespace Fluxera.Repository.LiteDB
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
		public static IServiceCollection AddLiteContext<TContext>(this IServiceCollection services, Func<IServiceProvider, TContext> implementationFactory)
			where TContext : LiteContext
		{
			services.AddScoped(implementationFactory);

			return services;
		}
	}
}
