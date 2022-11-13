﻿namespace Fluxera.Repository.UnitTests
{
	using Fluxera.Repository.InMemory;
	using JetBrains.Annotations;

	[PublicAPI]
	public sealed class RepositoryInMemoryContext : InMemoryContext
	{
		/// <inheritdoc />
		protected override void ConfigureOptions(InMemoryContextOptions options)
		{
			throw new System.NotImplementedException();
		}
	}
}
