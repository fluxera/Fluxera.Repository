namespace Fluxera.Repository.MongoDB.IntegrationTests
{
	using Fluxera.Utilities.Extensions;
	using MadEyeMatt.MongoDB.DbContext;

	public sealed class RepositoryMongoDbContext : MongoDbContext
	{
		public RepositoryMongoDbContext(MongoDbContextOptions options) 
			: base(options)
		{
		}

		/// <inheritdoc />
		protected override void OnConfiguring(MongoDbContextOptionsBuilder builder)
		{
			if(!builder.IsConfigured)
			{
				builder.UseDatabase(GlobalFixture.ConnectionString, GlobalFixture.Database);
			}
		}

		/// <inheritdoc />
		public override string GetCollectionName<TDocument>()
		{
			return typeof(TDocument).Name.Pluralize();
		}
	}
}
