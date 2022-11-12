namespace Fluxera.Repository.InMemory
{
	using System;
	using JetBrains.Annotations;

	[UsedImplicitly]
	internal sealed class InMemoryContextProvider : ContextProviderBase<InMemoryContext>
	{
		/// <inheritdoc />
		public InMemoryContextProvider(
			IServiceProvider serviceProvider,
			IRepositoryRegistry repositoryRegistry)
			: base("InMemory.Context", serviceProvider, repositoryRegistry)
		{
		}
	}
}
