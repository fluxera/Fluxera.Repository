namespace Fluxera.Repository.LiteDB
{
	using System;
	using JetBrains.Annotations;
	using Microsoft.Extensions.DependencyInjection;

	[UsedImplicitly]
	internal sealed class LiteContextProvider : ContextProviderBase<LiteContext>
	{
		private readonly IServiceProvider serviceProvider;

		/// <inheritdoc />
		public LiteContextProvider(
			IServiceProvider serviceProvider,
			IRepositoryRegistry repositoryRegistry)
			: base(serviceProvider, repositoryRegistry)
		{
			this.serviceProvider = serviceProvider;
		}

		/// <inheritdoc />
		protected override void PerformConfigureContext(LiteContext context, RepositoryName repositoryName)
		{
			DatabaseProvider databaseProvider = this.serviceProvider.GetRequiredService<DatabaseProvider>();

			context.Configure(repositoryName, databaseProvider);
		}
	}
}
