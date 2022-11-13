namespace Fluxera.Repository.LiteDB.IntegrationTests
{
	using System;
	using JetBrains.Annotations;

	[PublicAPI]
	public sealed class RepositoryLiteContext : LiteContext
	{
		/// <inheritdoc />
		protected override void ConfigureOptions(LiteContextOptions options)
		{
			options.Database = $"{Guid.NewGuid():N}.db";
		}
	}
}
