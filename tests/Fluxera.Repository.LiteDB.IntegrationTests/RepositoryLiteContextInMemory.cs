namespace Fluxera.Repository.LiteDB.IntegrationTests
{
	using System;
	using JetBrains.Annotations;

	[PublicAPI]
	public sealed class RepositoryLiteContextInMemory : LiteContext
	{
		/// <inheritdoc />
		protected override void ConfigureOptions(LiteContextOptions options)
		{
			options.Database = $"{Guid.NewGuid():N}";
			options.Persistent = false;
		}
	}
}
