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
			: base(serviceProvider, repositoryRegistry)
		{
		}

		/// <inheritdoc />
		protected override void PerformConfigureContext(InMemoryContext context, RepositoryName repositoryName)
		{
			context.Configure(repositoryName, this.ServiceProvider);
		}
	}
}
