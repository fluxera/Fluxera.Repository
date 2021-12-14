namespace Fluxera.Repository
{
	using System;
	using JetBrains.Annotations;

	[PublicAPI]
	public interface IRepositoryBuilder
	{
		IRepositoryBuilder AddRepository(string repositoryName, Type repositoryType, Action<IRepositoryOptionsBuilder> configure);
	}
}
