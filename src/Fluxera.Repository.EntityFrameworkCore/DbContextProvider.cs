namespace Fluxera.Repository.EntityFrameworkCore
{
	using System;
	using JetBrains.Annotations;
	using Microsoft.EntityFrameworkCore;

	[UsedImplicitly]
	internal sealed class DbContextProvider : ContextProviderBase<DbContext>
	{
		/// <inheritdoc />
		public DbContextProvider(
			IServiceProvider serviceProvider,
			IRepositoryRegistry repositoryRegistry)
			: base("EntityFrameworkCore.Context", serviceProvider, repositoryRegistry)
		{
		}
	}
}
