namespace Fluxera.Repository.EntityFrameworkCore
{
	using JetBrains.Annotations;

	/// <summary>
	///     Provides the settings for the EF repository implementation.
	/// </summary>
	[PublicAPI]
	public sealed class EntityFrameworkCorePersistenceSettings
	{
		/// <summary>
		///     Gets or sets the connection string.
		/// </summary>
		public string ConnectionString { get; set; }

		/// <summary>
		///     Flag, if EF should log the SQL of generated queries.
		/// </summary>
		public bool LogSQL { get; set; }
	}
}
