﻿namespace Fluxera.Repository.InMemory
{
	using System;
	using Fluxera.Guards;
	using JetBrains.Annotations;

	[PublicAPI]
	public static class RepositoryBuilderExtensions
	{
		public static IRepositoryBuilder AddInMemoryRepository(this IRepositoryBuilder builder, string repositoryName, Action<IRepositoryOptionsBuilder> configure)
		{
			Guard.Against.Null(builder, nameof(builder));
			Guard.Against.NullOrWhiteSpace(repositoryName, nameof(repositoryName));
			Guard.Against.Null(configure, nameof(configure));

			return builder.AddRepository(repositoryName, typeof(InMemoryRepository<>), configure);
		}
	}
}