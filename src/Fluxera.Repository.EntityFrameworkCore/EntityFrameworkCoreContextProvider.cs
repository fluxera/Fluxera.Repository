namespace Fluxera.Repository.EntityFrameworkCore
{
	using System;
	using JetBrains.Annotations;

	[UsedImplicitly]
	internal sealed class EntityFrameworkCoreContextProvider : ContextProviderBase<EntityFrameworkCoreContext>
	{
		private readonly IServiceProvider serviceProvider;

		/// <inheritdoc />
		public EntityFrameworkCoreContextProvider(
			IServiceProvider serviceProvider,
			IRepositoryRegistry repositoryRegistry)
			: base(serviceProvider, repositoryRegistry)
		{
			this.serviceProvider = serviceProvider;
		}

		/// <inheritdoc />
		protected override void PerformConfigureContext(EntityFrameworkCoreContext context, RepositoryName repositoryName)
		{
			context.Configure(repositoryName, this.serviceProvider);
		}
	}
}
