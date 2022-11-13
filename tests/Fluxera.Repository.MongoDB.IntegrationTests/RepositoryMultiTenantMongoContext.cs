namespace Fluxera.Repository.MongoDB.IntegrationTests
{
	using Fluxera.Repository.UnitTests.Core;
	using JetBrains.Annotations;

	[PublicAPI]
	public sealed class RepositoryMultiTenantMongoContext : MongoContext
	{
		private readonly TenantNameProvider tenantNameProvider;

		public RepositoryMultiTenantMongoContext(TenantNameProvider tenantNameProvider = null)
		{
			this.tenantNameProvider = tenantNameProvider;
		}

		/// <inheritdoc />
		protected override void ConfigureOptions(MongoContextOptions options)
		{
			options.ConnectionString = "mongodb://localhost:27017";

			string databaseName = "test";

			if(!string.IsNullOrWhiteSpace(this.tenantNameProvider?.Name))
			{
				databaseName += $"-{this.tenantNameProvider.Name}";
			}

			options.Database = databaseName;
		}
	}
}
