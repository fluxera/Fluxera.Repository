namespace Fluxera.Repository.MongoDB
{
	using JetBrains.Annotations;

	/// <summary>
	///     Provides the settings for the MongoDB repository implementation.
	/// </summary>
	[PublicAPI]
	public sealed class MongoPersistenceSettings
	{
		/// <summary>
		///     Gets or sets the connection string.
		/// </summary>
		public string ConnectionString { get; set; }

		/// <summary>
		///     Gets or sets the database name.
		/// </summary>
		public string Database { get; set; }

		/// <summary>
		///     Flag, if the connection uses SSL.
		/// </summary>
		public bool UseSsl { get; set; }
	}
}
