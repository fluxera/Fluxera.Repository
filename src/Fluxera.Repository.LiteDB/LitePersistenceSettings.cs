namespace Fluxera.Repository.LiteDB
{
	using JetBrains.Annotations;

	/// <summary>
	///     Provides the settings for the LiteDB repository implementation.
	/// </summary>
	[PublicAPI]
	public sealed class LitePersistenceSettings
	{
		/// <summary>
		///     Gets or sets the filename of the database to use.
		/// </summary>
		public string Database { get; set; } = null!;
	}
}
