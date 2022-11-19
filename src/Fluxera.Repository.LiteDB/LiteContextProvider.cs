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
			: base(serviceProvider, repositoryRegistry)
		{
		}

		/// <inheritdoc />
		protected override void PerformConfigureContext(LiteContext context, RepositoryName repositoryName)
		{
			context.Configure(repositoryName, this.ServiceProvider);
		}
	}
}
