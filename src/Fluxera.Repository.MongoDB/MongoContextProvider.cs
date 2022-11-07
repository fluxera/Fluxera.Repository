namespace Fluxera.Repository.MongoDB
{
	using System;
	using JetBrains.Annotations;

	[UsedImplicitly]
	internal sealed class MongoContextProvider : ContextProviderBase<MongoContext>
	{
		/// <inheritdoc />
		public MongoContextProvider(
			IServiceProvider serviceProvider,
			IRepositoryRegistry repositoryRegistry)
			: base("Mongo.DbContext", serviceProvider, repositoryRegistry)
		{
		}
	}
}
