namespace Fluxera.Repository.LiteDB.IntegrationTests
{
	using Fluxera.Repository.UnitTests.Core;
	using JetBrains.Annotations;

	[PublicAPI]
	public sealed class RepositoryMultiTenantLiteContext : LiteContext
	{
		private readonly TenantNameProvider tenantNameProvider;

		public RepositoryMultiTenantLiteContext(TenantNameProvider tenantNameProvider = null)
		{
			this.tenantNameProvider = tenantNameProvider;
		}

		/// <inheritdoc />
		protected override void ConfigureOptions(LiteContextOptions options)
		{
			string databaseName = "test";

			if(!string.IsNullOrWhiteSpace(this.tenantNameProvider?.Name))
			{
				databaseName += $"-{this.tenantNameProvider.Name}";
			}

			options.Database = $"{databaseName}";
		}
	}
}
