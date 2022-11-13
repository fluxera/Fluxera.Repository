﻿namespace Fluxera.Repository.MongoDB
{
	using JetBrains.Annotations;

	/// <summary>
	///     Provides the options for the MongoDB repository implementation.
	/// </summary>
	[PublicAPI]
	public sealed class MongoContextOptions
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

		/// <summary>
		///     Flag, if the diagnostics instrumentation should record the command text used.
		/// </summary>
		public bool CaptureCommandText { get; set; } = true;
	}
}