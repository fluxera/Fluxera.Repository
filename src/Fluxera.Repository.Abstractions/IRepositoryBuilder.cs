namespace Fluxera.Repository
{
	using System;
	using JetBrains.Annotations;
	using Microsoft.Extensions.DependencyInjection;

	[PublicAPI]
	public interface IRepositoryBuilder
	{
		IRepositoryBuilder AddRepository(string repositoryName, Type repositoryType, Action<IRepositoryOptionsBuilder> configure);

		IServiceCollection Services { get; }
	}
}
