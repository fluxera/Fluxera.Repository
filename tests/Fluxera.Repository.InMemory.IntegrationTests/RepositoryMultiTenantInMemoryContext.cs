namespace Fluxera.Repository.InMemory.IntegrationTests
{
	using Fluxera.Repository.UnitTests.Core;
	using JetBrains.Annotations;

	[PublicAPI]
	public sealed class RepositoryMultiTenantInMemoryContext : InMemoryContext
	{
		private readonly TenantNameProvider tenantNameProvider;

		public RepositoryMultiTenantInMemoryContext(TenantNameProvider tenantNameProvider = null)
		{
			this.tenantNameProvider = tenantNameProvider;
		}

		/// <inheritdoc />
		protected override void ConfigureOptions(InMemoryContextOptions options)
		{
			string databaseName = string.Empty;

			if(!string.IsNullOrWhiteSpace(this.tenantNameProvider?.Name))
			{
				databaseName = $"test-{this.tenantNameProvider.Name}";
			}

			options.Database = databaseName;
		}
	}
}
