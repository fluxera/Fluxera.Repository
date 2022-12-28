namespace Fluxera.Repository.MongoDB
{
	using System;
	using JetBrains.Annotations;

	/// <inheritdoc />
	[UsedImplicitly]
	public sealed class MongoContextProvider : ContextProviderBase<MongoContext>
	{
		/// <inheritdoc />
		public MongoContextProvider(
			IServiceProvider serviceProvider,
			IRepositoryRegistry repositoryRegistry)
			: base(serviceProvider, repositoryRegistry)
		{
		}

		/// <inheritdoc />
		protected override void PerformConfigureContext(MongoContext context, RepositoryName repositoryName)
		{
			context.Configure(repositoryName, this.ServiceProvider);
		}
	}
}
