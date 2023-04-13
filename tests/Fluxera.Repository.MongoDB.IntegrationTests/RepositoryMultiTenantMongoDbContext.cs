namespace Fluxera.Repository.MongoDB.IntegrationTests
{
	using Fluxera.Repository.UnitTests.Core;
	using Fluxera.Utilities.Extensions;
	using MadEyeMatt.MongoDB.DbContext;

	public sealed class RepositoryMultiTenantMongoDbContext : MongoDbContext
	{
		private readonly TenantNameProvider tenantNameProvider;

		public RepositoryMultiTenantMongoDbContext(
			MongoDbContextOptions options,
			TenantNameProvider tenantNameProvider = null)
			: base(options)
		{
			this.tenantNameProvider = tenantNameProvider;
		}

		/// <inheritdoc />
		protected override void OnConfiguring(MongoDbContextOptionsBuilder builder)
		{
			if(!builder.IsConfigured)
			{
				string databaseName = GlobalFixture.Database;
				if(!string.IsNullOrWhiteSpace(this.tenantNameProvider?.Name))
				{
					databaseName += $"-{this.tenantNameProvider.Name}";
				}

				builder.UseDatabase(GlobalFixture.ConnectionString, databaseName);
			}
		}

		/// <inheritdoc />
		public override string GetCollectionName<TDocument>()
		{
			return typeof(TDocument).Name.Pluralize();
		}
	}
}
