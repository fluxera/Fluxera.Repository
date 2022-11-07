namespace Fluxera.Repository.LiteDB
{
	using System;
	using JetBrains.Annotations;

	[UsedImplicitly]
	internal sealed class LiteContextProvider : ContextProviderBase<LiteContext>
	{
		/// <inheritdoc />
		public LiteContextProvider(
			IServiceProvider serviceProvider,
			IRepositoryRegistry repositoryRegistry)
			: base("Lite.DbContext", serviceProvider, repositoryRegistry)
		{
		}
	}
}
